namespace Main.Database
{
	using System;
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140305151800)]
	public class ChangeCrmDocumentAttributesToHighLowIds : Migration
	{
		private const int Low = 32;

		public override void Up()
		{
			var sb = new StringBuilder();
			sb.AppendLine("IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_DocumentAttributes_DocumentAttributes')");
			sb.AppendLine("ALTER TABLE [CRM].[DocumentAttributes] DROP CONSTRAINT FK_DocumentAttributes_DocumentAttributes");
			Database.ExecuteNonQuery(sb.ToString());

			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DocumentAttributes] DROP CONSTRAINT [PK_SMS.DocumentAttributes]");
			Database.RenameColumn("[CRM].[DocumentAttributes]", "Id", "Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DocumentAttributes] ADD Id bigint NULL");
			Database.ExecuteNonQuery("UPDATE [CRM].[DocumentAttributes] SET Id = Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DocumentAttributes] ALTER COLUMN Id bigint NOT NULL");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DocumentAttributes] DROP COLUMN Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DocumentAttributes] ADD CONSTRAINT [PK_SMS.DocumentAttributes] PRIMARY KEY(Id)");

			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DocumentAttributes] WITH NOCHECK ADD CONSTRAINT [FK_DocumentAttributes_DocumentAttributes] FOREIGN KEY([Id]) REFERENCES [Crm].[DocumentAttributes] ([Id])");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DocumentAttributes] CHECK CONSTRAINT [FK_DocumentAttributes_DocumentAttributes]");

			Database.ExecuteNonQuery("BEGIN IF ((SELECT COUNT(*) FROM dbo.hibernate_unique_key WHERE tablename = '[CRM].[DocumentAttributes]') = 0) INSERT INTO hibernate_unique_key (next_hi, tablename) values ((select (COALESCE(max(Id), 0) / " + Low + ") + 1 from [CRM].[DocumentAttributes] where Id is not null), '[CRM].[DocumentAttributes]') END");
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}