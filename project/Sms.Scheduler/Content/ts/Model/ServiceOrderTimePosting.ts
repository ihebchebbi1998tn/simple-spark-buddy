import {DateHelper, EventModel, EventModelConfig, Model, Store} from "@bryntum/schedulerpro";
import type {ISchedulerEvent} from "./Interfaces/ISchedulerEvent";

type ServiceOrderTimePostingMetaData = {
	type: "ServiceOrderTimePosting",
	data: Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting
}

export function isServiceOrderTimePosting(event: Model): event is ServiceOrderTimePosting {
	return event != null && event.constructor === ServiceOrderTimePosting;
}

export class ServiceOrderTimePosting extends EventModel implements ISchedulerEvent {
	durationUnit: string = "minute";
	OriginalData: Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting;
	Color: string = "Blue";
	ResourceKey: string;

	//constructor(data = null) {
	constructor(config?: Partial<EventModelConfig>, store?: Store, meta?: ServiceOrderTimePostingMetaData) {
		super(config, store, meta);

		this.OriginalData = meta.data;
		this.draggable = false;
		this.resizable = false;
		this.startDate = meta.data.From;
		this.endDate = meta.data.To;
		this.duration = DateHelper.diff(meta.data.From, meta.data.To, this.durationUnit);
		this.schedulingMode = 'FixedDuration';
		this.manuallyScheduled = true;
		this.ResourceKey = meta.data.Username;
	}

	static get $name() {
		return 'ServiceOrderTimePosting';
	}

	static get fields() {
		return [
			{name: 'OriginalData', type: 'object'},
			{name: 'name', type: 'string', dataSource: 'OriginalData.ItemNo'},
			{name: 'id', type: 'string', dataSource: 'OriginalData.Id'},
		]
	}
}