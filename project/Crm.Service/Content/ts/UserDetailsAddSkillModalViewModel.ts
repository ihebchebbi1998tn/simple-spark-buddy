import {namespace} from "@Main/namespace";
import {HelperLookup} from "@Main/helper/Helper.Lookup";
import type {UserDetailsViewModel} from "@Main/UserDetailsViewModel";

export class UserDetailsAddSkillModalViewModel extends window.Main.ViewModels.ViewModelBase {
	userSkill: KnockoutObservable<Main.Rest.Model.ObservableMain_UserSkill> = ko.observable<Main.Rest.Model.ObservableMain_UserSkill>(null);
	existingSkills: string[];
	parentViewModel: UserDetailsViewModel;
	errors = ko.validation.group(this.userSkill, {deep: true});
	
	constructor(parentViewModel: UserDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
	}

	async init(id: string): Promise<void> {
		await super.init();
		let userSkill: Main.Rest.Model.Main_UserSkill;
		if(id) {
			userSkill = await window.database.Main_UserSkill.find(id);
			window.database.attachOrGet(userSkill);
		} else {
			userSkill = window.database.Main_UserSkill.defaultType.create();
			userSkill.Username = this.parentViewModel.user().Id();
			window.database.add(userSkill);
		}
		this.userSkill(userSkill.asKoObservable());
		
		this.existingSkills = await window.database.Main_UserSkill.filter("it => it.Username == this.username", { username: this.parentViewModel.user().Id() }).map(x => x.SkillKey).toArray();
	}

	queryLookup(query: $data.Queryable<any>, term: string): $data.Queryable<any> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["Value.toLowerCase()"]);
		}
		const filterExpression = "!this.existingSkills.contains(it.Key)";

		return HelperLookup.getLocalizedQuery(query, null, filterExpression, { existingSkills: this.existingSkills });
	}

	dispose(): void {
		window.database.detach(this.userSkill().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);

		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}

		try {
			await window.database.saveChanges();
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};
}
namespace("Crm.Service.ViewModels").UserDetailsAddSkillModalViewModel = UserDetailsAddSkillModalViewModel;