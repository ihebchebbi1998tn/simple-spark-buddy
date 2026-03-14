namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20201013140300)]
	public class AddEntityLookupsBranch : Migration
	{
		public override void Up()
		{
			Database.AddTable("[LU].[Branch1]",
				new Column($"Branch1Id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
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
			for (int i = 2; i <= 4; i++)
			{
				string tableName = $"[LU].[Branch{i}]";
				if (!Database.TableExists(tableName))
				{
					Database.AddTable(tableName,
						new Column($"Branch{i}Id", DbType.Int32, ColumnProperty.Identity),
						new Column("Name", DbType.String, ColumnProperty.NotNull),
						new Column("ParentName", DbType.String, ColumnProperty.Null),
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
				}
			}
		}
	}
}
