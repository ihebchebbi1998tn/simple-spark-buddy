namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180820124900)]
	public class NotNullableTimeEntryReportDates : Migration
	{
		public override void Up()
		{
			var timeManagementTableExists = Database.TableExists("[SMS].[TimeManagementEvent]");
			var deleteEmptyReportQuery = "DELETE [SMS].[TimeEntryReport] FROM [SMS].[TimeEntryReport]" +
			                             " LEFT OUTER JOIN [SMS].[ServiceOrderTimePostings] ON [SMS].[TimeEntryReport].[Id] = [SMS].[ServiceOrderTimePostings].[TimeEntryReportId]" +
			                             " LEFT OUTER JOIN [SMS].[TimeEntry] ON [SMS].[TimeEntryReport].[Id] = [SMS].[TimeEntry].[TimeEntryReportId]";
			if (timeManagementTableExists)
			{
				deleteEmptyReportQuery += " LEFT OUTER JOIN [SMS].[TimeManagementEvent] ON [SMS].[TimeEntryReport].[Id] = [SMS].[TimeManagementEvent].[TimeEntryReportId]";
			}

			deleteEmptyReportQuery += " WHERE [SMS].[TimeEntry].[TimeEntryReportId] IS NULL AND [SMS].[ServiceOrderTimePostings].[TimeEntryReportId] IS NULL";
			if (timeManagementTableExists)
			{
				deleteEmptyReportQuery += " AND [SMS].[TimeManagementEvent].[TimeEntryReportId] IS NULL";
			}

			Database.ExecuteNonQuery(deleteEmptyReportQuery);
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeEntryReport] ALTER COLUMN [From] datetime not null");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeEntryReport] ALTER COLUMN [To] datetime not null");
		}
	}
}
