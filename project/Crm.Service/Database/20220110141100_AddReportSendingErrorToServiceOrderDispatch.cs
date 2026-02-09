namespace Crm.Service.Database
{
	using System.Data;
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20220110141100)]
	public class AddReportSendingErrorToServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatch", new Column("ReportSendingError", DbType.String, int.MaxValue, ColumnProperty.Null));
		}
	}
}