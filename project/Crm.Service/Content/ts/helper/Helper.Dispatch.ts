import type {ServiceOrderDispatchListIndexItemViewModel} from "../ServiceOrderDispatchListIndexViewModel";
import type moment from "moment";

export class HelperDispatch {
	static getCurrentJobItemGroup(viewModel: {dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>, items: KnockoutObservableArray<any>}): ItemGroup {
		if (!viewModel.dispatch() || !viewModel.dispatch().CurrentServiceOrderTime()) {
			return null;
		}
		if (viewModel.items().some(item => item.ServiceOrderTime() && item.ServiceOrderTime().Id() === viewModel.dispatch().CurrentServiceOrderTimeId())){
			return null;
		}
		return window.Crm.Service.ViewModels.DispatchDetailsViewModel.prototype.getServiceOrderPositionItemGroup.call(viewModel, { ServiceOrderTime: viewModel.dispatch().CurrentServiceOrderTime });
	}
	
	static async getDueDispatchesCount(): Promise<number> {
		let currentUser = await window.Helper.User.getCurrentUser();
		return await window.database.CrmService_ServiceOrderDispatch
			.filter(function (it) {
					return it.Username === this.username &&
						it.StatusKey in ["Released", "Read", "InProgress", "SignedByCustomer"] &&
						it.Date <= this.now;
				},
				{username: currentUser.Id, now: new Date()})
			.count();
	}

	static async getNewDispatchesCount(): Promise<number> {
		let currentUser = await window.Helper.User.getCurrentUser();
		return await window.database.CrmService_ServiceOrderDispatch
			.filter(function (it) {
					return it.Username === this.username && it.StatusKey === "Released";
				},
				{username: currentUser.Id})
			.count();
	}

	static filterTechnicianQuery(query: $data.Queryable<Main.Rest.Model.Main_User>, term: string, userGroupId: string): $data.Queryable<Main.Rest.Model.Main_User> {
		let techniciansQuery = query;
		// @ts-ignore
		if (techniciansQuery.specialFunctions.filterByPermissions[window.database.storageProvider.name]) {
			techniciansQuery = techniciansQuery.filter("filterByPermissions", "WebApiWrite::ServiceOrderDispatch");
		}
		return window.Helper.User.filterUserQuery(techniciansQuery, term?.toLowerCase(), userGroupId);
	}

	static mapForSelect2Display(dispatch: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch): Select2AutoCompleterResult {
		return {
			id: dispatch.Id,
			item: dispatch,
			text: dispatch.ServiceOrder.OrderNo
		};
	}

	static async toggleCurrentJob(dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>, selectedServiceOrderTimeId: string): Promise<void> {
		const currentServiceOrderTimeId = dispatch().CurrentServiceOrderTimeId();
		window.database.attachOrGet(dispatch().innerInstance);
		let newServiceOrderTime = await window.database.CrmService_ServiceOrderTime.include("Installation").find(selectedServiceOrderTimeId);			
		newServiceOrderTime.CompleteDate = null;
		newServiceOrderTime.CompleteUser = null;
		if (currentServiceOrderTimeId === newServiceOrderTime.Id) {
			dispatch().CurrentServiceOrderTimeId(null);
			dispatch().CurrentServiceOrderTime(null);
			window.database.attachOrGet(newServiceOrderTime);
			newServiceOrderTime.StatusKey = "Interrupted";
		} else {
			dispatch().CurrentServiceOrderTimeId(newServiceOrderTime.Id);
			dispatch().CurrentServiceOrderTime(newServiceOrderTime.asKoObservable());
			window.database.attachOrGet(newServiceOrderTime);
			newServiceOrderTime.StatusKey = "Started";
		}
		if (currentServiceOrderTimeId && currentServiceOrderTimeId !== selectedServiceOrderTimeId) {
			let previousServiceOrderTime = await window.database.CrmService_ServiceOrderTime.find(currentServiceOrderTimeId);
				
					if (previousServiceOrderTime.StatusKey === "Started") {
						window.database.attachOrGet(previousServiceOrderTime);
						previousServiceOrderTime.StatusKey = "Interrupted";
					}
				
		}
		await window.database.saveChanges();
	}

	static getCalendarBodyText(dispatch: (Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch & ServiceOrderDispatchListIndexItemViewModel)): string {
			let result = "";

			if (dispatch.ServiceOrder().Company() != null) {
				result = result + window.Helper.String.getTranslatedString("Customer") + ": " + window.Helper.Company.getDisplayName(dispatch.ServiceOrder().Company()) + "\\n";
			}
			if (dispatch.Installations() != null) {
				if (dispatch.Installations().length === 1) {
					result = result + window.Helper.String.getTranslatedString("Installation") + ": " + window.Helper.Installation.getDisplayName(dispatch.Installations()[0]) + "\\n";
				} else if (dispatch.Installations().length > 1) {
					result = result + window.Helper.String.getTranslatedString("Installations") + ": ";
					const installationNames = [];
					dispatch.Installations().forEach(function (installation) {
						installationNames.push(window.Helper.Installation.getDisplayName(installation));
					});
					result = result + installationNames.join(", ") + "\\n\\n";
				}
			}
			if (dispatch.ServiceOrder().Initiator() != null) {
				result = result + window.Helper.String.getTranslatedString("Initiator") + ": " + window.Helper.Company.getDisplayNameWithAddress(dispatch.ServiceOrder().Initiator()) + "\\n\\n";
			}
			if (dispatch.ServiceOrder().InitiatorPerson() != null) {
				result = result + window.Helper.String.getTranslatedString("InitiatorPerson") + ":\\n";
				result = result + window.Helper.String.getTranslatedString("Name") + ": " + window.Helper.Person.getDisplayNameWithSalutation(dispatch.ServiceOrder().InitiatorPerson(), null) + "\\n";
				const personContactData = window.Helper.Person.getContactData(dispatch.ServiceOrder().InitiatorPerson());
				if (personContactData.length > 0) {
					result = result + personContactData + "\\n\\n";
				}
			}
			if (dispatch.ServiceOrder().ResponsibleUserUser() != null) {
				result = result + window.Helper.String.getTranslatedString("ResponsibleUser") + ": " + window.Helper.User.getDisplayName(dispatch.ServiceOrder().ResponsibleUserUser()) + "\\n\\n";
			}

			result = result + window.Helper.String.getTranslatedString("ErrorMessage") + ": " + dispatch.ServiceOrder().ErrorMessage();

			return result;
	}
	
	static calculateDuration(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch) : moment.Duration{
		return window.moment.duration(window.moment(ko.unwrap(dispatch?.EndDate)).diff(window.moment(ko.unwrap(dispatch?.Date))));
	}

	static dispatchIsEditable(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): boolean {
		if (!dispatch?.StatusKey() || !window.AuthorizationManager.isAuthorizedForAction('Dispatch', 'Edit')) {
				return false;
		}
		return dispatch.StatusKey() === "InProgress";
	}

	static dispatchIsClosed(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): boolean {
		const statuses = ["ClosedComplete", "ClosedNotComplete", "Rejected"]
		if (!dispatch?.StatusKey() || !window.AuthorizationManager.isAuthorizedForAction('Dispatch', 'Edit')) {
			return false;
		}
		return statuses.indexOf(dispatch.StatusKey()) !== -1;
	}

	static dispatchIsNotStarted(dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): boolean {
		const statuses = ["Scheduled", "Released", "Read"];
		if (!dispatch?.StatusKey() || !window.AuthorizationManager.isAuthorizedForAction('Dispatch', 'Edit')) {
			return false;
		}
		return statuses.indexOf(dispatch.StatusKey()) !== -1;
	}
}