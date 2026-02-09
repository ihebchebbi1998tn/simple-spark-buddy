import {namespace} from "./namespace";
import {HelperString} from "./helper/Helper.String";
import {HelperUrl} from "./helper/Helper.Url";

export class AccountRemoveDeviceModalViewModel extends window.Main.ViewModels.ViewModelBase {
	parentViewModel: any;
	password: KnockoutObservable<string> = ko.observable();
	validationMessage: KnockoutObservable<string> = ko.observable();
	device: Main.Rest.Model.Main_Device;

	constructor(parentViewModel: any) {
		super();
		this.parentViewModel = parentViewModel;
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.device = await window.database.Main_Device.find(id);
	}
	
	async confirm(): Promise<void> {
		if (!window.navigator.onLine) {
			swal(HelperString.getTranslatedString("NoConnection"),
				HelperString.getTranslatedString("ConnectionNeeded"),
				"warning");
			return;
		}

		const result = await $.post(HelperUrl.resolveUrl("~/Main/Account/ValidatePassword"),
			{
				password: this.password()
			});
		
		if (result) {
			this.loading(false);
			this.validationMessage(HelperString.getTranslatedString(result.ErrorMessageKey));
		} else {
			window.database.remove(this.device);
			await window.database.saveChanges();
			this.loading(false);
			$(".modal:visible").modal("hide");
		}
	}
}

namespace("Main.ViewModels").AccountRemoveDeviceModalViewModel = AccountRemoveDeviceModalViewModel;