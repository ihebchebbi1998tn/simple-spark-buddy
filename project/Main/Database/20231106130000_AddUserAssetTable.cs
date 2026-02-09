namespace Main.Database
{
	using System.Data;
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	using Main.Model;

	[Migration(20231106130000)]
	public class AddUserAssetTable : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			if (!Database.TableExists("[CRM].[UserAsset]"))
			{
				Database.AddTable(
					"[CRM].[UserAsset]",
					new Column("Id", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("AssetKey", DbType.String, ColumnProperty.NotNull),
					new Column("Username", DbType.String, ColumnProperty.NotNull),
					new Column("ValidTo", DbType.DateTime, ColumnProperty.Null),
					new Column("DaysToNotifyBeforeExpiration", DbType.Int32, ColumnProperty.Null),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
				helper.AddOrUpdateEntityAuthDataColumn<UserAsset>("CRM", "UserAsset", "Id");
			} 
		}
	}
}
