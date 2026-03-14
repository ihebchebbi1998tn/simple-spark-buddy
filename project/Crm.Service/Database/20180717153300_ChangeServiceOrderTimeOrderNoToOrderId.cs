namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180717153300)]
	public class ChangeServiceOrderTimeOrderNoToOrderId : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderTimes", "OrderId") && Database.ColumnExists("SMS.ServiceOrderTimes", "OrderNo"))
			{
				Database.AddColumn("SMS.ServiceOrderTimes", new Column("OrderId", DbType.Guid, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderTimes SET OrderId = ContactKey FROM SMS.ServiceOrderTimes JOIN SMS.ServiceOrderHead ON SMS.ServiceOrderHead.OrderNo = SMS.ServiceOrderTimes.OrderNo");
				Database.ExecuteNonQuery("DELETE FROM SMS.ServiceOrderTimes WHERE OrderId IS NULL");
				Database.ChangeColumn("SMS.ServiceOrderTimes", new Column("OrderId", DbType.Guid, ColumnProperty.NotNull));
				Database.RemoveForeignKey("SMS.ServiceOrderTimes", "FK_ServiceOrderTimes_ServiceOrderHead");
				if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderTimes]') AND name = N'IX_ServiceOrderTimes_OrderNo'") == 1)
				{
					Database.ExecuteNonQuery("DROP INDEX [IX_ServiceOrderTimes_OrderNo] ON [SMS].[ServiceOrderTimes]");
				}
				Database.RemoveColumn("SMS.ServiceOrderTimes", "OrderNo");
				Database.AddForeignKey("FK_ServiceOrderTimes_ServiceOrderHead", "SMS.ServiceOrderTimes", "OrderId", "CRM.Contact", "ContactId");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimes_OrderId] ON [SMS].[ServiceOrderTimes] ([OrderId] ASC)");
			}
		}
	}
}