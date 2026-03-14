namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
	using Crm.Model;
	using Crm.Service.Model.Relationships;

	public class ServiceContractConfiguration : EntityConfiguration<ServiceContract>
	{
		public override void Initialize()
		{
			Property(x => x.ContractNo, c => c.Filterable());
			Property(x => x.ContractType, c => c.Filterable());
			Property(x => x.ExternalReference, c => c.Filterable());
			Property(x => x.ServiceObjectId, c => c.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<ServiceObject>("ContactAutocomplete", new { Plugin = "Crm", contactType = "ServiceObject" }, "CrmService_ServiceObject", "Helper.ServiceObject.getDisplayName", x => x.Id, x => x.ObjectNo, x => x.Name) { Caption = "ServiceObject", ShowOnMaterialTab = false })));
			Property(x => x.ParentId, c => c.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<Company>("CompanyAutocomplete", new { Plugin = "Crm" }, "Crm_Company", "Helper.Company.getDisplayName", x => x.Id, x => x.LegacyId, x => x.Name) { ShowOnMaterialTab = false })));
			Property(x => x.Installations, c => c.Filterable(f => f.Definition(new CollectionAutoCompleterFilterDefinition<ServiceContractInstallationRelationship, Installation>(x => x.ChildId, new AutoCompleterFilterDefinition<Installation>("InstallationIdAutocomplete", new { Plugin = "Crm.Service" }, "CrmService_Installation", "function(x) { return x.InstallationNo + (x.Description ? ' - ' + x.Description : '') }", x => x.Id, filterFunction: "Helper.Installation.getInstallationAutocompleteFilter")) { Caption = "Installation", ShowOnMaterialTab = false })));
			Property(x => x.ValidTo, c => c.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = true, AllowPastDates = true })));
			Property(x => x.InvoicedUntil, c => c.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = true, AllowPastDates = true })));
			Property(x => x.LastInvoiceNo, c => c.Filterable());
			Property(x => x.MaintenancePlans, c => c.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = true, AllowPastDates = false })));
		}
		public ServiceContractConfiguration(IEntityConfigurationHolder<ServiceContract> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
