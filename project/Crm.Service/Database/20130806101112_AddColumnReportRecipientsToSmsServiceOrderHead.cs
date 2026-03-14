namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130806101112)]
	public class AddColumnReportRecipientsToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("ReportRecipients", DbType.String, 4000, ColumnProperty.Null));
		}
	}
}