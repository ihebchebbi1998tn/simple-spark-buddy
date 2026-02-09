import {namespace} from "./namespace";

export class VisibilityEditModalViewModel extends window.VisibilityViewModel {
	entityWithVisibility: any;
	setVisibilityAlertText: () => Promise<void>;

	constructor(parentViewModel: any) {
		let entityType = window.Helper.Database.getDatabaseEntity(parentViewModel.entityWithVisibility).getType().fullName.match("[^_]+$")[0];
		let entityWithVisibility = {
			Visibility: ko.observable(parentViewModel.entityWithVisibility().Visibility()),
			VisibleToUserIds: ko.observable(parentViewModel.entityWithVisibility().VisibleToUserIds()),
			VisibleToUsergroupIds: ko.observable(parentViewModel.entityWithVisibility().VisibleToUsergroupIds())
		}
		super(entityWithVisibility, entityType);
		this.entityWithVisibility = parentViewModel.entityWithVisibility;
		this.setVisibilityAlertText = () => parentViewModel.setVisibilityAlertText();
	}

	async save(): Promise<void> {
		this.loading(true);
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			this.errors.scrollToError();
			return;
		}
		try {
			window.database.attachOrGet(this.entityWithVisibility().innerInstance);
			this.entityWithVisibility().Visibility(this.entity().Visibility());
			this.entityWithVisibility().VisibleToUserIds(this.entity().VisibleToUserIds());
			this.entityWithVisibility().VisibleToUsergroupIds(this.entity().VisibleToUsergroupIds());
			await window.database.saveChanges();
			await this.setVisibilityAlertText();
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	}
}

namespace("Main.ViewModels").VisibilityEditModalViewModel = VisibilityEditModalViewModel; 