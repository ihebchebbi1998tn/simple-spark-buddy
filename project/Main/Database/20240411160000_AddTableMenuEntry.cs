namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240411160000)]
	public class AddTableMenuEntry : Migration
	{
		public override void Up()
		{
			const string TableName = "Crm.MenuEntry";
			if (!Database.TableExists(TableName))
			{
				Database.AddTable(
					TableName,
					new[]
					{
						new Column("MenuEntryId",
							DbType.Guid,
							ColumnProperty.PrimaryKey),
						new Column("Title",
							DbType.String,
							ColumnProperty.Null),
						new Column("Category",
							DbType.String,
							ColumnProperty.Null),
						new Column("Priority",
							DbType.Int32,
							ColumnProperty.NotNull,
							0),
						new Column("CreateUser",
							DbType.String,
							ColumnProperty.NotNull),
						new Column("ModifyUser",
							DbType.String,
							ColumnProperty.NotNull),
						new Column("CreateDate",
							DbType.DateTime,
							ColumnProperty.NotNull,
							"GETUTCDATE()"),
						new Column("ModifyDate",
							DbType.DateTime,
							ColumnProperty.NotNull,
							"GETUTCDATE()"),
					}
				);
			}
		}
	}
}
