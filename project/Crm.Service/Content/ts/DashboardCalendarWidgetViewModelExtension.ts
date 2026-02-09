import type {
	DashboardCalendarWidgetFilter,
	TimeLineEvent
} from "@Main/DashboardCalendarWidgetViewModel";
import {Mixin} from "ts-mixer";

export class DashboardCalendarWidgetViewModelExtension extends window.Main.ViewModels.DashboardCalendarWidgetViewModel {
	constructor(options) {
		super(options);
		this.addTablesToLookups();
	}

	getTimelineEvent(it: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting | any): TimeLineEvent {
		if (window.database.CrmService_ServiceOrderDispatch && it.innerInstance instanceof window.database.CrmService_ServiceOrderDispatch.defaultType) {
			return window.Crm.Service.ViewModels.ServiceOrderDispatchListIndexViewModel.prototype.getTimelineEvent.call(this, it);
		}
		if (window.database.CrmService_ServiceOrderTimePosting && it.innerInstance instanceof window.database.CrmService_ServiceOrderTimePosting.defaultType) {
			const translatedTimePostingType = window.Helper.Lookup.getLookupValue(this.lookups.articleDescriptions, it.ItemNo());
			let timeLineEvent: TimeLineEvent = {
				entityType : window.Helper.getTranslatedString("TimePosting"),
				title: translatedTimePostingType,
				start: it.From() || it.Date(),
				end: it.To() || it.Date(),
				allDay: it.From() && it.To() && window.moment(it.From()).clone().startOf("day").isSame(it.From()) && window.moment(it.From()).clone().add(1, "day").startOf("day").isSame(it.To()),
				backgroundColor: window.Helper.Lookup.getLookupColor(this.lookups.articleDescriptions, it.ItemNo()),
				url: it.ServiceOrderDispatch() ? "#/Crm.Service/Dispatch/DetailsTemplate/" + window.ko.unwrap(window.ko.unwrap(it.ServiceOrderDispatch).Id) + "?tab=tab-time-postings" : null 
			};

			timeLineEvent.title = translatedTimePostingType;
			if (!!it.Description()) {
				timeLineEvent.title += it.Description();
			}
			timeLineEvent.description = it.ServiceOrder().OrderNo()

			return timeLineEvent;
		}
		return super.getTimelineEvent(it);
	};

	getTimelineEventQueries(): { query: $data.Queryable<any>, filter: DashboardCalendarWidgetFilter }[] {
		let queries = super.getTimelineEventQueries();
		if (window.database.CrmService_ServiceOrderDispatch && window.AuthorizationManager.isAuthorizedForAction("Dispatch", "CalendarEntry")) {
			queries.push({
				query: window.database.CrmService_ServiceOrderDispatch
					.include("ServiceOrder")
					.include("ServiceOrderTimePostings")
					.filter(function (it) {
							return it.Username === this.currentUser &&
								it.StatusKey in ["Released", "Read", "InProgress", "SignedByCustomer"] &&
								it.Date >= this.start &&
								it.Date <= this.end
						},
						{
							currentUser: this.currentUser(),
							start: this.timelineStart(),
							end: this.timelineEnd()
						}), filter: {
					Value: window.database.CrmService_ServiceOrderDispatch.defaultType.name + "_Worklist",
					Caption: window.Helper.String.getTranslatedString("UpcomingDispatches"),
					Tooltip: window.Helper.String.getTranslatedString("UpcomingDispatchesTooltip")
				}
			});
			if (window.AuthorizationManager.isAuthorizedForAction("ScheduledDispatches", "View")) {
				queries.push({
					query: window.database.CrmService_ServiceOrderDispatch
						.include("ServiceOrder")
						.include("ServiceOrderTimePostings")
						.filter(function (it) {
							return it.Username === this.currentUser &&
								it.StatusKey in ["Scheduled"] &&
								it.Date >= this.start &&
								it.Date <= this.end
						},
						{
							currentUser: this.currentUser(),
							start: this.timelineStart(),
							end: this.timelineEnd()
						}), filter: {
							Value: window.database.CrmService_ServiceOrderDispatch.defaultType.name + "_Scheduled",
							Caption: window.Helper.String.getTranslatedString("ScheduledDispatches"),
							Tooltip: window.Helper.String.getTranslatedString("ScheduledDispatchesTooltip")
						}
				});
			}
			queries.push({
				query: window.database.CrmService_ServiceOrderDispatch
					.include("ServiceOrder")
					.include("ServiceOrderTimePostings")
					.filter(function (it) {
							return it.Username === this.currentUser &&
								it.StatusKey in ["ClosedNotComplete", "ClosedComplete", "Rejected"] &&
								it.Date >= this.start &&
								it.Date <= this.end
						},
						{
							currentUser: this.currentUser(),
							start: this.timelineStart(),
							end: this.timelineEnd()
						}), filter: {
					Value: window.database.CrmService_ServiceOrderDispatch.defaultType.name + "_Closed",
					Caption: window.Helper.String.getTranslatedString("ClosedDispatches"),
					Tooltip: window.Helper.String.getTranslatedString("ClosedDispatchesTooltip")
				}
			});
		}
		if (window.database.CrmService_ServiceOrderTimePosting && window.AuthorizationManager.isAuthorizedForAction("ServiceOrderTimePosting", "CalendarEntry")) {
			queries.push({
				query: window.database.CrmService_ServiceOrderTimePosting
					.include("ServiceOrder")
					.include("ServiceOrderDispatch")
					.filter(function (it) {
							return it.Username === this.currentUser &&
								it.From >= this.start &&
								it.To <= this.end &&
								it.DispatchId !== null
						},
						{
							currentUser: this.currentUser(),
							start: this.timelineStart(),
							end: this.timelineEnd()
						}), filter: {
					Value: window.database.CrmService_ServiceOrderTimePosting.defaultType.name,
					Caption: window.Helper.String.getTranslatedString("TimePostings"),
					Tooltip: window.Helper.String.getTranslatedString("TimePostingsTooltip")
				}
			});
		}
		return queries;
	}

	removeTimeLineEvents(timeLineEvents: any[], removedFilterNames: string[]): any[] {
		const events = super.removeTimeLineEvents(timeLineEvents, removedFilterNames);
		if (removedFilterNames.length > 0 && removedFilterNames.indexOf(window.database.CrmService_ServiceOrderDispatch.defaultType.name + "_Worklist") === -1 && removedFilterNames.indexOf(window.database.CrmService_ServiceOrderDispatch.defaultType.name + "_Scheduled") === -1 && removedFilterNames.indexOf(window.database.CrmService_ServiceOrderDispatch.defaultType.name + "_Closed") === -1) {
			return events;
		}

		let removedStatusKeys = [];
		if (removedFilterNames.indexOf(window.database.CrmService_ServiceOrderDispatch.defaultType.name + "_Closed") !== -1) {
			removedStatusKeys = [...removedStatusKeys, "ClosedNotComplete", "ClosedComplete", "Rejected"];
		}
		if (removedFilterNames.indexOf(window.database.CrmService_ServiceOrderDispatch.defaultType.name + "_Scheduled") !== -1) {
			removedStatusKeys = [...removedStatusKeys, "Scheduled"];
		}
		if (removedFilterNames.indexOf(window.database.CrmService_ServiceOrderDispatch.defaultType.name + "_Worklist") !== -1) {
			removedStatusKeys = [...removedStatusKeys, "Released", "Read", "InProgress", "SignedByCustomer"];
		}

		return events.filter(e => {
			return !(new Set(removedStatusKeys).has(e.innerInstance.StatusKey));
		})
	}

	addTablesToLookups(): void {
		if (window.database.Main_Country) {
			this.lookups.countries = {$tableName: "Main_Country", $lazy: true};
			this.timelineEvents.subscribe(timelineEvents => {
				timelineEvents.forEach(timelineEvent => {
					if (window.database.CrmService_ServiceOrderDispatch && timelineEvent.innerInstance instanceof window.database.CrmService_ServiceOrderDispatch.defaultType && timelineEvent.ServiceOrder() && timelineEvent.ServiceOrder().CountryKey() !== null) {
						this.lookups.countries[timelineEvent.ServiceOrder().CountryKey()] ||= null;
					}
				});
			});
		}
		if (window.database.Main_Region) {
			this.lookups.regions = {$tableName: "Main_Region", $lazy: true};
			this.timelineEvents.subscribe(timelineEvents => {
				timelineEvents.forEach(timelineEvent => {
					if (window.database.CrmService_ServiceOrderDispatch && timelineEvent.innerInstance instanceof window.database.CrmService_ServiceOrderDispatch.defaultType && timelineEvent.ServiceOrder() && timelineEvent.ServiceOrder().RegionKey() !== null) {
						this.lookups.regions[timelineEvent.ServiceOrder().RegionKey()] ||= null;
					}
				});
			});
		}
		if (window.database.CrmService_ServiceOrderType) {
			this.lookups.serviceOrderTypes = {$tableName: "CrmService_ServiceOrderType", $lazy: true};
			this.timelineEvents.subscribe(timelineEvents => {
				timelineEvents.forEach(timelineEvent => {
					if (window.database.CrmService_ServiceOrderDispatch && timelineEvent.innerInstance instanceof window.database.CrmService_ServiceOrderDispatch.defaultType && timelineEvent.ServiceOrder() && timelineEvent.ServiceOrder().TypeKey() !== null) {
						this.lookups.serviceOrderTypes[timelineEvent.ServiceOrder().TypeKey()] ||= null;
					}
				});
			});
		}
		if (window.database.CrmService_ServiceOrderDispatchStatus) {
			this.lookups.serviceOrderDispatchStatuses = {$tableName: "CrmService_ServiceOrderDispatchStatus", $lazy: true};
			this.timelineEvents.subscribe(timelineEvents => {
				timelineEvents.forEach(timelineEvent => {
					if (window.database.CrmService_ServiceOrderDispatch && timelineEvent.innerInstance instanceof window.database.CrmService_ServiceOrderDispatch.defaultType && timelineEvent.StatusKey() !== null) {
						this.lookups.serviceOrderDispatchStatuses[timelineEvent.StatusKey()] ||= null;
					}
				});
			});
		}
		if (window.database.CrmArticle_ArticleDescription) {
			this.lookups.articleDescriptions = {$tableName: "CrmArticle_ArticleDescription", $lazy: true};
			this.timelineEvents.subscribe(timelineEvents => {
				timelineEvents.forEach(timelineEvent => {
					if (window.database.CrmService_ServiceOrderTimePosting && timelineEvent.innerInstance instanceof window.database.CrmService_ServiceOrderTimePosting.defaultType && timelineEvent.ItemNo() !== null) {
						this.lookups.articleDescriptions[timelineEvent.ItemNo()] ||= null;
					}
				});
			});
		}
	}
}

window.Main.ViewModels.DashboardCalendarWidgetViewModel = Mixin(DashboardCalendarWidgetViewModelExtension);