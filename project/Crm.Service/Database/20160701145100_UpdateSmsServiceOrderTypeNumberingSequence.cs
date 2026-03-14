namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160701145100)]
	public class UpdateSmsServiceOrderTypeNumberingSequence : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderType] ALTER COLUMN NumberingSequence nvarchar(50)");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderType] SET NumberingSequence = 'SMS.ServiceOrderHead:MaintenanceOrder' where NumberingSequence = 'SMS.MaintenanceOrder'");
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderType]	SET NumberingSequence = 'SMS.ServiceOrderHead:ServiceOrder'	where NumberingSequence = 'SMS.ServiceOrder'");
		}
	}
}