import {namespace} from "@Main/namespace";
import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";
import type {ServiceCaseDetailsViewModel} from "./ServiceCaseDetailsViewModel";

export class StatisticsKeyEditModalViewModel extends window.Main.ViewModels.ViewModelBase {

	parentViewModel: ServiceOrderDetailsViewModel | ServiceCaseDetailsViewModel;
	originalEntity = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey | Crm.Service.Rest.Model.ObservableCrmService_ServiceCase>();
	entity = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey | Crm.Service.Rest.Model.ObservableCrmService_ServiceCase>();

	constructor(parentViewModel: ServiceOrderDetailsViewModel | ServiceCaseDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		if (!id && !params.tableName) {
			id = this.parentViewModel.contact().Id();
			params.tableName = this.parentViewModel.contact().innerInstance.getType().name;
			this.originalEntity(this.parentViewModel.contact());
		}
		let entity = id === window.Helper.String.emptyGuid() ? window.database.CrmService_ServiceOrderStatisticsKey.defaultType.create() : await window.database[params.tableName].find(id);
		this.entity(entity.asKoObservable());

		this.getProperty("CrmService_StatisticsKeyProductType").subscribe(lookup => {
			this.getProperty("CrmService_StatisticsKeyMainAssembly")(null);
			this.getProperty("CrmService_StatisticsKeySubAssembly")(null);
			this.getProperty("CrmService_StatisticsKeyAssemblyGroup")(null);
		});
		this.getProperty("CrmService_StatisticsKeyMainAssembly").subscribe(lookup => {
			this.getProperty("CrmService_StatisticsKeySubAssembly")(null);
			this.getProperty("CrmService_StatisticsKeyAssemblyGroup")(null);
		});
		this.getProperty("CrmService_StatisticsKeySubAssembly").subscribe(lookup => {
			this.getProperty("CrmService_StatisticsKeyAssemblyGroup")(null);
		});

		if (params.dispatchId && (this.entity() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey).DispatchId() === null) {
			let newEntity = window.Helper.Database.createClone(this.entity().innerInstance) as Crm.Service.Rest.Model.CrmService_ServiceOrderStatisticsKey;
			newEntity.Id = window.$data.createGuid().toString().toLowerCase();
			newEntity.DispatchId = params.dispatchId;
			window.database.add(newEntity);
			this.entity(newEntity.asKoObservable());
		} else if (params.serviceOrderId && (this.entity() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey).ServiceOrderId() === window.Helper.String.emptyGuid()) {
			(this.entity() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey).ServiceOrderId(params.serviceOrderId);
			window.database.add(this.entity().innerInstance);
		} else {
			window.database.attachOrGet(this.entity().innerInstance);
		}
	};

	getProperty(tableName: "CrmService_StatisticsKeyProductType" | "CrmService_StatisticsKeyMainAssembly" | "CrmService_StatisticsKeySubAssembly" | "CrmService_StatisticsKeyAssemblyGroup" | "CrmService_StatisticsKeyFaultImage" | "CrmService_StatisticsKeyRemedy" | "CrmService_StatisticsKeyCause" | "CrmService_StatisticsKeyWeighting" | "CrmService_StatisticsKeyCauser", entity?: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderStatisticsKey | Crm.Service.Rest.Model.ObservableCrmService_ServiceCase): KnockoutObservable<string> {
		entity ??= this.entity();
		return entity[window.Helper.StatisticsKey.getModelPropertyNameByTable(entity, tableName)];
	}

	async save(): Promise<void> {
		this.loading(true);
		try {
			await window.database.saveChanges();
			if (this.originalEntity()) {
				this.getProperty("CrmService_StatisticsKeyProductType", this.parentViewModel.contact())(this.getProperty("CrmService_StatisticsKeyProductType")());
				this.getProperty("CrmService_StatisticsKeyMainAssembly", this.parentViewModel.contact())(this.getProperty("CrmService_StatisticsKeyMainAssembly")());
				this.getProperty("CrmService_StatisticsKeySubAssembly", this.parentViewModel.contact())(this.getProperty("CrmService_StatisticsKeySubAssembly")());
				this.getProperty("CrmService_StatisticsKeyAssemblyGroup", this.parentViewModel.contact())(this.getProperty("CrmService_StatisticsKeyAssemblyGroup")());
				this.getProperty("CrmService_StatisticsKeyFaultImage", this.parentViewModel.contact())(this.getProperty("CrmService_StatisticsKeyFaultImage")());
				this.getProperty("CrmService_StatisticsKeyRemedy", this.parentViewModel.contact())(this.getProperty("CrmService_StatisticsKeyRemedy")());
				this.getProperty("CrmService_StatisticsKeyCause", this.parentViewModel.contact())(this.getProperty("CrmService_StatisticsKeyCause")());
				this.getProperty("CrmService_StatisticsKeyWeighting", this.parentViewModel.contact())(this.getProperty("CrmService_StatisticsKeyWeighting")());
				this.getProperty("CrmService_StatisticsKeyCauser", this.parentViewModel.contact())(this.getProperty("CrmService_StatisticsKeyCauser")());
			}
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	}

	onSelectStatisticsKey(entity: Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyAssemblyGroup | Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyCause | Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyCauser | Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyRemedy | Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyFaultImage | Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyMainAssembly | Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyProductType | Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeySubAssembly | Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyWeighting): void {
		if (!entity) {
			return;
		}
		const lookupName = window.Helper.StatisticsKey.getJsNameByTable(entity.getType().name);
		let lookups = this.parentViewModel.lookups[lookupName] as { [key: string]: any };
		lookups[entity.Key] = entity;
	};

	dispose(): void {
		window.database.detach(this.entity().innerInstance);
	}
}

namespace("Crm.Service.ViewModels").StatisticsKeyEditModalViewModel = StatisticsKeyEditModalViewModel;