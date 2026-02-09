namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
	using Crm.Model;

	public class ServiceCaseConfiguration : EntityConfiguration<ServiceCase>
	{
		public override void Initialize()
		{
			Property(x => x.ServiceCaseNo, m => m.Filterable());
			Property(x => x.Status, c => { c.Filterable(); });
			Property(x => x.StatusKey, c => { c.Sortable(); });
			Property(x => x.Priority, c => { c.Filterable(); });
			Property(x => x.Priority, c => { c.Sortable(); });
			Property(x => x.Category, m => m.Filterable());
			Property(x => x.ResponsibleUser, m => m.Filterable(f => f.Definition(new UserFilterDefinition { CustomFilterExpression = true, WithGroups = true, FilterForGroup = true })));
			Property(x => x.ServiceCaseCreateUser, m => m.Filterable(f => f.Definition(new UserFilterDefinition { WithGroups = true, Caption = "CreateUser"})));
			Property(x => x.ErrorMessage, m => m.Filterable());
			Property(x => x.ErrorCode, m => m.Filterable());
			Property(x => x.ServiceObjectId, c => c.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<ServiceObject>("ContactAutocomplete", new { Plugin = "Crm", contactType = "ServiceObject" }, "CrmService_ServiceObject", "Helper.ServiceObject.getDisplayName", x => x.Id, x => x.ObjectNo, x => x.Name) { Caption = "ServiceObject" })));
			Property(x => x.AffectedInstallationKey, m => m.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<Installation>("InstallationIdAutocomplete", new { Plugin = "Crm.Service" }, "CrmService_Installation", "Helper.Installation.getDisplayName", x => x.Id, filterFunction: "Helper.Installation.getInstallationAutocompleteFilter") { Caption = "AffectedInstallation" })));
			Property(x => x.AffectedCompanyKey, m => m.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<Company>("CompanyAutocomplete", new { Plugin = "Crm" }, "Crm_Company", "Helper.Company.getDisplayName", x => x.Id, x => x.LegacyId, x => x.Name){ Caption = "AffectedCompany" })));

			Property(x => x.ServiceCaseCreateDate, m =>
			{
				m.Sortable(s => s.SortCaption("CreateDate"));
				m.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = false, AllowPastDates = true }));
				m.Filterable(f => f.Caption("CreateDate"));
			});
		}
		public ServiceCaseConfiguration(IEntityConfigurationHolder<ServiceCase> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
