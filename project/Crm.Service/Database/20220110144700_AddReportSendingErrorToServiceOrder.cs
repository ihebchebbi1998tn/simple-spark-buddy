namespace Crm.Service.Database
{
	using System.Data;
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20220110144700)]
	public class AddReportSendingErrorToServiceOrder : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead", new Column("ReportSendingError", DbType.String, int.MaxValue, ColumnProperty.Null));
		}
	}
}