namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20151112154801)]
	public class ChangeContactsToGuidId : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Contact' AND COLUMN_NAME='ContactId' AND DATA_TYPE = 'int')
				BEGIN
					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Communication_ContactKey')
					BEGIN
						ALTER TABLE [CRM].[Communication] DROP CONSTRAINT [FK_Communication_ContactKey]
					END
					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_Contact')
					BEGIN
						ALTER TABLE [CRM].[Contact] DROP CONSTRAINT [PK_Contact]
					END
					ALTER TABLE [CRM].[Contact] ADD [ContactIdOld] INT NULL
					EXEC('UPDATE [CRM].[Contact] SET [ContactIdOld] = [ContactId]')
					ALTER TABLE [CRM].[Contact] DROP COLUMN [ContactId]
					ALTER TABLE [CRM].[Contact] ADD [ContactId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [CRM].[Contact] ADD CONSTRAINT [PK_Contact] PRIMARY KEY([ContactId])
				END");
		}
	}
}