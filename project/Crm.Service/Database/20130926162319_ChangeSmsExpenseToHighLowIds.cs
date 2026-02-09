namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130926162319)]
	public class ChangeSmsExpenseToHighLowIds : Migration
	{
		private const int Low = 32;

		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[Expense] DROP CONSTRAINT PK_Expense");
			Database.RenameColumn("[SMS].[Expense]", "ExpenseId", "Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[Expense] ADD ExpenseId bigint NULL");
			Database.ExecuteNonQuery("UPDATE [SMS].[Expense] SET ExpenseId = Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[Expense] ALTER COLUMN ExpenseId bigint NOT NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[Expense] DROP COLUMN Id_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[Expense] ADD CONSTRAINT PK_Expense PRIMARY KEY(ExpenseId)");

			Database.ExecuteNonQuery("BEGIN IF ((SELECT COUNT(*) FROM dbo.hibernate_unique_key WHERE tablename = '[SMS].[Expense]') = 0) INSERT INTO hibernate_unique_key (next_hi, tablename) values ((select (COALESCE(max(ExpenseId), 0) / " + Low + ") + 1 from [SMS].[Expense] where ExpenseId is not null), '[SMS].[Expense]') END");
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}