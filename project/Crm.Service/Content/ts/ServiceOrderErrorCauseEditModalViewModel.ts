import { namespace } from "@Main/namespace";
import type { ServiceOrderDetailsViewModel } from "@Crm.Service/ServiceOrderDetailsViewModel";
import type { DispatchDetailsViewModel } from "@Crm.Service/DispatchDetailsViewModel";
import { HelperLookup } from "@Main/helper/Helper.Lookup";
import 'moment-duration-format';

export class ServiceOrderErrorCauseEditModalViewModel extends window.Main.ViewModels.ViewModelBase {
	serviceOrderErrorCause = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorCause>(null);
	parentViewModel: ServiceOrderDetailsViewModel | DispatchDetailsViewModel;
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	errors = ko.validation.group(this.serviceOrderErrorCause, { deep: true });

	lookups: LookupType = {
		statisticsKeyCauses: { $tableName: "CrmService_StatisticsKeyCause" },
		statisticsKeyFaultImages: { $tableName: "CrmService_StatisticsKeyFaultImage" },
	}

	constructor(parentViewModel: ServiceOrderDetailsViewModel | DispatchDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		await HelperLookup.getLocalizedArrayMaps(this.lookups)
		let user = await window.Helper.User.getCurrentUser()
		this.currentUser(user);
		let serviceOrderErrorCause: Crm.Service.Rest.Model.CrmService_ServiceOrderErrorCause;
		if (id) {
			serviceOrderErrorCause = await window.database.CrmService_ServiceOrderErrorCause
				.include("ServiceOrderErrorType")
				.find(id);
		} else {
			serviceOrderErrorCause = window.database.CrmService_ServiceOrderErrorCause.defaultType.create();
			serviceOrderErrorCause.ServiceOrderErrorTypeId = params.errorTypeId;
			const serviceOrderErrorType = await window.database.CrmService_ServiceOrderErrorType.find(params.errorTypeId);
			serviceOrderErrorCause.ServiceOrderErrorType = serviceOrderErrorType;
			serviceOrderErrorCause.DispatchId = serviceOrderErrorType.DispatchId;
			serviceOrderErrorCause.StatisticsKeyCauseKey = this.getFavoriteErrorCauseKey(serviceOrderErrorType.StatisticsKeyFaultImageKey);
		}

		this.serviceOrderErrorCause(serviceOrderErrorCause.asKoObservable());

		if (id) {
			window.database.attachOrGet(serviceOrderErrorCause);
		} else {
			window.database.add(serviceOrderErrorCause)
		}
	};

	errorCauseFilter(query: $data.Queryable<Crm.Service.Rest.Model.Lookups.CrmService_StatisticsKeyCause>, term: string) {
		if (this.serviceOrderErrorCause().ServiceOrderErrorType()?.StatisticsKeyFaultImageKey()) {
			query = query.filter("it.ErrorTypes === '' || it.ErrorTypes.toLowerCase().contains(this.errorTypeKey)",
				{errorTypeKey: this.serviceOrderErrorCause().ServiceOrderErrorType().StatisticsKeyFaultImageKey()});
		}
		return window.Helper.Lookup.queryLookup(query, term);
	}

	dispose(): void {
		window.database.detach(this.serviceOrderErrorCause().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}
		try {
			const isAdded = this.serviceOrderErrorCause().innerInstance.entityState === $data.EntityState.Added;
			await window.database.saveChanges();
			this.showSnackbar(window.Helper.String.getTranslatedString(isAdded ? "Added" : "Edited"));
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
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

namespace("Crm.Service.ViewModels").ServiceOrderErrorCauseEditModalViewModel = ServiceOrderErrorCauseEditModalViewModel;