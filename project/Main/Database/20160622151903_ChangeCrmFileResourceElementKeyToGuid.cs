namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160622151903)]
	public class ChangeCrmFileResourceElementKeyToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_DocumentAttributes_FileResource')
					BEGIN
						ALTER TABLE [CRM].[DocumentAttributes] DROP CONSTRAINT [FK_DocumentAttributes_FileResource]
					END

				IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_DocumentAttributes_DocumentAttributes')
					BEGIN
						ALTER TABLE [CRM].[DocumentAttributes] DROP CONSTRAINT [FK_DocumentAttributes_DocumentAttributes]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_SMS.DocumentAttributes')
					BEGIN
						ALTER TABLE [CRM].[DocumentAttributes] DROP CONSTRAINT [PK_SMS.DocumentAttributes]
					END
			
			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Expense_FileResource')
					BEGIN
						ALTER TABLE [SMS].[Expense] DROP CONSTRAINT [FK_Expense_FileResource]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_Expense')
					BEGIN
						ALTER TABLE [SMS].[Expense] DROP CONSTRAINT [PK_Expense]
					END
		
			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_File')
					BEGIN
						ALTER TABLE [CRM].[FileResource] DROP CONSTRAINT [PK_File]
					END

			IF EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_FileResource_ElementKey')
					BEGIN
						DROP INDEX [IX_FileResource_ElementKey] ON [CRM].[FileResource]
					END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='FileResource' AND COLUMN_NAME='Id' AND DATA_TYPE like '%int')
				BEGIN
					ALTER TABLE [CRM].[FileResource] ADD [IdOld] bigint NULL
					EXEC('UPDATE [CRM].[FileResource] SET [IdOld] = [Id]')
					ALTER TABLE [CRM].[FileResource] DROP COLUMN [Id]
					ALTER TABLE [CRM].[FileResource] ADD [Id] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [CRM].[FileResource] ADD  CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED ([Id] ASC)
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='FileResource' AND COLUMN_NAME='ElementKey' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[CRM].[FileResource].[ElementKey]', 'ElementKeyOld', 'COLUMN'					
					ALTER TABLE [CRM].[FileResource] ADD [ElementKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ElementKey] = b.[NoteId] FROM [Crm].[FileResource] a LEFT OUTER JOIN [Crm].[Note] b ON a.[ElementKeyOld] = b.[NoteIdOld]')
					ALTER TABLE [CRM].[FileResource] ALTER COLUMN [ElementKeyOld] bigint NULL
					CREATE NONCLUSTERED INDEX [IX_FileResource_ElementKey] ON [CRM].[FileResource] ([ElementKey] ASC)
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DocumentAttributes' AND COLUMN_NAME='Id' AND DATA_TYPE like '%int')
				BEGIN
					ALTER TABLE [CRM].[DocumentAttributes] ADD [IdOld] bigint NULL
					EXEC('UPDATE [CRM].[DocumentAttributes] SET [IdOld] = [Id]')
					ALTER TABLE [CRM].[DocumentAttributes] DROP COLUMN [Id]
					ALTER TABLE [CRM].[DocumentAttributes] ADD [Id] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [CRM].[DocumentAttributes] ADD CONSTRAINT [PK_SMS.DocumentAttributes] PRIMARY KEY CLUSTERED ([Id] ASC)
					ALTER TABLE [CRM].[DocumentAttributes] ADD CONSTRAINT [FK_DocumentAttributes_DocumentAttributes] FOREIGN KEY([Id]) REFERENCES [CRM].[DocumentAttributes] ([Id])
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DocumentAttributes' AND COLUMN_NAME='FileResourceKey' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[CRM].[DocumentAttributes].[FileResourceKey]', 'FileResourceKeyOld', 'COLUMN'					
					ALTER TABLE [CRM].[DocumentAttributes] ADD [FileResourceKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[FileResourceKey] = b.[Id] FROM [CRM].[DocumentAttributes] a LEFT OUTER JOIN [CRM].[FileResource] b ON a.[FileResourceKeyOld] = b.[IdOld]')
					ALTER TABLE [CRM].[DocumentAttributes] ALTER COLUMN [FileResourceKeyOld] bigint NULL
					ALTER TABLE[CRM].[DocumentAttributes]	ADD CONSTRAINT[FK_DocumentAttributes_FileResource] FOREIGN KEY([FileResourceKey]) REFERENCES[CRM].[FileResource]([Id]) ON UPDATE SET NULL ON DELETE SET NULL
				END");

			Database.ExecuteNonQuery(@"
				IF NOT EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_FileResource_Note')
				BEGIN
					ALTER TABLE [CRM].[FileResource]  WITH NOCHECK ADD  CONSTRAINT [FK_FileResource_Note] FOREIGN KEY([ElementKey]) REFERENCES [CRM].[Note] ([NoteId])
				END
			");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='Expense' AND COLUMN_NAME='ExpenseId' AND DATA_TYPE like '%int')
				BEGIN
					ALTER TABLE [SMS].[Expense] ADD [ExpenseIdOld] bigint NULL
					EXEC('UPDATE [SMS].[Expense] SET [ExpenseIdOld] = [ExpenseId]')
					ALTER TABLE [SMS].[Expense] DROP COLUMN [ExpenseId]
					ALTER TABLE [SMS].[Expense] ADD [ExpenseId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [SMS].[Expense] ADD  CONSTRAINT [PK_Expense] PRIMARY KEY CLUSTERED ([ExpenseId] ASC)
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='Expense' AND COLUMN_NAME='FileResourceKey' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[SMS].[Expense].[FileResourceKey]', 'FileResourceKeyOld', 'COLUMN'					
					ALTER TABLE [SMS].[Expense] ADD [FileResourceKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[FileResourceKey] = b.[Id] FROM [SMS].[Expense] a LEFT OUTER JOIN [CRM].[FileResource] b ON a.[FileResourceKeyOld] = b.[IdOld]')
					ALTER TABLE [SMS].[Expense] ALTER COLUMN [FileResourceKeyOld] bigint NULL
					ALTER TABLE [SMS].[Expense] ADD CONSTRAINT [FK_Expense_FileResource] FOREIGN KEY([FileResourceKey]) REFERENCES[CRM].[FileResource]([Id])
				END");

		}
	}
}