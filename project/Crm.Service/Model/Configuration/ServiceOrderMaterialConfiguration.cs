namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class ServiceOrderMaterialConfiguration : EntityConfiguration<ServiceOrderMaterial>
	{
		public override void Initialize()
		{
			Property(x => x.ItemNo, m => m.Filterable());
			Property(x => x.ItemDescription, m => m.Filterable(f => f.Caption("Article")));
			Property(x => x.Description, m => m.Filterable());
			Property(x => x.InternalRemark, m => m.Filterable());
		}

		public ServiceOrderMaterialConfiguration(IEntityConfigurationHolder<ServiceOrderMaterial> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
