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
	using Crm.Service.Model.Extensions;
	using Crm.Service.Model.Lookup;

	using Main.Infrastructure;
	using Main.Model.Lookups;
	using Main.Results;

	using Microsoft.AspNetCore.Routing;

	using NHibernate.Linq;

	public class ServiceOrderRssFeedProvider : RssFeedProvider<ServiceOrderHead>
	{
		private readonly IResourceManager resourceManager;
		private readonly ILookupManager lookupManager;
		public override IQueryable<ServiceOrderHead> Eager(IQueryable<ServiceOrderHead> items)
		{
			items = items
				.Fetch(x => x.CustomerContact)
				.ThenFetch(x => x.StandardAddress);

			items = items
				.Fetch(x => x.AffectedInstallation);

			return items;
		}
		protected override SyndicationFeedItemMapper<ServiceOrderHead> GetFeedMapper(Dictionary<string, object> argDictionary)
		{
			return new SyndicationFeedItemMapper<ServiceOrderHead>
				(
					GenerateTitle,
					GenerateBody,
					"ServiceOrder",
					"DetailsTemplate",
					f => f.Id.ToString(),
					f => f.ModifyDate
				);
		}
		protected virtual string GenerateTitle(ServiceOrderHead serviceOrderHead)
		{
			var tokens = new List<string>
			{
				String.Format("[{0}]", lookupManager.Get<ServiceOrderStatus>(serviceOrderHead.StatusKey, CultureInfo.TwoLetterISOLanguageName)), 
				lookupManager.Get<ServiceOrderType>(serviceOrderHead.TypeKey, CultureInfo.TwoLetterISOLanguageName), 
				serviceOrderHead.OrderNo
			};

			if (serviceOrderHead.AffectedInstallation != null)
			{
				tokens.Add(resourceManager.GetTranslation("for", CultureInfo));
				tokens.Add(serviceOrderHead.AffectedInstallation.InstallationNo);
			}

			return tokens.ToString(" ");
		}
		protected virtual string GenerateBody(ServiceOrderHead serviceOrderHead)
		{
			var body = new List<string>
			{
				String.Format("{0}: {1}", resourceManager.GetTranslation("ErrorMessage", CultureInfo), String.IsNullOrWhiteSpace(serviceOrderHead.ErrorMessage) ? "-" : serviceOrderHead.ErrorMessage),
				String.Empty
			};

			if (serviceOrderHead.CustomerContact != null)
			{
				body.Add(resourceManager.GetTranslation("Customer", CultureInfo));
				body.Add("------------");
				body.Add(serviceOrderHead.CustomerContact.LegacyName);
				if (!String.IsNullOrWhiteSpace(serviceOrderHead.CustomerContact.StandardAddressStreet))
					body.Add(serviceOrderHead.CustomerContact.StandardAddressStreet);
				if (!String.IsNullOrWhiteSpace(serviceOrderHead.CustomerContact.StandardAddressZipCode) || !String.IsNullOrWhiteSpace(serviceOrderHead.CustomerContact.StandardAddressCity))
					body.Add(String.Format("{0} {1}", serviceOrderHead.CustomerContact.StandardAddressZipCode, serviceOrderHead.CustomerContact.StandardAddressCity));

				var customerCountry = lookupManager.Get<Country>(serviceOrderHead.CustomerContact.StandardAddressCountryKey, CultureInfo.TwoLetterISOLanguageName);
				if (customerCountry != null)
				{
					body.Add(customerCountry.Value);
				}
				body.Add(String.Empty);
			}

			if (serviceOrderHead.HasCustomServiceLocationAddress())
			{
				body.Add(resourceManager.GetTranslation("ServiceLocation", CultureInfo));
				body.Add("------------");
				if (!String.IsNullOrWhiteSpace(serviceOrderHead.Name1))
					body.Add(serviceOrderHead.Name1);
				if (!String.IsNullOrWhiteSpace(serviceOrderHead.Name2))
					body.Add(serviceOrderHead.Name2);
				if (!String.IsNullOrWhiteSpace(serviceOrderHead.Name3))
					body.Add(serviceOrderHead.Name3);
				if (!String.IsNullOrWhiteSpace(serviceOrderHead.Street))
					body.Add(serviceOrderHead.Street);
				if (!String.IsNullOrWhiteSpace(serviceOrderHead.ZipCode) || !String.IsNullOrWhiteSpace(serviceOrderHead.City))
					body.Add(String.Format("{0} {1}", serviceOrderHead.ZipCode, serviceOrderHead.City));

				var slCountry = lookupManager.Get<Country>(serviceOrderHead.CountryKey, CultureInfo.TwoLetterISOLanguageName);
				if (slCountry != null)
				{
					body.Add(slCountry.Value);
				}
				body.Add(String.Empty);
			}

			return body.ToString("</br>");
		}

		protected override SyndicationFeedOptions GetFeedOptions(Dictionary<string, object> argDictionary)
		{
			return new SyndicationFeedOptions
				(
				resourceManager.GetTranslation("ServiceOrderTitle"),
				resourceManager.GetTranslation("ServiceOrderTitle"),
				absolutePathHelper.GetPath("IndexTemplate", "ServiceOrderHeadList", new RouteValueDictionary(new { plugin = "Crm.Service" }))
				);
		}

		public ServiceOrderRssFeedProvider(IAbsolutePathHelper absolutePathHelper, IUserService userService, IResourceManager resourceManager, ILookupManager lookupManager, Site site, IPluginProvider pluginProvider)
			: base(absolutePathHelper, userService, site, pluginProvider)
		{
			this.resourceManager = resourceManager;
			this.lookupManager = lookupManager;
		}
	}
}
