namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220714120000)]
	public class CreateDeviceTable : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[CRM].[Device]"))
			{
				Database.AddTable(
					"[CRM].[Device]",
					new Column("DeviceId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Fingerprint", DbType.String, ColumnProperty.NotNull),
					new Column("Token", DbType.String, ColumnProperty.Null),
					new Column("Username", DbType.String, ColumnProperty.NotNull),
					new Column("DeviceInfo", DbType.String, ColumnProperty.NotNull),
					new Column("IsTrusted", DbType.Boolean, ColumnProperty.NotNull),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'")
				);
			}
		}
	}
}
