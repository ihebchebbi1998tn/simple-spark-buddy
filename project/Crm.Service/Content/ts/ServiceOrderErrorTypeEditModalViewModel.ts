import { namespace } from "@Main/namespace";
import type { ServiceOrderDetailsViewModel } from "@Crm.Service/ServiceOrderDetailsViewModel";
import type { DispatchDetailsViewModel } from "@Crm.Service/DispatchDetailsViewModel";
import { HelperLookup } from "@Main/helper/Helper.Lookup";
import 'moment-duration-format';
import type { ServiceCaseDetailsViewModel } from "@Crm.Service/ServiceCaseDetailsViewModel";

export class ServiceOrderErrorTypeEditModalViewModel extends window.Main.ViewModels.ViewModelBase {
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	initialServiceOrderTimeId = ko.observable<string>(null);
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	serviceCase: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceCase>;
	serviceOrderTime = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime>(null);
	serviceOrderErrorType = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType>(null);
	serviceOrderErrorCause = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorCause>(null);
	StatisticsKeyCauseKey = ko.observable<string>(null);
	parentViewModel: ServiceOrderDetailsViewModel | DispatchDetailsViewModel | ServiceCaseDetailsViewModel;
	StatisticsKeyMainAssembly = ko.observable<Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyMainAssembly>(null);
	installationType = ko.observable<string>(null);
	productType = ko.observable<string>(null);
	errors = ko.validation.group(this.serviceOrderErrorType, { deep: true });

	lookups: LookupType = {
		statisticsKeyCauses: { $tableName: "CrmService_StatisticsKeyCause" },
		statisticsKeyFaultImages: { $tableName: "CrmService_StatisticsKeyFaultImage" },
	}

	constructor(parentViewModel: ServiceOrderDetailsViewModel | DispatchDetailsViewModel | ServiceCaseDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
		this.dispatch = parentViewModel?.dispatch || window.ko.observable(null);
		this.serviceOrder = parentViewModel?.serviceOrder || window.ko.observable(null);
		this.serviceCase = parentViewModel?.serviceCase || window.ko.observable(null);
	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		await HelperLookup.getLocalizedArrayMaps(this.lookups)
		let serviceOrderErrorType: Crm.Service.Rest.Model.CrmService_ServiceOrderErrorType;

		this.serviceOrderTime(this.dispatch() ? this.dispatch().CurrentServiceOrderTime() : null);
		serviceOrderErrorType = window.database.CrmService_ServiceOrderErrorType.defaultType.create();
		serviceOrderErrorType.OrderId = this.serviceOrder() ? this.serviceOrder().Id() : null;
		serviceOrderErrorType.DispatchId = this.dispatch() ? this.dispatch().Id() : null;
		serviceOrderErrorType.ServiceCaseId = this.serviceCase() ? this.serviceCase().Id() : null;
		serviceOrderErrorType.IsSuspected = this.serviceCase() !== null && this.serviceOrder() === null;
		serviceOrderErrorType.ServiceOrderTimeId = params.serviceOrderTimeId || serviceOrderErrorType.ServiceOrderTimeId || (this.dispatch() ? this.dispatch().CurrentServiceOrderTimeId() : null) || null;
		serviceOrderErrorType.StatisticsKeyFaultImageKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.statisticsKeyFaultImages);
		

		this.serviceOrderErrorType(serviceOrderErrorType.asKoObservable());

		let serviceOrderErrorCause: Crm.Service.Rest.Model.CrmService_ServiceOrderErrorCause;
		serviceOrderErrorCause = window.database.CrmService_ServiceOrderErrorCause.defaultType.create();
		serviceOrderErrorCause.DispatchId = this.dispatch() ? this.dispatch().Id() : null;
		this.serviceOrderErrorCause(serviceOrderErrorCause.asKoObservable());

		window.database.add(serviceOrderErrorType);
		window.database.add(serviceOrderErrorCause);

		serviceOrderErrorCause.ServiceOrderErrorTypeId = this.serviceOrderErrorType().Id();
		serviceOrderErrorCause.StatisticsKeyCauseKey = this.getFavoriteErrorCauseKey(serviceOrderErrorType.StatisticsKeyFaultImageKey);

		this.setInstallationData();
	};



	dispose(): void {
		window.database.detach(this.serviceOrderErrorType().innerInstance);
		window.database.detach(this.serviceOrderErrorCause().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);
		if (this.serviceOrderErrorCause().StatisticsKeyCauseKey() == null) {
			window.database.detach(this.serviceOrderErrorCause().innerInstance);
		} else {
			this.serviceOrderErrorCause().IsSuspected(this.serviceOrderErrorType().IsSuspected());
			this.serviceOrderErrorCause().IsConfirmed(this.serviceOrderErrorType().IsConfirmed());
			this.serviceOrderErrorCause().Description(this.serviceOrderErrorType().Description());
			this.serviceOrderErrorCause().InternalRemark(this.serviceOrderErrorType().InternalRemark());
		}
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}
		try {
			await window.database.saveChanges();
			this.showSnackbar(window.Helper.String.getTranslatedString("Added"));
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};


	getServiceOrderTimeAutocompleteDisplay(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): string {
		return window.Helper.ServiceOrderTime.getAutocompleteDisplay(serviceOrderTime,
			this.dispatch() ? this.dispatch().CurrentServiceOrderTimeId() : null);
	};

	getServiceOrderTimeAutocompleteFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		query = query.filter(function (it) {
			return it.OrderId === this.orderId;
		},
			{ orderId: this.serviceOrderErrorType().OrderId() });

		if (term) {
			query = window.Helper.String.contains(query, term, ["Description.toLowerCase()", "ItemNo.toLowerCase()", "PosNo.toLowerCase()"]);
		}
		return query;
	};

	onJobSelected(serviceOrderTime: Crm.Service.Rest.Model.CrmService_ServiceOrderTime): void {
		if (serviceOrderTime === null) {
			this.serviceOrderTime(null);
			this.serviceOrderErrorType().ServiceOrderTime(null);
			this.serviceOrderErrorType().ServiceOrderTimeId(null);
			this.installationType(null);
			this.productType(null);
		} else {
			this.serviceOrderTime(serviceOrderTime.asKoObservable());
			this.setInstallationData();
		}
		this.serviceOrderErrorType().StatisticsKeyFaultImageKey(null)
	};

	isJobEditable(): boolean {
		return this.dispatch() == null || !Crm.Service.Settings.Dispatch.RestrictEditingToActiveJob;
	}

	onErrorCauseSelected(errorCause: Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyCause): void {
		this.serviceOrderErrorCause().StatisticsKeyCauseKey(errorCause !== null ? errorCause.Key : null);
	};

	onErrorTypeSelected(errorType: Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyFaultImage): void {
		if (!errorType) {
			this.serviceOrderErrorCause().StatisticsKeyCauseKey(null);
			this.StatisticsKeyCauseKey(null);
			return;
		}

		const favoriteErrorCauseKey = this.getFavoriteErrorCauseKey(errorType.Key);
		this.serviceOrderErrorCause().StatisticsKeyCauseKey(favoriteErrorCauseKey);
		this.StatisticsKeyCauseKey(favoriteErrorCauseKey);
	};

	resetStatisticsKeyFaultImageKey(): void {
		this.serviceOrderErrorType().StatisticsKeyFaultImageKey(null);
	};

	errorTypeFilter(query: $data.Queryable<Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyFaultImage>, term: string): $data.Queryable<Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyFaultImage> {
		query = query.filter(function (it) {
			return (it.InstallationTypeKey === this.installationType || it.InstallationTypeKey == null || it.InstallationTypeKey == "")
				&& (it.ProductTypeKey === this.productType || it.ProductTypeKey == null || it.ProductTypeKey == "")
				&& (it.AssemblyGroupKey === this.assemblyGroup || it.AssemblyGroupKey == null || it.AssemblyGroupKey == "")
		},
			{ installationType: this.installationType(), productType: this.productType(), assemblyGroup: this.StatisticsKeyMainAssembly() });

		return window.Helper.Lookup.queryLookup(query, term);
	};

	errorCauseFilter(query: $data.Queryable<Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyCause>, term: string) {
		if (this.serviceOrderErrorType() && this.serviceOrderErrorType().StatisticsKeyFaultImageKey()) {
			query = query.filter("it.ErrorTypes === '' || it.ErrorTypes.toLowerCase().contains(this.errorTypeKey)",
				{errorTypeKey: this.serviceOrderErrorType().StatisticsKeyFaultImageKey()});
		}
		return window.Helper.Lookup.queryLookup(query, term);
	}

	statisticsKeyMainAssemblyFilter(query: $data.Queryable<Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyMainAssembly>, term: string): $data.Queryable<Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyMainAssembly> {
		query = query.filter(function (it) {
			return it.ProductTypeKey === this.productType || it.ProductTypeKey == null || it.ProductTypeKey == "";
		},
			{ productType: this.productType() });

		return window.Helper.Lookup.queryLookup(query, term);
	};


	setInstallationData() {
		if (this.serviceOrderTime()) {
			if (this.serviceOrderTime().Installation()) {
				this.installationType(this.serviceOrderTime().Installation().InstallationTypeKey());
				this.productType(this.serviceOrderTime().Installation().StatisticsKeyProductTypeKey());
			} else if (this.serviceOrder().Installation()) {
				this.installationType(this.serviceOrder().Installation().InstallationTypeKey());
				this.productType(this.serviceOrder().Installation().StatisticsKeyProductTypeKey());
			}
		} else if (this.serviceOrder()) {
			if (this.serviceOrder().Installation()) {
				this.installationType(this.serviceOrder().Installation().InstallationTypeKey());
				this.productType(this.serviceOrder().Installation().StatisticsKeyProductTypeKey());
			}
		} else if (this.serviceCase()) {
			if (this.serviceCase().AffectedInstallation()) {
				this.installationType(this.serviceCase().AffectedInstallation().InstallationTypeKey());
				this.productType(this.serviceCase().AffectedInstallation().StatisticsKeyProductTypeKey());
			}
		}
	};

	getFavoriteErrorCauseKey(errorTypeKey: string): string {
		const favoriteErrorCauseKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect<string>(this.lookups.statisticsKeyCauses);
		if (!favoriteErrorCauseKey) {
			return null;
		}

		let favoriteErrorCause: Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyCause;
		favoriteErrorCause = this.lookups.statisticsKeyCauses.$array.find(x => {
			return x.Key === favoriteErrorCauseKey;
		});
		if (!favoriteErrorCause.ErrorTypes) {
			return favoriteErrorCauseKey;
		}

		const isErrorCauseSelectable = favoriteErrorCause.ErrorTypes.split(",").indexOf(errorTypeKey) !== -1;
		return isErrorCauseSelectable ? favoriteErrorCauseKey : null;
	}

}

namespace("Crm.Service.ViewModels").ServiceOrderErrorTypeEditModalViewModel = ServiceOrderErrorTypeEditModalViewModel;