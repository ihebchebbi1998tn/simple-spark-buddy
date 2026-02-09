import {namespace} from "@Main/namespace";
import type {InstallationDetailsViewModel} from "./InstallationDetailsViewModel";

export class InstallationDetailsServiceCasesTabViewModel extends window.Crm.Service.ViewModels.ServiceCaseListIndexViewModel {
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	lookups: LookupType;
	installation: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_Installation>;

	constructor(parentViewModel: InstallationDetailsViewModel) {
		super();
		this.isTabViewModel(true);
		this.bulkActions([]);
		this.orderBy(["StatusKey", "CreateDate"]);
		this.orderByDirection(["ASC", "DESC"]);
		this.installation = parentViewModel.installation;
		this.getFilter("AffectedInstallationKey").extend({filterOperator: "==="})(this.installation().Id());
		this.infiniteScroll(true);
	}

	getItemGroup(serviceCase: Crm.Service.Rest.Model.ObservableCrmService_ServiceCase): ItemGroup {
		return {
			title: window.Helper.Lookup.getLookupValue(this.lookups.serviceCaseStatuses, serviceCase.StatusKey())
		};
	};
}

namespace("Crm.Service.ViewModels").InstallationDetailsServiceCasesTabViewModel = InstallationDetailsServiceCasesTabViewModel;