namespace Sms.Scheduler.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	using Sms.Scheduler.Model;

	[Migration(20231012153000)]
	public class AddEntityTypeToProfile : Migration {
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Profile>("RPL","Profile");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<ProfileResource>("RPL","ProfileResource");
		}
	}
}
