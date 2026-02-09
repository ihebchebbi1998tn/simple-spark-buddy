namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	using Main.Model;

	[Migration(20220919101900)]
	public class AddUserSubscriptionAuthData : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrUpdateEntityAuthDataColumn<UserSubscription>("CRM", "UserSubscription");
		}
	}
}
