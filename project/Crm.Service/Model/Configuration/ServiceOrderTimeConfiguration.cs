namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderTimeConfiguration : EntityConfiguration<ServiceOrderTime>
	{
		public override void Initialize()
		{
			Property(x => x.PosNo, m => m.Filterable());
			Property(x => x.Description, m => m.Filterable());
			Property(x => x.Comment, m => m.Filterable());
			Property(x => x.InstallationId, c => c.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<Installation>("InstallationIdAutocomplete", new { Plugin = "Crm.Service" }, "CrmService_Installation", "function(x) { return x.InstallationNo + (x.Description ? ' - ' + x.Description : '') }", x => x.Id, filterFunction: "$root.getInstallationAutocompleteFilter.bind($root)") { Caption = "Installation" })));
			Property(x => x.StatusKey, m => m.Filterable(f => f.Definition(new TypeFilterDefinition(typeof(ServiceOrderTimeStatus)))));
		}
		public ServiceOrderTimeConfiguration(IEntityConfigurationHolder<ServiceOrderTime> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
