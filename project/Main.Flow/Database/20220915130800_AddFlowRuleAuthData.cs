namespace Main.Flow.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Main.Flow.Model;

	[Migration(20220915130800)]
	public class AddFlowRuleAuthData : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrUpdateEntityAuthDataColumn<FlowRule>("CRM", "FlowRule");
		}
	}
}