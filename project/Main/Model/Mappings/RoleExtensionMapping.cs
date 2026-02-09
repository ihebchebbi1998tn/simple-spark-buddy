namespace Main.Model.Mappings
{
	using LMobile.Unicore;
	using LMobile.Unicore.NHibernate;

	using Main.Model.Extension;

	using NHibernate.Mapping.ByCode.Conformist;

	public class RoleExtensionMapping : ComponentMapping<PermissionSchemaRoleExtension>, INHibernateMappingExtension<PermissionSchemaRole, PermissionSchemaRoleExtension>
	{
		public RoleExtensionMapping()
		{
			Property(x => x.Description);
		}
	}
}
