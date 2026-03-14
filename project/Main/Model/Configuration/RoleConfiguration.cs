namespace Main.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	using LMobile.Unicore;

	public class RoleConfiguration : EntityConfiguration<PermissionSchemaRole>
	{
		public override void Initialize()
		{
			Property(x => x.Name, c =>
			{
				c.Sortable();
				c.Filterable();
			});
		}
		public RoleConfiguration(IEntityConfigurationHolder<PermissionSchemaRole> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
