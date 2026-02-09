namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180327124900)]
	public class RemoveTimeZoneOffsetFromCrmSite : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("[CRM].[Site]", "TimeZoneOffset");
		}
	}
}
