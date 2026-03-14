namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20190704130700)]
	public class AddLuDocumentCategory : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[DocumentCategory]"))
			{
				Database.AddTable(
					"LU.DocumentCategory",
					new Column("DocumentCategoryId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("OfflineRelevant", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
				Database.ExecuteNonQuery("INSERT INTO [LU].[DocumentCategory] ([Name], [Language], [Value]) VALUES ('Dokument', 'de', 'Document')");
				Database.ExecuteNonQuery("INSERT INTO [LU].[DocumentCategory] ([Name], [Language], [Value]) VALUES ('Document', 'en', 'Document')");
				Database.ExecuteNonQuery("INSERT INTO [LU].[DocumentCategory] ([Name], [Language], [Value]) VALUES ('Document', 'fr', 'Document')");
				Database.ExecuteNonQuery("INSERT INTO [LU].[DocumentCategory] ([Name], [Language], [Value]) VALUES ('Dokumentáció', 'hu', 'Document')");
			}

			Database.AddColumnIfNotExisting("[CRM].[DocumentAttributes]", new Column("DocumentCategoryKey", DbType.String, 50, ColumnProperty.NotNull, "'Document'"));
			Database.RemoveColumnIfExisting("[CRM].[DocumentAttributes]", "AttributeType");
		}
	}
}
