import {HelperDispatch} from "../ts/helper/Helper.Dispatch";
import {HelperInstallation} from "../ts/helper/Helper.Installation";
import {HelperInstallationPosition} from "../ts/helper/Helper.InstallationPosition";
import {HelperReplenishmentOrder} from "../ts/helper/Helper.ReplenishmentOrder";
import {HelperService} from "../ts/helper/Helper.Service";
import {HelperServiceCase} from "../ts/helper/Helper.ServiceCase";
import {HelperServiceCaseTemplate} from "../ts/helper/Helper.ServiceCaseTemplate";
import {HelperServiceContract} from "../ts/helper/Helper.ServiceContract";
import {HelperServiceObject} from "../ts/helper/Helper.ServiceObject";
import {HelperServiceOrder} from "../ts/helper/Helper.ServiceOrder";
import {HelperServiceOrderMaterial} from "../ts/helper/Helper.ServiceOrderMaterial";
import {HelperServiceOrderTime} from "../ts/helper/Helper.ServiceOrderTime";
import {HelperServiceOrderTimePosting} from "../ts/helper/Helper.ServiceOrderTimePosting";
import {HelperStatisticsKey} from "../ts/helper/Helper.StatisticsKey";
import {CompanyDetailsInstallationsTabViewModel as CompanyDetailsInstallationsTabViewModelType} from "../ts/CompanyDetailsInstallationsTabViewModel";
import {CompanyDetailsServiceCasesTabViewModel as CompanyDetailsServiceCasesTabViewModelType} from "../ts/CompanyDetailsServiceCasesTabViewModel";
import {CompanyDetailsServiceContractsTabViewModel as CompanyDetailsServiceContractsTabViewModelType} from "../ts/CompanyDetailsServiceContractsTabViewModel";
import {CompanyDetailsServiceOrdersTabViewModel as CompanyDetailsServiceOrdersTabViewModelType} from "../ts/CompanyDetailsServiceOrdersTabViewModel";
import {DispatchAdHocViewModel as DispatchAdHocViewModelType} from "../ts/DispatchAdHocViewModel";
import {DispatchAppointmentModalViewModel as DispatchAppointmentModalViewModelType} from "../ts/DispatchAppointmentModalViewModel";
import {DispatchChangeStatusModalViewModel as DispatchChangeStatusModalViewModelType} from "../ts/DispatchChangeStatusModalViewModel";
import {DispatchDetailsDocumentsTabViewModel as DispatchDetailsDocumentsTabViewModelType} from "../ts/DispatchDetailsDocumentsTabViewModel";
import {DispatchDetailsInstallationsTabViewModel as DispatchDetailsInstallationsTabViewModelType} from "../ts/DispatchDetailsInstallationsTabViewModel";
import {DispatchDetailsJobsTabViewModel as DispatchDetailsJobsTabViewModelType} from "../ts/DispatchDetailsJobsTabViewModel";
import {DispatchDetailsMaterialsTabViewModel as DispatchDetailsMaterialsTabViewModelType} from "../ts/DispatchDetailsMaterialsTabViewModel";
import {DispatchDetailsNotesTabViewModel as DispatchDetailsNotesTabViewModelType} from "../ts/DispatchDetailsNotesTabViewModel";
import {DispatchDetailsRelatedOrdersTabViewModel as DispatchDetailsRelatedOrdersTabViewModelType} from "../ts/DispatchDetailsRelatedOrdersTabViewModel";
import {DispatchDetailsServiceCasesTabViewModel as DispatchDetailsServiceCasesTabViewModelType} from "../ts/DispatchDetailsServiceCasesTabViewModel";
import {DispatchDetailsTimePostingsTabViewModel as DispatchDetailsTimePostingsTabViewModelType} from "../ts/DispatchDetailsTimePostingsTabViewModel";
import {DispatchDetailsViewModel as DispatchDetailsViewModelType} from "../ts/DispatchDetailsViewModel";
import {DispatchDocumentAttributeEditModalViewModel as DispatchDocumentAttributeEditModalViewModelType} from "../ts/DispatchDocumentAttributeEditModalViewModel";
import {DispatchRejectModalViewModel as DispatchRejectModalViewModelType} from "../ts/DispatchRejectModalViewModel";
import {DispatchReportPreviewModalViewModel as DispatchReportPreviewModalViewModelType} from "../ts/DispatchReportPreviewModalViewModel";
import {DispatchReportRecipientsModalViewModel as DispatchReportRecipientsModalViewModelType} from "../ts/DispatchReportRecipientsModalViewModel";
import {DispatchReportViewModel as DispatchReportViewModelType} from "../ts/DispatchReportViewModel";
import {DispatchScheduleModalViewModel as DispatchScheduleModalViewModelType} from "../ts/DispatchScheduleModalViewModel";
import {DispatchSignatureEditModalViewModel as DispatchSignatureEditModalViewModelType} from "../ts/DispatchSignatureEditModalViewModel";
import {InstallationAddressRelationshipEditModalViewModel as InstallationAddressRelationshipEditModalViewModelType} from "../ts/InstallationAddressRelationshipEditModalViewModel";
import {InstallationCreateViewModel as InstallationCreateViewModelType} from "../ts/InstallationCreateViewModel";
import {InstallationDetailsDocumentsTabViewModel as InstallationDetailsDocumentsTabViewModelType} from "../ts/InstallationDetailsDocumentsTabViewModel";
import {InstallationDetailsNotesTabViewModel as InstallationDetailsNotesTabViewModelType} from "../ts/InstallationDetailsNotesTabViewModel";
import {InstallationDetailsPositionsTabViewModel as InstallationDetailsPositionsTabViewModelType} from "../ts/InstallationDetailsPositionsTabViewModel";
import {InstallationDetailsRelationshipsTabViewModel as InstallationDetailsRelationshipsTabViewModelType} from "../ts/InstallationDetailsRelationshipsTabViewModel";
import {InstallationDetailsServiceCasesTabViewModel as InstallationDetailsServiceCasesTabViewModelType} from "../ts/InstallationDetailsServiceCasesTabViewModel";
import {InstallationDetailsServiceOrdersTabViewModel as InstallationDetailsServiceOrdersTabViewModelType} from "../ts/InstallationDetailsServiceOrdersTabViewModel";
import {InstallationDetailsViewModel as InstallationDetailsViewModelType} from "../ts/InstallationDetailsViewModel";
import {InstallationDocumentAttributeEditModalViewModel as InstallationDocumentAttributeEditModalViewModelType} from "../ts/InstallationDocumentAttributeEditModalViewModel";
import {InstallationListIndexViewModel as InstallationListIndexViewModelType} from "../ts/InstallationListIndexViewModel";
import {InstallationPositionEditModalViewModel as InstallationPositionEditModalViewModelType} from "../ts/InstallationPositionEditModalViewModel";
import {InstallationPositionListIndexViewModel as InstallationPositionListIndexViewModelType} from "../ts/InstallationPositionListIndexViewModel";
import {MaintenancePlanEditModalViewModel as MaintenancePlanEditModalViewModelType} from "../ts/MaintenancePlanEditModalViewModel";
import {ReplenishmentOrderItemEditModalViewModel as ReplenishmentOrderItemEditModalViewModelType} from "../ts/ReplenishmentOrderItemEditModalViewModel";
import {ReplenishmentOrderItemListIndexViewModel as ReplenishmentOrderItemListIndexViewModelType} from "../ts/ReplenishmentOrderItemListIndexViewModel";
import {ReplenishmentOrderReportViewModel as ReplenishmentOrderReportViewModelType} from "../ts/ReplenishmentOrderReportViewModel";
import {ServiceCaseAddToServiceOrderModalViewModel as ServiceCaseAddToServiceOrderModalViewModelType} from "../ts/ServiceCaseAddToServiceOrderModalViewModel";
import {ServiceCaseCreateServiceOrderModalViewModel as ServiceCaseCreateServiceOrderModalViewModelType} from "../ts/ServiceCaseCreateServiceOrderModalViewModel";
import {ServiceCaseCreateViewModel as ServiceCaseCreateViewModelType} from "../ts/ServiceCaseCreateViewModel";
import {ServiceCaseDetailsNotesTabViewModel as ServiceCaseDetailsNotesTabViewModelType} from "../ts/ServiceCaseDetailsNotesTabViewModel";
import {ServiceCaseDetailsViewModel as ServiceCaseDetailsViewModelType} from "../ts/ServiceCaseDetailsViewModel";
import {ServiceCaseDetailsServiceOrdersTabViewModel as ServiceCaseDetailsServiceOrdersTabViewModelType} from "../ts/ServiceCaseDetailsServiceOrdersTabViewModel";
import {ServiceCaseListIndexViewModel as ServiceCaseListIndexViewModelType} from "../ts/ServiceCaseListIndexViewModel";
import {ServiceCaseSetStatusModalViewModel as ServiceCaseSetStatusModalViewModelType} from "../ts/ServiceCaseSetStatusModalViewModel";
import {ServiceCaseTemplateCreateViewModel as ServiceCaseTemplateCreateViewModelType} from "../ts/ServiceCaseTemplateCreateViewModel";
import {ServiceCaseTemplateDetailsViewModel as ServiceCaseTemplateDetailsViewModelType} from "../ts/ServiceCaseTemplateDetailsViewModel";
import {ServiceCaseTemplateListIndexViewModel as ServiceCaseTemplateListIndexViewModelType} from "../ts/ServiceCaseTemplateListIndexViewModel";
import {ServiceContractAddressRelationshipEditModalViewModel as ServiceContractAddressRelationshipEditModalViewModelType} from "../ts/ServiceContractAddressRelationshipEditModalViewModel";
import {ServiceContractCreateViewModel as ServiceContractCreateViewModelType} from "../ts/ServiceContractCreateViewModel";
import {ServiceContractDetailsDocumentsTabViewModel as ServiceContractDetailsDocumentsTabViewModelType} from "../ts/ServiceContractDetailsDocumentsTabViewModel";
import {ServiceContractDetailsInstallationsTabViewModel as ServiceContractDetailsInstallationsTabViewModelType} from "../ts/ServiceContractDetailsInstallationsTabViewModel";
import {ServiceContractDetailsMaintenancePlansTabViewModel as ServiceContractDetailsMaintenancePlansTabViewModelType} from "../ts/ServiceContractDetailsMaintenancePlansTabViewModel";
import {ServiceContractDetailsNotesTabViewModel as ServiceContractDetailsNotesTabViewModelType} from "../ts/ServiceContractDetailsNotesTabViewModel";
import {ServiceContractDetailsRelationshipsTabViewModel as ServiceContractDetailsRelationshipsTabViewModelType} from "../ts/ServiceContractDetailsRelationshipsTabViewModel";
import {ServiceContractDetailsServiceOrdersTabViewModel as ServiceContractDetailsServiceOrdersTabViewModelType} from "../ts/ServiceContractDetailsServiceOrdersTabViewModel";
import {ServiceContractDetailsViewModel as ServiceContractDetailsViewModelType} from "../ts/ServiceContractDetailsViewModel";
import {ServiceContractDocumentAttributeEditModalViewModel as ServiceContractDocumentAttributeEditModalViewModelType} from "../ts/ServiceContractDocumentAttributeEditModalViewModel";
import {ServiceContractInstallationRelationshipEditModalViewModel as ServiceContractInstallationRelationshipEditModalViewModelType} from "../ts/ServiceContractInstallationRelationshipEditModalViewModel";
import {ServiceContractListIndexViewModel as ServiceContractListIndexViewModelType} from "../ts/ServiceContractListIndexViewModel";
import {ServiceObjectAddInstallationModalViewModel as ServiceObjectAddInstallationModalViewModelType} from "../ts/ServiceObjectAddInstallationModalViewModel";
import {ServiceObjectCreateViewModel as ServiceObjectCreateViewModelType} from "../ts/ServiceObjectCreateViewModel";
import {ServiceObjectDetailsInstallationsTabViewModel as ServiceObjectDetailsInstallationsTabViewModelType} from "../ts/ServiceObjectDetailsInstallationsTabViewModel";
import {ServiceObjectDetailsNotesTabViewModel as ServiceObjectDetailsNotesTabViewModelType} from "../ts/ServiceObjectDetailsNotesTabViewModel";
import {ServiceObjectDetailsPersonsTabViewModel as ServiceObjectDetailsPersonsTabViewModelType} from "../ts/ServiceObjectDetailsPersonsTabViewModel";
import {ServiceObjectDetailsViewModel as ServiceObjectDetailsViewModelType} from "../ts/ServiceObjectDetailsViewModel";
import {ServiceObjectListIndexViewModel as ServiceObjectListIndexViewModelType} from "../ts/ServiceObjectListIndexViewModel";
import {ServiceOrderCloseModalViewModel as ServiceOrderCloseModalViewModelType} from "../ts/ServiceOrderCloseModalViewModel";
import {ServiceOrderCreateInvoiceModalViewModel as ServiceOrderCreateInvoiceModalViewModelType} from "../ts/ServiceOrderCreateInvoiceModalViewModel";
import {ServiceOrderCreateViewModel as ServiceOrderCreateViewModelType} from "../ts/ServiceOrderCreateViewModel";
import {ServiceOrderDetailsDispatchesTabViewModel as ServiceOrderDetailsDispatchesTabViewModelType} from "../ts/ServiceOrderDetailsDispatchesTabViewModel";
import {ServiceOrderDetailsDocumentsTabViewModel as ServiceOrderDetailsDocumentsTabViewModelType} from "../ts/ServiceOrderDetailsDocumentsTabViewModel";
import {ServiceOrderDetailsErrorTabViewModel as ServiceOrderDetailsErrorTabViewModelType} from "../ts/ServiceOrderDetailsErrorTabViewModel";
import {ServiceOrderDetailsInstallationsTabViewModel as ServiceOrderDetailsInstallationsTabViewModelType} from "../ts/ServiceOrderDetailsInstallationsTabViewModel";
import {ServiceOrderDetailsJobsTabViewModel as ServiceOrderDetailsJobsTabViewModelType} from "../ts/ServiceOrderDetailsJobsTabViewModel";
import {ServiceOrderDetailsJobsTabViewModelBase as ServiceOrderDetailsJobsTabViewModelBaseType} from "../ts/ServiceOrderDetailsJobsTabViewModel";
import {ServiceOrderDetailsMaterialsTabViewModel as ServiceOrderDetailsMaterialsTabViewModelType} from "../ts/ServiceOrderDetailsMaterialsTabViewModel";
import {ServiceOrderDetailsNotesTabViewModel as ServiceOrderDetailsNotesTabViewModelType} from "../ts/ServiceOrderDetailsNotesTabViewModel";
import {ServiceOrderDetailsServiceCasesTabViewModel as ServiceOrderDetailsServiceCasesTabViewModelType} from "../ts/ServiceOrderDetailsServiceCasesTabViewModel";
import {ServiceOrderDetailsTimePostingsTabViewModel as ServiceOrderDetailsTimePostingsTabViewModelType} from "../ts/ServiceOrderDetailsTimePostingsTabViewModel";
import {ServiceOrderDetailsViewModel as ServiceOrderDetailsViewModelType} from "../ts/ServiceOrderDetailsViewModel";
import {ServiceOrderDispatchListIndexViewModel as ServiceOrderDispatchListIndexViewModelType} from "../ts/ServiceOrderDispatchListIndexViewModel";
import {ServiceOrderHeadListIndexViewModel as ServiceOrderHeadListIndexViewModelType} from "../ts/ServiceOrderHeadListIndexViewModel";
import {ServiceOrderMaterialEditModalViewModel as ServiceOrderMaterialEditModalViewModelType} from "../ts/ServiceOrderMaterialEditModalViewModel";
import {ServiceOrderMaterialReportScheduledMaterialModalViewModel as ServiceOrderMaterialReportScheduledMaterialModalViewModelType} from "../ts/ServiceOrderMaterialReportScheduledMaterialModalViewModel";
import {ServiceOrderNoInvoiceModalViewModel as ServiceOrderNoInvoiceModalViewModelType} from "../ts/ServiceOrderNoInvoiceModalViewModel";
import {ServiceOrderReportViewModel as ServiceOrderReportViewModelType} from "../ts/ServiceOrderReportViewModel";
import {ServiceOrderTemplateCreateViewModel as ServiceOrderTemplateCreateViewModelType} from "../ts/ServiceOrderTemplateCreateViewModel";
import {ServiceOrderTemplateDetailsDocumentsTabViewModel as ServiceOrderTemplateDetailsDocumentsTabViewModelType} from "../ts/ServiceOrderTemplateDetailsDocumentsTabViewModel";
import {ServiceOrderTemplateDetailsJobsTabViewModel as ServiceOrderTemplateDetailsJobsTabViewModelType} from "../ts/ServiceOrderTemplateDetailsJobsTabViewModel";
import {ServiceOrderTemplateDetailsMaterialsTabViewModel as ServiceOrderTemplateDetailsMaterialsTabViewModelType} from "../ts/ServiceOrderTemplateDetailsMaterialsTabViewModel";
import {ServiceOrderTemplateDetailsNotesTabViewModel as ServiceOrderTemplateDetailsNotesTabViewModelType} from "../ts/ServiceOrderTemplateDetailsNotesTabViewModel";
import {ServiceOrderTemplateDetailsTimePostingsTabViewModel as ServiceOrderTemplateDetailsTimePostingsTabViewModelType} from "../ts/ServiceOrderTemplateDetailsTimePostingsTabViewModel";
import {ServiceOrderTemplateDetailsViewModel as ServiceOrderTemplateDetailsViewModelType} from "../ts/ServiceOrderTemplateDetailsViewModel";
import {ServiceOrderTemplateListIndexViewModel as ServiceOrderTemplateListIndexViewModelType} from "../ts/ServiceOrderTemplateListIndexViewModel";
import {ServiceOrderTimeEditModalViewModel as ServiceOrderTimeEditModalViewModelType} from "../ts/ServiceOrderTimeEditModalViewModel";
import {ServiceOrderTimePostingEditModalViewModel as ServiceOrderTimePostingEditModalViewModelType} from "../ts/ServiceOrderTimePostingEditModalViewModel";
import {ServiceOrderTimePostingDispatchRelinkModalViewModel as ServiceOrderTimePostingDispatchRelinkModalViewModelType} from "../ts/ServiceOrderTimePostingDispatchRelinkModalViewModel";
import {StatisticsKeyEditModalViewModel as StatisticsKeyEditModalViewModelType} from "../ts/StatisticsKeyEditModalViewModel";
import {StatisticsKeyInfoViewModel as StatisticsKeyInfoViewModelType} from "../ts/StatisticsKeyInfoViewModel";
import {ServiceObjectDetailsServiceOrdersTabViewModel as ServiceObjectDetailsServiceOrdersTabViewModelType } from "../ts/ServiceObjectDetailsServiceOrdersTabViewModel";
import {ServiceObjectDetailsServiceCasesTabViewModel as ServiceObjectDetailsServiceCasesTabViewModelType } from "../ts/ServiceObjectDetailsServiceCasesTabViewModel";
import {ServiceObjectDetailsServiceContractsTabViewModel as ServiceObjectDetailsServiceContractsTabViewModelType } from "../ts/ServiceObjectDetailsServiceContractsTabViewModel";
import {ServiceCaseAddContactModalViewModel as ServiceCaseAddContactModalViewModelType } from "../ts/ServiceCaseAddContactModalViewModel";
import type {HelperServiceOrderMaterial} from "@Crm.Service/helper/Helper.ServiceOrderMaterial";

declare global {
	namespace Crm {
		namespace Service {
			namespace ServiceOrderMaterialType {
				let Invoice: string;
				let Preplanned: string;
				let Used: string;
			}
			namespace ServiceOrderTimePostingType {
				let Invoice: string;
				let Preplanned: string;
				let Used: string;
			}
			namespace Settings {
				namespace AdHoc {
					let AdHocNumberingSequenceName: string
				}
				namespace Dispatch {
					let DispatchNoIsCreateable: boolean;
					let DispatchNoIsEditable: boolean;
					let DispatchNoIsGenerated: boolean;
					let SuppressEmptyMaterialsInReport: boolean;
					let SuppressEmptyExpensePostingsInReport: boolean;
					let SuppressExpensePostingsInReport: boolean;
					let SuppressEmptyTimePostingsInReport: boolean;
					let SuppressEmptyJobsInReport: boolean;
					let SuppressEmptyErrorTypesInReport: boolean;
					let SuppressErrorTypesInReport: boolean;
					let RestrictEditingToActiveJob: boolean;
				}
				namespace Email {
					let ClosedByRecipientForReplenishmentReport: boolean;
					let DispatchReportRecipients: string;
					let ReplenishmentOrderRecipients: string;
					let SendDispatchFollowUpOrderNotificationEmails: boolean;
					let SendDispatchNotificationEmails: boolean;
					let SendDispatchRejectNotificationEmails: boolean;
					let SendDispatchReportToDispatcher: boolean;
					let SendDispatchReportToTechnician: boolean;
					let SendDispatchReportsOnCompletion: boolean;
					let SendServiceOrderReportToDispatchers: boolean;
					let SendServiceOrderReportsOnCompletion: boolean;
				}
				namespace Export {
					let ExportDispatchReportsControlFileContent: string;
					let ExportDispatchReportsControlFileExtension: string;
					let ExportDispatchReportsControlFilePattern: string;
					let ExportDispatchReportsFilePattern: string;
					let ExportDispatchReportsOnCompletion: boolean;
					let ExportDispatchReportsPath: string;
					let ExportDispatchReportsUncDomain: string;
					let ExportDispatchReportsUncPassword: string;
					let ExportDispatchReportsUncUser: string;
					let ExportServiceOrderReportsOnCompletion: boolean;
					let ExportServiceOrderReportsPath: string;
					let ExportServiceOrderReportsUncDomain: string;
					let ExportServiceOrderReportsUncPassword: string;
					let ExportServiceOrderReportsUncUser: string;
				}
				let InstallationNoIsCreateable: boolean;
				let InstallationNoIsEditable: boolean;
				let InstallationNoIsGenerated: boolean;
				let PosNoGenerationMethod: string;
				namespace ReplenishmentOrder {
					let ClosedReplenishmentOrderHistorySyncPeriod: string;
				}
				namespace Service {
					namespace Dispatch {
						namespace Requires {
							let CustomerSignature: boolean;
						}
						namespace Show {
							let EmptyTimesOrMaterialsWarning: string;
							let PrivacyPolicy: boolean;
						}
					}
					namespace Installation {
						namespace Position {
							let TreeLevelDisplayLimit: string;
						}
					}
				}
				namespace ServiceCase {
					let OnlyInstallationsOfReferencedCustomer: boolean;
					let ServiceCaseNoIsCreateable: boolean;
					let ServiceCaseNoIsEditable: boolean;
					let ServiceCaseNoIsGenerated: boolean;
					namespace Signature {
						namespace Enable {
							let Originator: boolean;
							let Technician: boolean;
						}
					}
					let ShowsRelatedServiceCasesOnInstallationSelection: boolean;
				}
				namespace ServiceContract {
					let CreateMaintenanceOrderTimeSpanDays: string;
					let MaintenanceOrderGenerationMode: string;
					namespace MaintenancePlan {
						let AvailableTimeUnits: string;
					}
					let OnlyInstallationsOfReferencedCustomer: boolean;
					namespace ReactionTime {
						let AvailableTimeUnits: string;
					}
					let ServiceContractNoIsCreateable: boolean;
					let ServiceContractNoIsEditable: boolean;
					let ServiceContractNoIsGenerated: boolean;
				}
				namespace ServiceObject {
					let ObjectNoIsCreateable: boolean;
					let ObjectNoIsEditable: boolean;
					let ObjectNoIsGenerated: boolean;
				}
				namespace ServiceOrder {
					let DefaultDuration: string;
					let GenerateAndAttachJobsToUnattachedTimePostings: boolean;
					let OnlyInstallationsOfReferencedCustomer: boolean;
					let OrderNoIsCreateable: boolean;
					let OrderNoIsEditable: boolean;
					let OrderNoIsGenerated: boolean;
					let ShowOrderNoBarcode: boolean;
				}
				namespace ServiceOrderDispatch {
					let CustomerSignatureIncludesLegacyId: boolean;
					let ReadGeolocationOnDispatchStart: boolean;
					let ToggleSingleJob: boolean;
				}
				namespace ServiceOrderMaterial {
					let CreateReplenishmentOrderItemsFromServiceOrderMaterial: boolean;
					let ShowPricesInMobileClient: boolean;
					let ValidateStock: boolean;
				}
				namespace ServiceOrderTimePosting {
					let ClosedTimePostingsHistorySyncPeriod: string;
					let MaxDaysAgo: string;
					let MinutesInterval: string;
					let ShowTechnicianSelection: boolean;
					let AllowOverlap: boolean;
					let MultipleTechnicianSelection: boolean;
					let ShowUserGroupAndRoleSelection: boolean;
					
				}
				namespace ServiceOrderExpensePosting {
					let UseArticleAsExpenseType: boolean;
				}
				namespace ServiceOrderTimes {
					let ShowPricesInMobileClient: boolean;
				}
				namespace UserExtension {
					let OnlyUnusedLocationNosSelectable: boolean;
				}
				namespace ArticleInstallations {
					let FilterByItemNo: boolean;
				}
			}
			namespace ViewModels {
				let CompanyDetailsInstallationsTabViewModel: typeof CompanyDetailsInstallationsTabViewModelType;
				let CompanyDetailsServiceCasesTabViewModel: typeof CompanyDetailsServiceCasesTabViewModelType;
				let CompanyDetailsServiceContractsTabViewModel: typeof CompanyDetailsServiceContractsTabViewModelType;
				let CompanyDetailsServiceOrdersTabViewModel: typeof CompanyDetailsServiceOrdersTabViewModelType;
				let DispatchAdHocViewModel: typeof DispatchAdHocViewModelType;
				let DispatchAppointmentModalViewModel: typeof DispatchAppointmentModalViewModelType;
				let DispatchChangeStatusModalViewModel: typeof DispatchChangeStatusModalViewModelType;
				let DispatchDetailsDocumentsTabViewModel: typeof DispatchDetailsDocumentsTabViewModelType;
				let DispatchDetailsInstallationsTabViewModel: typeof DispatchDetailsInstallationsTabViewModelType;
				let DispatchDetailsJobsTabViewModel: typeof DispatchDetailsJobsTabViewModelType;
				let DispatchDetailsMaterialsTabViewModel: typeof DispatchDetailsMaterialsTabViewModelType;
				let DispatchDetailsNotesTabViewModel: typeof DispatchDetailsNotesTabViewModelType;
				let DispatchDetailsRelatedOrdersTabViewModel: typeof DispatchDetailsRelatedOrdersTabViewModelType;
				let DispatchDetailsServiceCasesTabViewModel: typeof DispatchDetailsServiceCasesTabViewModelType;
				let DispatchDetailsTimePostingsTabViewModel: typeof DispatchDetailsTimePostingsTabViewModelType;
				let DispatchDetailsViewModel: typeof DispatchDetailsViewModelType;
				let DispatchDocumentAttributeEditModalViewModel: typeof DispatchDocumentAttributeEditModalViewModelType;
				let DispatchRejectModalViewModel: typeof DispatchRejectModalViewModelType;
				let DispatchReportPreviewModalViewModel: typeof DispatchReportPreviewModalViewModelType;
				let DispatchReportRecipientsModalViewModel: typeof DispatchReportRecipientsModalViewModelType;
				let DispatchReportViewModel: typeof DispatchReportViewModelType;
				let DispatchScheduleModalViewModel: typeof DispatchScheduleModalViewModelType;
				let DispatchSignatureEditModalViewModel: typeof DispatchSignatureEditModalViewModelType;
				let InstallationAddressRelationshipEditModalViewModel: typeof InstallationAddressRelationshipEditModalViewModelType;
				let InstallationCreateViewModel: typeof InstallationCreateViewModelType;
				let InstallationDetailsDocumentsTabViewModel: typeof InstallationDetailsDocumentsTabViewModelType;
				let InstallationDetailsNotesTabViewModel: typeof InstallationDetailsNotesTabViewModelType;
				let InstallationDetailsPositionsTabViewModel: typeof InstallationDetailsPositionsTabViewModelType;
				let InstallationDetailsRelationshipsTabViewModel: typeof InstallationDetailsRelationshipsTabViewModelType;
				let InstallationDetailsServiceCasesTabViewModel: typeof InstallationDetailsServiceCasesTabViewModelType;
				let InstallationDetailsServiceOrdersTabViewModel: typeof InstallationDetailsServiceOrdersTabViewModelType;
				let InstallationDetailsViewModel: typeof InstallationDetailsViewModelType;
				let InstallationDocumentAttributeEditModalViewModel: typeof InstallationDocumentAttributeEditModalViewModelType;
				let InstallationListIndexViewModel: typeof InstallationListIndexViewModelType;
				let InstallationPositionEditModalViewModel: typeof InstallationPositionEditModalViewModelType;
				let InstallationPositionListIndexViewModel: typeof InstallationPositionListIndexViewModelType;
				let MaintenancePlanEditModalViewModel: typeof MaintenancePlanEditModalViewModelType;
				let ReplenishmentOrderItemEditModalViewModel: typeof ReplenishmentOrderItemEditModalViewModelType;
				let ReplenishmentOrderItemListIndexViewModel: typeof ReplenishmentOrderItemListIndexViewModelType;
				let ReplenishmentOrderReportViewModel: typeof ReplenishmentOrderReportViewModelType;
				let ServiceCaseAddToServiceOrderModalViewModel: typeof ServiceCaseAddToServiceOrderModalViewModelType;
				let ServiceCaseCreateServiceOrderModalViewModel: typeof ServiceCaseCreateServiceOrderModalViewModelType;
				let ServiceCaseCreateViewModel: typeof ServiceCaseCreateViewModelType;
				let ServiceCaseDetailsNotesTabViewModel: typeof ServiceCaseDetailsNotesTabViewModelType;
				let ServiceCaseDetailsViewModel: typeof ServiceCaseDetailsViewModelType;
				let ServiceCaseDetailsServiceOrdersTabViewModel: typeof ServiceCaseDetailsServiceOrdersTabViewModelType;
				let ServiceCaseListIndexViewModel: typeof ServiceCaseListIndexViewModelType;
				let ServiceCaseSetStatusModalViewModel: typeof ServiceCaseSetStatusModalViewModelType;
				let ServiceCaseTemplateCreateViewModel: typeof ServiceCaseTemplateCreateViewModelType;
				let ServiceCaseTemplateDetailsViewModel: typeof ServiceCaseTemplateDetailsViewModelType;
				let ServiceCaseTemplateListIndexViewModel: typeof ServiceCaseTemplateListIndexViewModelType;
				let ServiceContractAddressRelationshipEditModalViewModel: typeof ServiceContractAddressRelationshipEditModalViewModelType;
				let ServiceContractCreateViewModel: typeof ServiceContractCreateViewModelType;
				let ServiceContractDetailsDocumentsTabViewModel: typeof ServiceContractDetailsDocumentsTabViewModelType;
				let ServiceContractDetailsInstallationsTabViewModel: typeof ServiceContractDetailsInstallationsTabViewModelType;
				let ServiceContractDetailsMaintenancePlansTabViewModel: typeof ServiceContractDetailsMaintenancePlansTabViewModelType;
				let ServiceContractDetailsNotesTabViewModel: typeof ServiceContractDetailsNotesTabViewModelType;
				let ServiceContractDetailsRelationshipsTabViewModel: typeof ServiceContractDetailsRelationshipsTabViewModelType;
				let ServiceContractDetailsServiceOrdersTabViewModel: typeof ServiceContractDetailsServiceOrdersTabViewModelType;
				let ServiceContractDetailsViewModel: typeof ServiceContractDetailsViewModelType;
				let ServiceContractDocumentAttributeEditModalViewModel: typeof ServiceContractDocumentAttributeEditModalViewModelType;
				let ServiceContractInstallationRelationshipEditModalViewModel: typeof ServiceContractInstallationRelationshipEditModalViewModelType;
				let ServiceContractListIndexViewModel: typeof ServiceContractListIndexViewModelType;
				let ServiceObjectAddInstallationModalViewModel: typeof ServiceObjectAddInstallationModalViewModelType;
				let ServiceObjectCreateViewModel: typeof ServiceObjectCreateViewModelType;
				let ServiceObjectDetailsInstallationsTabViewModel: typeof ServiceObjectDetailsInstallationsTabViewModelType;
				let ServiceObjectDetailsNotesTabViewModel: typeof ServiceObjectDetailsNotesTabViewModelType;
				let ServiceObjectDetailsPersonsTabViewModel: typeof ServiceObjectDetailsPersonsTabViewModelType;
				let ServiceObjectDetailsViewModel: typeof ServiceObjectDetailsViewModelType;
				let ServiceObjectListIndexViewModel: typeof ServiceObjectListIndexViewModelType;
				let ServiceOrderCloseModalViewModel: typeof ServiceOrderCloseModalViewModelType;
				let ServiceOrderCreateInvoiceModalViewModel: typeof ServiceOrderCreateInvoiceModalViewModelType;
				let ServiceOrderCreateViewModel: typeof ServiceOrderCreateViewModelType;
				let ServiceOrderDetailsDispatchesTabViewModel: typeof ServiceOrderDetailsDispatchesTabViewModelType;
				let ServiceOrderDetailsDocumentsTabViewModel: typeof ServiceOrderDetailsDocumentsTabViewModelType;
				let ServiceOrderDetailsErrorTabViewModel: typeof ServiceOrderDetailsErrorTabViewModelType;
				let ServiceOrderDetailsInstallationsTabViewModel: typeof ServiceOrderDetailsInstallationsTabViewModelType;
				let ServiceOrderDetailsJobsTabViewModel: typeof ServiceOrderDetailsJobsTabViewModelType;
				let ServiceOrderDetailsJobsTabViewModelBase: typeof ServiceOrderDetailsJobsTabViewModelBaseType;
				let ServiceOrderDetailsMaterialsTabViewModel: typeof ServiceOrderDetailsMaterialsTabViewModelType;
				let ServiceOrderDetailsNotesTabViewModel: typeof ServiceOrderDetailsNotesTabViewModelType;
				let ServiceOrderDetailsServiceCasesTabViewModel: typeof ServiceOrderDetailsServiceCasesTabViewModelType;
				let ServiceOrderDetailsTimePostingsTabViewModel: typeof ServiceOrderDetailsTimePostingsTabViewModelType;
				let ServiceOrderDetailsViewModel: typeof ServiceOrderDetailsViewModelType;
				let ServiceOrderDispatchListIndexViewModel: typeof ServiceOrderDispatchListIndexViewModelType;
				let ServiceOrderHeadListIndexViewModel: typeof ServiceOrderHeadListIndexViewModelType;
				let ServiceOrderMaterialEditModalViewModel: typeof ServiceOrderMaterialEditModalViewModelType;
				let ServiceOrderMaterialReportScheduledMaterialModalViewModel: typeof ServiceOrderMaterialReportScheduledMaterialModalViewModelType;
				let ServiceOrderNoInvoiceModalViewModel: typeof ServiceOrderNoInvoiceModalViewModelType;
				let ServiceOrderReportViewModel: typeof ServiceOrderReportViewModelType;
				let ServiceOrderTemplateCreateViewModel: typeof ServiceOrderTemplateCreateViewModelType;
				let ServiceOrderTemplateDetailsDocumentsTabViewModel: typeof ServiceOrderTemplateDetailsDocumentsTabViewModelType;
				let ServiceOrderTemplateDetailsJobsTabViewModel: typeof ServiceOrderTemplateDetailsJobsTabViewModelType;
				let ServiceOrderTemplateDetailsMaterialsTabViewModel: typeof ServiceOrderTemplateDetailsMaterialsTabViewModelType;
				let ServiceOrderTemplateDetailsNotesTabViewModel: typeof ServiceOrderTemplateDetailsNotesTabViewModelType;
				let ServiceOrderTemplateDetailsTimePostingsTabViewModel: typeof ServiceOrderTemplateDetailsTimePostingsTabViewModelType;
				let ServiceOrderTemplateDetailsViewModel: typeof ServiceOrderTemplateDetailsViewModelType;
				let ServiceOrderTemplateListIndexViewModel: typeof ServiceOrderTemplateListIndexViewModelType;
				let ServiceOrderTimeEditModalViewModel: typeof ServiceOrderTimeEditModalViewModelType;
				let ServiceOrderTimePostingEditModalViewModel: typeof ServiceOrderTimePostingEditModalViewModelType;
				let ServiceOrderTimePostingDispatchRelinkModalViewModel: typeof ServiceOrderTimePostingDispatchRelinkModalViewModelType;
				let StatisticsKeyEditModalViewModel: typeof StatisticsKeyEditModalViewModelType;
				let StatisticsKeyInfoViewModel: typeof StatisticsKeyInfoViewModelType;
				let ServiceObjectDetailsServiceOrdersTabViewModel: typeof ServiceObjectDetailsServiceOrdersTabViewModelType;
				let ServiceObjectDetailsServiceCasesTabViewModel: typeof ServiceObjectDetailsServiceCasesTabViewModelType;
				let ServiceObjectDetailsServiceContractsTabViewModel: typeof ServiceObjectDetailsServiceContractsTabViewModelType ;
				let ServiceCaseAddContactModalViewModel: typeof ServiceCaseAddContactModalViewModelType;

			}
		}
	}
	namespace Helper {
		let Dispatch: typeof HelperDispatch;
		let Installation: typeof HelperInstallation;
		let InstallationPosition: typeof HelperInstallationPosition;
		let ReplenishmentOrder: typeof HelperReplenishmentOrder;
		let Service: typeof HelperService;
		let ServiceCase: typeof HelperServiceCase;
		let ServiceCaseTemplate: typeof HelperServiceCaseTemplate;
		let ServiceContract: typeof HelperServiceContract;
		let ServiceObject: typeof HelperServiceObject;
		let ServiceOrder: typeof HelperServiceOrder;
		let ServiceOrderMaterial: typeof HelperServiceOrderMaterial;
		let ServiceOrderTime: typeof HelperServiceOrderTime;
		let ServiceOrderTimePosting: typeof HelperServiceOrderTimePosting;
		let StatisticsKey: typeof HelperStatisticsKey;
	}
}
