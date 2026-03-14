namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180626130000)]
	public class AddColumnReportRecipientsSentToDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatch", new Column("ReportRecipientsSentInternal", DbType.String, 4000, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatch", new Column("ReportRecipientsSentExternal", DbType.String, 4000, ColumnProperty.Null));
		}
	}
}