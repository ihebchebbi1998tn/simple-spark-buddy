namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150414112603)]
	public class AddTenantKeyFkToCrmUserTenant : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_UserTenant_Tenant'") == 0)
			{
				Database.ExecuteNonQuery("DELETE ut FROM [CRM].[UserTenant] ut LEFT OUTER JOIN [CRM].[Tenant] t ON ut.[TenantKey] = t.[TenantId] WHERE t.[TenantId] IS NULL");
				Database.AddForeignKey("FK_UserTenant_Tenant", "[CRM].[UserTenant]", "TenantKey", "[CRM].[Tenant]", "TenantId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}