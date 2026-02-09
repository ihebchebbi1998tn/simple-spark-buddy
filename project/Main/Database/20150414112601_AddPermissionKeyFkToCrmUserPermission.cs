namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150414112601)]
	public class AddPermissionKeyFkToCrmUserPermission : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_UserPermission_Permission'") == 0)
			{
				Database.ExecuteNonQuery("DELETE up FROM [CRM].[UserPermission] up LEFT OUTER JOIN [CRM].[Permission] p ON up.[PermissionKey] = p.[PermissionId] WHERE p.[PermissionId] IS NULL");
				Database.AddForeignKey("FK_UserPermission_Permission", "[CRM].[UserPermission]", "PermissionKey", "[CRM].[Permission]", "PermissionId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}