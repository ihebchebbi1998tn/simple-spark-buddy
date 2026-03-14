namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180221150000)]
	public class DropUnusedTables : Migration
	{
		public override void Up()
		{
			Database.DropTableIfExistingAndEmpty("SMS", "ArchiveStatus");
			Database.DropTableIfExistingAndEmpty("SMS", "ArticleMaintenanceType");
			Database.DropTableIfExistingAndEmpty("SMS", "ArticleType");
			Database.DropTableIfExistingAndEmpty("SMS", "AttributeType");
			Database.DropTableIfExistingAndEmpty("SMS", "CauseOfFailure");
			Database.DropTableIfExistingAndEmpty("SMS", "CauseOfFailureSolution");
			Database.DropTableIfExistingAndEmpty("SMS", "CauseOfFailureTrace");
			Database.DropTableIfExistingAndEmpty("SMS", "ClientPostings");
			Database.DropTableIfExistingAndEmpty("SMS", "ErrorCodeDecision");
			Database.DropTableIfExistingAndEmpty("SMS", "InstallationMonitoringData");
			Database.DropTableIfExistingAndEmpty("SMS", "InstallationTypeRhythms");
			Database.DropTableIfExistingAndEmpty("SMS", "LogEntries");
			Database.DropTableIfExistingAndEmpty("SMS", "LocalizedDescription");
			Database.DropTableIfExistingAndEmpty("SMS", "ODCPeriodData");
			Database.DropTableIfExistingAndEmpty("SMS", "ODCUserPostings");
			Database.DropTableIfExistingAndEmpty("SMS", "OfflineRoleTemplates");
			Database.DropTableIfExistingAndEmpty("SMS", "QaStatus");
			Database.DropTableIfExistingAndEmpty("SMS", "QualityPlan");
			Database.DropTableIfExistingAndEmpty("SMS", "QualityPlanComment");
			Database.DropTableIfExistingAndEmpty("SMS", "QualityResults");
			Database.DropTableIfExistingAndEmpty("SMS", "QualityResultType");
			Database.DropTableIfExistingAndEmpty("SMS", "QualityTasks");
			Database.DropTableIfExistingAndEmpty("SMS", "ServiceOrderHeadQualityPlan");
			Database.DropTableIfExistingAndEmpty("SMS", "ServiceOrderTimeQualityPlan");
			Database.DropTableIfExistingAndEmpty("SMS", "ReferenceType");
			Database.DropTableIfExistingAndEmpty("SMS", "ReplicationInstallation");
			Database.DropTableIfExistingAndEmpty("SMS", "ServiceOrderTimeScannedValues");
			Database.DropTableIfExistingAndEmpty("SMS", "ServiceOrderTimeScannedValueTypes");
			Database.DropTableIfExistingAndEmpty("SMS", "StockItems");
			Database.DropTableIfExistingAndEmpty("SMS", "StockJournal");
			Database.DropTableIfExistingAndEmpty("SMS", "TimeZones");
			Database.DropTableIfExistingAndEmpty("SMS", "UserProfiles");
			Database.DropTableIfExistingAndEmpty("SMS", "UsersInProfiles");
			Database.DropTableIfExistingAndEmpty("SMS", "WaitCauses");
			Database.DropTableIfExistingAndEmpty("SMS", "WorkflowTypes");
			Database.DropTableIfExistingAndEmpty("SMS", "PostingCode");
			Database.DropTableIfExistingAndEmpty("SMS", "PostingOrigin");
			Database.DropTableIfExistingAndEmpty("SMS", "TimeScale");
		}
	}
}