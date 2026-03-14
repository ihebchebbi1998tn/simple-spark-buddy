namespace Crm.Project.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160524092900)]
	public class AddIndexOrderNoToSmsServiceOrderTimes : Migration
	{
		public override void Up()
		{
			const string query = @"IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_ServiceOrderTimes_OrderNo')" +
													 "CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimes_OrderNo] ON [Sms].[ServiceOrderTimes] ([OrderNo] ASC)";

			Database.ExecuteNonQuery(query);
		}
	}
}