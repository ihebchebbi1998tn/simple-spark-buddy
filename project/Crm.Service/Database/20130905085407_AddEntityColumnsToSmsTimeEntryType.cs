namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130905085407)]
	public class AddEntityColumnsToSmsTimeEntryType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.TimeEntryType", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("SMS.TimeEntryType", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("SMS.TimeEntryType", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("SMS.TimeEntryType", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("SMS.TimeEntryType", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
		public override void Down()
		{
			Database.RemoveColumnIfExisting("SMS.TimeEntryType", "CreateDate");
			Database.RemoveColumnIfExisting("SMS.TimeEntryType", "ModifyDate");
			Database.RemoveColumnIfExisting("SMS.TimeEntryType", "CreateUser");
			Database.RemoveColumnIfExisting("SMS.TimeEntryType", "ModifyUser");
			Database.RemoveColumnIfExisting("SMS.TimeEntryType", "IsActive");
		}
	}
}