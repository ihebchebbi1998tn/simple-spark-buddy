namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180717155800)]
	public class ChangeServiceOrderMaterialOrderNoToOrderId : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderMaterial", "OrderId") && Database.ColumnExists("SMS.ServiceOrderMaterial", "OrderNo"))
			{
				Database.AddColumn("SMS.ServiceOrderMaterial", new Column("OrderId", DbType.Guid, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderMaterial SET OrderId = ContactKey FROM SMS.ServiceOrderMaterial JOIN SMS.ServiceOrderHead ON SMS.ServiceOrderHead.OrderNo = SMS.ServiceOrderMaterial.OrderNo");
				Database.ExecuteNonQuery("DELETE FROM SMS.ServiceOrderMaterial WHERE OrderId IS NULL");
				Database.ChangeColumn("SMS.ServiceOrderMaterial", new Column("OrderId", DbType.Guid, ColumnProperty.NotNull));
				if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SMS].[ServiceOrderMaterial]') AND name = N'IX_ServiceOrderMaterial'") == 1)
				{
					Database.ExecuteNonQuery("DROP INDEX [IX_ServiceOrderMaterial] ON [SMS].[ServiceOrderMaterial]");
				}
				Database.RemoveColumn("SMS.ServiceOrderMaterial", "OrderNo");
				Database.AddForeignKey("FK_ServiceOrderMaterial_ServiceOrderHead", "SMS.ServiceOrderMaterial", "OrderId", "CRM.Contact", "ContactId");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ServiceOrderMaterial_OrderId] ON [SMS].[ServiceOrderMaterial] ([OrderId] ASC)");
			}
		}
	}
}