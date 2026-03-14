import { namespace } from "@Main/namespace";
import type { ServiceCaseDetailsViewModel } from "./ServiceCaseDetailsViewModel";

export class ServiceCaseDetailsInstallationTabViewModel extends window.Main.ViewModels.ViewModelBase {
	serviceCase: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceCase>;
	installation = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_Installation>(null);
	lookups: LookupType;
	constructor(parentViewModel: ServiceCaseDetailsViewModel) {
		super();
		this.serviceCase = parentViewModel.serviceCase;
		this.lookups = parentViewModel.lookups;
		this.lookups.installationHeadStatuses = this.lookups.installationHeadStatuses || { $tableName: "CrmService_InstallationHeadStatus" };
		this.lookups.installationTypes = { $tableName: "CrmService_InstallationType" };
		this.lookups.manufacturers = { $tableName: "CrmService_Manufacturer" };
		this.lookups.countries = { $tableName: "Main_Country" };
		this.lookups.regions = { $tableName: "Main_Region" };
	}

	async init(): Promise<void> {
		await super.init();
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		if (this.serviceCase().AffectedInstallation()) {
			this.installation(this.serviceCase().AffectedInstallation());
		}
		if (Helper.AttributeForms && this.installation()) {
			Helper.AttributeForms.mixinEditing(this.installation(), "Installation");
			// @ts-ignore
			await this.installation().updateAttributeForms.call(this.installation());
		}
	};

	getAvatarColor(installation: Crm.Service.Rest.Model.ObservableCrmService_Installation): string {
		return "#9E9E9E";
	};

	getAvatarText(installation: Crm.Service.Rest.Model.ObservableCrmService_Installation): string {
		const installationTypeKey = ko.unwrap(installation.InstallationTypeKey);
		if (installationTypeKey) {
			const installationType = this.lookups.installationTypes[installationTypeKey];
			if (installationType && installationType.Value) {
				return installationType.Value[0];
			}
		}
		return "";
	};
}

namespace("Crm.Service.ViewModels").ServiceCaseDetailsInstallationTabViewModel = ServiceCaseDetailsInstallationTabViewModel;