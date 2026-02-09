namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180411180099)]
	public class RemoveIndexOrderItemOrderKeyIsActive : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('[CRM].[OrderItem]') AND name = 'IX_OrderItem_OrderKey_IsActive')
				BEGIN
					DROP INDEX IX_OrderItem_OrderKey_IsActive ON CRM.OrderItem
				END");
		}
	}
}
