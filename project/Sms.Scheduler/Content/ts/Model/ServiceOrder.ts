import {EventModelConfig, GridRowModel, Model, Store} from "@bryntum/schedulerpro";
import _ from "lodash";
import moment from "moment";
import {LazyOfTTMetadata} from "../Lazy";
import { Job } from "./Job";
import type {Technician} from "./Technicians";
import type { ServiceOrderTimePosting } from "./ServiceOrderTimePosting";

export enum MaterialAvailabilityStatusEnum {
	FullyStocked = "FullyStocked",
	PartsMissing = "PartsMissing",
	OutOfStock = "OutOfStock"
}

type ServiceOrderMetaData = {
	type: "ServiceOrderHead",
	data: Crm.Service.Rest.Model.CrmService_ServiceOrderHead,
	timePostings?: ServiceOrderTimePosting[],
	stock?: Crm.Article.Rest.Model.CrmArticle_Stock[],
	serviceOrderTimes?: Crm.Service.Rest.Model.CrmService_ServiceOrderTime[],
	serviceOrderMaterials?: Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial[],
}

export function isServiceOrder(order: Model): order is ServiceOrder {
	return order != null && order.constructor === ServiceOrder;
}

export class ServiceOrder extends GridRowModel {
	static readonly lookups: LookupType = {};
	OriginalData: Crm.Service.Rest.Model.CrmService_ServiceOrderHead;
	Stock: Crm.Article.Rest.Model.CrmArticle_Stock[];
	TimePostings: ServiceOrderTimePosting[];
	ServiceOrderTimes: Crm.Service.Rest.Model.CrmService_ServiceOrderTime[];
	ServiceOrderMaterials: Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial[];

	constructor(config?: Partial<EventModelConfig>, store?: Store, meta?: ServiceOrderMetaData) {
		super(config, store);
		this.OriginalData = meta.data;

		this.TimePostings = meta.timePostings ?? [];
		this.Stock = meta.stock ?? [];
		this.ServiceOrderTimes = meta.serviceOrderTimes ?? [];
		this.ServiceOrderMaterials = meta.serviceOrderMaterials ?? [];
	}

	populateChildren() {
		this.clearChildren();
		this.appendChild(this.ServiceOrderTimes?.map(x => new Job(undefined, undefined, { data: x, serviceOrder: this })));
	}

	static get $name() {
		return 'ServiceOrder';
	}

	static get fields() {
		return [
			{name: 'OriginalData', type: 'object'},
			{name: 'id', dataSource: 'OriginalData.Id', type: 'string'},
			{name: 'name', dataSource: 'OriginalData.OrderNo', type: 'string'},
			{
				name: 'City',
				type: 'string',
				dataSource: 'City',
				convert: (v) => (window.Helper.String.isNullOrEmpty(v) || v === 'Unspecified') ? window.Helper.String.getTranslatedString("Unspecified") : v
			},
		]
	}

	_Status: LazyOfTTMetadata<any, string> = null;

	get Status(): Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus {
		let key = this.OriginalData.StatusKey;
		if (this._Status == null || this._Status.Metadata != key) {
			this._Status = new LazyOfTTMetadata(() => window.Helper.Scheduler.CreateLookupProxy(ServiceOrder.lookups.serviceOrderStatuses, key), key);
		}
		return this._Status.value;
	}

	_StatusGroup: LazyOfTTMetadata<any, string> = null;

	get StatusGroup() {
		let key = this.OriginalData.StatusKey;
		if (this._StatusGroup == null || this._StatusGroup.Metadata != key) {
			let group: string;
			let sortOrder: number;

			if (key === 'New' || key === 'ReadyForScheduling') {
				group = window.Helper.getTranslatedString('New');
				sortOrder = 1;
			} else if (key === 'PartiallyCompleted') {
				group = window.Helper.getTranslatedString('FollowUpServiceOrderNeeded');
				sortOrder = 2;
			} else if (key) {
				group = window.Helper.getTranslatedString('DispatchStatus.Scheduled');
				sortOrder = 3;
			} else {
				group = window.Helper.String.getTranslatedString("Unspecified");
				sortOrder = Number.MAX_SAFE_INTEGER;
			}

			this._StatusGroup = new LazyOfTTMetadata(() => ({
				toString: () => group,
				SortOrder: sortOrder
			}), key);
		}
		return this._StatusGroup.value;
	}

	_Type: LazyOfTTMetadata<any, string> = null;

	get Type(): Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderType {
		let key = this.OriginalData.TypeKey;
		if (this._Type == null || this._Type.Metadata != key) {
			this._Type = new LazyOfTTMetadata(() => window.Helper.Scheduler.CreateLookupProxy(ServiceOrder.lookups.serviceOrderTypes, key), key);
		}
		return this._Type.value;
	}

	_Country: LazyOfTTMetadata<any, string> = null;

	get Country(): Main.Rest.Model.Lookups.Main_Country {
		let key = this.OriginalData.CountryKey;
		if (this._Country == null || this._Country.Metadata != key) {
			this._Country = new LazyOfTTMetadata(() => window.Helper.Scheduler.CreateLookupProxy(ServiceOrder.lookups.countries, key), key);
		}
		return this._Country.value;
	}

	_Region: LazyOfTTMetadata<any, string> = null;

	get Region(): Main.Rest.Model.Lookups.Main_Region {
		let key = this.OriginalData.RegionKey;
		if (this._Region == null || this._Region.Metadata != key) {
			this._Region = new LazyOfTTMetadata(() => window.Helper.Scheduler.CreateLookupProxy(ServiceOrder.lookups.regions, key), key);
		}
		return this._Region.value;
	}

	_Priority: LazyOfTTMetadata<any, string> = null;

	get Priority(): Crm.Service.Rest.Model.Lookups.CrmService_ServicePriority {
		let key = this.OriginalData.PriorityKey;
		if (this._Priority == null || this._Priority.Metadata != key) {
			this._Priority = new LazyOfTTMetadata(() => window.Helper.Scheduler.CreateLookupProxy(ServiceOrder.lookups.priorities, key), key);
		}
		return this._Priority.value;
	}

	_Skills: LazyOfTTMetadata<any, string[]> = null;

	get Skills(): Main.Rest.Model.Lookups.Main_Skill[] {
		let keys = this.OriginalData.RequiredSkillKeys;
		if (this._Skills == null || !_.isEqual(this._Skills.Metadata, keys)) {
			this._Skills = new LazyOfTTMetadata(() => keys.map(key => window.Helper.Scheduler.CreateLookupProxy(ServiceOrder.lookups.skills, key)), keys);
		}
		return this._Skills.value;
	}

	_Assets: LazyOfTTMetadata<any, string[]> = null;

	get Assets(): Main.Rest.Model.Lookups.Main_Asset[] {
		let keys = this.OriginalData.RequiredAssetKeys;
		if (this._Assets == null || !_.isEqual(this._Assets.Metadata, keys)) {
			this._Assets = new LazyOfTTMetadata(() => keys.map(key => window.Helper.Scheduler.CreateLookupProxy(ServiceOrder.lookups.assets, key)), keys);
		}
		return this._Assets.value;
	}

	_ServiceObjectCategory: LazyOfTTMetadata<any, string> = null;

	get ServiceObjectCategory(): Crm.Service.Rest.Model.Lookups.CrmService_ServiceObjectCategory {
		let key = this.OriginalData.ServiceObject?.CategoryKey;
		if (this._ServiceObjectCategory == null || this._ServiceObjectCategory.Metadata != key) {
			this._ServiceObjectCategory = new LazyOfTTMetadata(() => window.Helper.Scheduler.CreateLookupProxy(ServiceOrder.lookups.serviceObjectCategories, key), key);
		}
		return this._ServiceObjectCategory.value;
	}

	_InstallationType: LazyOfTTMetadata<any, string> = null;

	get InstallationType(): string {
		let key = this.OriginalData.Installation?.InstallationTypeKey;
		if (this._InstallationType == null || this._InstallationType.Metadata != key) {
			this._InstallationType = new LazyOfTTMetadata(() => window.Helper.Scheduler.CreateLookupProxy(ServiceOrder.lookups.installationTypes, key), key);
		}
		return this._InstallationType.value;

	}

	_InstallationStatus: LazyOfTTMetadata<any, string> = null;

	get InstallationStatus(): string {
		let key = this.OriginalData.Installation?.StatusKey;
		if (this._InstallationStatus == null || this._InstallationStatus.Metadata != key) {
			this._InstallationStatus = new LazyOfTTMetadata(() => window.Helper.Scheduler.CreateLookupProxy(ServiceOrder.lookups.installationHeadStatuses, key), key);
		}
		return this._InstallationStatus.value;
	}

	_PlannedDuration: LazyOfTTMetadata<number, string[]> = null;

	get PlannedDuration(): number {
		const keys = this.TimePostings.map(t => t.id as string);

		if (this._PlannedDuration == null || !_.isEqual(this._PlannedDuration.Metadata, keys)) {
			this._PlannedDuration = new LazyOfTTMetadata(() => {
				if (this.TimePostings.length > 0) {
					const sum = this.TimePostings
						.filter(t => window.Helper.ServiceOrderTimePosting.isPrePlanned(t.OriginalData))
						.reduce((s, t) => s + moment.duration(t.OriginalData.Duration).asMilliseconds(), 0);

					return sum > 0 ? sum : null;
				}

				return null;
			}, keys);
		}

		return this._PlannedDuration.value;
	}

	_MaterialAvailability: LazyOfTTMetadata<MaterialAvailabilityStatusEnum, string[]> = null;

	get MaterialAvailability(): string {
		const keys = (this.Stock ?? []).map(t => t.Id);

		if (this._MaterialAvailability == null || !_.isEqual(this._MaterialAvailability.Metadata, keys)) {
			this._MaterialAvailability = new LazyOfTTMetadata(() => {
				if ((this.Stock ?? []).length == 0) {
					return MaterialAvailabilityStatusEnum.FullyStocked;
				}
				if (this.Stock.every(s => s.Quantity < 1)) {
					return MaterialAvailabilityStatusEnum.OutOfStock;
				}
				let materialAvailability: number[] = [];

				for (const material of this.ServiceOrderMaterials.filter(x => x.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Preplanned)) {
					const allStock = this.Stock.filter(s => s.ArticleKey == material.ArticleId).reduce((accumulator, currentValue) => accumulator + currentValue.Quantity, 0);
					materialAvailability.push(allStock - material.Quantity);
				}

				if (materialAvailability.every(ma => ma < 0)) {
					return MaterialAvailabilityStatusEnum.OutOfStock;
				} else if (materialAvailability.some(ma => ma < 0)) {
					return MaterialAvailabilityStatusEnum.PartsMissing;
				}
				return MaterialAvailabilityStatusEnum.FullyStocked;
			}, keys);
		}

		return this._MaterialAvailability.value;
	}

	get Station() {
		return this.OriginalData.StationKey != null ? this.OriginalData.Station?.Name : window.Helper.String.getTranslatedString("Unspecified");
	}

	get Company() {
		return this.OriginalData.Company?.Name
	}

	get CustomerNo() {
		return this.OriginalData.Company?.CompanyNo;
	}

	get ResponsibleUser() {
		return window.Helper.User.getDisplayName(this.OriginalData.ResponsibleUserUser);
	}

	get InstallationName() {
		return this.OriginalData.Installation?.Name;
	}

	get InstallationDescription() {
		return this.OriginalData.Installation?.Description;
	}

	get ZipCodeArea() {
		let count = window.Sms.Scheduler.Settings.ServiceOrderZipCodeAreaLength > 0 ? window.Sms.Scheduler.Settings.ServiceOrderZipCodeAreaLength : 1;
		return this.OriginalData.ZipCode?.substring(0, Math.min(this.OriginalData.ZipCode?.length ?? 0, count));
	}

	get Dispatches() {
		return (async () => {
			try {
				let dispatches = await window.database.CrmService_ServiceOrderDispatch
					.include("DispatchedUser")
					.filter("it=>it.OrderId == orderId", {orderId: this.id})
					.orderBy("it.Date")
					.toArray();

				const dateFormat = (from: Date, to: Date) => `${moment(from).format("lll")} - ${moment(to).format(window.Helper.Date.areOnSameDay(from, to) ? "LT" : "lll")}`;
				const mapped = dispatches.map(d => `${dateFormat(d.Date, d.EndDate)}, ${window.Helper.User.getDisplayName(d.DispatchedUser)}`);
				return mapped.join('\r\n');
			} catch (e) {
				return undefined; // fallback value;
			}
		})();
	}

	get RequiredSkillKeys() {
		return this.OriginalData.RequiredSkillKeys;
	}

	get RequiredAssetKeys() {
		return this.OriginalData.RequiredAssetKeys;
	}

	get PreferredTechnician(): string {
		return this.OriginalData.PreferredTechnician;
	}

	get PreferredUserGroup(): string {
		return this.OriginalData.PreferredTechnicianUsergroupObject?.Name;
	}

	get Planned(): Date {
		return this.OriginalData.Planned;
	}

	get Deadline(): Date {
		return this.OriginalData.Deadline;
	}

	get DisplayPreferredUser(): string {
		//Installations preferred here as a fallback if preferredTechnician is null like old scheduler?
		let preferredTechnician = this.OriginalData.PreferredTechnicianUser;
		return preferredTechnician ? window.Helper.User.getDisplayName(preferredTechnician) : this.OriginalData.PreferredTechnician;
	}

	get DisplayPlannedDate(): string {
		let parts = [];
		if (this.OriginalData.Planned)
			parts.push(window.Globalize.formatDate(this.OriginalData.Planned, {date: "medium"}));
		if (this.OriginalData.PlannedTime)
			parts.push(window.moment.duration(this.OriginalData.PlannedTime).format("hh:mm", {stopTrim: "h"}));
		if (this.OriginalData.PlannedDateFix)
			parts.push(window.Helper.String.getTranslatedString("Fix"));

		let result = parts.join(" ");

		return result.length ? result : null;
	}

	get ServiceObjectNo(): string {
		return this.OriginalData.ServiceObject?.ObjectNo;
	}

	get ServiceObjectName(): string {
		return this.OriginalData.ServiceObject?.Name;
	}

	get Latitude(): number {
		return this.OriginalData.Latitude;
	}

	get Longitude(): number {
		return this.OriginalData.Longitude;
	}

	get InstallationNo(): Promise<string> {
		return (async () => {
			let result: string = null;
			if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === 'OrderPerInstallation') {
				result = this.OriginalData.Installation?.InstallationNo ?? null;
			} else {
				if(this.ServiceOrderTimes.length > 0) {
					result = this.ServiceOrderTimes
						.filter(item => item.Installation != null)
						.map(item => item.Installation.InstallationNo)
						.join('\r\n');
				}
			}
			return result;
		})();
	}

	get Street(): string {
		return this.OriginalData.Street;
	}

	get ZipCode(): string {
		return this.OriginalData.ZipCode;
	}

	get City(): string {
		return this.OriginalData.City;
	}

	get ErrorMessage() {
		return this.OriginalData.ErrorMessage;
	}

	get OrderNo() {
		return this.OriginalData.OrderNo;
	}

	get name(): string {
		return this.OriginalData.OrderNo;
	}

	public requiredMaterialsAreInDefaultStoreForTechnician(resource: Technician): boolean {
		return this.ServiceOrderMaterials?.map(m => m.FromWarehouse).every(wh => wh === resource.DefaultStore);
	}

	getPopupInformation(): string {
		let item = this.OriginalData;
		let helperElement = document.createElement("div");
		if (!item) {
			return helperElement.innerHTML;
		}
		let order: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead & {
			Installations: KnockoutObservableArray<Crm.Service.Rest.Model.ObservableCrmService_Installation>
		} = item.asKoObservable() as unknown as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead & {
			Installations: KnockoutObservableArray<Crm.Service.Rest.Model.ObservableCrmService_Installation>
		};
		order.Installations = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_Installation>(window.Helper.ServiceOrder.getRelatedInstallations(order));
		const element = $(`#MapItemTemplate`);
		const context = ko.contextFor(element[0]).createChildContext(order);
		helperElement.innerHTML = element[0].innerHTML;
		ko.applyBindings(context, helperElement);
		return helperElement.innerHTML;
	}

	getRowData(propertyName: string): string {
		return this[propertyName.replace("ServiceOrder.", "")];
	}

	override toString() {
		return `${this.name}`;
	}
}