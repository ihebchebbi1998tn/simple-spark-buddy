namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180625094000)]
	public class AddPrimaryKeyToExpense : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE [type] = 'PK' AND [parent_object_id] = OBJECT_ID('SMS.Expense')) BEGIN
					ALTER TABLE [SMS].[Expense] ADD CONSTRAINT [PK_Expense] PRIMARY KEY ([ExpenseId])
				END;
			");
		}
	}
}