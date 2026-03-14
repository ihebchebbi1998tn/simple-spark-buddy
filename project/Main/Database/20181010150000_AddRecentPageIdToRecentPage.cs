namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20181010150000)]
	public class AddRecentPageIdToRecentPage : Migration
	{
		public override void Up()
		{

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='UserRecentPages' AND COLUMN_NAME='Id' AND DATA_TYPE = 'int')
				BEGIN
					ALTER TABLE [CRM].[UserRecentPages] drop CONSTRAINT [PK__UserRecentPages] 
					ALTER TABLE [CRM].[UserRecentPages] drop CONSTRAINT [UK_Url_Username] 
					ALTER TABLE [CRM].[UserRecentPages] DROP COLUMN [Id]
					ALTER TABLE [CRM].[UserRecentPages] ADD [Id] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [CRM].[UserRecentPages] ADD CONSTRAINT [PK__UserRecentPages] PRIMARY KEY CLUSTERED ([Id] ASC)
				END");

		}
	}
}
