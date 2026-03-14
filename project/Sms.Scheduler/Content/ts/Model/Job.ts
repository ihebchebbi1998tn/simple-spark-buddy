import {EventModelConfig, GridRowModel, type Model, Store} from "@bryntum/schedulerpro";
import type { ServiceOrder } from "./ServiceOrder";

export function isJob(event: Model): event is Job {
	return event !== null && event.constructor === Job;
}

export class Job extends GridRowModel {
	OriginalData: Crm.Service.Rest.Model.CrmService_ServiceOrderTime;
	readonly serviceOrder: ServiceOrder;
	type: string;

	constructor(config?: Partial<EventModelConfig>, store?: Store, meta?: { data: Crm.Service.Rest.Model.CrmService_ServiceOrderTime, serviceOrder: ServiceOrder }) {
		super(config, store);
		this.OriginalData = meta.data;
		this.serviceOrder = meta.serviceOrder;
		this.type = "Job";
	}

	static get fields() {
		return [
			{name: 'OriginalData', type: 'object'},
			{name: 'id', dataSource: 'OriginalData.Id', type: 'string'}
		]
	}

	public get name() {
		return this.toString();
	}

	override toString() {
		return window.Helper.ServiceOrderTime.getAutocompleteDisplay(this.OriginalData, null);
	}
}