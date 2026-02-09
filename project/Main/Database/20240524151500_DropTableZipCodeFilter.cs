namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240524151500)]
	public class DropTableZipCodeFilter : Migration
	{
		public override void Up()
		{
			Database.RemoveTableIfExisting("[LU].[ZipCodeFilter]");
		}
	}
}
