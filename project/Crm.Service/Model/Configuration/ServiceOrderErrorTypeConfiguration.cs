namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
		using Crm.Service.Model.Lookup;

		public class ServiceOrderErrorTypeConfiguration : EntityConfiguration<ServiceOrderErrorType>
	{
		public override void Initialize()
		{
			Property(x => x.StatisticsKeyFaultImageKey, m => m.Filterable(f => f.Definition(new TypeFilterDefinition(typeof(StatisticsKeyFaultImage)))));
			Property(x => x.IsSuspected, m => m.Filterable());
			Property(x => x.IsConfirmed, m => m.Filterable());
			Property(x => x.IsMainErrorType, m => m.Filterable());
			Property(x => x.Description, m => m.Filterable());
			Property(x => x.InternalRemark, m => m.Filterable());
		}

		public ServiceOrderErrorTypeConfiguration(IEntityConfigurationHolder<ServiceOrderErrorType> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
