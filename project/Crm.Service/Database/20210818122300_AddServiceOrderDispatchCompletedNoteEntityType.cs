namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Service.Model.Notes;

	[Migration(20210818122300)]
	public class AddServiceOrderDispatchCompletedNoteEntityType : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderDispatchCompletedNote>("CRM", "Note");
		}
	}
}