namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class ReplenishmentOrderItemConfiguration : EntityConfiguration<ReplenishmentOrderItem>
	{
		public override void Initialize()
		{
			Property(x => x.MaterialNo, m => {
				m.Filterable();
				m.Sortable();
			});
			Property(x => x.Description, m => {
				m.Filterable();
				m.Sortable();
			});
			Property(x => x.Remark, m => m.Filterable());
			Property(x => x.Quantity, m => m.Sortable());
		}
		public ReplenishmentOrderItemConfiguration(IEntityConfigurationHolder<ReplenishmentOrderItem> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
