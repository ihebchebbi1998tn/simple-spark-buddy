import {Mixin} from "ts-mixer";
import {PersonDetailsViewModelExtension} from "./PersonDetailsViewModelExtensions";

export class PersonDetailsStaffTabViewModelExtension extends window.Crm.ViewModels.PersonDetailsStaffTabViewModel {
	getCreatePersonLink(): string {
		if (this.parentViewModel instanceof PersonDetailsViewModelExtension && this.parentViewModel.serviceObject()) {
			return "#/Crm/Person/CreateTemplate?parentId=" +
				this.parentViewModel.serviceObject().Id() +
				"&parentType=ServiceObject&redirectUrl=/Crm/Person/DetailsTemplate/" +
				this.personId() +
				"%3Ftab%3Dtab-staff";
		}
		return super.getCreatePersonLink();
	};
}

window.Crm.ViewModels.PersonDetailsStaffTabViewModel = Mixin(PersonDetailsStaffTabViewModelExtension);