namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160623164400)]
	public class ChangeTimeEntryIdToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_TimeEntry')
					BEGIN
						ALTER TABLE [SMS].[TimeEntry] DROP CONSTRAINT [PK_TimeEntry]
					END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='TimeEntry' AND COLUMN_NAME='TimeEntryId' AND DATA_TYPE like '%int')
				BEGIN
					ALTER TABLE [SMS].[TimeEntry] ADD [TimeEntryIdOld] bigint NULL
					EXEC('UPDATE [SMS].[TimeEntry] SET [TimeEntryIdOld] = [TimeEntryId]')
					ALTER TABLE [SMS].[TimeEntry] DROP COLUMN [TimeEntryId]
					ALTER TABLE [SMS].[TimeEntry] ADD [TimeEntryId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [SMS].[TimeEntry] ADD  CONSTRAINT [PK_TimeEntry] PRIMARY KEY CLUSTERED ([TimeEntryId] ASC)
				END");
		}
	}
}