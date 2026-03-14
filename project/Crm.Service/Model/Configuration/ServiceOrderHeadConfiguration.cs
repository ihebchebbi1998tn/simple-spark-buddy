namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Model;
	using Crm.Service.Enums;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderHeadConfiguration : EntityConfiguration<ServiceOrderHead>
	{
		private readonly IAppSettingsProvider appSettingsProvider;

		public ServiceOrderHeadConfiguration(IEntityConfigurationHolder<ServiceOrderHead> entityConfigurationHolder, IAppSettingsProvider appSettingsProvider)
			: base(entityConfigurationHolder)
		{
			this.appSettingsProvider = appSettingsProvider;
		}
		public override void Initialize()
		{
			Property(x => x.OrderNo, c =>
			{
				c.Filterable();
				c.Sortable();
			});
			Property(x => x.ServiceOrderTemplateId, m => m.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<ServiceOrderHead>(null, null, "CrmService_ServiceOrderHead", "Helper.ServiceOrder.getDisplayName", x => x.Id, filterFunction: "Helper.ServiceOrder.getServiceOrderTemplateAutocompleteFilter") { Caption = "ServiceOrderTemplate" })));
			NestedProperty(x => x.ServiceCase.Id, c => c.Filterable(f =>
			{
				f.Definition(new AutoCompleterFilterDefinition<ServiceCase>("ServiceCaseIdAutocomplete", new { Plugin = "Crm.Service" }, "CrmService_ServiceCase", "Helper.ServiceCase.getDisplayName", x => x.Id, x => x.ServiceCaseNo, x => x.ErrorMessage) { Caption = "ServiceCase" });
			}));
			Property(x => x.Type, c => c.Filterable());
			Property(x => x.ResponsibleUser, c => c.Filterable(f => f.Definition(new UserFilterDefinition { CustomFilterExpression = true, WithGroups = true, FilterForGroup = true })));
			var maintenanceOrderGenerationMode = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.MaintenanceOrderGenerationMode);
			if (maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.JobPerInstallation)
			{
				Property(x => x.ServiceOrderTimes, c => c.Filterable(f => f.Definition(new CollectionAutoCompleterFilterDefinition<ServiceOrderTime, Installation>(x => x.InstallationId, new AutoCompleterFilterDefinition<Installation>("InstallationIdAutocomplete", new { Plugin = "Crm.Service" }, "CrmService_Installation", "Helper.Installation.getDisplayName", x => x.Id, filterFunction: "Helper.Installation.getInstallationAutocompleteFilter")){ Caption = "Installation" })));
			}
			else
			{
				Property(x => x.InstallationId, c => c.Filterable(f =>
				{
					f.Definition(new AutoCompleterFilterDefinition<Installation>("InstallationIdAutocomplete", new { Plugin = "Crm.Service" }, "CrmService_Installation", "Helper.Installation.getDisplayName", x => x.Id, filterFunction: "Helper.Installation.getInstallationAutocompleteFilter") { Caption = "Installation" });
				}));
			}
			Property(x => x.ErrorCode, c => c.Filterable());
			Property(x => x.ServiceObjectId, c => c.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<ServiceObject>("ContactAutocomplete", new { Plugin = "Crm", contactType = "ServiceObject" }, "CrmService_ServiceObject", "Helper.ServiceObject.getDisplayName", x => x.Id, x => x.ObjectNo, x => x.Name) { Caption = "ServiceObject" })));
			Property(x => x.ServiceContractId, c => c.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<ServiceContract>("ContactAutocomplete", new { Plugin = "Crm", contactType = "ServiceContract" }, "CrmService_ServiceContract", "Helper.ServiceContract.getDisplayName", x => x.Id, x => x.ContractNo) { Caption = "ServiceContract" })));
			Property(x => x.CustomerContactId, m => m.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<Company>("CompanyAutocomplete", new { Plugin = "Crm" }, "Crm_Company", "Helper.Company.getDisplayName", x => x.Id, x => x.LegacyId, x => x.Name))));
			Property(x => x.InitiatorId, m => m.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<Company>("CompanyAutocomplete", new { Plugin = "Crm" }, "Crm_Company", "Helper.Company.getDisplayName", x => x.Id, x => x.LegacyId, x => x.Name))));

			Property(x => x.StationKey, c => c.Filterable(f =>
			{
				f.Definition(new AutoCompleterFilterDefinition<Station>("StationAutocomplete", new { Plugin = "Crm.Service" }, "Crm_Station", "Helper.Station.getDisplayName", x => x.Id, filterFunction: "Helper.Station.getSelect2Filter") { Caption = "Station" });
			}));
			Property(x => x.Region, c => { c.Filterable(); });
			Property(x => x.ZipCode, c => { c.Filterable(); });
			Property(x => x.City, c => { c.Filterable(); });
			Property(x => x.Street, c => { c.Filterable(); });

			Property(x => x.CreateDate, c =>
				{
					c.Sortable();
					c.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = false, AllowPastDates = true }));
				});
			Property(x => x.Planned, c =>
				{
					c.Sortable();
					c.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = false, AllowPastDates = true }));
				});
			Property(x => x.Deadline, c =>
				{
					c.Sortable();
					c.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = false, AllowPastDates = true }));
				});
			Property(x => x.Closed, c =>
				{
					c.Sortable();
					c.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = false, AllowPastDates = true }));
				});
			Property(x => x.Status, c => { c.Filterable(); });
			Property(x => x.StatusKey, c => { c.Sortable(); });
			Property(x => x.Priority, c => { c.Filterable(); });
			Property(x => x.PriorityKey, c => { c.Sortable(); });
			Property(x => x.Latitude, c => c.Filterable(f => f.Definition(new GeoCoordinateFilterDefinition())));
			Property(x => x.Longitude, c => c.Filterable(f => f.Definition(new GeoCoordinateFilterDefinition())));
			Property(x => x.PurchaseOrderNo, c => { c.Filterable(); });
		}
	}
}
