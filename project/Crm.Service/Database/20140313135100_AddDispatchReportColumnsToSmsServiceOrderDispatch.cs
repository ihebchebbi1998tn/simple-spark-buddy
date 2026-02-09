namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140313135100)]
	public class AddDispatchReportColumnsToSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("ReportSent", DbType.Boolean, ColumnProperty.NotNull, false));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("ReportSendingRetries", DbType.Int32, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("ReportSendingDetails", DbType.String, Int32.MaxValue, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("ReportSaved", DbType.Boolean, ColumnProperty.NotNull, false));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("ReportSavingRetries", DbType.Int32, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("ReportSavingDetails", DbType.String, Int32.MaxValue, ColumnProperty.Null));
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderDispatch] SET ReportSent = 1 WHERE Status IN ('ClosedComplete', 'ClosedNotComplete')");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderDispatch] SET ReportSaved = 1 WHERE Status IN ('ClosedComplete', 'ClosedNotComplete')");
		}

		public override void Down()
		{
		}
	}
}