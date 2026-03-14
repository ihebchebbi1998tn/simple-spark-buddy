namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220329130000)]
	public class RenameStatisticsKeyReferences : Migration
	{
		public override void Up()
		{
			Database.RenameColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyProductType", "StatisticsKeyProductTypeKey");
			Database.RenameColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyMainAssembly", "StatisticsKeyMainAssemblyKey");
			Database.RenameColumn("[SMS].[ServiceOrderHead]", "StatisticsKeySubAssembly", "StatisticsKeySubAssemblyKey");
			Database.RenameColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyAssemblyGroup", "StatisticsKeyAssemblyGroupKey");
			Database.RenameColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyFaultImage", "StatisticsKeyFaultImageKey");
			Database.RenameColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyRemedy", "StatisticsKeyRemedyKey");
			Database.RenameColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyCause", "StatisticsKeyCauseKey");
			Database.RenameColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyWeighting", "StatisticsKeyWeightingKey");
			Database.RenameColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyCauser", "StatisticsKeyCauserKey");

			Database.RenameColumn("[SMS].[ServiceNotifications]", "StatisticsKeyProductType", "StatisticsKeyProductTypeKey");
			Database.RenameColumn("[SMS].[ServiceNotifications]", "StatisticsKeyMainAssembly", "StatisticsKeyMainAssemblyKey");
			Database.RenameColumn("[SMS].[ServiceNotifications]", "StatisticsKeySubAssembly", "StatisticsKeySubAssemblyKey");
			Database.RenameColumn("[SMS].[ServiceNotifications]", "StatisticsKeyAssemblyGroup", "StatisticsKeyAssemblyGroupKey");
			Database.RenameColumn("[SMS].[ServiceNotifications]", "StatisticsKeyFaultImage", "StatisticsKeyFaultImageKey");
			Database.RenameColumn("[SMS].[ServiceNotifications]", "StatisticsKeyRemedy", "StatisticsKeyRemedyKey");
			Database.RenameColumn("[SMS].[ServiceNotifications]", "StatisticsKeyCause", "StatisticsKeyCauseKey");
			Database.RenameColumn("[SMS].[ServiceNotifications]", "StatisticsKeyWeighting", "StatisticsKeyWeightingKey");
			Database.RenameColumn("[SMS].[ServiceNotifications]", "StatisticsKeyCauser", "StatisticsKeyCauserKey");
		}
	}
}