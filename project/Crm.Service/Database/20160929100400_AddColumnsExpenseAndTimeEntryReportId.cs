namespace Crm.Service.Database
{
  using System.Data;

  using Crm.Library.Data.MigratorDotNet.Framework;
  using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

  [Migration(20160929100400)]
  public class AddColumnsExpenseAndTimeEntryReportId : Migration
  {
    public override void Up()
    {
      Database.AddColumnIfNotExisting("[SMS].[Expense]", new Column("ExpenseReportId", DbType.Guid, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[TimeEntry]", new Column("TimeEntryReportId", DbType.Guid, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimePostings]", new Column("TimeEntryReportId", DbType.Guid, ColumnProperty.Null));
		}

    public override void Down()
    {
		}
  }
}