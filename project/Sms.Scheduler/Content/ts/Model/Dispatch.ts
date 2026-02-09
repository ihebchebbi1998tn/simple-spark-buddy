import {DateHelper, EventModel, EventModelConfig, Store} from "@bryntum/schedulerpro";
import {Lazy, LazyOfTTMetadata} from "../Lazy";
import {ServiceOrder} from "./ServiceOrder";
import type {ServiceOrderTimePosting} from "./ServiceOrderTimePosting";
import type {ISchedulerEvent} from "./Interfaces/ISchedulerEvent";
import {namespace} from "@Main/namespace";

type DispatchMetaData = {
	type: "ServiceOrderDispatch",
	data: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch,
	serviceOrder: ServiceOrder,
	timePostings?: ServiceOrderTimePosting[],
	serviceOrderTimeDispatches?: Crm.Service.Rest.Model.CrmService_ServiceOrderTimeDispatch[],
}

export function isDispatch(event: any): event is Dispatch {
	return event?.OriginalData instanceof Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch;
}

export class Dispatch extends EventModel implements ISchedulerEvent {
	static readonly lookups: LookupType = {};
	durationUnit: string = "minute";
	OriginalData: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch;
	SchedulingGroupId: string;
	TimePostings: ServiceOrderTimePosting[];
	private readonly ___ServiceOrder: Lazy<ServiceOrder>;
	private _ServiceOrderTimeDispatch: LazyOfTTMetadata<Crm.Service.Rest.Model.CrmService_ServiceOrderTimeDispatch[], string[]> = null;
	private _ServiceOrderTimeDispatches: Crm.Service.Rest.Model.CrmService_ServiceOrderTimeDispatch[];

	constructor(config?: Partial<EventModelConfig>, store?: Store, meta?: DispatchMetaData) {
		super(config, store, meta);

		this.OriginalData = meta.data;

		this.startDate = meta.data.Date;

		if (meta.data.NetWorkMinutes) {
			this.duration = DateHelper.as(this.durationUnit, meta.data.NetWorkMinutes, "minute");
		} else {
			this.duration = null;
			this.manuallyScheduled = true;
		}

		this.endDate = meta.data.EndDate;

		if (meta.serviceOrder) {
			this.___ServiceOrder = new Lazy(() => meta.serviceOrder);
		} else if (this.OriginalData.ServiceOrder) {
			this.___ServiceOrder = new Lazy(() => new ServiceOrder(undefined, undefined, {
				type: "ServiceOrderHead",
				data: this.OriginalData.ServiceOrder
			}));
		} else {
			throw new Error("Dispatch constructor needs ServiceOrder.");
		}

		this.TimePostings = meta.timePostings ?? [];

		this._ServiceOrderTimeDispatches = meta.serviceOrderTimeDispatches ?? [];
	}

	static get $name() {
		return 'Dispatch';
	}

	static get fields() {
		return [
			{name: 'OriginalData', type: 'object'},
			{name: 'id', type: 'string', dataSource: 'OriginalData.Id'},
			{name: 'SchedulingGroupId', type: 'string', dataSource: 'OriginalData.ExtensionValues.SchedulingGroupId'},
			{name: 'ServiceOrder', type: 'object'},
			{name: 'CreateUser', type: 'string', dataSource: 'OriginalData.CreateUser'},
			{name: 'CreateDate', type: 'Date', dataSource: 'OriginalData.CreateDate'},
			{name: 'ModifyUser', type: 'string', dataSource: 'OriginalData.ModifyUser'},
			{name: 'ModifyDate', type: 'Date', dataSource: 'OriginalData.ModifyDate'},
			{name: 'Date', type: 'Date', dataSource: 'OriginalData.Date'},
			{name: 'EndDate', type: 'Date', dataSource: 'OriginalData.EndDate'},
			{name: 'TimeZone', type: 'string', dataSource: 'OriginalData.TimeZone'},
			{
				name: 'manuallyScheduled',
				type: 'boolean',
				defaultValue: `${JSON.parse(window.Sms.Scheduler.Settings.WorkingTime.IgnoreWorkingTimesInEndDateCalculation)}`
			},
		]
	}

	_Status: LazyOfTTMetadata<any, string> = null;

	get Status(): Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderDispatchStatus {
		let key = this.OriginalData.StatusKey;
		if (this._Status == null || this._Status.Metadata != key) {
			this._Status = new LazyOfTTMetadata(() => window.Helper.Scheduler.CreateLookupProxy(Dispatch.lookups.serviceOrderDispatchStatuses, key), key);
		}
		return this._Status.value;
	}

	private _NetWorkMinutes: LazyOfTTMetadata<any, number> = null;

	public get NetWorkMinutes(): number {
		let key = this.OriginalData.NetWorkMinutes;
		if (this._NetWorkMinutes == null || this._NetWorkMinutes.Metadata !== key) {
			let result = null;

			if (this.OriginalData.NetWorkMinutes != null) {
				const formatted = window.moment.duration(key, "minute").format();

				result = {
					valueOf: () => key,
					toString: () => formatted
				};
			}

			this._NetWorkMinutes = new LazyOfTTMetadata(() => result, key);
		}
		return this._NetWorkMinutes.value;
	}

	public get StartTime(): Date {
		return this.OriginalData.Date;
	}

	public get ServiceOrder(): ServiceOrder {
		return this.___ServiceOrder.value;
	}

	public get dispatchInEditableState() {
		return this.OriginalData.StatusKey === 'Scheduled' || (window.Sms.Scheduler.Settings.DispatchesAfterReleaseAreEditable && ['Scheduled', 'Released', 'Read'].includes(this.OriginalData.StatusKey));
	}

	public get readonly() {
		return ['Rejected', 'ClosedNotComplete', 'ClosedComplete'].includes(this.OriginalData.StatusKey) || !this.dispatchInEditableState;
	}
	//@ts-ignore
	public get name(): string {
		return this.ServiceOrder.OriginalData.OrderNo;
	}

	public get ServiceOrderTimeDispatches(): Crm.Service.Rest.Model.CrmService_ServiceOrderTimeDispatch[] {
		const keys = this.ServiceOrder.ServiceOrderTimes?.filter(x => this._ServiceOrderTimeDispatches?.some(y => y.ServiceOrderTimeId == x.Id))?.map(x => x.Id) || [];

		if (this._ServiceOrderTimeDispatch == null || !window._.isEqual(this._ServiceOrderTimeDispatch.Metadata, keys)) {
			this._ServiceOrderTimeDispatch = new LazyOfTTMetadata(() => {
				const keySet = new Set(keys);
				const jobs = new Map(this.ServiceOrder.ServiceOrderTimes
					?.filter(x => keySet.has(x.Id))
					.map(x => [x.Id, x]) ?? []);

				if (jobs.size > 0) {
					const serviceOrderTimeDispatches = this._ServiceOrderTimeDispatches
						?.filter(x => jobs.has(x.ServiceOrderTimeId))
						.map(x => [jobs.get(x.ServiceOrderTimeId).PosNo, x] as const)
						.map(([title, x]) => new Proxy(x, {
							get: function (target, prop, receiver) {
								if (prop === 'toString') {
									return () => title;
								}
								// @ts-ignore
								return Reflect.get(...arguments);
							}
						}));

					return serviceOrderTimeDispatches;
				}

				return [];
			}, keys);
		}

		return this._ServiceOrderTimeDispatch.value;
	}

	public set ServiceOrderTimeDispatches(value: Crm.Service.Rest.Model.CrmService_ServiceOrderTimeDispatch[]) {
		this.OriginalData.ServiceOrderTimeDispatches = value;
		this._ServiceOrderTimeDispatches = value;
		this._ServiceOrderTimeDispatch = null;
	}

	//@ts-ignore
	public get draggable(): boolean {
		return this.isLeaf && !this.readonly && !this.OriginalData.IsFixed;
	}

	//@ts-ignore
	public get resizable(): boolean {
		return this.draggable;
	}

	public get OrderId(): string {
		return this.OriginalData.OrderId;
	}

	public get ResourceKey(): string {
		return this.OriginalData.Username;
	}

	get Color(): string {
		return this.Status.Color;
	}

	get Remark(): string {
		return this.OriginalData.Remark;
	}

	set Remark(value: string) {
		this.OriginalData.Remark = ko.unwrap(value);
	}
	get InfoForTechnician(): string {
		return this.OriginalData.InfoForTechnician;
	}
	set InfoForTechnician(value: string) {
		this.OriginalData.InfoForTechnician = ko.unwrap(value);
	}

	public isTeamDispatch(): boolean {
		return this.assignments.length > 1;
	}

	async getRowData(propertyNames: string[]): Promise<string> {
		const self = this;
		let result: string[] = [];
		function reflectGetNested(obj: any, path: string[]): string | Promise<string> {
			return path.reduce((acc, key): string | Promise<string> => Reflect.get(acc, key), obj);
		}

		for(let propertyName of propertyNames) {
			const propertyReference = propertyName.replace("ServiceOrderDispatch.", "");
			const splitProperties = propertyReference.split('.');
			let propertyValue = reflectGetNested(self, splitProperties);
			if(propertyValue instanceof Promise) {
				propertyValue = await propertyValue;
			}
			if (propertyValue == null) {
				continue
			}

			result.push(propertyValue);
		}
		return result.join(' - ');
	}
	//@ts-ignore
	get eventColor() {
		return this.Color;
	}
}
namespace("Sms.Scheduler.Model").Dispatch = Dispatch;