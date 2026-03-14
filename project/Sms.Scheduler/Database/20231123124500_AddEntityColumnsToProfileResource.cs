namespace Sms.Scheduler.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20231123124500)]
	public class AddedEntityColumnsToProfileResource : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("RPL.ProfileResource", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("RPL.ProfileResource", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("RPL.ProfileResource", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("RPL.ProfileResource", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("RPL.ProfileResource", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
	}
}
