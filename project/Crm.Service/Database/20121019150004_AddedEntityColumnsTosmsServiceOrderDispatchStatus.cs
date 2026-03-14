namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20121019150004)]
	public class AddedEntityColumnsToSmsServiceOrderDispatchStatus : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatchStatus", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "getDate()"));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatchStatus", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "getDate()"));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatchStatus", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatchStatus", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatchStatus", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
		public override void Down()
		{
			Database.RemoveColumnIfExisting("SMS.ServiceOrderDispatchStatus", "CreateDate");
			Database.RemoveColumnIfExisting("SMS.ServiceOrderDispatchStatus", "ModifyDate");
			Database.RemoveColumnIfExisting("SMS.ServiceOrderDispatchStatus", "CreateUser");
			Database.RemoveColumnIfExisting("SMS.ServiceOrderDispatchStatus", "ModifyUser");
			Database.RemoveColumnIfExisting("SMS.ServiceOrderDispatchStatus", "IsActive");
		}
	}
}