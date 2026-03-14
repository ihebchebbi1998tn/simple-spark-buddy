import {Mixin} from "ts-mixer";

export class PersonDetailsViewModelExtension extends window.Crm.ViewModels.PersonDetailsViewModel {
	serviceObject = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceObject>(null);

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		if (!this.parent() && window.database.CrmService_ServiceObject) {
			let results = await window.database.CrmService_ServiceObject
				.filter("it.Id === this.id", {id: this.person().ParentId()}).take(1).toArray();
			if (results.length > 0) {
				this.serviceObject(results[0].asKoObservable());
				this.parent(this.serviceObject());
			}
		}
		return null;
	}
}

window.Crm.ViewModels.PersonDetailsViewModel = Mixin(PersonDetailsViewModelExtension);