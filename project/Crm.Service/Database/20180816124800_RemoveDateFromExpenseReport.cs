namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180816124800)]
	public class RemoveDateFromExpenseReport : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.ExpenseReport", "Date");
		}
	}
}