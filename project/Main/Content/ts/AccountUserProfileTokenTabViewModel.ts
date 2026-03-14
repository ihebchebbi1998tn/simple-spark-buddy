import {namespace} from "./namespace";
import type {AccountUserProfileViewModel} from "./AccountUserProfileViewModel";
import { HelperString } from "./helper/Helper.String";

export class AccountUserProfileTokenTabViewModel extends window.Main.ViewModels.ViewModelBase {
	user: KnockoutObservable<Main.Rest.Model.ObservableMain_User>;
	userGeneralToken = ko.observable<string>(null);
	barcode = ko.observable<string>(null);

	constructor(parentViewModel: AccountUserProfileViewModel) {
		super();
		this.userGeneralToken.subscribe(this.userTokenBarcodeGenerator.bind(this));
		this.user = parentViewModel.user;
	}

	async init(): Promise<void> {
		await super.init();
		this.userGeneralToken(this.user().GeneralToken());
	}

	async resetGeneralToken(): Promise<void> {
		if (this.loading()) {
			return;
		}
		this.loading(true);
		if (!await window.Helper.User.resetGeneralToken(this.user())) {
			this.loading(false);
		} else {
			this.userGeneralToken(this.user().GeneralToken());
			this.showSnackbar(HelperString.getTranslatedString("TokenSuccessfullyReset"));
			this.loading(false);
		}
	};

	userTokenBarcodeGenerator = namespace("Main.ViewModels").BarcodePreviewModalViewModel.prototype.barcodeGenerator;
}

namespace("Main.ViewModels").AccountUserProfileTokenTabViewModel = AccountUserProfileTokenTabViewModel;