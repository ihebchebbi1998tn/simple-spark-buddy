using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20170120110700)]
	public class AlterNameColumnInLookups : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[Skill] ALTER COLUMN [Name] NVARCHAR(50) NOT NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderTimeScannedValueTypes] ALTER COLUMN [Name] NVARCHAR(50) NOT NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[NotificationStandardAction] ALTER COLUMN [Name] NVARCHAR(50) NOT NULL");
		}
	}
}