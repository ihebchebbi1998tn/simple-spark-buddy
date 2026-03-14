namespace Crm.Service.Database
{

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240906140600)]
	public class RemoveKilometersFromServiceOrderTimePosting : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("[SMS].[ServiceOrderTimePostings]", "Kilometers");
		}
	}
}
