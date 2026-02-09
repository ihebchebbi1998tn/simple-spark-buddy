import {namespace} from "@Main/namespace";
import { HelperServiceOrder } from "./helper/Helper.ServiceOrder";
import type {ServiceContractDetailsViewModel} from "./ServiceContractDetailsViewModel";

export class ServiceContractDetailsMaintenancePlansTabViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_MaintenancePlan, Crm.Service.Rest.Model.ObservableCrmService_MaintenancePlan> {
	parentViewModel: ServiceContractDetailsViewModel;
	lookups: LookupType = {
		timeUnits: {$tableName: "Main_TimeUnit"}
	};
	maintenanceOrderDefined = ko.observable<boolean>(false);

	constructor(parentViewModel: ServiceContractDetailsViewModel) {
		const joinServiceOrders = {
			Selector: "ServiceOrders",
			Operation: "filter(function(it2) { return it2.StatusKey in ['New', 'ReadyForScheduling', 'Scheduled', 'PartiallyReleased', 'Released', 'InProgress', 'PartiallyCompleted', 'Completed']; })"
		};
		super(
			"CrmService_MaintenancePlan",
			["Name", "NextDate"],
			["ASC", "ASC"],
			[joinServiceOrders]);
		this.parentViewModel = parentViewModel;
		this.getFilter("ServiceContractId").extend({filterOperator: "==="})(parentViewModel.serviceContract().Id());
	}

	async initItems(items: Crm.Service.Rest.Model.ObservableCrmService_MaintenancePlan[]): Promise<Crm.Service.Rest.Model.ObservableCrmService_MaintenancePlan[]> {
		const queries = [];
		items.forEach(function (item) {
			if (item.ServiceOrderTemplateId()) {
				queries.push({
					queryable: window.database.CrmService_ServiceOrderHead
						.filter("it.Id == this.id", {id: item.ServiceOrderTemplateId()}),
					method: "toArray",
					handler:
						function (serviceOrder) {
							item.ServiceOrderTemplate(serviceOrder[0].asKoObservable());
						}
				});
			}
		});

		if (queries.length > 0) {
			await window.Helper.Batch.Execute(queries);
		}
		return items;
	};

	calculateServiceDateExceedanceLevelStyle(date: Date, time: string ): string {
		const parentViewModel = this.parentViewModel;
		const contractType = parentViewModel.lookups.serviceContractTypes.$array.find(function (x) {
			return x.Key === parentViewModel.serviceContract().ContractTypeKey();
		});
		const gracePeriodInDays = contractType.GracePeriodInDays ?? 0;

		return HelperServiceOrder.calculateServiceDateExceedanceLevelStyle(date, time, gracePeriodInDays);
	};

	canGenerateServiceOrderForMaintenancePlan(maintenancePlan: Crm.Service.Rest.Model.ObservableCrmService_MaintenancePlan): boolean {
		return maintenancePlan.AllowPrematureMaintenance() &&
			window.AuthorizationManager.isAuthorizedForAction("ServiceContract", "Edit") &&
			this.parentViewModel.serviceContract().StatusKey() === "Active";
	};

	async generateServiceOrderForMaintenancePlan(maintenancePlan: Crm.Service.Rest.Model.ObservableCrmService_MaintenancePlan): Promise<void> {
		this.loading(true);
		try {
			await $.get(window.Helper.resolveUrl("~/Crm.Service/ServiceContract/GenerateOrderFromMaintenancePlan?maintenancePlanId=" +
				maintenancePlan.Id()));
			this.filter();
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};

	async init(): Promise<void> {
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await window.Helper.Lookup.getLocalizedArrayMap("CrmService_ServiceOrderType")
			.then(lookup => {
				this.maintenanceOrderDefined(lookup.$array.filter(l => l.MaintenanceOrder).length > 0);
			});
		await super.init();
	};

	async remove(maintenancePlan: Crm.Service.Rest.Model.ObservableCrmService_MaintenancePlan): Promise<void> {
		let confirm = await window.Helper.Confirm.confirmDeleteAsync();
		if (!confirm) {
			return;
		}
		this.loading(true);
		window.database.remove(maintenancePlan.innerInstance);
		await window.database.saveChanges();
	};

	getNextGenerationDates(maintenancePlan: Crm.Service.Rest.Model.ObservableCrmService_MaintenancePlan): void {
		window.swal("", Helper.ServiceContract.getNextXGenerationDates(maintenancePlan, 5).join("\n"), "info");
	};
}

namespace("Crm.Service.ViewModels").ServiceContractDetailsMaintenancePlansTabViewModel = ServiceContractDetailsMaintenancePlansTabViewModel;