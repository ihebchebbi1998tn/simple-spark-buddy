import {namespace} from "@Main/namespace";
import type {Moment} from "moment";
import type {DashboardCalendarWidgetViewModelExtension} from "./DashboardCalendarWidgetViewModelExtension";

class DispatchEvent {
	dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch;
	start: Moment;
	end: Moment
}

export class DispatchScheduleModalViewModel extends window.Main.ViewModels.ViewModelBase {
	calendar = new window.Main.ViewModels.DashboardCalendarWidgetViewModel({
		dayClick: (day: Moment) => this.onDayOrEventClick(day),
		eventClick: (dispatchEvent: DispatchEvent) => this.onDayOrEventClick(dispatchEvent)
	}) as DashboardCalendarWidgetViewModelExtension;
	canSelectUser = ko.observable<boolean>(window.AuthorizationManager.isAuthorizedForAction("Dispatch", "CreateForOtherUsers"));
	dispatches = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>([]);
	dispatchedUsernames = ko.pureComputed<string[]>(() => {
		return window._.uniq(this.dispatches().map(function (x) {
			return x.Username();
		})).filter(Boolean);
	});
	isAddAllowed = ko.observable<boolean>(false);
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	errors = ko.validation.group(this.dispatches, {deep: true, live: true});
	returnUrl: string;
	serviceOrderId: string;
	parentViewModel: any;

	constructor(parentViewModel: any) {
		super();
		this.parentViewModel = parentViewModel;
		this.dispatchedUsernames.subscribe(usernames => {
			this.calendar.currentUser(usernames.length === 1 ? usernames[0] : null);
		});
		this.calendar.timelineEvents.subscribe(timelineEvents => {
			this.calendar.timelineEvents.remove(x => x !== this.dispatches()[0] && x.Id() === this.dispatches()[0].Id());
			for (const dispatch of this.dispatches()) {
				if (this.calendar.timelineEvents.indexOf(dispatch) === -1){
					this.calendar.timelineEvents.push(dispatch);
				}
			}
		});
		let getTimelineEvent = this.calendar.getTimelineEvent;
		this.calendar.getTimelineEvent = (it) => {
			if (this.dispatches().indexOf(it) !== -1) {
				return {
					allDay: false,
					backgroundColor: "#4CAF50",
					dispatch: it,
					end: it.EndDate(),
					start: it.Date(),
					title: this.isAddAllowed() ? window.Helper.String.getTranslatedString("Dispatch") + " #" + (this.dispatches().indexOf(it) + 1) : it.ServiceOrder().OrderNo(),
					url: "#"
				};
			}
			const result = getTimelineEvent.call(this.calendar, it);
			result.url = "#";
			return result;
		};
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.returnUrl = params.returnUrl;
		if (params.tab) {
			this.returnUrl += "?tab=" + params.tab;
		}
		let username = window.Helper.User.getCurrentUserName();
		if (params.username !== undefined && this.canSelectUser()) {
			username = params.username || null;
		}
		if (id === window.Helper.String.emptyGuid()) {
			this.isAddAllowed(true);
			this.serviceOrderId = params.serviceOrderId;
			if (!this.serviceOrder) {
				this.serviceOrder = ko.observable((await window.database.CrmService_ServiceOrderHead
					.find(this.serviceOrderId)).asKoObservable())
			}
			this.addNewDispatch(username);
		} else {
			let dispatch = await window.database.CrmService_ServiceOrderDispatch
				.include("ServiceOrder")
				.find(id);
			username = dispatch.Username;
			window.database.attachOrGet(dispatch);
			this.pushDispatch(dispatch);
			this.calendar.options.defaultDate = dispatch.Date;
			if (!this.serviceOrder) {
				this.serviceOrder = ko.observable(dispatch.ServiceOrder.asKoObservable());
			}
		}
		this.calendar.currentUser(username);
		await this.calendar.init();
	};

	filterTechnicianQuery(query, term: string, dispatch: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch): $data.Queryable<Main.Rest.Model.Main_User> {
		return window.Helper.Dispatch.filterTechnicianQuery(query, term, null); 
	};

	onDayOrEventClick(dateOrEvent: DispatchEvent | Moment): boolean {
		let dispatch = this.dispatches()[this.dispatches().length - 1];
		let date: Moment;
		if (window.moment.isMoment(dateOrEvent)) {
			date = dateOrEvent;
		} else {
			if (dateOrEvent.dispatch === dispatch) {
				return false;
			}
			date = dateOrEvent.end || dateOrEvent.start;
		}

		dispatch.Date(date.local(true).toDate());
		return false;
	};

	pushDispatch(dispatch: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch): void {
		const observableDispatch = dispatch.asKoObservable();
		// @ts-ignore
		observableDispatch.release = ko.pureComputed<boolean>({
			read: () => observableDispatch.StatusKey() === "Released",
			write: value => {
				observableDispatch.StatusKey(value ? "Released" : "Scheduled");
			}
		});
		if (!dispatch.StatusKey) {
			// @ts-ignore
			observableDispatch.release(true);
		}
		this.dispatches.push(observableDispatch);
		this.calendar.timelineEvents.push(observableDispatch);
	};

	async addNewDispatch(username: string): Promise<void> {
		const dispatch = window.database.CrmService_ServiceOrderDispatch.defaultType.create();
		dispatch.Id = window.$data.createGuid().toString().toLowerCase();
		dispatch.OrderId = this.serviceOrderId;
		dispatch.StatusKey = "Released";
		dispatch.Username = username;
		let moment = window.moment();
		const dispatches = this.dispatches();
		const lastDispatch = dispatches[dispatches.length - 1];
		if (lastDispatch) {
			moment = window.moment(lastDispatch.EndDate());
			dispatch.Username = lastDispatch.Username();
		}
		dispatch.Date = moment.toDate();
		dispatch.EndDate = window.moment(dispatch.Date).add(window.moment.duration(window.Crm.Service.Settings.ServiceOrder.DefaultDuration)).toDate();
		window.database.add(dispatch);
		if (this.serviceOrderId) {
			dispatch.ServiceOrder = await window.database.CrmService_ServiceOrderHead.find(this.serviceOrderId);
		}
		this.pushDispatch(dispatch);
	};

	removeDispatch(index: KnockoutObservable<number>): void {
		this.dispatches.splice(index(), 1).forEach(dispatch => {
			this.calendar.timelineEvents.remove(dispatch);
			window.database.detach(dispatch.innerInstance);
		});
	};

	dispose(): void {
		this.dispatches().forEach(dispatch => {
			window.database.detach(dispatch.innerInstance);
		});
	};

	async save(): Promise<void> {
		this.loading(true);
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}
		let releaseServiceOrder = false;
		let partiallyReleaseServiceOrder = false;
		try {
			await Promise.all(this.dispatches().map(async dispatch => {
				let dispatchNo = await window.NumberingService.createNewNumberBasedOnAppSettings(window.Crm.Service.Settings.Dispatch.DispatchNoIsGenerated, window.Crm.Service.Settings.Dispatch.DispatchNoIsCreateable, dispatch.DispatchNo(), "SMS.ServiceOrderDispatch", window.database.CrmService_ServiceOrderDispatch, "DispatchNo");
				if (dispatchNo !== null) {
					dispatch.DispatchNo(dispatchNo)
				}
				if (dispatch.StatusKey() === "Released") {
					releaseServiceOrder = true;
				}
				if (dispatch.StatusKey() === "PartiallyReleased") {
					partiallyReleaseServiceOrder = true;
				}
				if (dispatch.StatusKey() === 'CancelledNotComplete') {
					dispatch.StatusKey("Scheduled")
				}
				if (this.dispatch) {
					this.dispatch(dispatch);
				}
			}))
			if (!releaseServiceOrder) {
				window.database.attachOrGet(this.serviceOrder().innerInstance);
				if (partiallyReleaseServiceOrder) {
					this.serviceOrder().StatusKey("PartiallyReleased");
				} else {
					this.serviceOrder().StatusKey("Scheduled");
				}
			}
			let serviceOrder: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead;
			if (this.serviceOrder()) {
				serviceOrder = this.serviceOrder();
			}
			if (this.serviceOrderId) {
				serviceOrder = (await window.database.CrmService_ServiceOrderHead
					.find(this.serviceOrderId)).asKoObservable();
			}
			if (serviceOrder) {
				window.database.attachOrGet(serviceOrder.innerInstance);
				serviceOrder.StatusKey("Released");
			}
			await window.database.saveChanges();
			$(".modal:visible").modal("hide");
			if (this.returnUrl) {
				window.location.hash = this.returnUrl;
			}
			this.loading(false);
		} catch(e) {
			this.loading(false);
			window.Log.error(e);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};
}

namespace("Crm.Service.ViewModels").DispatchScheduleModalViewModel = DispatchScheduleModalViewModel;