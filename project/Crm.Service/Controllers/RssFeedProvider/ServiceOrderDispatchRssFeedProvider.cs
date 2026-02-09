namespace Crm.Service.Controllers.RssFeedProvider
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Enums;
	using Crm.Service.Model;
	using Crm.Service.Model.Extensions;
	using Crm.Service.Model.Lookup;

	using Main.Infrastructure;
	using Main.Model.Lookups;
	using Main.Results;

	using Microsoft.AspNetCore.Routing;

	using NHibernate.Linq;

	public class ServiceOrderDispatchRssFeedProvider : RssFeedProvider<ServiceOrderDispatch>
	{
		private readonly IResourceManager resourceManager;
		private readonly ILookupManager lookupManager;
		private readonly IAppSettingsProvider appSettingsProvider;
		public override IQueryable<ServiceOrderDispatch> Eager(IQueryable<ServiceOrderDispatch> items)
		{
			items = items
				.Fetch(x => x.OrderHead);
			items = items
				.Fetch(x => x.OrderHead.CustomerContact)
				.ThenFetch(x => x.StandardAddress);
			var maintenanceOrderGenerationMode = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.MaintenanceOrderGenerationMode);
			if (maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.JobPerInstallation)
			{
				items = items
					.FetchMany(x => x.OrderHead.ServiceOrderTimes)
					.ThenFetch(x => x.Installation);
			}
			else
			{
				items = items
					.Fetch(x => x.OrderHead.AffectedInstallation);
			}

			items = items
				.Fetch(x => x.OrderHead.ServiceObject);

			return items;
		}
		protected override SyndicationFeedItemMapper<ServiceOrderDispatch> GetFeedMapper(Dictionary<string, object> argDictionary)
		{
			return new SyndicationFeedItemMapper<ServiceOrderDispatch>(
				GenerateTitle,
				GenerateBody,
				"Dispatch",
				"DetailsTemplate",
				f => f.Id.ToString(),
				f => f.ModifyDate
			);
		}
		protected virtual string GenerateTitle(ServiceOrderDispatch dispatch)
		{
			var tokens = new List<string>
			{
				String.Format("[{0}]", lookupManager.Get<ServiceOrderDispatchStatus>(dispatch.StatusKey, CultureInfo.TwoLetterISOLanguageName)),
				dispatch.OrderHead.OrderNo,
				"-",
				dispatch.Date.ToString("MMM d, yyyy", CultureInfo),
				dispatch.Date.ToString("t"),
				"-",
				dispatch.EndDate.ToString("MMM d, yyyy", CultureInfo),
				dispatch.EndDate.ToString("t")
			};

			return tokens.ToString(" ");
		}
		protected virtual string GetInstallationDisplayName(Installation installation)
		{
			if (installation == null)
			{
				return "";
			}
			var displayName = installation.InstallationNo + " - " + installation.Description;
			if (!String.IsNullOrEmpty(installation.LegacyInstallationId))
			{
				displayName += $" ({installation.LegacyInstallationId})";
			}

			return displayName;
		}
		protected virtual string GenerateBody(ServiceOrderDispatch dispatch)
		{
			var body = new List<string>();

			body.Add(String.Format("{0}: {1}",
				resourceManager.GetTranslation("ErrorMessage", CultureInfo),
				String.IsNullOrWhiteSpace(dispatch.OrderHead.ErrorMessage) ? "-" : dispatch.OrderHead.ErrorMessage));
			body.Add(String.Empty);

			var maintenanceOrderGenerationMode = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.MaintenanceOrderGenerationMode);
			if (maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.JobPerInstallation)
			{
				if (dispatch.OrderHead.ServiceOrderTimes.Any(t => t.InstallationId.HasValue))
				{
					body.Add(resourceManager.GetTranslation("Installations", CultureInfo));
					body.Add("------------");
					foreach (var installation in dispatch.OrderHead.ServiceOrderTimes.Where(t => t.InstallationId.HasValue))
					{
						body.Add(GetInstallationDisplayName(installation.Installation));
					}
				}
			}
			else
			{
				body.Add(resourceManager.GetTranslation("Installation", CultureInfo));
				body.Add("------------");
				body.Add(GetInstallationDisplayName(dispatch.OrderHead.AffectedInstallation));
			}

			body.Add(String.Empty);

			if (dispatch.OrderHead.ServiceObjectId.HasValue)
			{
				body.Add(resourceManager.GetTranslation("ServiceObject", CultureInfo));
				body.Add("------------");
				body.Add($"{dispatch.OrderHead.ServiceObject.ObjectNo} - {dispatch.OrderHead.ServiceObject.Name}");
				body.Add(String.Empty);
			}

			if (dispatch.OrderHead.CustomerContact != null)
			{
				body.Add(resourceManager.GetTranslation("Customer", CultureInfo));
				body.Add("------------");
				body.Add(dispatch.OrderHead.CustomerContact.LegacyName);
				if (!String.IsNullOrWhiteSpace(dispatch.OrderHead.CustomerContact.StandardAddressStreet))
					body.Add(dispatch.OrderHead.CustomerContact.StandardAddressStreet);
				if (!String.IsNullOrWhiteSpace(dispatch.OrderHead.CustomerContact.StandardAddressZipCode) || !String.IsNullOrWhiteSpace(dispatch.OrderHead.CustomerContact.StandardAddressCity))
					body.Add(String.Format("{0} {1}",
						dispatch.OrderHead.CustomerContact.StandardAddressZipCode,
						dispatch.OrderHead.CustomerContact.StandardAddressCity));

				var customerCountry = lookupManager.Get<Country>(dispatch.OrderHead.CustomerContact.StandardAddressCountryKey, CultureInfo.TwoLetterISOLanguageName);
				if (customerCountry != null)
				{
					body.Add(customerCountry.Value);
				}

				body.Add(String.Empty);
			}

			if (dispatch.OrderHead.HasCustomServiceLocationAddress())
			{
				body.Add(resourceManager.GetTranslation("ServiceLocation", CultureInfo));
				body.Add("------------");
				if (!String.IsNullOrWhiteSpace(dispatch.OrderHead.Name1))
					body.Add(dispatch.OrderHead.Name1);
				if (!String.IsNullOrWhiteSpace(dispatch.OrderHead.Name2))
					body.Add(dispatch.OrderHead.Name2);
				if (!String.IsNullOrWhiteSpace(dispatch.OrderHead.Name3))
					body.Add(dispatch.OrderHead.Name3);
				if (!String.IsNullOrWhiteSpace(dispatch.OrderHead.Street))
					body.Add(dispatch.OrderHead.Street);
				if (!String.IsNullOrWhiteSpace(dispatch.OrderHead.ZipCode) || !String.IsNullOrWhiteSpace(dispatch.OrderHead.City))
					body.Add(String.Format("{0} {1}",
						dispatch.OrderHead.ZipCode,
						dispatch.OrderHead.City));

				var slCountry = lookupManager.Get<Country>(dispatch.OrderHead.CountryKey, CultureInfo.TwoLetterISOLanguageName);
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
			var title = "";
			switch (argDictionary["filter"])
			{
				case string workList when workList.Contains("StatusKey eq 'Released' or StatusKey eq 'Read' or StatusKey eq 'InProgress' or StatusKey eq 'SignedByCustomer'"):
					title = resourceManager.GetTranslation("UpcomingDispatches");
					break;
				case string scheduled when scheduled.Contains("StatusKey eq 'Scheduled'"):
					title = resourceManager.GetTranslation("ScheduledDispatches");
					break;
				case string closed when closed.Contains("StatusKey eq 'ClosedNotComplete' or StatusKey eq 'ClosedComplete' or StatusKey eq 'Rejected'"):
					title = resourceManager.GetTranslation("ClosedDispatches");
					break;
				default:
					title = resourceManager.GetTranslation("ServiceOrderDispatch");
					break;
			}

			return new SyndicationFeedOptions(
				title,
				title,
				absolutePathHelper.GetPath("IndexTemplate",
					"ServiceOrderDispatchList",
					new RouteValueDictionary(new { plugin = "Crm.Service" }))
			);
		}

		public ServiceOrderDispatchRssFeedProvider(IAbsolutePathHelper absolutePathHelper, IUserService userService, IResourceManager resourceManager, ILookupManager lookupManager, Site site, IPluginProvider pluginProvider, IAppSettingsProvider appSettingsProvider)
			: base(absolutePathHelper,
				userService,
				site,
				pluginProvider)
		{
			this.resourceManager = resourceManager;
			this.lookupManager = lookupManager;
			this.appSettingsProvider = appSettingsProvider;
		}
	}
}
