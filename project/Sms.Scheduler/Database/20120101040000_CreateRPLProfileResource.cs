namespace Sms.Scheduler.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120101040000)]
	public class CreateRPLProfileResource : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[RPL].[ProfileResource]"))
			{
				Database.AddTable("[RPL].[ProfileResource]",
					new Column("Id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("ProfileKey", DbType.Int32, ColumnProperty.Null),
					new Column("ResourceKey", DbType.String, 256, ColumnProperty.Null),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "getutcdate()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "getutcdate()"),
					new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
			}
		}
	}
}
