import {namespace} from "@Main/namespace";
import type { SchedulerDetailsViewModel } from "../SchedulerDetailsViewModel";


export class SchedulerAddProfileModalViewModel extends window.Main.ViewModels.ViewModelBase {
	parentViewModel: SchedulerDetailsViewModel = null;
	loading: KnockoutObservable<boolean> = ko.observable<boolean>(false);
	profile: KnockoutObservable<Sms.Scheduler.Rest.Model.ObservableSmsScheduler_Profile> = ko.observable<Sms.Scheduler.Rest.Model.ObservableSmsScheduler_Profile>(null);
	
	constructor(parentViewModel) {
		super();
		this.parentViewModel = parentViewModel;
	}
	async init(...params: any[]): Promise<void> {
		let newProfile = window.database.SmsScheduler_Profile.defaultType.create();
		newProfile.Username = window.Helper.User.getCurrentUserName();
		window.database.add(newProfile);
		newProfile.ClientConfig = await window.database.SmsScheduler_Profile.GetDefaultProfileConfig().first();
		this.profile(newProfile.asKoObservable());
		this.profile().Name.extend({
			required: {
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Name")),
				params: true
			}
		})
	}
	async save(data, event): Promise<void> {
		$(event.target).closest(".modal-content").data("keepchanges", true);

		const errors = ko.validation.group(this.profile());
		if (errors().length > 0) {
			errors.showAllMessages();
			return;
		}
		this.loading(true);
		try {
			await window.database.saveChanges();
			this.parentViewModel.profile(this.profile().innerInstance);
			await this.parentViewModel.initAndloadInlineData();
			$(".modal:visible").modal("hide");
		} catch (e) {
			window.Log.error(e);
			swal(window.Helper.String.getTranslatedString("Error"), window.Helper.String.getTranslatedString("SomethingWentWrong"), "error");
		} finally {
			this.loading(false);
		}
	}

	dispose() {
		window.database.detach(this.profile());
	}

	cancel(data, event): void {
		$(event.target).closest(".modal-content").data("keepchanges", true);

		window.database.detach(this.profile());
		$(".modal:visible").modal("hide");
	}
}

namespace("Sms.Scheduler.ViewModels").SchedulerAddProfileModalViewModel = SchedulerAddProfileModalViewModel;