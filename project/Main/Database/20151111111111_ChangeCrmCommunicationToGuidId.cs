namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20151111111111)]
	public class ChangeCrmCommunicationToGuidId : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'CRM' AND TABLE_NAME = 'Communication' AND COLUMN_NAME = 'CommunicationId' AND DATA_TYPE = 'uniqueidentifier'") == 0)
			{
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Communication] DROP CONSTRAINT [PK_Communication]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Communication] ADD [CommunicationIdOld] INT NULL");
				Database.ExecuteNonQuery("UPDATE [CRM].[Communication] SET [CommunicationIdOld] = [CommunicationId]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Communication] DROP COLUMN [CommunicationId]");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Communication] ADD [CommunicationId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())");
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Communication] ADD CONSTRAINT [PK_Communication] PRIMARY KEY([CommunicationId])");
			}
		}
	}
}