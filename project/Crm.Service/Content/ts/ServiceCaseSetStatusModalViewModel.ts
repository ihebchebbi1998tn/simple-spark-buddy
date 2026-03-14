import {namespace} from "@Main/namespace";
import type {ServiceCaseListIndexViewModel} from "./ServiceCaseListIndexViewModel";

export class ServiceCaseSetStatusModalViewModel extends window.Main.ViewModels.ViewModelBase {
	parentViewModel: ServiceCaseListIndexViewModel;
	arrayOrQueryable: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase[] | $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>;
	statusKey = ko.observable<string>(null).extend({
		required: {
			message: window.Helper.String.getTranslatedString("RuleViolation.Required")
				.replace("{0}", window.Helper.String.getTranslatedString("ServiceCaseStatus")),
			params: true
		},
		validation: {
			async: true,
			validator: async (val, params, callback) => {
				if (this.statusKey() === null) {
					callback(true);
				}
				const invalidStatusKeys = this.lookups.serviceCaseStatuses.$array.filter(x => {
					if (x.Key === null) {
						return false;
					}
					const settableStatusKeys = (x.SettableStatuses || "").split(",").map(function (statusKey) {
						return parseInt(statusKey);
					});
					return settableStatusKeys.indexOf(this.statusKey()) === -1;
				}).map(function (x) {
					return x.Key;
				});
				let invalidServiceCases: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase[];
				if (Array.isArray(this.arrayOrQueryable)) {
					invalidServiceCases = this.arrayOrQueryable.filter(function (x) {
						return invalidStatusKeys.indexOf(x.StatusKey) !== -1;
					});
				} else {
					invalidServiceCases = (await this.parentViewModel.getFilterQuery(false,
						false,
						{"it.StatusKey in this.statusKeys": {statusKeys: invalidStatusKeys}})
						.withInlineCount()
						.take(10)
						.toArray()).map(x => x.asKoObservable());
				}
				if (invalidServiceCases.length === 0) {
					callback(true);
				} else {
					// @ts-ignore
					const additionalCount = invalidServiceCases.totalCount > 10 ? invalidServiceCases.totalCount - 10 : 0;
					let serviceCasesText = invalidServiceCases.map(window.Helper.ServiceCase.getDisplayName).join(",\r\n");
					if (additionalCount > 0) {
						serviceCasesText += "\r\n" +
							window.Helper.String.getTranslatedString("AndXAdditional")
								.replace("{0}", additionalCount.toString());
					}
					const message = window.Helper.String.getTranslatedString("StatusNotSettable").replace("{0}", serviceCasesText);
					callback({isValid: false, message: message});
				}
			}
		}
	});
	status = ko.pureComputed<Crm.Service.Rest.Model.Lookups.CrmService_ServiceCaseStatus>(() => {
		return this.statusKey() === null
			? null
			: this.lookups.serviceCaseStatuses.$array.find(x => x.Key === this.statusKey());
	});
	lookups: LookupType = {
		serviceCaseStatuses: {$tableName: "CrmService_ServiceCaseStatus"}
	};
	errors = ko.validation.group(this);

	constructor(parentViewModel: ServiceCaseListIndexViewModel) {
		super();
		this.parentViewModel = parentViewModel;
		this.arrayOrQueryable = this.parentViewModel.allItemsSelected() === true
			? this.parentViewModel.getFilterQuery(false, false)
			: this.parentViewModel.selectedItems();
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		return window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
	};

	async setStatus(serviceCases: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase[], status: Crm.Service.Rest.Model.Lookups.CrmService_ServiceCaseStatus): Promise<void> {
		serviceCases = serviceCases.map(function (serviceCase) {
			return window.Helper.Database.getDatabaseEntity(serviceCase);
		});
		serviceCases.forEach(function (serviceCase) {
			window.database.attachOrGet(serviceCase);
		});
		window.Helper.ServiceCase.setStatus(serviceCases, status);
		await window.database.saveChanges();
	};

	async submit(): Promise<void> {
		if (this.errors().length > 0) {
			this.errors.showAllMessages();
			return;
		}
		this.errors.showAllMessages(false);
		this.loading(true);
		if (Array.isArray(this.arrayOrQueryable)) {
			let array: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase[] = this.arrayOrQueryable;
			await this.setStatus(array, this.status());
		} else if (this.arrayOrQueryable instanceof $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>) {
			const pageSize = 25;
			let page = 0;
			let queryable: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase> = this.arrayOrQueryable;
			const processNextPage = async () => {
				let serviceCases = (await queryable
					.orderBy("it.Id")
					.skip(page * pageSize)
					.take(pageSize)
					.toArray()).map(x => x.asKoObservable());
				await this.setStatus(serviceCases, this.status());
				if (serviceCases.length === pageSize) {
					page++;
					await processNextPage();
				}
			};
			await processNextPage();
		}
		$(".modal:visible").modal("hide");
	};
}

namespace("Crm.Service.ViewModels").ServiceCaseSetStatusModalViewModel = ServiceCaseSetStatusModalViewModel;