namespace Crm.Service.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class InstallationPosConfiguration : EntityConfiguration<InstallationPos>
	{
		public override void Initialize()
		{
			Property(x => x.Supplier, c => c.Filterable());
			Property(x => x.ItemNo, c => c.Filterable());
		}
		public InstallationPosConfiguration(IEntityConfigurationHolder<InstallationPos> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
