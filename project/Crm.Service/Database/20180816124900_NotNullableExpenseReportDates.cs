namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180816124900)]
	public class NotNullableExpenseReportDates : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("DELETE [SMS].[ExpenseReport] FROM [SMS].[ExpenseReport] LEFT OUTER JOIN [SMS].[Expense] ON [SMS].[ExpenseReport].[Id] = [SMS].[Expense].[ExpenseReportId] WHERE [SMS].[Expense].[ExpenseReportId] IS NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ExpenseReport] ALTER COLUMN [From] datetime not null");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ExpenseReport] ALTER COLUMN [To] datetime not null");
		}
	}
}
