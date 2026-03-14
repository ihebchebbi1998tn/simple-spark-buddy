namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404161103)]
	public class AddIndexForSmsServiceOrderDispatchOrderId : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderDispatch]') AND name = N'IX_OrderId'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_OrderId] ON [SMS].[ServiceOrderDispatch]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_OrderId] ON [SMS].[ServiceOrderDispatch] ([OrderId])");
		}
	}
}