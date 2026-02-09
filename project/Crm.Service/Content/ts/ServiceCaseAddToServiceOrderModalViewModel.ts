import {namespace} from "@Main/namespace";
import type {ServiceCaseListIndexViewModel} from "./ServiceCaseListIndexViewModel";

export class ServiceCaseAddToServiceOrderModalViewModel extends window.Main.ViewModels.ViewModelBase {
	parentViewModel: ServiceCaseListIndexViewModel;
	arrayOrQueryable = ko.observable<Array<Crm.Service.Rest.Model.ObservableCrmService_ServiceCase> | $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>>([]);
	multipleInstallationsSelected: boolean = false;
	multipleServiceCasesSelected = ko.pureComputed<boolean>(() => {
		return !Array.isArray(this.arrayOrQueryable()) || this.arrayOrQueryable().length > 1;
	});
	jobPerServiceCase = ko.observable<boolean>(null).extend({
		equal: {
			message: window.Helper.String.getTranslatedString("RuleViolation.Required")
				.replace("{0}", window.Helper.String.getTranslatedString("JobPerServiceCase")),
			onlyIf: () => {
				return this.multipleServiceCasesSelected() && this.multipleInstallationsSelected;
			},
			params: true
		}
	});
	installationId = ko.observable<string>(null);
	serviceObjectId = ko.observable<string>(null);
	serviceOrderId = ko.observable<string>(null).extend({
		required: {
			message: window.Helper.String.getTranslatedString("RuleViolation.Required")
				.replace("{0}", window.Helper.String.getTranslatedString("ServiceOrder")),
			params: true
		}
	});
	serviceOrderTimeId = ko.observable<string>(null);
	showJobSelection = ko.pureComputed<boolean>(() => {
		return !!this.serviceOrderId() &&
			!this.jobPerServiceCase() &&
			!this.multipleInstallationsSelected;
	});
	errors = ko.validation.group([this.jobPerServiceCase, this.serviceOrderId]);

	constructor(parentViewModel: ServiceCaseListIndexViewModel) {
		super();
		this.parentViewModel = parentViewModel;
		this.multipleServiceCasesSelected.subscribe(value => {
			this.jobPerServiceCase(value);
		});
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.jobPerServiceCase.subscribe(() => {
			this.serviceOrderTimeId(null);
		});
		this.serviceOrderId.subscribe(() => {
			this.serviceOrderTimeId(null);
		});
		let serviceCase: Crm.Service.Rest.Model.CrmService_ServiceCase = null;
		if (id) {
			serviceCase = await window.database.CrmService_ServiceCase
				.include("ServiceOrderErrorTypes")
				.find(id);
		}
		if (serviceCase) {
			this.arrayOrQueryable([serviceCase.asKoObservable()]);
			this.installationId(serviceCase.AffectedInstallationKey);
			this.multipleInstallationsSelected = false;
			this.serviceObjectId(serviceCase.ServiceObjectId);
		} else {
			this.arrayOrQueryable(this.parentViewModel.allItemsSelected() === true
				? this.parentViewModel.getFilterQuery(false, false)
				: this.parentViewModel.selectedItems());
		}
		if (id) {
			return;
		}
		let multiple = await this.parentViewModel.bulkSelectionHasMultipleSelectedValues("AffectedInstallationKey");
		this.multipleInstallationsSelected = multiple;
		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "OrderPerInstallation" &&
			this.multipleInstallationsSelected) {
			$(".modal:visible").modal("hide");
			window.swal(window.Helper.String.getTranslatedString("Error"),
				window.Helper.String.getTranslatedString(
					"CannotCreateServiceOrderForMultipleInstallationsInOrderPerInstallationMode"),
				"error");
		} else if (!multiple) {
			this.installationId(this.parentViewModel.selectedItems()[0].AffectedInstallationKey());
		}
		multiple = await this.parentViewModel.bulkSelectionHasMultipleSelectedValues("ServiceObjectId");
		if (!multiple) {
			this.serviceObjectId(this.parentViewModel.selectedItems()[0].ServiceObjectId());
		}
	};

	installationFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["InstallationNo", "Description"]);
		}
		if (this.serviceObjectId()) {
			query = query.filter("it.FolderId === this.serviceObjectId",
				{serviceObjectId: this.serviceObjectId()});
		}
		return query;
	};

	serviceObjectFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceObject>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceObject> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["ObjectNo", "Name"]);
		}
		return query;
	};

	serviceOrderFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["OrderNo", "ErrorMessage"]);
		}
		query = query.filter("it.StatusKey in this.statusKeys",
			{statusKeys: this.serviceOrderStatusKeys});
		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "OrderPerInstallation") {
			query = query.filter("it.InstallationId === this.installationId",
				{installationId: this.installationId()});
		} else if (this.installationId()) {
			if (window.database.storageProvider.name === "webSql") {
				query = query.filter("it.ServiceOrderTimes.InstallationId === this.installationId",
					{installationId: this.installationId()});
			} else {
				query = query.filter("it.ServiceOrderTimes.some(function(it2) { return it2.InstallationId === this.installationId; })",
					{installationId: this.installationId()});
			}
		}
		if (this.serviceObjectId()) {
			query = query.filter("it.ServiceObjectId === this.serviceObjectId",
				{serviceObjectId: this.serviceObjectId()});
		}
		return query;
	};

	serviceOrderStatusKeys = [
		"New", "ReadyForScheduling", "Scheduled", "PartiallyReleased", "Released", "InProgress", "PartiallyCompleted"
	];

	serviceOrderTimeFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["PosNo", "ItemNo", "Description"]);
		}
		if (this.installationId() &&
			window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation") {
			query = query.filter("it.InstallationId === this.installationId",
				{installationId: this.installationId()});
		}
		query = query.filter("it.OrderId === this.serviceOrderId",
			{serviceOrderId: this.serviceOrderId()});
		return query;
	};

	async submit(): Promise<void> {
		if (this.errors().length > 0) {
			this.errors.showAllMessages();
			return;
		}
		this.errors.showAllMessages(false);
		this.loading(true);
		let maxPosNo: number;
		let serviceOrderTimeId = this.serviceOrderTimeId();
		if (this.jobPerServiceCase() === false && !this.serviceOrderTimeId()) {
			const newServiceOrderTime = window.database.CrmService_ServiceOrderTime.defaultType.create();
			newServiceOrderTime.DiscountType = Crm.Article.Model.Enums.DiscountType.Absolute;
			if (!this.multipleInstallationsSelected &&
				window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation") {
				newServiceOrderTime.InstallationId = this.installationId();
			}
			newServiceOrderTime.OrderId = this.serviceOrderId();
			let arrayOrQueryableValue = this.arrayOrQueryable();
			if (Array.isArray(arrayOrQueryableValue) && arrayOrQueryableValue.length > 0) {
				const firstServiceCase = arrayOrQueryableValue[0];
				if (firstServiceCase?.ServiceCaseNo) {
					newServiceOrderTime.Description = firstServiceCase.ServiceCaseNo();
				}
			} else if (arrayOrQueryableValue instanceof $data.Queryable) {
				const firstServiceCase = await arrayOrQueryableValue.take(1).toArray();
				if (firstServiceCase?.length > 0 && firstServiceCase[0]?.ServiceCaseNo) {
					newServiceOrderTime.Description = firstServiceCase[0].ServiceCaseNo;
				}
			}
			await window.Helper.ServiceOrder.updateJobPosNo(newServiceOrderTime)
			window.database.add(newServiceOrderTime);
			serviceOrderTimeId = newServiceOrderTime.Id;
		} else {
			maxPosNo = await window.Helper.ServiceOrder.getMaxPosNo(this.serviceOrderId());
		}

		let arrayOrQueryable = this.arrayOrQueryable();
		if (Array.isArray(arrayOrQueryable)) {
			await window.Helper.ServiceCase.addServiceCasesToServiceOrder(arrayOrQueryable,
				this.serviceOrderId(),
				serviceOrderTimeId,
				maxPosNo);
		} else if (arrayOrQueryable instanceof $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>) {
			const pageSize = 25;
			let page = 0;
			const processNextPage = async () => {
				let serviceCases = await (arrayOrQueryable as $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>)
					.include("ServiceOrderErrorTypes")
					.orderBy("it.Id")
					.skip(page * pageSize)
					.take(pageSize)
					.toArray();
				let results = await window.Helper.ServiceCase.addServiceCasesToServiceOrder(serviceCases,
					this.serviceOrderId(),
					serviceOrderTimeId,
					maxPosNo
				);
				if (serviceCases.length === pageSize) {
					page++;
					maxPosNo = results.map(function (x) {
						return parseInt(x.PosNo);
					}).sort().pop();
					await processNextPage();
				}
			};
			await processNextPage();
		} else {
			throw "arrayOrQueryable is neither array nor queryable";
		}
		$(".modal:visible").modal("hide");
		window.location.hash = "/Crm.Service/ServiceOrder/DetailsTemplate/" + this.serviceOrderId();
	};
}

namespace("Crm.Service.ViewModels").ServiceCaseAddToServiceOrderModalViewModel = ServiceCaseAddToServiceOrderModalViewModel;