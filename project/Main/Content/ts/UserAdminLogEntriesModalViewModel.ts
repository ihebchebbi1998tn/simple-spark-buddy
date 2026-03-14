import {namespace} from "./namespace";

export class UserAdminLogEntriesModalViewModel extends window.Main.ViewModels.ViewModelBase {
	localDatabaseLogEntries = ko.observableArray<string>([]);
	localStorageLogEntries = ko.observableArray<string>([]);
	logEntry = ko.observable<string>(null);
	username = ko.observable<string>(null);

	getDate(ticks: number): Date {
		const ticksToMicrotime = ticks / 10000;
		const epochMicrotimeDiff = Math.abs(new Date(0, 0, 1).setFullYear(1));
		return new Date(ticksToMicrotime - epochMicrotimeDiff);
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.username(id);
		let signalRInformation = await window.database.Main_User.GetSignalRInformation()
			.filter("it.Id === this.id", {id: id})
			.first();
		this.localDatabaseLogEntries(signalRInformation.LocalDatabaseLogs.sort().reverse());
		this.localStorageLogEntries(signalRInformation.LocalStorageLogs.sort().reverse());
	}

	async showLogEntry(ticks: number, type: string): Promise<void> {
		this.loading(true);
		this.logEntry(null);
		let method = type === "LocalStorage" ? "GetLocalStorageFromLog" : "GetLocalDatabaseFromLog";
		let logEntry = await window.database.Main_User[method](this.username(), ticks).first();
		this.logEntry(logEntry);
		this.loading(false);
	}
}

namespace("Main.ViewModels").UserAdminLogEntriesModalViewModel = UserAdminLogEntriesModalViewModel;