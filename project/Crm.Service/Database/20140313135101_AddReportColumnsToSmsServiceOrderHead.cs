namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140313135101)]
	public class AddReportColumnsToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("ReportSent", DbType.Boolean, ColumnProperty.NotNull, false));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("ReportSendingRetries", DbType.Int32, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("ReportSendingDetails", DbType.String, Int32.MaxValue, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("ReportSaved", DbType.Boolean, ColumnProperty.NotNull, false));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("ReportSavingRetries", DbType.Int32, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("ReportSavingDetails", DbType.String, Int32.MaxValue, ColumnProperty.Null));
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderHead] SET ReportSent = 1 WHERE Status = 'Closed'");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderHead] SET ReportSaved = 1 WHERE Status = 'Closed'");
		}

		public override void Down()
		{
		}
	}
}