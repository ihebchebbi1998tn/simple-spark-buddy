namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130905130747)]
	public class ChangeSmsTimeEntryToHighLowIds : Migration
	{
		private const int Low = 32;

		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeEntry] DROP CONSTRAINT PK_TimeEntry");
			Database.RenameColumn("[SMS].[TimeEntry]", "TimeEntryId", "Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeEntry] ADD TimeEntryId bigint NULL");
			Database.ExecuteNonQuery("UPDATE [SMS].[TimeEntry] SET TimeEntryId = Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeEntry] ALTER COLUMN TimeEntryId bigint NOT NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeEntry] DROP COLUMN Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeEntry] ADD CONSTRAINT PK_TimeEntry PRIMARY KEY(TimeEntryId)");

			Database.ExecuteNonQuery("BEGIN IF ((SELECT COUNT(*) FROM dbo.hibernate_unique_key WHERE tablename = '[SMS].[TimeEntry]') = 0) INSERT INTO hibernate_unique_key (next_hi, tablename) values ((select (COALESCE(max(TimeEntryId), 0) / " + Low + ") + 1 from [SMS].[TimeEntry] where TimeEntryId is not null), '[SMS].[TimeEntry]') END");
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}