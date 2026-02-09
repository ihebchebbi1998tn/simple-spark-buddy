namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	
	[Migration(20150413144700)]
	public class AddFileResourceKeyFkToSmsExpense : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_Expense_FileResource'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE e SET e.[FileResourceKey] = NULL FROM [SMS].[Expense] e LEFT OUTER JOIN [CRM].[FileResource] fr on e.[FileResourceKey] = fr.[Id] WHERE fr.[Id] IS NULL");
				Database.AddForeignKey("FK_Expense_FileResource", "[SMS].[Expense]", "FileResourceKey", "[CRM].[FileResource]", "Id", ForeignKeyConstraint.NoAction);
			}
		}
	}
}