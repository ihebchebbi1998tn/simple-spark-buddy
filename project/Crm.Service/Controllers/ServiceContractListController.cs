namespace Crm.Service.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main;
	using Main.Controllers;
	using Main.Model.Lookups;

	using Microsoft.AspNetCore.Mvc;

	using NHibernate.Linq;

	public class ServiceContractListController : GenericListController<ServiceContract>
	{
		public ServiceContractListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ServiceContract>> rssFeedProviders, IEnumerable<ICsvDefinition<ServiceContract>> csvDefinitions, IEntityConfigurationProvider<ServiceContract> entityConfigurationProvider, IRepository<ServiceContract> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Read, Group = ServicePlugin.PermissionGroup.ServiceContract)]
		[RenderAction("ServiceContractItemTemplateActions", Priority = 100)]
		public virtual ActionResult ActionDetails()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceContract)]
		public override ActionResult FilterTemplate()
		{
			return base.FilterTemplate();
		}
		protected override string GetTitle()
		{
			return "ServiceContracts";
		}
		protected override string GetEmptySlate()
		{
			if (!repository.GetAll().Any())
			{
				return String.Join(Environment.NewLine, resourceManager.GetTranslation("NoServiceContractsInfo"), resourceManager.GetTranslation("GeneralServiceContractInfo1"), resourceManager.GetTranslation("GeneralServiceContractInfo2"));
			}

			return resourceManager.GetTranslation("SearchCriteriaYieldedNoResults");
		}
		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ServiceContract)]
		public override ActionResult IndexTemplate()
		{
			return base.IndexTemplate();
		}
		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceContract)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}
		
		[RenderAction("TopSearchMaterialItemTemplate")]
		[RequiredPermission(nameof(ServiceContract), Group = MainPlugin.PermissionGroup.TopSearch)]
		public virtual ActionResult TopSearchMaterialItemTemplate()
		{
			var model = GetGenericListTemplateViewModel();
			return View("Material/GenericListItem", model);
		}

		public class ServiceContractCsvDefinition : CsvDefinition<ServiceContract>
		{
			private readonly ILookupManager lookupManager;
			public override IQueryable<ServiceContract> Eager(IQueryable<ServiceContract> query)
			{
				return query
					.Fetch(x => x.InvoiceRecipient)
					.Fetch(x => x.ParentCompany)
					.Fetch(x => x.Payer)
					;
			}

			public ServiceContractCsvDefinition(IResourceManager resourceManager, ILookupManager lookupManager)
				: base(resourceManager)
			{
				this.lookupManager = lookupManager;
			}

			public override IEnumerable<KeyValuePair<string, Func<ServiceContract, object>>> GetCsv(IEnumerable<ServiceContract> items) {
				var serviceContractTypes = lookupManager.List<ServiceContractType>();
				var currencies = lookupManager.List<Currency>();

				Property("Id", x => x.Id);
				Property("ContractNo", x => x.ContractNo ?? string.Empty);
				Property("CreateDate", x => x.CreateDate.ToShortDateString());
				Property("ContractType", x => x.ContractTypeKey.IsNotNullOrEmpty() ? serviceContractTypes.FirstOrDefault(c => c.Key == x.ContractTypeKey)?.Value : string.Empty);
				Property("ExternalReference", x => x.ExternalReference);
				Property("ValidFrom", x => x.ValidFrom.ToShortDateString());
				Property("ValidTo", x => x.ValidTo.ToShortDateString());
				Property("BusinessPartner", x => x.ParentCompany);
				Property("Payer", x => x.Payer);
				Property("InvoiceRecipient", x => x.InvoiceRecipient);
				Property("Price", x => x.Price);
				Property("Currency", x => x.PriceCurrencyKey.IsNotNullOrEmpty() ? currencies.FirstOrDefault(c => c.Key == x.PriceCurrencyKey)?.Value : string.Empty);
				Property("BackgroundInfo", x => x.BackgroundInfo);

				//Internal Ids
				Property("ContractTypeKey", x => x.ContractTypeKey);
				Property("PriceCurrencyKey", x => x.PriceCurrencyKey);

				return base.GetCsv(items);
			}
		}
	}
}
