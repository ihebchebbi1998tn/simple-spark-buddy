import { LazyOfTTMetadata } from "../Lazy";
import _ from "lodash";
import type {IResourceBase} from "./Interfaces/IResourceBase";
import {ResourceModel, Model} from "@bryntum/schedulerpro";
import {namespace} from "@Main/namespace";

export function isArticle(resource: any): resource is Article {
	return resource?.OriginalData instanceof Crm.Article.Rest.Model.CrmArticle_Article;
}
export function isTool(resource: Model): resource is Article {
	return isArticle(resource) && resource?.ResourceType === 'Tool';
}
export function isVehicle(resource: Model): resource is Article {
	return isArticle(resource) && resource?.ResourceType === 'Vehicle';
}
export class Article extends ResourceModel implements IResourceBase {
	static readonly lookups: LookupType = {};
	static get $name() {
		return 'Article';
	}
	static get $type() {
		return "Article";
	}

	OriginalData: Crm.Article.Rest.Model.CrmArticle_Article;
	AssignedUsers: Crm.Article.Rest.Model.CrmArticle_ArticleUserRelationship[];
	_Skills: LazyOfTTMetadata<any, string[]> = null;
	_Assets: LazyOfTTMetadata<any, string[]> = null;
	_Stations: LazyOfTTMetadata<any, string> = null;
	DisplayName: string = null;

	get Assets(): Main.Rest.Model.Lookups.Main_Asset[] {
		let keys = this.OriginalData.RequiredAssetKeys;
		if (this._Assets == null || !_.isEqual(this._Assets.Metadata, keys)) {
			this._Assets = new LazyOfTTMetadata(() => keys.map(key => window.Helper.Scheduler.CreateLookupProxy(Article.lookups.assets, key)), keys);
		}
		return this._Assets.value;
	}
	get Skills(): Main.Rest.Model.Lookups.Main_Skill[] {
		let keys = this.OriginalData.RequiredSkillKeys;
		if (this._Skills == null || !_.isEqual(this._Skills.Metadata, keys)) {
			this._Skills = new LazyOfTTMetadata(() => keys.map(key => window.Helper.Scheduler.CreateLookupProxy(Article.lookups.skills, key)), keys);
		}
		return this._Skills.value;
	}

	get Stations(): Crm.Rest.Model.Crm_Station[] {
		let key = this.OriginalData.StationKey;
		if (this._Stations == null || this._Stations.Metadata != key) {
			this._Stations = new LazyOfTTMetadata(() => {
				if (this.OriginalData.Station) {
					return [new Proxy(this.OriginalData.Station, {
						get: function (target, prop, receiver) {
							if (prop === 'toString') {
								return () => window.Helper.Station.getDisplayName(target);
							}
							// @ts-ignore
							return Reflect.get(...arguments);
						}
					})];
				} else {
					return undefined;
				}
			}, key);
		}
		return this._Stations.value;
	}
	get ResourceKey() {
		return this.OriginalData.Id;
	}
	get ResourceType() {
		return this.OriginalData.ArticleTypeKey;
	}
	constructor(data: Crm.Article.Rest.Model.CrmArticle_Article, imageUrl: string) {
		super();
		this.OriginalData = data;
		this.DisplayName = this.OriginalData.ItemNo + " - " + this.OriginalData.Description;

		if (imageUrl) {
			this.imageUrl = imageUrl;
		}
	}
	static get fields() {
		return [
			{ name: 'OriginalData', type: 'object'},
			{ name: 'id', dataSource: 'OriginalData.Id', type: 'string'},
			{ name: 'name', dataSource: 'OriginalData.Description', type: 'string'},
			{ name: 'ResourceKey', type: 'string', dataSource: 'OriginalData.Id' },
			{ name: 'ItemNo', type: 'string', dataSource: 'OriginalData.ItemNo' },
			{ name: 'Description', type: 'string', dataSource: 'OriginalData.Description' },
		]
	}
}

namespace("Sms.Scheduler.Model").Article = Article;