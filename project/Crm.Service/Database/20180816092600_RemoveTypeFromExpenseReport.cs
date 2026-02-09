namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180816092600)]
	public class RemoveTypeFromExpenseReport : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.ExpenseReport", "Type");
		}
	}
}