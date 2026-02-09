namespace Main.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140305153600)]
    public class ChangeCrmFileResourceToHighLowIds : Migration
    {
        private const int Low = 32;

        public override void Up()
        {
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[FileResource] DROP CONSTRAINT [PK_File]");
            Database.RenameColumn("[CRM].[FileResource]", "Id", "Id_Identity");
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[FileResource] ADD Id bigint NULL");
            Database.ExecuteNonQuery("UPDATE [CRM].[FileResource] SET Id = Id_Identity");
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[FileResource] ALTER COLUMN Id bigint NOT NULL");
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[FileResource] DROP COLUMN Id_Identity");
            Database.ExecuteNonQuery("ALTER TABLE [CRM].[FileResource] ADD CONSTRAINT [PK_File] PRIMARY KEY(Id)");

            Database.ExecuteNonQuery("BEGIN IF ((SELECT COUNT(*) FROM dbo.hibernate_unique_key WHERE tablename = '[CRM].[FileResource]') = 0) INSERT INTO hibernate_unique_key (next_hi, tablename) values ((select (COALESCE(max(Id), 0) / " + Low + ") + 1 from [CRM].[FileResource] where Id is not null), '[CRM].[FileResource]') END");
        }
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}