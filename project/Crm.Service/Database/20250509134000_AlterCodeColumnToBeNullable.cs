namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20250509134000)]
	public class AlterCodeColumnToBeNullable : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [LU].[StatisticsKeyFaultImage] ALTER COLUMN [Code] nvarchar(50) NULL");
			Database.ExecuteNonQuery("ALTER TABLE [LU].[StatisticsKeyCause] ALTER COLUMN [Code] nvarchar(50) NULL");
		}
	}
}
