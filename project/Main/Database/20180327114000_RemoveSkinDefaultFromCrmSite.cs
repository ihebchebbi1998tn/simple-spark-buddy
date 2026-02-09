namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180327114000)]
	public class RemoveSkinDefaultFromCrmSite : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("[CRM].[Site]", "SkinDefault");
		}
	}
}
