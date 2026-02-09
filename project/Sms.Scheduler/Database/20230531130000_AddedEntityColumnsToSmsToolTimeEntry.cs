namespace Sms.Scheduler.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230531130000)]
	public class AddedEntityColumnsToSmsToolTimeEntry : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ToolTimeEntry", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("SMS.ToolTimeEntry", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("SMS.ToolTimeEntry", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("SMS.ToolTimeEntry", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("SMS.ToolTimeEntry", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
		public override void Down()
		{
			Database.RemoveColumnIfExisting("SMS.ToolTimeEntry", "CreateDate");
			Database.RemoveColumnIfExisting("SMS.ToolTimeEntry", "ModifyDate");
			Database.RemoveColumnIfExisting("SMS.ToolTimeEntry", "CreateUser");
			Database.RemoveColumnIfExisting("SMS.ToolTimeEntry", "ModifyUser");
			Database.RemoveColumnIfExisting("SMS.ToolTimeEntry", "IsActive");
		}
	}
}
