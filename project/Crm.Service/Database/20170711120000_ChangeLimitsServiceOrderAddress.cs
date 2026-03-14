namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170711120000)]
	public class ChangeLimitsServiceOrderAddress : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				ALTER TABLE [SMS].[ServiceOrderHead]
				ALTER COLUMN Name1 NVARCHAR(180) NULL");
			Database.ExecuteNonQuery(@"
				ALTER TABLE [SMS].[ServiceOrderHead]
				ALTER COLUMN Name2 NVARCHAR(180) NULL");
			Database.ExecuteNonQuery(@"
				ALTER TABLE [SMS].[ServiceOrderHead]
				ALTER COLUMN Name3 NVARCHAR(180) NULL");
			Database.ExecuteNonQuery(@"
				ALTER TABLE [SMS].[ServiceOrderHead]
				ALTER COLUMN Street NVARCHAR(4000) NULL");
		}
	}
}