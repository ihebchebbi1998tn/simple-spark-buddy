import {EventModel, type EventModelConfig, type Store, Model} from "@bryntum/schedulerpro";
import type {ISchedulerEvent} from "./Interfaces/ISchedulerEvent";

type AbsenceMetaData = {
	type: "Absence",
	data: Sms.Scheduler.Rest.Model.SmsScheduler_Absence | Crm.Article.Rest.Model.CrmArticle_ArticleDowntime,
	typeData: Crm.PerDiem.Rest.Model.Lookups.CrmPerDiem_TimeEntryType | Crm.Article.Model.Lookups.CrmArticle_ArticleDowntimeReason
}
export function isAbsence(event: Model): event is Absence {
	return event != null && event.constructor === Absence;
}
export class Absence extends EventModel implements ISchedulerEvent {
	static get $name() {
		return 'Absence';
	}
	durationUnit = "minute";
	OriginalData: Sms.Scheduler.Rest.Model.SmsScheduler_Absence | Crm.Article.Rest.Model.CrmArticle_ArticleDowntime;
	ResourceKey: string;
	AbsenceTypeData: Crm.PerDiem.Rest.Model.Lookups.CrmPerDiem_TimeEntryType | Crm.Article.Model.Lookups.CrmArticle_ArticleDowntimeReason;

	static get fields() {
		return [
			{ name: 'OriginalData', type: 'object'},
			{ name: 'AbsenceTypeData', type: 'object'},
			{ name: 'id', type: 'string', dataSource: 'OriginalData.Id' },
		]
	}
	//@ts-ignore
	public get name(): string {
		return this.OriginalData.Description;
	}

	public get AbsenceType(): string {
		return this.AbsenceTypeData.Value;
	}

	constructor(config?: Partial<EventModelConfig>, store?: Store, meta?: AbsenceMetaData) {
		super(config, store, meta);

		this.ignoreResourceCalendar = true;
		this.OriginalData = meta.data;
		this.AbsenceTypeData = meta.typeData;
		this.duration = window.moment(meta.data.To).diff(meta.data.From, this.durationUnit as any, true);
		this.manuallyScheduled = true;
	}
	get Color(): string {
		return this.AbsenceTypeData.Color;
	}
	//@ts-ignore
	public get readonly() {
		return this.OriginalData instanceof Crm.PerDiem.Rest.Model.CrmPerDiem_UserTimeEntry;
	}
	//@ts-ignore
	get eventColor() {
		return this.Color;
	}
}