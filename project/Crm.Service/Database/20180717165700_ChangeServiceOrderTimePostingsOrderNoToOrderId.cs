namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180717165700)]
	public class ChangeServiceOrderTimePostingsOrderNoToOrderId : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderTimePostings", "OrderId") && Database.ColumnExists("SMS.ServiceOrderTimePostings", "OrderNo"))
			{
				Database.AddColumn("SMS.ServiceOrderTimePostings", new Column("OrderId", DbType.Guid, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderTimePostings SET OrderId = ContactKey FROM SMS.ServiceOrderTimePostings JOIN SMS.ServiceOrderHead ON SMS.ServiceOrderHead.OrderNo = SMS.ServiceOrderTimePostings.OrderNo");
				Database.ExecuteNonQuery("DELETE FROM SMS.ServiceOrderTimePostings WHERE OrderId IS NULL");
				Database.ChangeColumn("SMS.ServiceOrderTimePostings", new Column("OrderId", DbType.Guid, ColumnProperty.NotNull));
				Database.RemoveColumn("SMS.ServiceOrderTimePostings", "OrderNo");
				Database.AddForeignKey("FK_ServiceOrderTimePostings_ServiceOrderHead", "SMS.ServiceOrderTimePostings", "OrderId", "CRM.Contact", "ContactId");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimePostings_OrderId] ON [SMS].[ServiceOrderTimePostings] ([OrderId] ASC)");
			}
		}
	}
}