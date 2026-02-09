import {namespace} from "@Main/namespace";
import {Breadcrumb} from "@Main/breadcrumbs";

export class ServiceContractDetailsViewModel extends window.Crm.ViewModels.ContactDetailsViewModel {
	tabs = ko.observable<{}>({});
	serviceContract = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceContract>(null);
	lookups: LookupType = {
		countries: {$tableName: "Main_Country"},
		currencies: {$tableName: "Main_Currency"},
		paymentConditions: {$tableName: "Main_PaymentCondition"},
		paymentIntervals: {$tableName: "Main_PaymentInterval"},
		paymentTypes: {$tableName: "Main_PaymentType"},
		regions: {$tableName: "Main_Region"},
		serviceContractStatuses: {$tableName: "CrmService_ServiceContractStatus"},
		serviceContractTypes: {$tableName: "CrmService_ServiceContractType"},
		sparePartsBudgetInvoiceTypes: {$tableName: "CrmService_SparePartsBudgetInvoiceType"},
		sparePartsBudgetTimeSpanUnits: {$tableName: "CrmService_SparePartsBudgetTimeSpanUnit"},
		timeUnits: {$tableName: "Main_TimeUnit"}
	};
	settableStatuses = ko.pureComputed<Crm.Service.Rest.Model.Lookups.CrmService_ServiceContractStatus[]>(() => {
		const currentStatus = this.lookups.serviceContractStatuses.$array.find(x => x.Key === this.serviceContract().StatusKey());
		const settableStatusKeys = currentStatus
			? (currentStatus.SettableStatuses || "").split(",")
			: [];
		return this.lookups.serviceContractStatuses.$array.filter(function (x) {
			return x === currentStatus || settableStatusKeys.indexOf(x.Key) !== -1;
		});
	});
	canSetStatus = ko.pureComputed<boolean>(() => {
		return this.settableStatuses().length > 1 &&
			window.AuthorizationManager.isAuthorizedForAction("ServiceContract", "SetStatus");
	});
	reactionTimeUnits = ko.observableArray<string>(window._.compact(window.Crm.Service.Settings.ServiceContract.ReactionTime.AvailableTimeUnits.split(',')));
	dropboxName = ko.pureComputed<string>(() => {
		return this.serviceContract().ContractNo() + "-" + this.serviceContract().ParentCompany().Name().substring(0, 25);
	});
	additionalTitle = ko.observable<string>(null);

	constructor() {
		super();
		this.contactType("ServiceContract");
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		this.contactId(id);
		await super.init(id, params);
		let serviceContract = await window.database.CrmService_ServiceContract
			.include("InvoiceRecipient")
			.include("InvoiceAddress")
			.include("ParentCompany")
			.include("Payer")
			.include("PayerAddress")
			.include("ResponsibleUserUser")
			.include("ServiceObject")
			.include2("Tags.orderBy(function(t) { return t.Name; })")
			.find(id);
		this.serviceContract(serviceContract.asKoObservable());
		this.serviceContract().ContractTypeKey.subscribe(function () {
			this.setAdditionalTitle();
		}, this);
		this.contact(this.serviceContract());
		this.contactName(this.serviceContract().Name());
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await this.setVisibilityAlertText();
		this.title(this.serviceContract().ContractNo());
		this.serviceContract().ContractNo.subscribe(async (newTitle: string) => {
			this.title(newTitle);
		}, this);
		this.setAdditionalTitle();
		await this.setBreadcrumbs(id, this.title());
		
	};

	rhythmUnitFilter(query: $data.Queryable<Main.Rest.Model.Lookups.Main_TimeUnit>): $data.Queryable<Main.Rest.Model.Lookups.Main_TimeUnit> {
		return window.Helper.Lookup.queryLookup(query.filter("it.Key === null || it.Key in this.reactionTimeUnits",
			{reactionTimeUnits: this.reactionTimeUnits()}));
	};

	invoiceAddressFilter =
		window.Crm.Service.ViewModels.ServiceContractCreateViewModel.prototype.invoiceAddressFilter;

	onSelectInvoiceRecipient =
		window.Crm.Service.ViewModels.ServiceContractCreateViewModel.prototype.onSelectInvoiceRecipient;

	onSelectPayer =
		window.Crm.Service.ViewModels.ServiceContractCreateViewModel.prototype.onSelectPayer;

	payerAddressFilter =
		window.Crm.Service.ViewModels.ServiceContractCreateViewModel.prototype.payerAddressFilter;

	async setStatus(status: Crm.Service.Rest.Model.Lookups.CrmService_ServiceContractStatus): Promise<void> {
		this.loading(true);
		window.database.attachOrGet(this.serviceContract().innerInstance);
		this.serviceContract().StatusKey(status.Key);
		await window.database.saveChanges();
		this.loading(false);
	};

	async setBreadcrumbs(id: string, title: string): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("ServiceContract"), "ServiceContract::Index", "#/Crm.Service/ServiceContractList/IndexTemplate"),
			new Breadcrumb(title, null, window.location.hash, null, id)
		]);
	};

	setAdditionalTitle(): void {
		const contractTypeKey = this.serviceContract().ContractTypeKey();
		const contractType = this.lookups.serviceContractTypes.$array.find(function (x) {
			return x.Key === contractTypeKey;
		});
		this.additionalTitle(contractType ? contractType.Value : "");
	};
}

namespace("Crm.Service.ViewModels").ServiceContractDetailsViewModel = ServiceContractDetailsViewModel;
