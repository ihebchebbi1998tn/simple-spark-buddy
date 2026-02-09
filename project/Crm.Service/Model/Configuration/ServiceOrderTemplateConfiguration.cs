namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderTemplateConfiguration : EntityConfiguration<ServiceOrderTemplate>
	{
		public ServiceOrderTemplateConfiguration(IEntityConfigurationHolder<ServiceOrderTemplate> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
		public override void Initialize()
		{
			Property(x => x.OrderNo, m =>
			{
				m.Filterable(f => f.Caption("TemplateOrderNo"));
				m.Sortable();
			});
			Property(x => x.ErrorMessage, m => m.Filterable(f => f.Caption("Name")));
			Property(x => x.TypeKey, m => m.Filterable(f => f.Definition(new TypeFilterDefinition(typeof(ServiceOrderType)))));
			Property(x => x.ResponsibleUser, m => m.Filterable(f => f.Definition(new UserFilterDefinition { WithGroups = true })));
			Property(x => x.CreateDate, c =>
			{
				c.Sortable();
				c.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = false, AllowPastDates = true }));
			});
		}
	}
}