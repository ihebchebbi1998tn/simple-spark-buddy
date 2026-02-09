import { namespace } from "@Main/namespace";
import type { ServiceObjectDetailsViewModel } from "./ServiceObjectDetailsViewModel";

export class ServiceObjectDetailsServiceContractsTabViewModel extends window.Crm.Service.ViewModels.ServiceContractListIndexViewModel {
	constructor(parentViewModel: ServiceObjectDetailsViewModel) {
		super();
		this.bulkActions([]);
		this.infiniteScroll(true);
		const serviceObject = parentViewModel.serviceObject().Id();
		this.getFilter("ServiceObjectId").extend({ filterOperator: "===" })(serviceObject);
		this.isTabViewModel(true);
		this.joins.remove("ServiceObject");
	}
}

namespace("Crm.Service.ViewModels").ServiceObjectDetailsServiceContractsTabViewModel = ServiceObjectDetailsServiceContractsTabViewModel;