namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Unicore;

	using LMobile.Unicore;

	[Migration(20180109130000)]
	public class AddTableDomain : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[dbo].[Domain]"))
			{
				Database.AddTable(
					"dbo.Domain",
					new Column("UId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Version", DbType.Int64, ColumnProperty.NotNull, 1),
					new Column("CreatedBy", DbType.String, 256, ColumnProperty.Null, "'System'"),
					new Column("CreatedAt", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifiedBy", DbType.String, 256, ColumnProperty.Null, "'System'"),
					new Column("ModifiedAt", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("IsDeleted", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("DeletedAt", DbType.DateTime, ColumnProperty.Null),
					new Column("Name", DbType.String, 256, ColumnProperty.NotNull),
					new Column("Type", DbType.Int32, ColumnProperty.NotNull, (int)DomainType.Normal),
					new Column("MasterId", DbType.Guid, ColumnProperty.Null),
					new Column("LegacyId", DbType.String, 50, ColumnProperty.Null),
					new Column("LegacyVersion", DbType.Int64, ColumnProperty.Null),
					new Column("TenantKeyOld", DbType.Int32, ColumnProperty.Null)
				);
				Database.AddForeignKey("FK_Domain_MasterId", "[dbo].[Domain]", "MasterId", "[dbo].[Domain]", "UId");
				Database.ExecuteNonQuery($"	INSERT INTO Domain (UId, Name) VALUES ('{UnicoreDefaults.CommonDomainId}', 'Common')");
			}
		}
	}
}