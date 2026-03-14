namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20132904100212)]
	public class AlterSmsInstallationHeadColumnStatusToNvarchar : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[InstallationHead] ALTER COLUMN [Status] NVARCHAR(50) NOT NULL");
		}
		public override void Down()
		{
			throw new System.NotImplementedException();
		}
	}
}