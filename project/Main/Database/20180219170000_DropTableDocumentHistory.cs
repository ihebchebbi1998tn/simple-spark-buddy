namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180219170000)]
	public class DropTableDocumentHistory : Migration
	{
		public override void Up()
		{
			Database.DropTableIfExistingAndEmpty("CRM", "DocumentHistory");
		}
	}
}
