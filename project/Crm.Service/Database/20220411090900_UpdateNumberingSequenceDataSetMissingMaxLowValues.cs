using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20220411090900)]
	public class UpdateNumberingSequenceDataSetMissingMaxLowValues : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[dbo].[NumberingSequence]")) {
				Database.ExecuteNonQuery("Update dbo.NumberingSequence Set MaxLow = 32 Where SequenceName = 'SMS.ServiceOrderHead.ServiceOrder' OR SequenceName = 'SMS.ServiceCase' OR SequenceName = 'SMS.ServiceObject' OR SequenceName = 'SMS.ServiceContract' Or SequenceName = 'SMS.ServiceOrderHead.MaintenanceOrder'");
			}
		}
	}
}
