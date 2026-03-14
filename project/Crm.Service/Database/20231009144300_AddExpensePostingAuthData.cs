namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Service.Model;

	[Migration(20231009144300)]
	public class AddExpensePostingAuthData : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderExpensePosting>("SMS", "ServiceOrderExpensePostings", "ServiceOrderExpensePostingId");
		}
	}
}
