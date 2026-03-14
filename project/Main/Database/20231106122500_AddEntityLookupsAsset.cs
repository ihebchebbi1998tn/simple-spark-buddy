namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	using Main.Model.Lookups;

	[Migration(20231106122500)]
	public class AddEntityLookupsAsset : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			Database.AddTable("[LU].[Asset]",
				new Column($"AssetId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
				new Column("Name", DbType.String, ColumnProperty.NotNull),
				new Column("Language", DbType.StringFixedLength, 2, ColumnProperty.NotNull),
				new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
				new Column("SortOrder", DbType.Int32, ColumnProperty.Null),
				new Column("Value", DbType.String, ColumnProperty.NotNull),
				new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
				new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
				new Column("CreateUser", DbType.String, ColumnProperty.NotNull),
				new Column("ModifyUser", DbType.String, ColumnProperty.NotNull),
				new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, 1)
			);
			helper.AddOrUpdateEntityAuthDataColumn<Asset>("LU", "Asset");
		}
	}
}
