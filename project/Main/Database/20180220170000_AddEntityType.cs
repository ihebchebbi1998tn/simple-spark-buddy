namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Library.Model;

	using Main.Model;

	[Migration(20180220170000)]
	public class AddEntityType : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Message>("CRM", "Message");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<User>("CRM", "User");
		}
	}
}
