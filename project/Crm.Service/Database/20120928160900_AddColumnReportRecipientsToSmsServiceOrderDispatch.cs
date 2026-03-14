namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120928160900)]
    public class AddColumnReportRecipientsToSmsServiceOrderDispatch : Migration
    {
        public override void Up()
        {
			if (!Database.ColumnExists("[SMS].[ServiceOrderDispatch]", "ReportRecipients"))
			{
				Database.AddColumn("[SMS].[ServiceOrderDispatch]", "ReportRecipients", DbType.String, 4000, ColumnProperty.Null);
			}
        }
        public override void Down()
		{
			if (Database.ColumnExists("[SMS].[ServiceOrderDispatch]", "ReportRecipients"))
			{
				Database.RemoveColumn("[SMS].[ServiceOrderDispatch]", "ReportRecipients");
			}
        }
    }
}