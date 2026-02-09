namespace Crm.Service
{
	using Crm.Library.Configuration;
	using Crm.Library.Modularization;
	using Crm.Service.Enums;

	[Plugin(ModuleId = "FLD00010", Requires = "Crm,Crm.Article,Crm.PerDiem")]
	public class ServicePlugin : Plugin
	{
		public static string PluginName = typeof(ServicePlugin).Namespace;

		public static class Roles
		{
			public const string ServiceBackOffice = "ServiceBackOffice";
			public const string HeadOfService = "HeadOfService";
			public const string InternalService = "InternalService";
			public const string FieldService = "FieldService";
		}

		public static class PermissionGroup
		{
			public const string Dispatch = "Dispatch";
			public const string UpcomingDispatches = "UpcomingDispatches";
			public const string ScheduledDispatches = "ScheduledDispatches";
			public const string ClosedDispatches = "ClosedDispatches";
			public const string Adhoc = "Adhoc";
			public const string ServiceOrderTemplate = "ServiceOrderTemplate";
			public const string ServiceOrder = "ServiceOrder";
			public const string ServiceOrderMaterial = "ServiceOrderMaterial";
			public const string ServiceOrderExpensePosting = "ServiceOrderExpensePosting";
			public const string ServiceOrderTimePosting = "ServiceOrderTimePosting";
			public const string ServiceOrderErrorCause = "ServiceOrderErrorCause";
			public const string ServiceOrderErrorType = "ServiceOrderErrorType";
			public const string Installation = "Installation";
			public const string InstallationPosition = "InstallationPosition";
			public const string InstallationPositionSerial = "InstallationPositionSerial";
			public const string ServiceObject = "ServiceObject";
			public const string ServiceCase = "ServiceCase";
			public const string ServiceCaseTemplate = "ServiceCaseTemplate";
			public const string ServiceContract = "ServiceContract";
			public const string ServiceOrderTime = "ServiceOrderTime";
			public const string ReplenishmentOrder = "ReplenishmentOrder";
			public const string ReplenishmentOrderItem = "ReplenishmentOrderItem";
			public const string DocumentArchive = "DocumentArchive";
			public const string StatisticsKey = "StatisticsKey";
			public const string Replication = "Replication";
		}

		public static class PermissionName
		{
			public const string DispatchesTab = "DispatchesTab";
			public const string InstallationsTab = "InstallationsTab";
			public const string JobsTab = "JobsTab";
			public const string RelatedOrdersTab = "RelatedOrdersTab";
			public const string ServiceCasesTab = "ServiceCasesTab";
			public const string ServiceContractsTab = "ServiceContractsTab";
			public const string ServiceOrdersTab = "ServiceOrdersTab";
			public const string MaintenancePlansTab = "MaintenancePlansTab";
			public const string RemoveInstallationRelationship = "RemoveInstallationRelationship";
			public const string RemoveMaintenancePlan = "RemoveMaintenancePlan";
			public const string SaveInstallationRelationship = "SaveInstallationRelationship";
			public const string SaveMaintenancePlan = "SaveMaintenancePlan";
			public const string SendReplenishmentOrder = "SendReplenishmentOrder";
			public const string SeeAllUsersDispatches = "SeeAllUsersDispatches";
			public const string SeeAllUsersServiceOrders = "SeeAllUsersServiceOrders";
			public const string SelectNonMobileLookupValues = "SelectNonMobileLookupValues";
			public const string EditStandardAction = "EditStandardAction";
			public const string CreateStandardAction = "CreateStandardAction";
			public const string CreatePos = "CreatePos";
			public const string CreatePosSerial = "CreatePosSerial";
			public const string ImportPositionsFromCsv = "ImportPositionsFromCsv";

			public const string CreateCost = "CreateCost";
			public const string EditCost = "EditCost";
			public const string DeleteCost = "DeleteCost";
			public const string CreateMaterial = "CreateMaterial";
			public const string EditMaterial = "EditMaterial";
			public const string EditMaterialPrePlannedJob = "EditMaterialPrePlannedJob";
			public const string EditMaterialPrePlannedPrice = "EditMaterialPrePlannedPrice";
			public const string EditMaterialUsedPrice = "EditMaterialUsedPrice";
			public const string DeleteMaterial = "DeleteMaterial";
			public const string MaterialSerialsEdit = "MaterialSerialsEdit";
			public const string MaterialSerialSave = "MaterialSerialSave";
			public const string MaterialSerialRemove = "MaterialSerialRemove";
			public const string SeeTechnicianChoice = "SeeTechnicianChoice";
			public const string CreateForOtherUsers = "CreateForOtherUsers";

			public const string AddSkill = "AddSkill";
			public const string RemoveSkill = "RemoveSkill";
			public const string AddAsset = "AddAsset";
			public const string RemoveAsset = "RemoveAsset";
			public const string SetHeadStatus = "SetHeadStatus";
			public const string SetAdditionalHeadStatuses = "SetAdditionalHeadStatuses";
			public const string NoInvoice = "NoInvoice";
			public const string SetHeadCommissioningStatus = "SetHeadCommissioningStatus";
			public const string SetInvoicingType = "SetInvoicingType";
			public const string TimePostingPrePlannedAdd = "TimePostingPrePlannedAdd";
			public const string TimePostingPrePlannedEdit = "TimePostingPrePlannedEdit";
			public const string TimePostingPrePlannedEditJob = "TimePostingPrePlannedEditJob";
			public const string TimePostingPrePlannedRemove = "TimePostingPrePlannedRemove";
			public const string TimePostingAdd = "TimePostingAdd";
			public const string TimePostingsEdit = "TimePostingsEdit";
			public const string SinglePostingEdit = "SinglePostingEdit";
			public const string TimePostingSave = "TimePostingSave";
			public const string TimePostingRemove = "TimePostingRemove";
			public const string DispatchRelink = "DispatchRelink";
			public const string CreateAllUsersTimePostings = "CreateAllUsersTimePostings";

			public const string ErrorTab = "ErrorTab";
			public const string ExpenseTab = "ExpenseTab";

			public const string TimeAdd = "TimeAdd";
			public const string TimeNew = "TimeNew";
			public const string TimeEdit = "TimeEdit";
			public const string TimeSave = "TimeSave";
			public const string TimeDelete = "TimeDelete";
			public const string TimeDeleteSelfCreated = "TimeDeleteSelfCreated";

			public const string TabMaterial = "TabMaterial";
			public const string DocumentArchive = "DocumentArchive";

			public const string ReplenishmentsFromOtherUsersSelectable = "ReplenishmentsFromOtherUsersSelectable";
			public const string CreateItem = "CreateItem";
			public const string EditItem = "EditItem";
			public const string DeleteItem = "DeleteItem";

			public const string AppointmentRequest = "AppointmentRequest";
			public const string AppointmentConfirmation = "AppointmentConfirmation";
			public const string RejectScheduled = "RejectScheduled";
			public const string RejectReleased = "RejectReleased";
			public const string ConfirmScheduled = "ConfirmScheduled";
			public const string ConfirmReleased = "ConfirmReleased";
			public const string Reschedule = "Reschedule";
			public const string Complete = "Complete";
			public const string ReportPreview = "ReportPreview";
			public const string ReportRecipients = "ReportRecipients";
			public const string Signature = "Signature";
			public const string Cancel = "Cancel";

			public const string EditClosed = "EditClosed";
			public const string EditNotAssigned = "EditNotAssigned";

			public const string DownloadReport = "DownloadReport";
			public const string SeeClosedReplenishmentOrders = "SeeClosedReplenishmentOrders";

			public const string AutonomousDispatchPlanning = "AutonomousDispatchPlanning";
			public const string EditMaterialWhenDispatchClosed = "EditMaterialWhenDispatchClosed";
			public const string EditTimePostingWhenDispatchClosed = "EditTimePostingWhenDispatchClosed";
			public const string DetailsTechnicianInfo = "DetailsTechnicianInfo";
			public const string ReadInstallationCompanyRelationship = "ReadInstallationCompanyRelationship";
			public const string CreateInstallationCompanyRelationship = "CreateInstallationCompanyRelationship";
			public const string EditInstallationCompanyRelationship = "EditInstallationCompanyRelationship";
			public const string RemoveInstallationCompanyRelationship = "RemoveInstallationCompanyRelationship";
			public const string ReadInstallationPersonRelationship = "ReadInstallationPersonRelationship";
			public const string CreateInstallationPersonRelationship = "CreateInstallationPersonRelationship";
			public const string EditInstallationPersonRelationship = "EditInstallationPersonRelationship";
			public const string RemoveInstallationPersonRelationship = "RemoveInstallationPersonRelationship";
			public const string EditErrorTypeIsSuspected = "EditErrorTypeIsSuspected";
			public const string EditErrorCauseIsSuspected = "EditErrorCauseIsSuspected";
		}

		public static class Settings
		{
			public static class AdHoc
			{
				public static SettingDefinition<string> AdHocNumberingSequenceName => new SettingDefinition<string>("AdHoc/AdHocNumberingSequenceName", PluginName);
			}

			public static class Email
			{
				public static SettingDefinition<bool> ClosedByRecipientForReplenishmentReport => new SettingDefinition<bool>("Email/ClosedByRecipientForReplenishmentReport", PluginName);
				public static SettingDefinition<string[]> DispatchReportRecipients => new SettingDefinition<string[]>("Email/DispatchReportRecipients", PluginName);
				public static SettingDefinition<string[]> ReplenishmentOrderRecipients => new SettingDefinition<string[]>("Email/ReplenishmentOrderRecipients", PluginName);
				public static SettingDefinition<bool> SendDispatchFollowUpOrderNotificationEmails => new SettingDefinition<bool>("Email/SendDispatchFollowUpOrderNotificationEmails", PluginName);
				public static SettingDefinition<bool> SendDispatchNotificationEmails => new SettingDefinition<bool>("Email/SendDispatchNotificationEmails", PluginName);
				public static SettingDefinition<bool> SendDispatchRejectNotificationEmails => new SettingDefinition<bool>("Email/SendDispatchRejectNotificationEmails", PluginName);
				public static SettingDefinition<bool> SendDispatchReportsOnCompletion => new SettingDefinition<bool>("Email/SendDispatchReportsOnCompletion", PluginName);
				public static SettingDefinition<bool> SendDispatchReportToDispatcher => new SettingDefinition<bool>("Email/SendDispatchReportToDispatcher", PluginName);
				public static SettingDefinition<bool> SendDispatchReportToTechnician => new SettingDefinition<bool>("Email/SendDispatchReportToTechnician", PluginName);
				public static SettingDefinition<bool> SendServiceOrderReportsOnCompletion => new SettingDefinition<bool>("Email/SendServiceOrderReportsOnCompletion", PluginName);
				public static SettingDefinition<bool> SendServiceOrderReportToDispatchers => new SettingDefinition<bool>("Email/SendServiceOrderReportToDispatchers", PluginName);
			}

			public static class Export
			{
				public static SettingDefinition<bool> ExportDispatchReportsOnCompletion => new SettingDefinition<bool>("Export/ExportDispatchReportsOnCompletion", PluginName);
				public static SettingDefinition<string> ExportDispatchReportsPath => new SettingDefinition<string>("Export/ExportDispatchReportsPath", PluginName);
				public static SettingDefinition<string> ExportDispatchReportsFilePattern => new SettingDefinition<string>("Export/ExportDispatchReportsFilePattern", PluginName);
				public static SettingDefinition<string> ExportDispatchReportsControlFilePattern => new SettingDefinition<string>("Export/ExportDispatchReportsControlFilePattern", PluginName);
				public static SettingDefinition<string> ExportDispatchReportsControlFileExtension => new SettingDefinition<string>("Export/ExportDispatchReportsControlFileExtension", PluginName);
				public static SettingDefinition<string> ExportDispatchReportsControlFileContent => new SettingDefinition<string>("Export/ExportDispatchReportsControlFileContent", PluginName);
				public static SettingDefinition<bool> ExportServiceOrderReportsOnCompletion => new SettingDefinition<bool>("Export/ExportServiceOrderReportsOnCompletion", PluginName);
				public static SettingDefinition<string> ExportServiceOrderReportsPath => new SettingDefinition<string>("Export/ExportServiceOrderReportsPath", PluginName);
				public static SettingDefinition<string> ExportServiceOrderReportsUncUser => new SettingDefinition<string>("Export/ExportServiceOrderReportsUncUser", PluginName);
				public static SettingDefinition<string> ExportServiceOrderReportsUncDomain => new SettingDefinition<string>("Export/ExportServiceOrderReportsUncDomain", PluginName);
				public static SettingDefinition<string> ExportServiceOrderReportsUncPassword => new SettingDefinition<string>("Export/ExportServiceOrderReportsUncPassword", PluginName);
				public static SettingDefinition<string> ExportDispatchReportsUncUser => new SettingDefinition<string>("Export/ExportDispatchReportsUncUser", PluginName);
				public static SettingDefinition<string> ExportDispatchReportsUncDomain => new SettingDefinition<string>("Export/ExportDispatchReportsUncDomain", PluginName);
				public static SettingDefinition<string> ExportDispatchReportsUncPassword => new SettingDefinition<string>("Export/ExportDispatchReportsUncPassword", PluginName);
			}
				public static SettingDefinition<int> TreeLevelDisplayLimit => new SettingDefinition<int>("Service/Installation/Position/TreeLevelDisplayLimit", PluginName);

			public static class ReplenishmentOrder
			{
				public static SettingDefinition<int> ClosedReplenishmentOrderHistorySyncPeriod => new SettingDefinition<int>("ReplenishmentOrder/ClosedReplenishmentOrderHistorySyncPeriod", PluginName);
			}

			public static class ServiceContract
			{
				public static SettingDefinition<bool> OnlyInstallationsOfReferencedCustomer => new SettingDefinition<bool>("ServiceContract/OnlyInstallationsOfReferencedCustomer", PluginName);
				public static SettingDefinition<int> CreateMaintenanceOrderTimeSpanDays => new SettingDefinition<int>("ServiceContract/CreateMaintenanceOrderTimeSpanDays", PluginName);
				public static SettingDefinition<MaintenanceOrderGenerationMode> MaintenanceOrderGenerationMode => new SettingDefinition<MaintenanceOrderGenerationMode>("ServiceContract/MaintenanceOrderGenerationMode", PluginName);

				public static class MaintenancePlan
				{
					public static SettingDefinition<string[]> AvailableTimeUnits => new SettingDefinition<string[]>("ServiceContract/MaintenancePlan/AvailableTimeUnits", PluginName);
				}
				public static class ReactionTime
				{
					public static SettingDefinition<string[]> AvailableTimeUnits => new SettingDefinition<string[]>("ServiceContract/ReactionTime/AvailableTimeUnits", PluginName);
				}
			}

			public static class ServiceCase
			{
				public static SettingDefinition<bool> OnlyInstallationsOfReferencedCustomer => new SettingDefinition<bool>("ServiceCase/OnlyInstallationsOfReferencedCustomer", PluginName);

				public static class Signature
				{
					public static SettingDefinition<bool> EnableSignatureForTechnician => new SettingDefinition<bool>("ServiceCase/Signature/Enable/Technician", PluginName);
					public static SettingDefinition<bool> EnableSignatureForOriginator => new SettingDefinition<bool>("ServiceCase/Signature/Enable/Originator", PluginName);
					public static SettingDefinition<bool> ShowPrivacyPolicy => new SettingDefinition<bool>("Service/Dispatch/Show/PrivacyPolicy", PluginName);
					public static SettingDefinition<string> EmptyTimesOrMaterialsWarning => new SettingDefinition<string>("Service/Dispatch/Show/EmptyTimesOrMaterialsWarning", PluginName);

				}
			}

			public static class ServiceOrder
			{
				public static SettingDefinition<string> DefaultDuration => new SettingDefinition<string>("ServiceOrder/DefaultDuration", PluginName);
				public static SettingDefinition<bool> OnlyInstallationsOfReferencedCustomer => new SettingDefinition<bool>("ServiceOrder/OnlyInstallationsOfReferencedCustomer", PluginName);
				public static SettingDefinition<bool> GenerateAndAttachJobsToUnattachedTimePostings => new SettingDefinition<bool>("ServiceOrder/GenerateAndAttachJobsToUnattachedTimePostings", PluginName);
			}

			public static class ServiceOrderDispatch
			{
				public static SettingDefinition<bool> ToggleSingleJob => new SettingDefinition<bool>("ServiceOrderDispatch/ToggleSingleJob", PluginName);
				public static SettingDefinition<bool> ReadGeolocationOnDispatchStart => new SettingDefinition<bool>("ServiceOrderDispatch/ReadGeolocationOnDispatchStart", PluginName);
				public static SettingDefinition<bool> CustomerSignatureIncludesLegacyId => new SettingDefinition<bool>("ServiceOrderDispatch/CustomerSignatureIncludesLegacyId", PluginName);
				public static SettingDefinition<bool> RequiresCustomerSignature => new SettingDefinition<bool>("Service/Dispatch/Requires/CustomerSignature", PluginName);
			}

			public static class ServiceOrderMaterial
			{
				public static SettingDefinition<bool> ShowPricesInMobileClient => new SettingDefinition<bool>("ServiceOrderMaterial/ShowPricesInMobileClient", PluginName);
				public static SettingDefinition<bool> CreateReplenishmentOrderItemsFromServiceOrderMaterial => new SettingDefinition<bool>("ServiceOrderMaterial/CreateReplenishmentOrderItemsFromServiceOrderMaterial", PluginName);
			}

			public static class ServiceOrderExpensePosting
			{
				public static SettingDefinition<bool> UseArticleAsExpenseType => new SettingDefinition<bool>("ServiceOrderExpensePosting/UseArticleAsExpenseType", PluginName);
			}

			public static class ServiceOrderTimePosting
			{
				public static SettingDefinition<int> ClosedTimePostingsHistorySyncPeriod => new SettingDefinition<int>("ServiceOrderTimePosting/ClosedTimePostingsHistorySyncPeriod", PluginName);
				public static SettingDefinition<int> MaxDaysAgo => new SettingDefinition<int>("ServiceOrderTimePosting/MaxDaysAgo", PluginName);
				public static SettingDefinition<int> MinutesInterval => new SettingDefinition<int>("ServiceOrderTimePosting/MinutesInterval", PluginName);
				public static SettingDefinition<bool> ShowTechnicianSelection => new SettingDefinition<bool>("ServiceOrderTimePosting/ShowTechnicianSelection", PluginName);
				public static SettingDefinition<bool> AllowOverlap => new SettingDefinition<bool>("ServiceOrderTimePosting/AllowOverlap", PluginName);
			}


			public static class UserExtension
			{
				public static SettingDefinition<bool> OnlyUnusedLocationNosSelectable => new SettingDefinition<bool>("UserExtension/OnlyUnusedLocationNosSelectable", PluginName);
			}
			public static SettingDefinition<PosNoGenerationMethod> PosNoGenerationMethod => new SettingDefinition<PosNoGenerationMethod>("PosNoGenerationMethod", PluginName);

			public static class Dispatch
			{
				public static SettingDefinition<bool> SuppressEmptyMaterialsInReport => new SettingDefinition<bool>("Dispatch/SuppressEmptyMaterialsInReport", PluginName);
				public static SettingDefinition<bool> SuppressEmptyExpensePostingsInReport => new SettingDefinition<bool>("Dispatch/SuppressEmptyExpensePostingsInReport", PluginName);
				public static SettingDefinition<bool> SuppressExpensePostingsInReport => new SettingDefinition<bool>("Dispatch/SuppressExpensePostingsInReport", PluginName);
				public static SettingDefinition<bool> SuppressEmptyTimePostingsInReport => new SettingDefinition<bool>("Dispatch/SuppressEmptyTimePostingsInReport", PluginName);
				public static SettingDefinition<bool> SuppressEmptyJobsInReport => new SettingDefinition<bool>("Dispatch/SuppressEmptyJobsInReport", PluginName);
				public static SettingDefinition<bool> SuppressEmptyErrorTypesInReport => new SettingDefinition<bool>("Dispatch/SuppressEmptyErrorTypesInReport", PluginName);
				public static SettingDefinition<bool> SuppressErrorTypesInReport => new SettingDefinition<bool>("Dispatch/SuppressErrorTypesInReport", PluginName);
			}
			public static class AttentionTasks
			{
				public static SettingDefinition<bool> CheckAllInstallationsWarranties => new SettingDefinition<bool>("AttentionTasks/CheckAllInstallationsWarranties", PluginName);
			}

		}
	}
}
