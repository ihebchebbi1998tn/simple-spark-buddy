import {LazyOfTTMetadata} from "../Lazy";
import _ from "lodash";
import type {IResourceBase} from "./Interfaces/IResourceBase";
import {ResourceModel} from "@bryntum/schedulerpro";
import {namespace} from "@Main/namespace";

export function isTechnician(resource: any): resource is Technician {
	return resource?.ResourceType === "Technician";
}

export class Technician extends ResourceModel implements IResourceBase {
	static readonly lookups: LookupType = {};
	OriginalData: Main.Rest.Model.Main_User;
	UserSkills: Main.Rest.Model.Main_UserSkill[] = [];
	UserAssets: Main.Rest.Model.Main_UserAsset[] = [];
	UserStations: Crm.Rest.Model.Crm_Station[] = [];
	SortOrder: number = null;
	DisplayName: string = null;
	private _HomeAddress: LazyOfTTMetadata<Crm.Rest.Model.Crm_Address, string>;

	constructor(data: Main.Rest.Model.Main_User) {
		super();
		this.OriginalData = data;
		let displayText = window.Helper.User.getDisplayName(data);
		if (data.Discharged && window.moment(data.DischargeDate).isSameOrAfter(new Date(), 'day')) {
			displayText = displayText.concat(` - (${window.Helper.String.getTranslatedString('Inactive')})`);
		}
		this.DisplayName = displayText;
		this.name = this.DisplayName;

		if (data.Avatar)
			this.imageUrl = 'data:image;base64,' + data.Avatar;
		else
			this.imageUrl = window.Helper.Url.resolveUrl("~/Plugins/Main/Content/img/avatar.jpg");
	}

	static get $name() {
		return 'Technician';
	}

	static get $type() {
		return "technician";
	}

	static get fields() {
		return [
			{name: 'OriginalData', type: 'object'},
			{name: 'id', dataSource: 'OriginalData.Id', type: 'string'},
			{name: 'ResourceKey', type: 'string'},
			{name: 'Remark', type: 'string', dataSource: 'OriginalData.Remark'},
			{name: 'PersonnelId', type: 'string', dataSource: 'OriginalData.PersonnelId'},
			{name: 'Email', type: 'string', dataSource: 'OriginalData.Email'},
			{name: 'Firstname', type: 'string', dataSource: 'OriginalData.FirstName'},
			{name: 'Lastname', type: 'string', dataSource: 'OriginalData.LastName'}
		]
	}

	_ExpiredSkills: LazyOfTTMetadata<any, string[]> = null;

	get ExpiredSkills(): Main.Rest.Model.Lookups.Main_Skill[] {
		let date = window.moment(new Date()).hours(0).minutes(0).toDate();
		let keys = this.UserSkills?.filter(s => (s.ValidFrom != null && window.moment(date).isBefore(s.ValidFrom, 'day')) && (s.ValidTo != null && window.moment(date).isAfter(s.ValidTo, 'day'))).map(s => s.SkillKey);
		if (this._ExpiredSkills == null || !_.isEqual(this._ExpiredSkills.Metadata, keys)) {
			this._ExpiredSkills = new LazyOfTTMetadata(() => keys.map(key => window.Helper.Scheduler.CreateLookupProxy(Technician.lookups.skills, key)), keys);
		}
		return this._ExpiredSkills.value;
	}

	_ExpiredAssets: LazyOfTTMetadata<any, string[]> = null;

	get ExpiredAssets(): Main.Rest.Model.Lookups.Main_Asset[] {
		let date = window.moment(new Date()).hours(0).minutes(0).toDate();
		let keys = this.UserAssets?.filter(a => (a.ValidFrom != null || window.moment(date).isBefore(a.ValidFrom, 'day')) && (a.ValidTo != null || window.moment(date).isAfter(a.ValidTo, 'day'))).map(a => a.AssetKey);
		if (this._ExpiredAssets == null || !_.isEqual(this._ExpiredAssets.Metadata, keys)) {
			this._ExpiredAssets = new LazyOfTTMetadata(() => keys.map(key => window.Helper.Scheduler.CreateLookupProxy(Technician.lookups.assets, key)), keys);
		}
		return this._ExpiredAssets.value;
	}

	_ValidSkills: LazyOfTTMetadata<any, string[]> = null;

	get ValidSkills(): Main.Rest.Model.Lookups.Main_Skill[] {
		let date = window.moment(new Date()).hours(0).minutes(0).toDate();
		let keys = this.UserSkills?.filter(s => (s.ValidFrom == null || window.moment(date).isSameOrAfter(s.ValidFrom, 'day')) && (s.ValidTo == null || window.moment(date).isSameOrBefore(s.ValidTo, 'day'))).map(s => s.SkillKey);
		if (this._ValidSkills == null || !_.isEqual(this._ValidSkills.Metadata, keys)) {
			this._ValidSkills = new LazyOfTTMetadata(() => keys.map(key => window.Helper.Scheduler.CreateLookupProxy(Technician.lookups.skills, key)), keys);
		}
		return this._ValidSkills.value;
	}

	_ValidAssets: LazyOfTTMetadata<any, string[]> = null;

	get ValidAssets(): Main.Rest.Model.Lookups.Main_Asset[] {
		let date = window.moment(new Date()).hours(0).minutes(0).toDate();
		let keys = this.UserAssets?.filter(a => (a.ValidFrom == null || window.moment(date).isSameOrAfter(a.ValidFrom, 'day')) && (a.ValidTo == null || window.moment(date).isSameOrBefore(a.ValidTo, 'day'))).map(a => a.AssetKey);
		if (this._ValidAssets == null || !_.isEqual(this._ValidAssets.Metadata, keys)) {
			this._ValidAssets = new LazyOfTTMetadata(() => keys.map(key => window.Helper.Scheduler.CreateLookupProxy(Technician.lookups.assets, key)), keys);
		}
		return this._ValidAssets.value;
	}

	_Stations: LazyOfTTMetadata<any, string[]> = null;

	get Stations(): Crm.Rest.Model.Crm_Station[] {
		let keys = this.UserStations.map(s => s.Id);
		if (this._Stations == null || !_.isEqual(this._Stations.Metadata, keys)) {
			this._Stations = new LazyOfTTMetadata(() => {
				if (this.UserStations && this.UserStations.length > 0) {
					function proxyGet(target, prop, reciever) {
						if (prop === 'toString') {
							return () => Helper.Station.getDisplayName(target);
						}
						// @ts-ignore
						return Reflect.get(...arguments);
					}

					return this.UserStations.map(s => new Proxy(s, {
						get: proxyGet
					}));
				} else {
					return undefined;
				}
			}, keys);
		}
		return this._Stations.value;
	}

	get UserGroups(): string[] {
		return this.OriginalData.Usergroups;
	}

	get License(): string {
		return this.OriginalData.LicensedAt != null ? window.Helper.getTranslatedString("Licensed") : window.Helper.getTranslatedString("NotLicensed");
	}

	get DefaultStore(): string {
		return this.OriginalData.ExtensionValues?.DefaultStoreNo;
	}

	get DefaultLocation(): string {
		return this.OriginalData.ExtensionValues?.DefaultLocationNo;
	}

	get hasHomeAddressId() {
		return this.OriginalData.ExtensionValues.HomeAddressId != null;
	}

	get isHomeAddressLoaded() {
		return this._HomeAddress != null && this._HomeAddress.Metadata === this.OriginalData.ExtensionValues.HomeAddressId;
	}

	get homeAddress(): Crm.Rest.Model.Crm_Address {
		if (this.hasHomeAddressId) {
			if (this.isHomeAddressLoaded) {
				return this._HomeAddress.value;
			} else {
				throw new Error("technician has homeaddress but it is not loaded prior to read.");
			}
		} else {
			return null;
		}
	}

	get homeAddressLocationString() {
		const homeAddress = this.homeAddress;
		return homeAddress && homeAddress.Latitude !== null && homeAddress.Longitude !== null ? `[${homeAddress.Latitude},${homeAddress.Longitude}]` : null;
	}

	get Skills() {
		const skills = this.ValidSkills;
		if (skills && skills.length > 0) {
			return skills;
		} else {
			return undefined;
		}
	}

	get Assets() {
		const assets = this.ValidAssets;
		if (assets && assets.length > 0) {
			return assets;
		} else {
			return undefined;
		}
	}

	get Latitude(): number {
		return this.OriginalData.Latitude;
	}

	get Longitude(): number {
		return this.OriginalData.Longitude;
	}

	get LastStatusUpdate(): Date {
		return this.OriginalData.LastStatusUpdate;
	}

	get ResourceKey() {
		return this.OriginalData.Id;
	}

	get ResourceType() {
		return "Technician";
	}

	get Capacity(): number {
		return this.OriginalData.Discharged ? 0 : this.OriginalData.ExtensionValues.WorkingHoursPerDay;
	}

	async loadHomeAddress() {
		if (!this.isHomeAddressLoaded) {
			const homeAddressId = this.OriginalData?.ExtensionValues?.HomeAddressId;
			let address: Crm.Rest.Model.Crm_Address = null;

			if (homeAddressId) {
				const addresses = await window.database.Crm_Address.filter("it.Id == homeAddressId", {homeAddressId}).toArray();

				if (addresses.length > 0) {
					address = addresses[0];
				}
			}
			this._HomeAddress = new LazyOfTTMetadata<Crm.Rest.Model.Crm_Address, string>(() => address, homeAddressId);
		}
	}

	async getHomeAddress() {
		await this.loadHomeAddress();
		return this.homeAddress;
	}

	isActiveAtDate(date: Date): boolean {
		return this.OriginalData.Discharged ? window.moment(this.OriginalData.DischargeDate).isAfter(date, 'day') : true;
	}
}

namespace("Sms.Scheduler.Model").Technician = Technician;
