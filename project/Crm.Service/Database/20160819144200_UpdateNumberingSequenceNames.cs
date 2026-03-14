namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160819144200)]
	public class UpdateNumberingSequenceRows : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"UPDATE [dbo].[NumberingSequence] SET [SequenceName] = 'SMS.ServiceOrderHead.AdHoc'	WHERE [SequenceName] = 'SMS.ServiceOrderHead:AdHoc'");
			Database.ExecuteNonQuery(@"UPDATE [dbo].[NumberingSequence] SET [SequenceName] = 'SMS.ServiceOrderHead.MaintenanceOrder'	WHERE [SequenceName] = 'SMS.ServiceOrderHead:MaintenanceOrder'");
			Database.ExecuteNonQuery(@"UPDATE [dbo].[NumberingSequence] SET [SequenceName] = 'SMS.ServiceOrderHead.ServiceOrder'	WHERE [SequenceName] = 'SMS.ServiceOrderHead:ServiceOrder'");
			Database.ExecuteNonQuery(@"UPDATE [SMS].[ServiceOrderType] SET [NumberingSequence] = 'SMS.ServiceOrderHead.AdHoc'	WHERE [NumberingSequence] = 'SMS.ServiceOrderHead:AdHoc'");
			Database.ExecuteNonQuery(@"UPDATE [SMS].[ServiceOrderType] SET [NumberingSequence] = 'SMS.ServiceOrderHead.MaintenanceOrder'	WHERE [NumberingSequence] = 'SMS.ServiceOrderHead:MaintenanceOrder'");
			Database.ExecuteNonQuery(@"UPDATE [SMS].[ServiceOrderType] SET [NumberingSequence] = 'SMS.ServiceOrderHead.ServiceOrder'	WHERE [NumberingSequence] = 'SMS.ServiceOrderHead:ServiceOrder'");
		}
	}
}