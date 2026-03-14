namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20151111173000)]
	public class AddServiceCaseNoToSmsServiceNotifications : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[SMS].[ServiceNotifications]", "ServiceCaseNo"))
			{
				var hasContactKeyChangedToGuid = (int)Database.ExecuteScalar("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'SMS' AND TABLE_NAME = 'ServiceNotifications' AND COLUMN_NAME = 'ContactKey' and DATA_TYPE = 'uniqueidentifier'") == 1;
				var columnName = hasContactKeyChangedToGuid ? "ContactKeyOld" : "ContactKey";
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceNotifications] ADD [ServiceCaseNo] nvarchar(20) NULL");
				Database.ExecuteNonQuery(string.Format("UPDATE [SMS].[ServiceNotifications] SET [ServiceCaseNo] = [{0}]", columnName));
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceNotifications] ALTER COLUMN [ServiceCaseNo] nvarchar(20) NOT NULL");
				Database.ExecuteNonQuery($"INSERT INTO [dbo].[NumberingSequence] ([SequenceName], [LastNumber], [Format]) VALUES('SMS.ServiceCase', COALESCE((SELECT MAX([{columnName}]) FROM [SMS].[ServiceNotifications]), 0), '00000')");
			}
		}
	}
}