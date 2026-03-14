namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Service.Model.Notes;

	[Migration(20200218145600)]
	public class AddServiceCaseCreatedNoteEntityType : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrGetEntityTypeId<ServiceCaseCreatedNote>();
		}
	}
}
