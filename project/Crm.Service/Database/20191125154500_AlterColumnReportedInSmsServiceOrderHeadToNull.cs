namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191125154500)]
	public class AlterColumnReportedInSmsServiceOrderHeadToNull : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderHead] ALTER COLUMN [Reported] datetime NULL");
		}
	}
}