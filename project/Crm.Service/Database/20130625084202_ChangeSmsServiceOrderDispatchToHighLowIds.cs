namespace Crm.Service.Database
{
	using System;
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130625084202)]
	public class ChangeSmsServiceOrderDispatchToHighLowIds : Migration
	{
		private const int Low = 32;

		public override void Up()
		{
			var sb = new StringBuilder();
			sb.AppendLine("IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderTimePostings_ServiceOrderDispatch')");
			sb.AppendLine("ALTER TABLE [SMS].[ServiceOrderTimePostings] DROP CONSTRAINT FK_ServiceOrderTimePostings_ServiceOrderDispatch");
			sb.AppendLine("IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderMaterial_ServiceOrderDispatch')");
			sb.AppendLine("ALTER TABLE [SMS].[ServiceOrderMaterial] DROP CONSTRAINT FK_ServiceOrderMaterial_ServiceOrderDispatch");
			Database.ExecuteNonQuery(sb.ToString());

			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderDispatch] DROP CONSTRAINT PK_ServiceOrderDispatch");
			Database.RenameColumn("[SMS].[ServiceOrderDispatch]", "DispatchId", "Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderDispatch] ADD DispatchId bigint NULL");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderDispatch] SET DispatchId = Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderDispatch] ALTER COLUMN DispatchId bigint NOT NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderDispatch] DROP COLUMN Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderDispatch] ADD CONSTRAINT PK_ServiceOrderDispatch PRIMARY KEY(DispatchId)");

			Database.ExecuteNonQuery("BEGIN IF ((SELECT COUNT(*) FROM dbo.hibernate_unique_key WHERE tablename = '[SMS].[ServiceOrderDispatch]') = 0) INSERT INTO hibernate_unique_key (next_hi, tablename) values ((select (COALESCE(max(DispatchId), 0) / " + Low + ") + 1 from [SMS].[ServiceOrderDispatch] where DispatchId is not null), '[SMS].[ServiceOrderDispatch]') END");
 
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderTimePostings] ALTER COLUMN DispatchId bigint NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderTimePostings] WITH NOCHECK ADD CONSTRAINT [FK_ServiceOrderTimePostings_ServiceOrderDispatch] FOREIGN KEY([DispatchId]) REFERENCES [SMS].[ServiceOrderDispatch] ([DispatchId])");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderTimePostings] CHECK CONSTRAINT [FK_ServiceOrderTimePostings_ServiceOrderDispatch]");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderMaterial] ALTER COLUMN DispatchId bigint NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderMaterial] WITH NOCHECK ADD CONSTRAINT [FK_ServiceOrderMaterial_ServiceOrderDispatch] FOREIGN KEY([DispatchId]) REFERENCES [SMS].[ServiceOrderDispatch] ([DispatchId])");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderMaterial] CHECK CONSTRAINT [FK_ServiceOrderMaterial_ServiceOrderDispatch]");
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}