namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180327122800)]
	public class RemoveScriptsPathFromCrmSite : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("[CRM].[Site]", "ScriptsPath");
		}
	}
}
