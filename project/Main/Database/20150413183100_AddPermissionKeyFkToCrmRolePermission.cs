namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413183100)]
	public class AddPermissionKeyFkToCrmRolePermission : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_RolePermission_Permission'") == 0)
			{
				Database.ExecuteNonQuery("DELETE rp FROM [CRM].[RolePermission] rp LEFT OUTER JOIN [CRM].[Permission] p ON rp.[PermissionKey] = p.[PermissionId] WHERE p.[PermissionId] IS NULL");
				Database.AddForeignKey("FK_RolePermission_Permission", "[CRM].[RolePermission]", "PermissionKey", "[CRM].[Permission]", "PermissionId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}