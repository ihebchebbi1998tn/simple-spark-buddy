namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160517151100)]
	public class AddIndexToSmsServiceOrderTimes : Migration
	{
		public override void Up()
		{
			const string query = @"IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_ServiceOrderTimes_ItemNo')" +
													 "CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimes_ItemNo] ON [SMS].[ServiceOrderTimes] ([ItemNo]) INCLUDE ([id])";

			Database.ExecuteNonQuery(query);
		}
	}
}