import {namespace} from "./namespace";
import type {AccountUserProfileViewModel} from "./AccountUserProfileViewModel";
import {HelperDatabase} from "./helper/Helper.Database";

export class AccountUserProfileDevicesTabViewModel extends window.Main.ViewModels.GenericListViewModel<Main.Rest.Model.Main_Device, Main.Rest.Model.ObservableMain_Device> {
	currentFingerprint = ko.observable<string>(null);
	user: KnockoutObservable<Main.Rest.Model.ObservableMain_User>;

	constructor(parentViewModel: AccountUserProfileViewModel) {
		super("Main_Device", "CreateDate", "DESC");
		this.user = parentViewModel.user;
	}

	async init(): Promise<void> {
		await super.init();
		this.currentFingerprint(window.Helper.DOM.getCookie("fingerprint"));
	}
}

namespace("Main.ViewModels").AccountUserProfileDevicesTabViewModel = AccountUserProfileDevicesTabViewModel;