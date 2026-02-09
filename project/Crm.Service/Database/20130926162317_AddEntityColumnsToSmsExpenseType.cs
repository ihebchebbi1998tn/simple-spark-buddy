namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130926162317)]
	public class AddEntityColumnsToSmsExpenseType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ExpenseType", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("SMS.ExpenseType", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("SMS.ExpenseType", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("SMS.ExpenseType", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("SMS.ExpenseType", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
		public override void Down()
		{
			Database.RemoveColumnIfExisting("SMS.ExpenseType", "CreateDate");
			Database.RemoveColumnIfExisting("SMS.ExpenseType", "ModifyDate");
			Database.RemoveColumnIfExisting("SMS.ExpenseType", "CreateUser");
			Database.RemoveColumnIfExisting("SMS.ExpenseType", "ModifyUser");
			Database.RemoveColumnIfExisting("SMS.ExpenseType", "IsActive");
		}
	}
}