namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180625094500)]
	public class AddForeignKeyExpenseFileResource : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE [parent_object_id] = OBJECT_ID('SMS.Expense') AND [referenced_object_id] = OBJECT_ID('CRM.FileResource')) BEGIN
					ALTER TABLE SMS.Expense
					ADD CONSTRAINT FK_Expense_FileResource FOREIGN KEY (FileResourceKey) REFERENCES CRM.FileResource
				END;
			");
		}
	}
}