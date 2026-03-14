namespace Crm.Project.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160524093700)]
	public class AddIndexMaintenancePlanIdToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			const string query = @"IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_ServiceOrderHead_MaintenancePlanId')" +
													 "CREATE NONCLUSTERED INDEX [IX_ServiceOrderHead_MaintenancePlanId] ON [Sms].[ServiceOrderHead] ([MaintenancePlanId] ASC)";

			Database.ExecuteNonQuery(query);
		}
	}
}