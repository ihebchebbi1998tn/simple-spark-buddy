namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class ServiceCaseTemplateConfiguration : EntityConfiguration<ServiceCaseTemplate>
	{
		public override void Initialize()
		{
			Property(
				x => x.Name,
				m =>
				{
					m.Filterable();
					m.Sortable();
				});
			Property(x => x.CreateDate, m => { m.Sortable(); });
			Property(x => x.ResponsibleUser, m => m.Filterable(f => f.Definition(new UserFilterDefinition { WithGroups = true })));
		}
		public ServiceCaseTemplateConfiguration(IEntityConfigurationHolder<ServiceCaseTemplate> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}