import {namespace} from "@Main/namespace";
import type {VisibilityViewModel} from "@Main/VisibilityViewModel";
import { Breadcrumb } from "@Main/breadcrumbs";

export class ServiceContractCreateViewModel extends window.Main.ViewModels.ViewModelBase {
	numberingSequenceName = "SMS.ServiceContract";
	serviceContract = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceContract>(null);
	isThereAnyServiceObject = ko.observable<boolean>(true);
	visibilityViewModel: VisibilityViewModel = new window.VisibilityViewModel(this.serviceContract, "ServiceContract");
	errors = ko.validation.group(this.serviceContract, {deep: true});
	lookups: LookupType = {
		currencies: {$tableName: "Main_Currency"},
		paymentConditions: {$tableName: "Main_PaymentCondition"},
		paymentIntervals: {$tableName: "Main_PaymentInterval"},
		paymentTypes: {$tableName: "Main_PaymentType"},
		serviceContractStatuses: {$tableName: "CrmService_ServiceContractStatus"},
		serviceContractTypes: {$tableName: "CrmService_ServiceContractType"}
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		const serviceContract = window.database.CrmService_ServiceContract.defaultType.create();
		serviceContract.ContractTypeKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.serviceContractTypes, serviceContract.ContractTypeKey);
		serviceContract.PaymentConditionKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.paymentConditions, serviceContract.PaymentConditionKey);
		serviceContract.PaymentIntervalKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.paymentIntervals, serviceContract.PaymentIntervalKey);
		serviceContract.PaymentTypeKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.paymentTypes, serviceContract.PaymentTypeKey);
		serviceContract.PriceCurrencyKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.currencies, serviceContract.PriceCurrencyKey);
		serviceContract.ResponsibleUser = window.Helper.User.getCurrentUserName();
		serviceContract.StatusKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.serviceContractStatuses, serviceContract.StatusKey);
		serviceContract.LastActivity = new Date();
		if (params && params.parentId) {
			serviceContract.ParentId = params.parentId;
		}
		if (params && params.serviceObjectId) {
			serviceContract.ServiceObjectId = params.serviceObjectId;
		}
		this.serviceContract(serviceContract.asKoObservable());

		let count = await window.database.CrmService_ServiceObject.count();
		this.isThereAnyServiceObject(count > 0);

		await this.visibilityViewModel.init();
		await this.setBreadcrumbs();
		window.database.add(this.serviceContract().innerInstance);
	};

	cancel(): void {
		window.database.detach(this.serviceContract().innerInstance);
		window.history.back();
	};

	invoiceAddressFilter(serviceContract: Crm.Service.Rest.Model.ObservableCrmService_ServiceContract, query: $data.Queryable<Crm.Rest.Model.Crm_Address>): $data.Queryable<Crm.Rest.Model.Crm_Address> {
		return query.filter(function (it) {
				return this.invoiceRecipientId !== null && it.CompanyId === this.invoiceRecipientId;
			},
			{invoiceRecipientId: serviceContract.InvoiceRecipientId});
	};

	onSelectInvoiceRecipient(serviceContract: Crm.Service.Rest.Model.ObservableCrmService_ServiceContract, value: Crm.Rest.Model.Crm_Company): void {
		if (value) {
			serviceContract.InvoiceRecipient(value.asKoObservable());
			if (serviceContract.InvoiceAddress() && serviceContract.InvoiceAddress().CompanyId() !== value.Id) {
				serviceContract.InvoiceAddress(null);
				serviceContract.InvoiceAddressKey(null);
			}
		} else {
			serviceContract.InvoiceRecipient(null);
			serviceContract.InvoiceAddress(null);
			serviceContract.InvoiceAddressKey(null);
		}
	}

	onSelectPayer(serviceContract: Crm.Service.Rest.Model.ObservableCrmService_ServiceContract, value: Crm.Rest.Model.Crm_Company): void {
		if (value) {
			serviceContract.Payer(value.asKoObservable());
			if (serviceContract.PayerAddress() && serviceContract.PayerAddress().CompanyId() !== value.Id) {
				serviceContract.PayerAddress(null);
				serviceContract.PayerAddressId(null);
			}
		} else {
			serviceContract.Payer(null);
			serviceContract.PayerAddress(null);
			serviceContract.PayerAddressId(null);
		}
	};

	payerAddressFilter(serviceContract: Crm.Service.Rest.Model.ObservableCrmService_ServiceContract, query: $data.Queryable<Crm.Rest.Model.Crm_Address>, term: string): $data.Queryable<Crm.Rest.Model.Crm_Address> {
		query = window.Helper.Address.getAutocompleteFilter(query, term);
		return query.filter(function (it) {
				return this.payerId !== null && it.CompanyId === this.payerId;
			},
			{payerId: serviceContract.PayerId});
	};

	async submit(): Promise<void> {
		this.loading(true);
		let contractNo = await window.NumberingService.createNewNumberBasedOnAppSettings(window.Crm.Service.Settings.ServiceContract.ServiceContractNoIsGenerated, window.Crm.Service.Settings.ServiceContract.ServiceContractNoIsCreateable, this.serviceContract().Name(), this.numberingSequenceName, window.database.CrmService_ServiceContract, "ContractNo");
		if (contractNo !== null) {
			this.serviceContract().ContractNo(contractNo);
			this.serviceContract().Name(contractNo);
		} else {
			this.serviceContract().ContractNo(this.serviceContract().Name())
		}
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			this.errors.scrollToError();
			this.errors.expandCollapsiblesWithErrors();
			return;
		}
		await window.database.saveChanges();
		window.location.hash = "/Crm.Service/ServiceContract/DetailsTemplate/" + this.serviceContract().Id();
	};

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.getTranslatedString("ServiceContract"), "ServiceContract::Index", "#/Crm.Service/ServiceContractList/IndexTemplate"),
			new Breadcrumb(window.Helper.getTranslatedString("CreateServiceContract"), null, window.location.hash, null, null)
		]);
	};
}

namespace("Crm.Service.ViewModels").ServiceContractCreateViewModel = ServiceContractCreateViewModel;