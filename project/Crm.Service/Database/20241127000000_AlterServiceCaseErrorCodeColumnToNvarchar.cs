namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20241127000000)]
	public class AlterServiceCaseErrorCodeColumnToNvarchar : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceNotifications] ALTER COLUMN [ErrorCode] nvarchar(100) NULL");
		}
	}
}
