namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240703120500)]
	public class ChangeLookupsReplicationToGuidId : Migration
	{
		public override void Up()
		{
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "CommissioningStatus", "CommissioningStatusId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "Components", "ComponentId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ErrorCode", "ErrorCodeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "InstallationAddressRelationshipType", "InstallationAddressRelationshipTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "InstallationCompanyRelationshipType", "InstallationCompanyRelationshipTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "InstallationPersonRelationshipType", "InstallationPersonRelationshipTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "InstallationHeadStatus", "InstallationHeadStatusId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "InstallationType", "InstallationTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "Manufacturer", "ManufacturerId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "MonitoringDataType", "MonitoringDataTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "NoCausingItemPreviousSerialNoReason", "NoCausingItemPreviousSerialNoReasonId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "NoCausingItemSerialNoReason", "NoCausingItemSerialNoReasonId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "NoPreviousSerialNoReason", "NoPreviousSerialNoReasonId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "NotificationStandardAction", "NotificationStandardActionId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceNotificationCategory", "ServiceNotificationCategoryId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceNotificationStatus", "ServiceNotificationStatusId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "ServiceContractAddressRelationshipType", "ServiceContractAddressRelationshipTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceContractLimitType", "ServiceContractLimitTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceContractStatus", "ServiceContractStatusId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceContractType", "ServiceContractTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "ServiceObjectCategory", "ServiceObjectCategoryId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceOrderDispatchRejectReason", "ServiceOrderDispatchRejectReasonId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceOrderDispatchStatus", "ServiceOrderDispatchTechnicianStatusId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceOrderInvoiceReason", "ServiceOrderInvoiceReasonId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceOrderNoInvoiceReason", "ServiceOrderNoInvoiceReasonId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceOrderStatus", "ServiceOrderStatusId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceOrderTimeCategory", "ServiceOrderTimeCategoryId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceOrderTimeLocation", "ServiceOrderTimeLocationId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceOrderTimePriority", "ServiceOrderTimePriorityId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceOrderTimeStatus", "ServiceOrderTimeStatusId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServiceOrderType", "ServiceOrderType");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "ServicePriority", "ServiceNotificationPriorityId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "SparePartsBudgetInvoiceType", "SparePartsBudgetInvoiceTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "SparePartsBudgetTimeSpanUnit", "SparePartsBudgetTimeSpanUnitId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "StatisticsKeyAssemblyGroup", "StatisticsKeyAssemblyGroupId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "StatisticsKeyCause", "StatisticsKeyCauseId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "StatisticsKeyCauser", "StatisticsKeyCauserId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "StatisticsKeyFaultImage", "StatisticsKeyFaultImageId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "StatisticsKeyMainAssembly", "StatisticsKeyMainAssemblyId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "StatisticsKeyProductType", "StatisticsKeyProductTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "StatisticsKeyRemedy", "StatisticsKeyRemedyId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "StatisticsKeySubAssembly", "StatisticsKeySubAssemblyId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "StatisticsKeyWeighting", "StatisticsKeyWeightingId");
		}
	}
}
