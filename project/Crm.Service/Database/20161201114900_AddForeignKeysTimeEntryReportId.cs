namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161201114900)]
	public class AddForeignKeysTimeEntryReportId : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sysobjects where name = 'FK_TimeEntry_TimeEntryReport'") == 0)
			{
				var query = new StringBuilder();

				query.AppendLine("ALTER TABLE [SMS].[TimeEntry] WITH NOCHECK ADD CONSTRAINT [FK_TimeEntry_TimeEntryReport] FOREIGN KEY([TimeEntryReportId])");
				query.AppendLine("REFERENCES [SMS].[TimeEntryReport] ([Id])");
				query.AppendLine("ALTER TABLE [SMS].[TimeEntry] CHECK CONSTRAINT [FK_TimeEntry_TimeEntryReport]");

				Database.ExecuteNonQuery(query.ToString());
			}
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sysobjects where name = 'FK_ServiceOrderTimePostings_TimeEntryReport'") == 0)
			{
				var query = new StringBuilder();

				query.AppendLine("ALTER TABLE [SMS].[ServiceOrderTimePostings] WITH NOCHECK ADD CONSTRAINT [FK_ServiceOrderTimePostings_TimeEntryReport] FOREIGN KEY([TimeEntryReportId])");
				query.AppendLine("REFERENCES [SMS].[TimeEntryReport] ([Id])");
				query.AppendLine("ALTER TABLE [SMS].[ServiceOrderTimePostings] CHECK CONSTRAINT [FK_ServiceOrderTimePostings_TimeEntryReport]");

				Database.ExecuteNonQuery(query.ToString());
			}
		}
	}
}