namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230825100200)]
	public class ChangeLookupsToGuidId : Migration
	{
		public override void Up()
		{
			Database.ChangeTableFromIntToGuidId("SMS", "CommissioningStatus", "CommissioningStatusId");
			Database.ChangeTableFromIntToGuidId("SMS", "Components", "ComponentId");
			Database.ChangeTableFromIntToGuidId("SMS", "ErrorCode", "ErrorCodeId");
			Database.ChangeTableFromIntToGuidId("LU", "InstallationAddressRelationshipType", "InstallationAddressRelationshipTypeId");
			Database.ChangeTableFromIntToGuidId("LU", "InstallationCompanyRelationshipType", "InstallationCompanyRelationshipTypeId");
			Database.ChangeTableFromIntToGuidId("LU", "InstallationPersonRelationshipType", "InstallationPersonRelationshipTypeId");
			Database.ChangeTableFromIntToGuidId("SMS", "InstallationHeadStatus", "InstallationHeadStatusId");
			Database.ChangeTableFromIntToGuidId("SMS", "InstallationType", "InstallationTypeId");
			Database.ChangeTableFromIntToGuidId("LU", "Manufacturer", "ManufacturerId");
			Database.ChangeTableFromIntToGuidId("SMS", "MonitoringDataType", "MonitoringDataTypeId");
			Database.ChangeTableFromIntToGuidId("LU", "NoCausingItemPreviousSerialNoReason", "NoCausingItemPreviousSerialNoReasonId");
			Database.ChangeTableFromIntToGuidId("LU", "NoCausingItemSerialNoReason", "NoCausingItemSerialNoReasonId");
			Database.ChangeTableFromIntToGuidId("LU", "NoPreviousSerialNoReason", "NoPreviousSerialNoReasonId");
			Database.ChangeTableFromIntToGuidId("SMS", "NotificationStandardAction", "NotificationStandardActionId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceNotificationCategory", "ServiceNotificationCategoryId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceNotificationStatus", "ServiceNotificationStatusId");
			Database.ChangeTableFromIntToGuidId("LU", "ServiceContractAddressRelationshipType", "ServiceContractAddressRelationshipTypeId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceContractLimitType", "ServiceContractLimitTypeId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceContractStatus", "ServiceContractStatusId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceContractType", "ServiceContractTypeId");
			Database.ChangeTableFromIntToGuidId("LU", "ServiceObjectCategory", "ServiceObjectCategoryId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceOrderDispatchRejectReason", "ServiceOrderDispatchRejectReasonId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceOrderDispatchStatus", "ServiceOrderDispatchTechnicianStatusId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceOrderInvoiceReason", "ServiceOrderInvoiceReasonId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceOrderNoInvoiceReason", "ServiceOrderNoInvoiceReasonId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceOrderStatus", "ServiceOrderStatusId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceOrderTimeCategory", "ServiceOrderTimeCategoryId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceOrderTimeLocation", "ServiceOrderTimeLocationId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceOrderTimePriority", "ServiceOrderTimePriorityId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceOrderTimeStatus", "ServiceOrderTimeStatusId");
			Database.ChangeTableFromIntToGuidId("SMS", "ServiceOrderType", "ServiceOrderType");
			Database.ChangeTableFromIntToGuidId("SMS", "ServicePriority", "ServiceNotificationPriorityId");
			Database.ChangeTableFromIntToGuidId("LU", "SparePartsBudgetInvoiceType", "SparePartsBudgetInvoiceTypeId");
			Database.ChangeTableFromIntToGuidId("LU", "SparePartsBudgetTimeSpanUnit", "SparePartsBudgetTimeSpanUnitId");
			Database.ChangeTableFromIntToGuidId("LU", "StatisticsKeyAssemblyGroup", "StatisticsKeyAssemblyGroupId");
			Database.ChangeTableFromIntToGuidId("LU", "StatisticsKeyCause", "StatisticsKeyCauseId");
			Database.ChangeTableFromIntToGuidId("LU", "StatisticsKeyCauser", "StatisticsKeyCauserId");
			Database.ChangeTableFromIntToGuidId("LU", "StatisticsKeyFaultImage", "StatisticsKeyFaultImageId");
			Database.ChangeTableFromIntToGuidId("LU", "StatisticsKeyMainAssembly", "StatisticsKeyMainAssemblyId");
			Database.ChangeTableFromIntToGuidId("LU", "StatisticsKeyProductType", "StatisticsKeyProductTypeId");
			Database.ChangeTableFromIntToGuidId("LU", "StatisticsKeyRemedy", "StatisticsKeyRemedyId");
			Database.ChangeTableFromIntToGuidId("LU", "StatisticsKeySubAssembly", "StatisticsKeySubAssemblyId");
			Database.ChangeTableFromIntToGuidId("LU", "StatisticsKeyWeighting", "StatisticsKeyWeightingId");
		}
	}
}
