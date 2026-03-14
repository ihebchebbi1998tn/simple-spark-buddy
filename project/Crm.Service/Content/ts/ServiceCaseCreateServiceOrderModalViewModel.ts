import {namespace} from "@Main/namespace";
import type {ServiceCaseListIndexViewModel} from "./ServiceCaseListIndexViewModel";

export class ServiceCaseCreateServiceOrderModalViewModel extends window.Main.ViewModels.ViewModelBase {
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
	selectedAddress = ko.observable<Crm.Rest.Model.Crm_Address>(null);
	selectedServiceOrderType = ko.observable<Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderType>(null);
	serviceOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(null);
	showCompanySelection = ko.pureComputed<boolean>(() => {
		return this.multipleServiceCasesSelected() ||
			(this.arrayOrQueryable().length === 1 && !this.arrayOrQueryable()[0].AffectedCompanyKey());
	});
	showServiceObjectSelection = ko.pureComputed<boolean>(() => {
		return this.multipleServiceCasesSelected() ||
			(this.arrayOrQueryable().length === 1 && !this.arrayOrQueryable()[0].ServiceObjectId());
	});
	errors = ko.validation.group(this);
	lookups: LookupType = {
		skills: {$tableName: "Main_Skill"},
		assets: {$tableName: "Main_Asset"},
	};

	constructor(parentViewModel: ServiceCaseListIndexViewModel) {
		super();
		this.parentViewModel = parentViewModel;
		this.multipleServiceCasesSelected.subscribe(value => {
			if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation")
				this.jobPerServiceCase(value);
		});
	}

	addressFilter(query: $data.Queryable<Crm.Rest.Model.Crm_Address>, term: string): $data.Queryable<Crm.Rest.Model.Crm_Address> {
		const contactIds = [this.serviceOrder().CustomerContactId(), this.serviceOrder().ServiceObjectId()]
			.filter(Boolean);
		if (contactIds.length > 0) {
			query = query.filter("it.CompanyId in this.contactIds",
				{contactIds: contactIds});
		}
		if (term) {
			query = window.Helper.String.contains(query, term, ["Name1", "Name2", "Name3", "ZipCode", "City", "Street"]);
		}
		return query;
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		const serviceOrder = window.database.CrmService_ServiceOrderHead.defaultType.create();
		serviceOrder.Reported = new Date();
		serviceOrder.StatusKey = this.serviceOrderStatusKey;
		serviceOrder.LastActivity = new Date();
		let serviceCase: Crm.Service.Rest.Model.CrmService_ServiceCase = null;
		if (id) {
			serviceCase = await window.database.CrmService_ServiceCase
				.include("ServiceOrderErrorTypes")
				.find(id);
		}
		if (serviceCase) {
			serviceOrder.CustomerContactId = serviceCase.AffectedCompanyKey;
			serviceOrder.ErrorMessage = serviceCase.ErrorMessage;
			serviceOrder.InstallationId = serviceCase.AffectedInstallationKey;
			serviceOrder.PriorityKey = serviceCase.PriorityKey;
			serviceOrder.ResponsibleUser = serviceCase.ResponsibleUser;
			serviceOrder.ServiceObjectId = serviceCase.ServiceObjectId;
			serviceOrder.StationKey = serviceCase.StationKey;
			serviceOrder.RequiredSkillKeys = serviceCase.RequiredSkillKeys;
			serviceOrder.RequiredAssetKeys = serviceCase.RequiredAssetKeys;
			this.arrayOrQueryable([serviceCase.asKoObservable()]);
			if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "OrderPerInstallation") {
				serviceOrder.ServiceCaseKey = serviceCase.Id;
			}
		} else {
			this.arrayOrQueryable(this.parentViewModel.allItemsSelected() === true
				? this.parentViewModel.getFilterQuery(false, false)
				: this.parentViewModel.selectedItems());
			if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode ===
				"OrderPerInstallation") {
				serviceOrder.InstallationId =
					this.parentViewModel.selectedItems()[0].AffectedInstallationKey();
			}
		}
		window.database.add(serviceOrder);
		this.serviceOrder(serviceOrder.asKoObservable());
		await window.Helper.User.getCurrentUser();
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);

		let favorites = await window.database.CrmService_ServiceOrderType.filter(x => x.Favorite == true).toArray();
		let favorite = favorites.sort(function (a, b) {
			return a.SortOrder - b.SortOrder;
		})[0];
		if (favorite) {
			this.serviceOrder().TypeKey(favorite.Key);
		}

		if (id) {
			serviceCase.ServiceOrderErrorTypes.forEach((errorType) => {
				errorType.OrderId = serviceOrder.Id;
			})
			return;
		}
		let multiple = await this.parentViewModel.bulkSelectionHasMultipleSelectedValues("AffectedInstallationKey");
		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "OrderPerInstallation" &&
			multiple) {
			$(".modal:visible").modal("hide");
			window.swal(window.Helper.String.getTranslatedString("Error"),
				window.Helper.String.getTranslatedString(
					"CannotCreateServiceOrderForMultipleInstallationsInOrderPerInstallationMode"),
				"error");
		} else if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode ===
			"OrderPerInstallation" &&
			!multiple) {
			this.serviceOrder()
				.InstallationId(this.parentViewModel.selectedItems()[0].AffectedInstallationKey());
		}
		multiple = await this.parentViewModel.bulkSelectionHasMultipleSelectedValues("AffectedCompanyKey");
		if (!multiple) {
			this.serviceOrder()
				.CustomerContactId(this.parentViewModel.selectedItems()[0].AffectedCompanyKey());
		}
		multiple = await this.parentViewModel.bulkSelectionHasMultipleSelectedValues("ErrorMessage");
		let multipleErrorMessagesSelected = multiple;
		if (!multiple) {
			this.serviceOrder()
				.ErrorMessage(this.parentViewModel.selectedItems()[0].ErrorMessage());
		}
		multiple = await this.parentViewModel.bulkSelectionHasMultipleSelectedValues("ServiceObjectId");
		if (!multiple) {
			this.serviceOrder()
				.ServiceObjectId(this.parentViewModel.selectedItems()[0].ServiceObjectId());
		}
		multiple = await this.parentViewModel.bulkSelectionHasMultipleSelectedValues("PriorityKey");
		if (!multiple) {
			this.serviceOrder()
				.PriorityKey(this.parentViewModel.selectedItems()[0].PriorityKey());
		}
		multiple = await this.parentViewModel.bulkSelectionHasMultipleSelectedValues("ResponsibleUser");
		if (!multiple) {
			this.serviceOrder()
				.ResponsibleUser(this.parentViewModel.selectedItems()[0].ResponsibleUser());
		}
		multiple = await this.parentViewModel.bulkSelectionHasMultipleSelectedValues("StationKey");
		if (!multiple) {
			this.serviceOrder()
				.StationKey(this.parentViewModel.selectedItems()[0].StationKey());
		}
	};

	serviceOrderStatusKey = "New";

	async submit() {
		if (this.errors().length > 0) {
			this.errors.showAllMessages();
			return;
		}
		this.errors.showAllMessages(false);
		this.loading(true);

		let serviceOrderNo = await window.NumberingService.createNewNumberBasedOnAppSettings(window.Crm.Service.Settings.ServiceOrder.OrderNoIsGenerated, window.Crm.Service.Settings.ServiceOrder.OrderNoIsCreateable, this.serviceOrder().OrderNo(), this.selectedServiceOrderType().NumberingSequence || "SMS.ServiceOrderHead.ServiceOrder", window.database.CrmService_ServiceOrderHead, "OrderNo");
		if (serviceOrderNo !== null) {
			this.serviceOrder().Name(serviceOrderNo);
			this.serviceOrder().OrderNo(serviceOrderNo);
			await new window.Helper.ServiceOrder.CreateServiceOrderData(this.serviceOrder(),
				this.serviceOrder().ServiceOrderTemplate(),
				[]).create();
		} else {
			this.serviceOrder().Name(this.serviceOrder().OrderNo())
		}
		let serviceOrderTimeId = null;
		let maxPosNo = 0;
		if (!!this.serviceOrder().ServiceOrderTemplate()) {
			maxPosNo = await window.database.CrmService_ServiceOrderTime
				.filter("it.OrderId === this.orderId", {
					orderId: this.serviceOrder().ServiceOrderTemplate().Id()
				}).count();

			maxPosNo += await window.database.CrmService_ServiceOrderMaterial
				.filter("it.OrderId === this.orderId", {
					orderId: this.serviceOrder().ServiceOrderTemplate().Id()
				}).count();
		}
		if (this.jobPerServiceCase() === false && window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation") {
			const newServiceOrderTime =
				window.database.CrmService_ServiceOrderTime.defaultType.create();
			newServiceOrderTime.DiscountType = Crm.Article.Model.Enums.DiscountType.Absolute;
			if (!this.multipleInstallationsSelected) {
				newServiceOrderTime.InstallationId =
					this.parentViewModel.selectedItems()[0].AffectedInstallationKey();
			}
			newServiceOrderTime.OrderId = this.serviceOrder().Id();
			newServiceOrderTime.PosNo = window.Helper.ServiceOrder.formatPosNo(maxPosNo + 1);
			window.database.add(newServiceOrderTime);
			serviceOrderTimeId = newServiceOrderTime.Id;
		}
		let arrayOrQueryable = this.arrayOrQueryable();
		if (Array.isArray(arrayOrQueryable)) {
			await window.Helper.ServiceCase.addServiceCasesToServiceOrder(arrayOrQueryable,
				this.serviceOrder().Id(),
				serviceOrderTimeId,
				maxPosNo);
		} else if (arrayOrQueryable instanceof $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>) {
			const pageSize = 25;
			let page = 0;
			const processNextPage = async () => {
				let serviceCases = await (arrayOrQueryable as $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>)
					.orderBy("it.Id")
					.skip(page * pageSize)
					.take(pageSize)
					.toArray();
				let results = await window.Helper.ServiceCase.addServiceCasesToServiceOrder(serviceCases,
					this.serviceOrder().Id(),
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
		window.location.hash = "/Crm.Service/ServiceOrder/DetailsTemplate/" + this.serviceOrder().Id();
	};

	selectedAddressOnSelect(selectedAddress: Crm.Rest.Model.Crm_Address): void {
		if (selectedAddress) {
			this.serviceOrder().Name1(selectedAddress.Name1);
			this.serviceOrder().Name2(selectedAddress.Name2);
			this.serviceOrder().Name3(selectedAddress.Name3);
			this.serviceOrder().Street(selectedAddress.Street);
			this.serviceOrder().ZipCode(selectedAddress.ZipCode);
			this.serviceOrder().City(selectedAddress.City);
			this.serviceOrder().CountryKey(selectedAddress.CountryKey);
			this.serviceOrder().RegionKey(selectedAddress.RegionKey);
		} else {
			this.serviceOrder().Name1(null);
			this.serviceOrder().Name2(null);
			this.serviceOrder().Name3(null);
			this.serviceOrder().Street(null);
			this.serviceOrder().ZipCode(null);
			this.serviceOrder().City(null);
			this.serviceOrder().CountryKey(null);
			this.serviceOrder().RegionKey(null);
		}
	};

	onServiceOrderTemplateSelect = window.Crm.Service.ViewModels.ServiceOrderCreateViewModel.prototype.onServiceOrderTemplateSelect;

	getSkillsFromKeys(keys: string[]): Select2AutoCompleterResult[] {
		return this.lookups.skills.$array.filter(function (x) {
			return keys.indexOf(x.Key) !== -1;
		}).map(window.Helper.Lookup.mapLookupForSelect2Display);
	};
	getAssetsFromKeys(keys: string[]): Select2AutoCompleterResult[] {
		return this.lookups.assets.$array.filter(function (x) {
			return keys.indexOf(x.Key) !== -1;
		}).map(window.Helper.Lookup.mapLookupForSelect2Display);
	};
}

namespace("Crm.Service.ViewModels").ServiceCaseCreateServiceOrderModalViewModel = ServiceCaseCreateServiceOrderModalViewModel;