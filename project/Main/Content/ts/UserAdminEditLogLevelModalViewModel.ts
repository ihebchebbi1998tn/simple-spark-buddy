import {namespace} from "./namespace";

export class UserAdminEditLogLevelModalViewModel extends window.Main.ViewModels.ViewModelBase {
	user = ko.observable<Main.Rest.Model.ObservableMain_User>(null);
	logLevel = ko.observable<Crm.Library.Signalr.JavaScriptLogLevel>(null);
	logLevels = ko.observableArray([]);

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.user((await window.database.Main_User.find(id)).asKoObservable());
		this.logLevels(["NONE", "DEBUG", "INFO", "WARN", "ERROR"].map(x => ({
			Key: window.Crm.Library.Signalr.JavaScriptLogLevel[x],
			Value: x
		})));
		let signalRInformation = await window.database.Main_User.GetSignalRInformation()
			.filter("it.Id === this.id", {id: id})
			.first();
		this.logLevel(signalRInformation.JavaScriptLogLevel);
	}

	async save(): Promise<void> {
		this.loading(true);
		try {
			await window.database.Main_User.SetLogLevel({
				User: this.user().innerInstance,
				LogLevel: this.logLevel()
			}).first();
			this.user().innerInstance.entityState = $data.EntityState.Modified;
			window.database.processEntityTypeAfterEventHandler({data: this.user().innerInstance});
			$(".modal:visible").modal("hide");
			this.loading(false);
		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
			throw e;
		}
	}
}

namespace("Main.ViewModels").UserAdminEditLogLevelModalViewModel = UserAdminEditLogLevelModalViewModel;