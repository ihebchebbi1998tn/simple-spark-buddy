namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20121019150005)]
	public class AddedEntityColumnsToSmsInstallationHeadStatus : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.InstallationHeadStatus", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "getDate()"));
			Database.AddColumnIfNotExisting("SMS.InstallationHeadStatus", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "getDate()"));
			Database.AddColumnIfNotExisting("SMS.InstallationHeadStatus", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("SMS.InstallationHeadStatus", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("SMS.InstallationHeadStatus", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
		public override void Down()
		{
			Database.RemoveColumnIfExisting("SMS.InstallationHeadStatus", "CreateDate");
			Database.RemoveColumnIfExisting("SMS.InstallationHeadStatus", "ModifyDate");
			Database.RemoveColumnIfExisting("SMS.InstallationHeadStatus", "CreateUser");
			Database.RemoveColumnIfExisting("SMS.InstallationHeadStatus", "ModifyUser");
			Database.RemoveColumnIfExisting("SMS.InstallationHeadStatus", "IsActive");
		}
	}
}