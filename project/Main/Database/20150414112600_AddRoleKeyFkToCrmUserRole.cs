namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150414112600)]
	public class AddRoleKeyFkToCrmUserRole : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_UserRole_Role'") == 0)
			{
				Database.ExecuteNonQuery("DELETE ur FROM [CRM].[UserRole] ur LEFT OUTER JOIN [CRM].[Role] r ON ur.[RoleKey] = r.[RoleId] WHERE r.[RoleId] IS NULL");
				Database.AddForeignKey("FK_UserRole_Role", "[CRM].[UserRole]", "RoleKey", "[CRM].[Role]", "RoleId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}