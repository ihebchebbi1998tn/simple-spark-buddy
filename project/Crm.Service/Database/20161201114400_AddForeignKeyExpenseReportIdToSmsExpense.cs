namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20161201114400)]
	public class AddForeignKeyExpenseReportIdToSmsExpense : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sysobjects where name = 'PK_Expense'") == 0)
			{
				Database.AddPrimaryKey("PK_Expense", "[SMS].[Expense]", "ExpenseId");
			}
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sysobjects where name = 'FK_Expense_ExpenseReport'") == 0)
			{
				var query = new StringBuilder();

				query.AppendLine("ALTER TABLE [SMS].[Expense] WITH NOCHECK ADD CONSTRAINT [FK_Expense_ExpenseReport] FOREIGN KEY([ExpenseReportId])");
				query.AppendLine("REFERENCES [SMS].[ExpenseReport] ([Id])");
				query.AppendLine("ALTER TABLE [SMS].[Expense] CHECK CONSTRAINT [FK_Expense_ExpenseReport]");

				Database.ExecuteNonQuery(query.ToString());
			}
		}
	}
}