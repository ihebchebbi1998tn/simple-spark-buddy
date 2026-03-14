namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170124174300)]
	public class AddBusinessTitleLookup : Migration
	{
		public override void Up()
		{
			const string tableName = "[LU].[BusinessTitle]";
			if (Database.TableExists(tableName))
			{
				return;
			}
			Database.AddTable(tableName,
				new Column("BusinessTitleId", DbType.Int32, ColumnProperty.Identity),
				new Column("Name", DbType.String, ColumnProperty.NotNull),
				new Column("Language", DbType.StringFixedLength, 2, ColumnProperty.NotNull),
				new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
				new Column("SortOrder", DbType.Int32, ColumnProperty.Null),
				new Column("Value", DbType.String, ColumnProperty.NotNull),
				new Column("TenantKey", DbType.Int32, ColumnProperty.Null),
				new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
				new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
				new Column("CreateUser", DbType.String, ColumnProperty.NotNull),
				new Column("ModifyUser", DbType.String, ColumnProperty.NotNull),
				new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, 1)
				);
		}
	}
}
