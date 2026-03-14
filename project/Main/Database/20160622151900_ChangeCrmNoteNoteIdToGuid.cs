namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160622151900)]
	public class ChangeCrmNoteNoteIdToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Note_ElementKey')
					BEGIN
						DROP INDEX [IX_Note_ElementKey] ON [CRM].[Note]
					END

			IF EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Note_Plugin')
					BEGIN
						DROP INDEX [IX_Note_Plugin] ON [CRM].[Note]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_LinkResource_Note')
					BEGIN
						ALTER TABLE [CRM].[LinkResource] DROP CONSTRAINT [FK_LinkResource_Note]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_FileResource_Note')
					BEGIN
						ALTER TABLE [CRM].[FileResource] DROP CONSTRAINT [FK_FileResource_Note]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_Task_Note')
					BEGIN
						ALTER TABLE [CRM].[Task] DROP CONSTRAINT [FK_Task_Note]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_Note')
					BEGIN
						ALTER TABLE [CRM].[Note] DROP CONSTRAINT [PK_Note]
					END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Note' AND COLUMN_NAME='NoteId' AND DATA_TYPE like '%int')
				BEGIN
					ALTER TABLE [CRM].[Note] ADD [NoteIdOld] BIGINT NULL
					EXEC('UPDATE [CRM].[Note] SET [NoteIdOld] = [NoteId]')
					ALTER TABLE [CRM].[Note] DROP COLUMN [NoteId]
					ALTER TABLE [CRM].[Note] ADD [NoteId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					CREATE NONCLUSTERED INDEX [IX_Note_ElementKey] ON [CRM].[Note] ([ElementKey] ASC)
					CREATE NONCLUSTERED INDEX [IX_Note_Plugin] ON [CRM].[Note] ([Plugin] ASC) INCLUDE([ElementKey])
					ALTER TABLE [CRM].[Note] ADD CONSTRAINT [PK_Note] PRIMARY KEY CLUSTERED ([NoteId] ASC)
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='LinkResource' AND COLUMN_NAME='ElementKey' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[CRM].[LinkResource].[ElementKey]', 'ElementKeyOld', 'COLUMN'					
					ALTER TABLE [CRM].[LinkResource] ADD [ElementKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ElementKey] = b.[NoteId] FROM [Crm].[LinkResource] a LEFT OUTER JOIN [CRM].[Note] b ON a.[ElementKeyOld] = b.[NoteIdOld]')
					ALTER TABLE [CRM].[LinkResource] ALTER COLUMN [ElementKeyOld] bigint NULL
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Task' AND COLUMN_NAME='NoteKey' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[CRM].[Task].[NoteKey]', 'NoteKeyOld', 'COLUMN'					
					ALTER TABLE [CRM].[Task] ADD [NoteKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[NoteKey] = b.[NoteId] FROM [Crm].[Task] a LEFT OUTER JOIN [CRM].[Note] b ON a.[NoteKeyOld] = b.[NoteIdOld]')
					ALTER TABLE [CRM].[Task] ALTER COLUMN [NoteKeyOld] bigint NULL
				END");

			Database.ExecuteNonQuery(@"
				ALTER TABLE [CRM].[LinkResource] ADD CONSTRAINT [FK_LinkResource_Note] FOREIGN KEY([ElementKey]) REFERENCES [CRM].[Note] ([NoteId])
				ALTER TABLE [CRM].[Task] ADD CONSTRAINT [FK_Task_Note] FOREIGN KEY([NoteKey]) REFERENCES[CRM].[Note]([NoteId])
			");
		}
	}
}