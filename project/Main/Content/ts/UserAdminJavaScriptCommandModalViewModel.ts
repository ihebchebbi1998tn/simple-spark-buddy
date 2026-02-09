import {namespace} from "./namespace";

export class UserAdminJavaScriptCommandModalViewModel extends window.Main.ViewModels.ViewModelBase {

	username = ko.observable<string>(null);
	javascript = ko.observable<string>(null);

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.username(id);
	}

	async send(): Promise<void> {
		this.loading(true);
		try {
			await fetch(window.Helper.Url.resolveUrl("~/Main/UserAdmin/SendJavaScriptCommand"), {
				method: "POST",
				body: new URLSearchParams({
					"username": this.username(),
					"javascript": this.javascript()
				})
			});
			$(".modal:visible").modal("hide");
			this.loading(false);
		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
			throw e;
		}
	}
}

namespace("Main.ViewModels").UserAdminJavaScriptCommandModalViewModel = UserAdminJavaScriptCommandModalViewModel;