namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180213130000)]
	public class AddTableEntityType : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[dbo].[EntityType]"))
			{
				Database.AddTable(
					"dbo.EntityType",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("CreatedBy", DbType.String, 256, ColumnProperty.Null, "'System'"),
					new Column("CreatedAt", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifiedBy", DbType.String, 256, ColumnProperty.Null, "'System'"),
					new Column("ModifiedAt", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("IsDeleted", DbType.Boolean, ColumnProperty.Null, false),
					new Column("DeletedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("Name", DbType.String, 256, ColumnProperty.NotNull)
				);
				Database.ExecuteNonQuery("CREATE UNIQUE INDEX [IX_UQ_ActiveEntityType] ON dbo.EntityType (Name, IsDeleted) WHERE IsDeleted = 0");
			}
		}
	}
}