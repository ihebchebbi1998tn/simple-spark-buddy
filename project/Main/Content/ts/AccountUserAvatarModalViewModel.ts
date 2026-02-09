import {namespace} from "./namespace";

export class AccountUserAvatarModalViewModel extends window.Main.ViewModels.ViewModelBase {
	avatarBlob = ko.observable<Blob>(null);
	avatarContent = ko.observable<string>(null);
	avatarUrl = ko.observable<string>(null);
	maxFileSize = 500;
	user = ko.observable<Main.Rest.Model.ObservableMain_User>(null);

	constructor() {
		super();
		this.avatarBlob.extend({
			validation: {
				validator: (val) => {
					return val == null || !val.name || val.type.indexOf("image/") !== -1;
				},
				message: window.Helper.String.getTranslatedString("M_SelectedFileNotImage")
			}
		});
		this.avatarBlob.extend({
			validation: {
				validator: (val) => {
					return val == null || val.size <= (this.maxFileSize * 1000);
				},
				message: window.Helper.String.getTranslatedString("FileTooLarge")
			}
		});

	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		const currentUserName = window.Helper.User.getCurrentUserName();
		const user = await window.database.Main_User.include("expandAvatar").find(currentUserName);
		window.database.attachOrGet(user);
		const blob = user.Avatar !== null ? window.Helper.String.base64toBlob(user.Avatar) : null;
		if (blob !== null) {
			this.avatarBlob(blob);
			this.avatarContent(user.Avatar);
			this.avatarUrl(window.URL.createObjectURL(blob));
		}
		this.user(user.asKoObservable());
	}

	async submit(): Promise<void> {
		const errors = ko.validation.group(this, {deep: true});
		if (errors().length > 0) {
			errors.showAllMessages();
			return;
		}
		this.loading(true);
		this.user().Avatar(this.avatarContent());
		try {
			await window.database.saveChanges();
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch (e) {
			this.loading(false);
			window.Log.error(e);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
		}
	};
}

namespace("Main.ViewModels").AccountUserAvatarModalViewModel = AccountUserAvatarModalViewModel;