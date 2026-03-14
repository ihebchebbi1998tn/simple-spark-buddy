namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180717181100)]
	public class ChangeLocationStoreNoToStoreId : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.Location", "StoreId") && Database.ColumnExists("SMS.Location", "StoreNo"))
			{
				Database.AddColumn("SMS.Location", new Column("StoreId", DbType.Guid, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE SMS.Location SET SMS.Location.StoreId = SMS.Store.StoreId FROM SMS.Location JOIN SMS.Store ON SMS.Store.StoreNo = SMS.Location.StoreNo");
				Database.ExecuteNonQuery("DELETE FROM SMS.Location WHERE StoreId IS NULL");
				Database.ChangeColumn("SMS.Location", new Column("StoreId", DbType.Guid, ColumnProperty.NotNull));
				Database.RemoveColumn("SMS.Location", "StoreNo");
				Database.AddForeignKey("FK_Location_Store", "SMS.Location", "StoreId", "SMS.Store", "StoreId");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_Location_StoreId] ON [SMS].[Location] ([StoreId] ASC)");
			}
		}
	}
}