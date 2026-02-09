import {namespace} from "@Main/namespace";
import {Mixin} from "ts-mixer";
import type {DispatchDetailsViewModel} from "@Crm.Service/DispatchDetailsViewModel";

export class DispatchDetailsNotesTabViewModel extends window.Crm.Service.ViewModels.ServiceOrderDetailsNotesTabViewModel<DispatchDetailsViewModel> {
	constructor(parentViewModel: DispatchDetailsViewModel) {
		super(parentViewModel);
		this.contactType("ServiceOrder");
		this.minDate(parentViewModel.dispatch().CreateDate());
	}
}

namespace("Crm.Service.ViewModels").DispatchDetailsNotesTabViewModel = DispatchDetailsNotesTabViewModel;

export class NoteEditModalViewModelExtension extends window.Crm.ViewModels.NoteEditModalViewModel {

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		if (this.parentViewModel instanceof window.Crm.Service.ViewModels.DispatchDetailsViewModel) {
			this.note().Plugin("Crm.Service");
			this.note().ExtensionValues().DispatchId(this.parentViewModel.dispatch().Id());
		}
	}
}

window.Crm.ViewModels.NoteEditModalViewModel = Mixin(NoteEditModalViewModelExtension);