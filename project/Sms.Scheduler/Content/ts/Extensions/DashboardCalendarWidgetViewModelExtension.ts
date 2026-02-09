import { Mixin } from "ts-mixer";
import type {
	DashboardCalendarWidgetFilter,
	TimeLineEvent
} from "@Main/DashboardCalendarWidgetViewModel";


export class DashboardCalendarWidgetViewModelExtension extends window.Main.ViewModels.DashboardCalendarWidgetViewModel {
	constructor(options) {
		super(options);
		if (window.database.CrmPerDiem_TimeEntryType) {
			this.lookups.timeEntryTypes = { $tableName: "CrmPerDiem_TimeEntryType", $lazy: true };
			this.timelineEvents.subscribe(timelineEvents => {
				timelineEvents.forEach(timelineEvent => {
					if (window.database.SmsScheduler_Absence && timelineEvent.innerInstance instanceof window.database.SmsScheduler_Absence.defaultType && timelineEvent.TimeEntryTypeKey() !== null) {
						this.lookups.timeEntryTypes[timelineEvent.TimeEntryTypeKey()] ||= null;
					}
				});
			});
		}
	}

	getTimelineEvent(it: Sms.Scheduler.Rest.Model.ObservableSmsScheduler_Absence): TimeLineEvent {
		if (window.database.SmsScheduler_Absence
			&& it.innerInstance instanceof window.database.SmsScheduler_Absence.defaultType) {
			return this.getTimelineEventForSmsAbsence(it);
		}
		return super.getTimelineEvent(it);
	};

	getTimelineEventQueries(): { query: $data.Queryable<any>, filter: DashboardCalendarWidgetFilter }[] {
		let queries = super.getTimelineEventQueries();
		if (window.database.SmsScheduler_Absence && window.Sms.Scheduler.Settings.DashboardCalendar.ShowAbsencesInCalendar) {
			queries.push({
				query: window.database.SmsScheduler_Absence
					.filter(function (it) {
						return it.ResponsibleUser === this.currentUser &&
							it.From < this.end &&
							this.start < it.To
					},
						{
							currentUser: this.currentUser(),
							start: this.timelineStart(),
							end: this.timelineEnd()
						}), filter: {
							Value: window.database.SmsScheduler_Absence.defaultType.name,
							Caption: window.Helper.String.getTranslatedString("SmsScheduler_Absence"),
							Tooltip: window.Helper.String.getTranslatedString("SmsAbsenceEntryTooltip"),
						}
			});
		}
		return queries;
	}

	getTimelineEventForSmsAbsence(absence: Sms.Scheduler.Rest.Model.ObservableSmsScheduler_Absence): TimeLineEvent {
		const startTime = absence.From();
		const endTime = absence.To();
		const absenceType = this.lookups.timeEntryTypes[absence.TimeEntryTypeKey()];
		const description: string = absence.Description();

		return {
			title: absenceType.Value ? absenceType.Value : window.Helper.String.getTranslatedString("SmsScheduler_Absence"),
			entityType: absenceType.Value ? null : window.Helper.String.getTranslatedString("SmsScheduler_Absence"),
			start: startTime,
			end: endTime,
			allDay: false,
			backgroundColor: absenceType.Color ? absenceType.Color : '#0cf993',
			description: description
		};
	};
}

window.Main.ViewModels.DashboardCalendarWidgetViewModel = Mixin(DashboardCalendarWidgetViewModelExtension);