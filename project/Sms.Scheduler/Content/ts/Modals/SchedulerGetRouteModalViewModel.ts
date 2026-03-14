import {namespace} from "@Main/namespace";
import {isTechnician, Technician} from "../Model/Technicians";
import type {SchedulerDetailsViewModel} from "../SchedulerDetailsViewModel";
import {RouteData} from "../Model/RouteData";
import moment from "moment";
import type { Model } from "@bryntum/schedulerpro";

export class SchedulerGetRouteModalViewModel extends window.Main.ViewModels.ViewModelBase {
	parentViewModel: SchedulerDetailsViewModel = null;
	loading: KnockoutObservable<boolean> = ko.observable<boolean>(false);
	availableResources: KnockoutObservableArray<Technician> = ko.observableArray<Technician>([]);

	routeData = ko.observableArray<RouteData>([]).extend({
		validation: {
			message: window.Helper.String.getTranslatedString("MinResourceNumber"),
			validator: (val: RouteData[], minLength: number) => Array.isArray(val) && val.length >= minLength && val.some(r => r.technician()),
			params: 1,
			propertyName: 'routeData'
		}
	});

	startDate = ko.observable<Date>(null).extend({
		validation: [{
			validator: (value, params) => value != null,
			message: params => window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("StartDate"))
		}]
	});

	endDate = ko.observable<Date>(null).extend({
		validation: [{
			validator: (value, params) => value != null,
			message: params => window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("EndDate"))
		},
			{
				validator: (value, params) => moment(value).isSameOrAfter(this.startDate(), "day"),
				onlyIf: () => this.startDate() != null && this.endDate() != null,
				message: params => window.Helper.getTranslatedString("RuleViolation.Less").replace("{0}", window.Helper.String.getTranslatedString("StartDate")).replace("{1}", window.Helper.getTranslatedString("EndDate"))
			}]
	});

	useTechnicianHomeAddressAsOrigin = ko.observable(false);
	useTechnicianHomeAddressAsFinalDestination = ko.observable(false);
	getRoutePerDay = ko.observable(false);

	errors = ko.validation.group([this.startDate, this.endDate, this.routeData], {deep: true});

	constructor(parentViewModel: SchedulerDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
		//Set start and end date to the timeline's start and end date
		this.startDate(this.parentViewModel.schedulerComponent().scheduler.timeAxis.startDate);
		this.endDate(this.parentViewModel.schedulerComponent().scheduler.timeAxis.endDate);
	}

	async init(): Promise<void> {
		this.availableResources(this.parentViewModel.scheduler().resourceStore.records.filter(isTechnician));

		(this.parentViewModel.scheduler().selectedRecords as Model[]).filter(isTechnician).forEach(r => {
			const colors = this.parentViewModel.profile().ClientConfig.RouteColors;
			this.routeData.push(new RouteData(ko.observable(r.OriginalData.Id), ko.observable(colors[this.routeData().length % colors.length])));
		});
	}

	async save(data, event): Promise<void> {
		$(event.target).closest(".modal-content").data("keepchanges", true);

		if (this.errors().length > 0) {
			this.errors.showAllMessages();
			this.errors.scrollToError();
			return;
		}

		this.loading(true);
		try {
			this.parentViewModel.schedulerComponent().routeConfig({
				routeData: this.routeData(),
				useTechnicianHomeAddressAsOrigin: this.useTechnicianHomeAddressAsOrigin(),
				useTechnicianHomeAddressAsFinalDestination: this.useTechnicianHomeAddressAsFinalDestination(),
				getRoutePerDay: this.getRoutePerDay()
			});
			await this.parentViewModel.schedulerComponent().getRoute(this.parentViewModel.schedulerComponent().routeConfig(), window.moment(this.startDate()).startOf('d').toDate(), window.moment(this.endDate()).add(1, 'd').startOf('d').toDate());
			$(".modal:visible").modal("hide");
		} catch (e) {
			swal(window.Helper.String.getTranslatedString("Error"), window.Helper.String.getTranslatedString("SomethingWentWrong"), "error");
			window.Log.error((e as Error));
		} finally {
			this.loading(false);
		}
	}

	resourceFilter(query, term) {
		let self = this;
		if (self.routeData().length > 0) {
			query = query.filter("t => t.Id in availableResources", {availableResources: window._.xor(self.availableResources().map(r => r.id), self.routeData().map(d => d.technician())).filter(Boolean)});
		} else {
			query = query.filter("t => t.Id in availableResources", {availableResources: self.availableResources().map(r => r.id)});
		}
		if (term) {
			query = query.filter("it.Id.indexOf(this.term) !== -1", {term: term});
		}
		return query
			.orderBy("it.LastName")
			.orderBy("it.FirstName");
	}

	addRouteData(): void {
		const colors = this.parentViewModel.profile().ClientConfig.RouteColors;
		this.routeData.push(new RouteData(ko.observable(null), ko.observable(colors[this.routeData().length % colors.length])));
	};

	removeRouteData(routeData: RouteData): void {
		this.routeData.remove(routeData);
	};

	cancel(data, event): void {
		$(event.target).closest(".modal-content").data("keepchanges", true);
		$(".modal:visible").modal("hide");
	}
}

namespace("Sms.Scheduler.ViewModels").SchedulerGetRouteModalViewModel = SchedulerGetRouteModalViewModel;