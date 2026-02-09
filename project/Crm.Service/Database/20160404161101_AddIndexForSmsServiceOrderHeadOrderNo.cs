namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404161101)]
	public class AddIndexForSmsServiceOrderHeadOrderNo : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderHead]') AND name = N'IX_OrderNo'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_OrderNo] ON [SMS].[ServiceOrderHead]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_OrderNo] ON [SMS].[ServiceOrderHead] ([OrderNo]) INCLUDE ([ContactKey])");
		}
	}
}