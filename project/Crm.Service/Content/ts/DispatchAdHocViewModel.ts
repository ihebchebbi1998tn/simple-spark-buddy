import {namespace} from "@Main/namespace";
import { Breadcrumb } from "@Main/breadcrumbs";

export class DispatchAdHocViewModel extends window.Crm.Service.ViewModels.ServiceOrderCreateViewModel {
	dispatch = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>(null);
	errors = ko.validation.group([this.dispatch, this.serviceOrder], {deep: true});
	isThereAnyServiceObject = ko.observable<boolean>(true);
	technicianFilterQuery: Function;
	isModal = ko.observable<boolean>(false);

	constructor() {
		super();
		this.lookups.components = {$tableName: "CrmService_Component"};
		this.lookups.errorCodes = { $tableName: "CrmService_ErrorCode" };
		this.technicianFilterQuery = Helper.User.filterUserQuery;
	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		let count = await window.database.CrmService_ServiceObject.count();
		this.isThereAnyServiceObject(count > 0);

		const dispatch = window.database.CrmService_ServiceOrderDispatch.defaultType.create();
		dispatch.ComponentKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.components, dispatch.ComponentKey);
		dispatch.Date = window.moment().startOf('minute').toDate();
		dispatch.Date.setUTCMinutes(Math.ceil(dispatch.Date.getUTCMinutes() / 5) * 5);
		dispatch.EndDate = window.moment(dispatch.Date).add(window.moment.duration(window.Crm.Service.Settings.ServiceOrder.DefaultDuration)).toDate();
		dispatch.EndDate.setUTCMinutes(Math.ceil(dispatch.Date.getUTCMinutes() / 5) * 5);
		dispatch.StatusKey = "Released";
		dispatch.OrderId = this.serviceOrder().Id();
		dispatch.Username = this.currentUser().Id;
		window.database.add(dispatch);
		this.dispatch(dispatch.asKoObservable());

		Object.defineProperty(this.dispatch(), "release", { value: ko.pureComputed<boolean>({
			read: () => this.dispatch().StatusKey() === "Released",
			write: value => {
				this.dispatch().StatusKey(value ? "Released" : "Scheduled");
			}
		})});
	};

	onAssignedTechnicianSelected(user: Main.Rest.Model.Main_User): void {
		if (user === null) {

			this.dispatch().Username(this.currentUser().Id);

		}
		else {
			this.dispatch().Username(user.Id);
		}
	}

	async submit() {
		this.loading(true);
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			this.errors.scrollToError();
			this.errors.expandCollapsiblesWithErrors();
			return;
		}
		let dispatchNo = await window.NumberingService.createNewNumberBasedOnAppSettings(window.Crm.Service.Settings.Dispatch.DispatchNoIsGenerated, window.Crm.Service.Settings.Dispatch.DispatchNoIsCreateable, this.dispatch().DispatchNo(), "SMS.ServiceOrderDispatch", window.database.CrmService_ServiceOrderDispatch, "DispatchNo");
		if (dispatchNo !== null) {
			this.dispatch().DispatchNo(dispatchNo)
		}
		
		this.serviceOrder().StatusKey("Released");

		if (!this.serviceOrder().OrderNo()) {
			this.dispatch().OrderId(this.serviceOrder().Id());
			const adHocSequenceName = window.Crm.Service.Settings.AdHoc.AdHocNumberingSequenceName;
			let serviceOrderNo = await window.NumberingService.getNextFormattedNumber(adHocSequenceName);
			this.serviceOrder().OrderNo(serviceOrderNo);
		}
		this.dispatch().TimeZone(Helper.Date.getTimeZoneForUser(await Helper.User.getCurrentUser()));
		const startDate = this.dispatch().Date();
		const endDate = this.dispatch().EndDate();

		await new window.Helper.ServiceOrder.CreateServiceOrderData(
			this.serviceOrder(),
			this.serviceOrder().ServiceOrderTemplate(),
			this.installationIds(),
			startDate,
			endDate
		).create();

		await window.database.saveChanges();
		window.location.hash = "/Crm.Service/Dispatch/DetailsTemplate/" + this.dispatch().Id();
	};


	async setBreadcrumbs(): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.getTranslatedString("CreateAdHocOrder"), null, window.location.hash, null, null)
		]);
	};
}

namespace("Crm.Service.ViewModels").DispatchAdHocViewModel = DispatchAdHocViewModel;