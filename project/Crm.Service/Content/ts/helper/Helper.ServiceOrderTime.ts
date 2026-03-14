export class HelperServiceOrderTime {

	static async calculateEstimatedDuration(serviceOrderTimeId: string, serviceOrderTimePostingIdToSkip: string, additionalDuration: string): Promise<void> {
		let serviceOrderTime = await window.database.CrmService_ServiceOrderTime.find(serviceOrderTimeId);
		window.database.attachOrGet(serviceOrderTime);
		let plannedDurations = await window.database.CrmService_ServiceOrderTimePosting
			.filter("it.ServiceOrderTimeId === this.serviceOrderTimeId && it.Id !== this.id && it.ServiceOrderTimePostingType === '" + window.Crm.Service.ServiceOrderTimePostingType.Preplanned + "'", {
				serviceOrderTimeId: serviceOrderTimeId,
				id: serviceOrderTimePostingIdToSkip
			})
			.map("it.Duration")
			.toArray();
		plannedDurations.push(additionalDuration);
		serviceOrderTime.EstimatedDuration = plannedDurations.filter(Boolean).reduce(function (sum, plannedDuration) {
			return sum.add(window.moment.duration(plannedDuration));
		}, window.moment.duration()).toString();
	}

	static getAutocompleteColumns(): string[] {
		return ["PosNo", "ItemNo", "Description", "Installation.InstallationNo", "Installation.Description"];
	}

	static getAutocompleteDisplay(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime | Crm.Service.Rest.Model.CrmService_ServiceOrderTime, currentServiceOrderTimeId: string): string {
		let result = "";
		serviceOrderTime = window.Helper.Database.getDatabaseEntity(serviceOrderTime) as Crm.Service.Rest.Model.CrmService_ServiceOrderTime;
		if (serviceOrderTime.PosNo) {
			result += serviceOrderTime.PosNo + ": ";
		}
		if (serviceOrderTime.ItemNo) {
			result += serviceOrderTime.ItemNo + " - ";
		}
		if (serviceOrderTime.Description) {
			result += serviceOrderTime.Description;
		}
		if (serviceOrderTime.Description && serviceOrderTime.Installation) {
			result += " - ";
		}
		if (serviceOrderTime.Installation) {
			result += serviceOrderTime.Installation.InstallationNo + " - " + serviceOrderTime.Installation.Description;
		}
		if (serviceOrderTime.Id === currentServiceOrderTimeId) {
			result += " (" + window.Helper.String.getTranslatedString("CurrentServiceOrderTime") + ")";
		}
		return result;
	}

	static getAutocompleteJoins(): string[] {
		return ["Installation"];
	}

	static mapForSelect2Display(serviceOrderTime: Crm.Service.Rest.Model.CrmService_ServiceOrderTime): Select2AutoCompleterResult {
		return {
			id: serviceOrderTime.Id,
			item: serviceOrderTime,
			text: [serviceOrderTime.PosNo, serviceOrderTime.ItemNo, serviceOrderTime.Description].filter(Boolean)
				.join(" - ")
		};
	}
}