namespace Crm.Service.Controllers.RssFeedProvider
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main.Infrastructure;
	using Main.Results;

	using Microsoft.AspNetCore.Routing;

	using NHibernate.Linq;

	public class ServiceCaseRssFeedProvider : RssFeedProvider<ServiceCase>
	{
		private readonly IResourceManager resourceManager;
		private readonly ILookupManager lookupManager;
		public override IQueryable<ServiceCase> Eager(IQueryable<ServiceCase> items)
		{
			return items.Fetch(x => x.AffectedInstallation);
		}
		public ServiceCaseRssFeedProvider(IAbsolutePathHelper absolutePathHelper, IUserService userService, IResourceManager resourceManager, Site site, IPluginProvider pluginProvider, ILookupManager lookupManager)
			: base(absolutePathHelper, userService, site, pluginProvider)
		{
			this.resourceManager = resourceManager;
			this.lookupManager = lookupManager;
		}
		protected override SyndicationFeedItemMapper<ServiceCase> GetFeedMapper(Dictionary<string, object> argDictionary)
		{
			return new SyndicationFeedItemMapper<ServiceCase>(
				GenerateTitle,
				GenerateBody,
				"ServiceCase",
				"DetailsTemplate",
				f => f.Id.ToString(),
				f => f.Reported != null ? f.Reported.Value : DateTimeOffset.Now
			);
		}

		public virtual string GenerateTitle(ServiceCase serviceCase)
		{
			var tokens = new List<string>
			{
				String.Format("[{0}] {1}", lookupManager.Get<ServiceCaseStatus>(serviceCase.StatusKey, CultureInfo.TwoLetterISOLanguageName), 
					serviceCase.ErrorCode != null ? serviceCase.ErrorCode.Key.ToString() : ""), 
					serviceCase.ServiceCaseNo,
			};

			if (serviceCase.AffectedInstallation != null)
			{
				tokens.Add(resourceManager.GetTranslation("for", CultureInfo));
				tokens.Add(serviceCase.AffectedInstallation.InstallationNo);
			}

			if (serviceCase.Reported != null)
			{
				tokens.Add(resourceManager.GetTranslation("at", CultureInfo));
				tokens.Add(serviceCase.Reported.ToString());
			}

			return tokens.ToString(" ");
		}

		public virtual string GenerateBody(ServiceCase serviceCase)
		{

			var body = new List<string> { String.Format("{0}: {1}", resourceManager.GetTranslation("ErrorMessage", CultureInfo), String.IsNullOrWhiteSpace(serviceCase.ErrorMessage) ? "-" : serviceCase.ErrorMessage), "<br>" };

			if (serviceCase.PriorityKey != null)
			{
				body.Add(resourceManager.GetTranslation("Priority", CultureInfo));
				body.Add(": ");
				body.Add(lookupManager.Get<ServicePriority>(serviceCase.PriorityKey, CultureInfo.TwoLetterISOLanguageName).Value);
				body.Add("<br>");
			}
			
			if (serviceCase.AffectedCompany != null)
			{
				body.Add(resourceManager.GetTranslation("Customer", CultureInfo));
				body.Add(": ");
				body.Add(serviceCase.AffectedCompany.LegacyName);
				
				if (!String.IsNullOrWhiteSpace(serviceCase.AffectedCompany.StandardAddressStreet))
				{
					body.Add("<br>");
					body.Add(resourceManager.GetTranslation("Address", CultureInfo));
					body.Add(": ");
					body.Add(serviceCase.AffectedCompany.StandardAddressStreet);
				}

				if (!String.IsNullOrWhiteSpace(serviceCase.AffectedCompany.StandardAddressZipCode) || !String.IsNullOrWhiteSpace(serviceCase.AffectedCompany.StandardAddressCity))
					body.Add(String.Format(" - {0} {1}", serviceCase.AffectedCompany.StandardAddressZipCode, serviceCase.AffectedCompany.StandardAddressCity));
				
				body.Add("<br>");
			}

			return body.ToString("");
		}
		protected override SyndicationFeedOptions GetFeedOptions(Dictionary<string, object> argDictionary)
		{
			return new SyndicationFeedOptions(
				resourceManager.GetTranslation("ServiceCase"),
				resourceManager.GetTranslation("ServiceCase"),
				absolutePathHelper.GetPath("IndexTemplate", "ServiceCaseList", new RouteValueDictionary(new { plugin = "Crm.Service" }))
			);
		}
	}
}
