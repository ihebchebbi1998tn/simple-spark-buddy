namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.PerDiem.Model.Lookups;
	using Crm.Service.Model.Lookup;

	[Migration(20230825100300)]
	public class AddAuthDataIdToLookups : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrUpdateEntityAuthDataColumn<CommissioningStatus>("SMS", "CommissioningStatus");
			helper.AddOrUpdateEntityAuthDataColumn<Component>("SMS", "Components", "ComponentId");
			helper.AddOrUpdateEntityAuthDataColumn<ErrorCode>("SMS", "ErrorCode");
			helper.AddOrUpdateEntityAuthDataColumn<InstallationAddressRelationshipType>("LU", "InstallationAddressRelationshipType");
			helper.AddOrUpdateEntityAuthDataColumn<InstallationCompanyRelationshipType>("LU", "InstallationCompanyRelationshipType");
			helper.AddOrUpdateEntityAuthDataColumn<InstallationPersonRelationshipType>("LU", "InstallationPersonRelationshipType");
			helper.AddOrUpdateEntityAuthDataColumn<InstallationHeadStatus>("SMS", "InstallationHeadStatus");
			helper.AddOrUpdateEntityAuthDataColumn<InstallationType>("SMS", "InstallationType");
			helper.AddOrUpdateEntityAuthDataColumn<Manufacturer>("LU", "Manufacturer");
			helper.AddOrUpdateEntityAuthDataColumn<MonitoringDataType>("SMS", "MonitoringDataType");
			helper.AddOrUpdateEntityAuthDataColumn<NoCausingItemPreviousSerialNoReason>("LU", "NoCausingItemPreviousSerialNoReason");
			helper.AddOrUpdateEntityAuthDataColumn<NoCausingItemSerialNoReason>("LU", "NoCausingItemSerialNoReason");
			helper.AddOrUpdateEntityAuthDataColumn<NoPreviousSerialNoReason>("LU", "NoPreviousSerialNoReason");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceCaseCategory>("SMS", "ServiceNotificationCategory");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceCaseStatus>("SMS", "ServiceNotificationStatus");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceContractAddressRelationshipType>("LU", "ServiceContractAddressRelationshipType");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceContractLimitType>("SMS", "ServiceContractLimitType");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceContractStatus>("SMS", "ServiceContractStatus");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceContractType>("SMS", "ServiceContractType");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceObjectCategory>("LU", "ServiceObjectCategory");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderDispatchRejectReason>("SMS", "ServiceOrderDispatchRejectReason");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderDispatchStatus>("SMS", "ServiceOrderDispatchStatus", "ServiceOrderDispatchTechnicianStatusId");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderInvoiceReason>("SMS", "ServiceOrderInvoiceReason");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderNoInvoiceReason>("SMS", "ServiceOrderNoInvoiceReason");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderStatus>("SMS", "ServiceOrderStatus");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderTimeCategory>("SMS", "ServiceOrderTimeCategory");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderTimeLocation>("SMS", "ServiceOrderTimeLocation");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderTimePriority>("SMS", "ServiceOrderTimePriority");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderTimeStatus>("SMS", "ServiceOrderTimeStatus");
			helper.AddOrUpdateEntityAuthDataColumn<ServiceOrderType>("SMS", "ServiceOrderType", "ServiceOrderType");
			helper.AddOrUpdateEntityAuthDataColumn<ServicePriority>("SMS", "ServicePriority", "ServiceNotificationPriorityId");
			helper.AddOrUpdateEntityAuthDataColumn<SparePartsBudgetInvoiceType>("LU", "SparePartsBudgetInvoiceType");
			helper.AddOrUpdateEntityAuthDataColumn<SparePartsBudgetTimeSpanUnit>("LU", "SparePartsBudgetTimeSpanUnit");
			helper.AddOrUpdateEntityAuthDataColumn<StatisticsKeyAssemblyGroup>("LU", "StatisticsKeyAssemblyGroup");
			helper.AddOrUpdateEntityAuthDataColumn<StatisticsKeyCause>("LU", "StatisticsKeyCause");
			helper.AddOrUpdateEntityAuthDataColumn<StatisticsKeyCauser>("LU", "StatisticsKeyCauser");
			helper.AddOrUpdateEntityAuthDataColumn<StatisticsKeyFaultImage>("LU", "StatisticsKeyFaultImage");
			helper.AddOrUpdateEntityAuthDataColumn<StatisticsKeyMainAssembly>("LU", "StatisticsKeyMainAssembly");
			helper.AddOrUpdateEntityAuthDataColumn<StatisticsKeyProductType>("LU", "StatisticsKeyProductType");
			helper.AddOrUpdateEntityAuthDataColumn<StatisticsKeyRemedy>("LU", "StatisticsKeyRemedy");
			helper.AddOrUpdateEntityAuthDataColumn<StatisticsKeySubAssembly>("LU", "StatisticsKeySubAssembly");
			helper.AddOrUpdateEntityAuthDataColumn<StatisticsKeyWeighting>("LU", "StatisticsKeyWeighting");
		}
	}
}
