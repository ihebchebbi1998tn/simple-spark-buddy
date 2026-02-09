import {HelperString} from "./Helper.String";

export class HelperConfirm {
	static genericConfirm(config: {
		type: "warning" | "error" | "success" | "info"
		text?: string;
		title?: string;
		showCancelButton?: boolean,
		confirmButtonText?: string;
		cancelButtonText?: string;
	}): JQuery.Promise<void> {
		const deferred = $.Deferred();
		HelperConfirm.genericConfirmAsync(config).then(result => {
			if (result) {
				deferred.resolve();
			} else {
				deferred.reject();
			}
		});
		return deferred.promise();
	}

	static async genericConfirmAsync(config: {
		type?: "warning" | "error" | "success" | "info";
		text?: string;
		title?: string;
		showCancelButton?: boolean,
		confirmButtonText?: string;
		cancelButtonText?: string;
	}): Promise<boolean> {
		return new Promise<boolean>((resolve, reject) => {
			const activeModal = $(".modal:visible");
			activeModal.hide();
			const waitForActiveSwalToHide = $(".hideSweetAlert:visible").length > 0 ? 200 : 0;
			setTimeout(function () {
				swal({
					title: config.title || "",
					text: config.text || "",
					type: config.type,
					showCancelButton: config.showCancelButton !== undefined ? config.showCancelButton : true,
					confirmButtonText: config.confirmButtonText || HelperString.getTranslatedString("Ok"),
					cancelButtonText: config.cancelButtonText || HelperString.getTranslatedString("Cancel"),
					closeOnConfirm: true
				}, function (isConfirm) {
					setTimeout(function () {
						if (isConfirm) {
							resolve(true);
						} else {
							resolve(false);
						}
						activeModal.show();
					}, 200);
				});
			}, waitForActiveSwalToHide);
		});
	}

	static confirmDelete(): JQuery.Promise<void> {
		return this.genericConfirm({
			text: HelperString.getTranslatedString("ConfirmDelete"),
			type: "error",
			confirmButtonText: HelperString.getTranslatedString("Delete")
		});
	}

	static async confirmDeleteAsync(): Promise<boolean> {
		return this.genericConfirmAsync({
			text: HelperString.getTranslatedString("ConfirmDelete"),
			type: "error",
			confirmButtonText: HelperString.getTranslatedString("Delete")
		});
	}

	static confirmContinue(): JQuery.Promise<void> {
		return this.genericConfirm({
			text: HelperString.getTranslatedString("PendingChangesWarningText"),
			type: "warning",
			confirmButtonText: HelperString.getTranslatedString("Discard")
		});
	}

	static async confirmContinueAsync(): Promise<boolean> {
		return this.genericConfirmAsync({
			text: HelperString.getTranslatedString("PendingChangesWarningText"),
			type: "warning",
			confirmButtonText: HelperString.getTranslatedString("Discard")
		});
	}
}
