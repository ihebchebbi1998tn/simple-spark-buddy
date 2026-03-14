namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140312162100)]
    public class AddColumnFileResourceKeyToSmsExpense : Migration
    {
        public override void Up()
        {
			if (!Database.ColumnExists("[SMS].[Expense]", "FileResourceKey"))
			{
                Database.AddColumn("[SMS].[Expense]", "FileResourceKey", DbType.Int64, ColumnProperty.Null);
			}
        }
        public override void Down()
		{
            if (Database.ColumnExists("[SMS].[Expense]", "FileResourceKey"))
			{
                Database.RemoveColumn("[SMS].[Expense]", "FileResourceKey");
			}
        }
    }
}