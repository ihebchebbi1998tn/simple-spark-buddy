namespace Main.Database
{
	using System;
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140227152800)]
    public class ChangeCrmNoteToHighLowIds : Migration
    {
        private const int Low = 32;

        public override void Up()
        {
            var sb = new StringBuilder();
            sb.AppendLine("IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_LinkResource_Note')");
            sb.AppendLine("ALTER TABLE [CRM].[LinkResource] DROP CONSTRAINT FK_LinkResource_Note");
            sb.AppendLine("IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_FileResource_Note')");
            sb.AppendLine("ALTER TABLE [CRM].[FileResource] DROP CONSTRAINT FK_FileResource_Note");
            Database.ExecuteNonQuery(sb.ToString());

            Database.ExecuteNonQuery("ALTER TABLE [CRM].[Note] DROP CONSTRAINT PK_Note");
            Database.RenameColumn("[CRM].[Note]", "NoteId", "Id_Identity");
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[Note] ADD NoteId bigint NULL");
            Database.ExecuteNonQuery("UPDATE [CRM].[Note] SET NoteId = Id_Identity");
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[Note] ALTER COLUMN NoteId bigint NOT NULL");
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[Note] DROP COLUMN Id_Identity");
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[Note] ADD CONSTRAINT PK_Note PRIMARY KEY(NoteId)");

            Database.ExecuteNonQuery("BEGIN IF ((SELECT COUNT(*) FROM dbo.hibernate_unique_key WHERE tablename = '[CRM].[Note]') = 0) INSERT INTO hibernate_unique_key (next_hi, tablename) values ((select (COALESCE(max(NoteId), 0) / " + Low + ") + 1 from [CRM].[Note] where NoteId is not null), '[CRM].[Note]') END");

            var sb2 = new StringBuilder();
            sb2.AppendLine("IF EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FileResource_ElementKey')");
            sb2.AppendLine("DROP INDEX [IX_FileResource_ElementKey] ON [CRM].[FileResource];");
            Database.ExecuteNonQuery(sb2.ToString());

            Database.ExecuteNonQuery("ALTER TABLE [CRM].[FileResource] ALTER COLUMN ElementKey bigint NULL");
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[FileResource] WITH NOCHECK ADD CONSTRAINT [FK_FileResource_Note] FOREIGN KEY([ElementKey]) REFERENCES [Crm].[Note] ([NoteId])");
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[FileResource] CHECK CONSTRAINT [FK_FileResource_Note]");
            
            var sb3 = new StringBuilder();
            sb3.AppendLine("IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_FileResource_ElementKey')");
            sb3.AppendLine("CREATE NONCLUSTERED INDEX [IX_FileResource_ElementKey] ON [CRM].[FileResource] ([ElementKey] ASC)");
            Database.ExecuteNonQuery(sb3.ToString());

            Database.ExecuteNonQuery("ALTER TABLE [CRM].[LinkResource] ALTER COLUMN ElementKey bigint NULL");
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[LinkResource] WITH NOCHECK ADD CONSTRAINT [FK_LinkResource_Note] FOREIGN KEY([ElementKey]) REFERENCES [Crm].[Note] ([NoteId])");
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[LinkResource] CHECK CONSTRAINT [FK_LinkResource_Note]");
        }
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}