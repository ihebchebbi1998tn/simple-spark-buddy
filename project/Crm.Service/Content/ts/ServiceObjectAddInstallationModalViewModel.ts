import {namespace} from "@Main/namespace";

export class ServiceObjectAddInstallationModalViewModel extends window.Main.ViewModels.ViewModelBase {

	installation = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_Installation>(null);
	installationId = ko.observable<string>(null);
	serviceObjectId = ko.observable<string>(null);
	errors = ko.validation.group(this.installationId);
	lookups: LookupType = {};

	installationFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		query = query.filter(function (it) {
			return it.FolderId === null;
		});
		if (term) {
			query = window.Helper.String.contains(query, term, ["InstallationNo", "Description"]);
		}
		return query;
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.serviceObjectId(id);
		this.installationId.extend({
			required: {
				params: true,
				message: window.Helper.String.getTranslatedString("RuleViolation.Required")
					.replace("{0}", window.Helper.String.getTranslatedString("Installation"))
			}
		});
	};

	onSelectInstallation(installation: Crm.Service.Rest.Model.CrmService_Installation): void {
		this.installation(installation ? installation.asKoObservable() : null);
	};

	async save(): Promise<void> {
		this.loading(true);
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}
		window.database.attachOrGet(window.Helper.Database.getDatabaseEntity(this.installation));
		this.installation().FolderId(this.serviceObjectId());
		try {
			await window.database.saveChanges();
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch (e) {
			window.Log.error(e);
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};
}

namespace("Crm.Service.ViewModels").ServiceObjectAddInstallationModalViewModel = ServiceObjectAddInstallationModalViewModel;