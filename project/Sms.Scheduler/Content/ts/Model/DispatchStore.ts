import {EventModel, EventStore, EventStoreConfig, Model} from "@bryntum/schedulerpro";
import {Dispatch, isDispatch} from "./Dispatch";
import {isServiceOrderTimePosting, ServiceOrderTimePosting} from "./ServiceOrderTimePosting";
import {Absence, isAbsence} from "./Absence";

export enum StoreIds {
	DispatchStore = "DispatchStore",
	AssignmentStore = "AssignmentStore"
}

export class DispatchStore extends EventStore {
	static get $name() {
		return StoreIds.DispatchStore;
	}

	static get $type() {
		return StoreIds.DispatchStore;
	}

	static get defaultConfig(): Partial<EventStoreConfig> {
		return {
			removeUnassignedEvent: false,
			//@ts-ignore
			storeId: StoreIds.DispatchStore
		}
	}

	createRecord(data, skipExpose = false, rawData = false) {
		let modelClass = EventModel as typeof Model;
		const record = data as EventModel;

		if (data instanceof Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch || data instanceof Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting || data instanceof Crm.Article.Rest.Model.CrmArticle_Article) {
			throw new Error("DispatchStore createRecord data type is not metadata");
		}

		if (isDispatch(record)) {
			modelClass = Dispatch as typeof Model;
		} else if (isServiceOrderTimePosting(record)) {
			modelClass = ServiceOrderTimePosting as typeof Model;
		} else if (isAbsence(record)) {
			modelClass = Absence as typeof Model;
		}

		return new modelClass(undefined, this, data);
	}
}

DispatchStore.initClass();