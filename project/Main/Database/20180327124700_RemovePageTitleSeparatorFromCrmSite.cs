namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180327124700)]
	public class RemovePageTitleSeparatorFromCrmSite : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("[CRM].[Site]", "PageTitleSeparator");
		}
	}
}
