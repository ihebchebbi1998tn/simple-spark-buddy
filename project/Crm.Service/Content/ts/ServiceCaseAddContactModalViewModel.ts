import { namespace } from "@Main/namespace";
import type { ServiceCaseCreateViewModel } from "./ServiceCaseCreateViewModel";
import { HelperString } from "@Main/helper/Helper.String";
import { Breadcrumb } from "@Main/breadcrumbs";

export class ServiceCaseAddContactModalViewModel extends window.Crm.ViewModels.PersonCreateViewModel {
	parentViewModel: ServiceCaseCreateViewModel;
	trackedEntities: any;
	constructor(parentViewModel: ServiceCaseCreateViewModel) {
		super();
		this.parentViewModel = parentViewModel;
		this.trackedEntities = window.database.stateManager.trackedEntities;
		window.database.stateManager.trackedEntities = [];
	}

	async init(id, params) {
		await super.init(id, params);
		this.person().ParentId(this.parentViewModel.serviceCase().AffectedCompanyKey());
	}

	cancel(): void {
		window.database.detach(this.person().innerInstance);
		$(".modal:visible").modal("hide");
		window.database.stateManager.trackedEntities = this.trackedEntities;
	}

	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.getTranslatedString("ServiceCase"), "ServiceCase::Index", "#/Crm.Service/ServiceCaseList/IndexTemplate"),
			new Breadcrumb(window.Helper.getTranslatedString("CreateServiceCase"), null, window.location.hash, null, null)
		]);
	};

	async submit(): Promise<void> {
		this.loading(true);
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			this.errors.scrollToError();
			return;
		}
		try {
			if (this.addressEditor) {
				if (!this.addressEditor.hasErrors()) {
					this.addressEditor.removeEmptyEntries();
				}
				this.addressEditor.showValidationErrors();
			}
			let personNo = await window.NumberingService.createNewNumberBasedOnAppSettings(window.Crm.Settings.PersonNoIsGenerated, window.Crm.Settings.PersonNoIsCreateable, this.person().PersonNo(), "CRM.Person", window.database.Crm_Person, "PersonNo");
			if (personNo !== null) {
				this.person().PersonNo(personNo)
			}

			//need to switch order of Person([0]) and Address([1]), because Person needs Address first, communication data needs both; after change: Address, Person, Communication
			let person = window.database.stateManager.trackedEntities[0];
			window.database.stateManager.trackedEntities[0] = window.database.stateManager.trackedEntities[1];
			window.database.stateManager.trackedEntities[1] = person;

			this.person().Name(this.person().Surname() + ", " + this.person().Firstname());
			await window.database.saveChanges();
			window.database.stateManager.trackedEntities = [];
			$(".modal:visible").modal("hide");
			window.database.stateManager.trackedEntities = this.trackedEntities;
			this.parentViewModel.serviceCase().ContactPersonId(this.person().Id());
			this.loading(false);
		} catch {
			this.loading(false);
			swal(
				HelperString.getTranslatedString("UnknownError"),
				HelperString.getTranslatedString("Error_InternalServerError"),
				"error"
			);
		}
	};

}

namespace("Crm.Service.ViewModels").ServiceCaseAddContactModalViewModel = ServiceCaseAddContactModalViewModel;