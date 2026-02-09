import {namespace} from "./namespace";
import {HelperDatabase} from "./helper/Helper.Database";

export type TimeLineEvent = {
	entityType?: string;
	secondaryColor?: any;
	description?: string;
	allDay: boolean;
	backgroundColor?: string;
	start: Date;
	end: Date;
	title: string;
	url?: string;
	hatched?: boolean
}

export class DashboardCalendarWidgetFilter {
	Value: string;
	Caption: string;
	Tooltip?: string;
}

export class DashboardCalendarWidgetViewModel {

	loading = ko.observable<boolean>(true);
	currentUser = ko.observable<string>(null);
	options: { 
		defaultDate?: Date,
		handleWindowResize?: false
	};
	lookups: LookupType = {};
	timelineEvents = ko.observableArray<any>([]);
	maxResults = ko.observable<number>(100);
	timelineStart = ko.observable<Date>(null);
	timelineEnd = ko.observable<Date>(null);
	filterOptions = ko.observableArray<DashboardCalendarWidgetFilter>([]);
	showAll = ko.observable<boolean>(false);
	selectedFilters = ko.observableArray<DashboardCalendarWidgetFilter>([]);
	changeStatus = {
		added: "added",
		deleted: "deleted"
	}

	constructor(options?: {}) {
		this.currentUser(window.Helper.User.getCurrentUserName());
		this.options = options || {};
		this.options["nextDayThreshold"] = "00:00:00";
		this.lookups = {};
		const now = new Date();
		this.timelineStart(now);
		this.timelineEnd(now);
	}

	async init(): Promise<void> {
		this.filterOptions(this.getTimelineEventQueries().map(x => x.filter));
		this.selectedFilters.subscribe(() => {
			if (this.selectedFilters().length === this.filterOptions().length && !this.showAll()) {
				this.showAll(true);
			} else if (this.selectedFilters().length != this.filterOptions().length && this.showAll()) {
				this.showAll(false);
			}

			this.setSavedFilters(this.selectedFilters().slice());
		});
		this.showAll.subscribe((showAll) => {
			if (showAll) {
				this.selectedFilters(this.filterOptions().slice());
			} else if (this.selectedFilters().length === this.filterOptions().length) {
				this.selectedFilters.removeAll();
			}
		});
		this.selectedFilters.subscribe(async changes => {
			if (this.timelineStart() === this.timelineEnd()) {
				return;
			}
			let addedFilterNames = changes.filter(change => change.status === this.changeStatus.added && change.moved === undefined).map(change => change.value.Value);
			let removeFilterNames = changes.filter(change => change.status === this.changeStatus.deleted && change.moved === undefined).map(change => change.value.Value);
			this.loading(true);
			let timeLineEvents = [];
			let queries = this.getTimelineEventQueries()
				.filter(x => addedFilterNames.some(addedFilterName => x.filter.Value === addedFilterName))
				.map(query => {
					return {
						queryable: query.query.take(this.maxResults()),
						method: "toArray" as const,
						handler: function(results) {
							timeLineEvents = timeLineEvents.concat(results.map(x => x.asKoObservable()));
						}
					}
				});
			await window.Helper.Batch.Execute(queries);
			this.timelineEvents(this.timelineEvents().concat(timeLineEvents));
			this.timelineEvents(this.removeTimeLineEvents(this.timelineEvents(), removeFilterNames));
			await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
			this.loading(false);
		}, null, "arrayChange");
		const savedFilters = this.getSavedFilters();
		if (!savedFilters) {
			this.showAll(true);
		} else {
			this.selectedFilters(this.filterOptions()
				.slice()
				.filter(item => savedFilters.some(filter => filter.Value === item.Value)));
		}
		this.currentUser.subscribe(this.filter.bind(this));
		this.timelineEnd.subscribe(this.filter.bind(this));
	}

	removeTimeLineEvents(timeLineEvents: any[], removedFilterNames: string[]): any[] {
		return timeLineEvents.filter(event => {
			return !removedFilterNames.some(removedFilterName => event.innerInstance.constructor.name === removedFilterName);
		});
	}

	dispose(): void {
	}

	async filter(): Promise<void> {
		if (this.timelineStart() === this.timelineEnd()) {
			return;
		}
		this.loading(true);
		let timeLineEvents = [];
		let queries = this.getTimelineEventQueries()
			.filter(x => this.selectedFilters().some(selectedFilter => selectedFilter.Value === x.filter.Value))
			.map(query => {
			return {
				queryable: query.query.take(this.maxResults()),
				method: "toArray" as const,
				handler: function(results) {
					timeLineEvents = timeLineEvents.concat(results.map(x => x.asKoObservable()));
				}
			}
		});
		await window.Helper.Batch.Execute(queries);
		this.timelineEvents(timeLineEvents);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		this.loading(false);
	}

	getTimelineEventQueries(): { query: $data.Queryable<any>, filter: DashboardCalendarWidgetFilter }[] {
		return [];
	}

	getTimelineEvent(item: any): any {
		return null;
	}

	setSavedFilters(filters: Array<{ Value: string, Caption: string }>): void {
		HelperDatabase.saveToLocalStorage("dashboardSavedFilters", JSON.stringify(filters));
	}

	getSavedFilters(): [{ Value: string, Caption: string }] {
		return JSON.parse(HelperDatabase.getFromLocalStorage("dashboardSavedFilters"));
	}
}

namespace("Main.ViewModels").DashboardCalendarWidgetViewModel ||= DashboardCalendarWidgetViewModel;