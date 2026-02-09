import {AssignmentStore, AssignmentStoreConfig, Model} from "@bryntum/schedulerpro";
import {Assignment} from "./Assignment";
import {isTechnician, Technician} from "./Technicians";
import type {Dispatch} from "./Dispatch";
import type {Absence} from "./Absence";
import type {Article} from "./Article";
import { StoreIds } from "./DispatchStore";

export class CrmAssignmentStore extends AssignmentStore {
	static get $name() {
		return 'CrmAssignmentStore';
	}	
	static get $type() {
		return 'CrmAssignmentStore';
	}
	static get defaultConfig(): Partial<AssignmentStoreConfig> {
		return {
			modelClass: Assignment,
			//@ts-ignore
			storeId: StoreIds.AssignmentStore
		}
	}
	
	createRecord(data: { event: (Dispatch | Absence), resource: (Technician | Article) } , skipExpose?: boolean) {
		let modelClass = Assignment as typeof Model;
		let assignment = isTechnician(data.resource)
			? window.database.SmsScheduler_DispatchPersonAssignment.defaultType.create()
			: window.database.SmsScheduler_DispatchArticleAssignment.defaultType.create();
		assignment.Id = window.$data.createGuid().toString().toLowerCase();
		assignment.DispatchKey = data.event.OriginalData.Id as string;
		assignment.ResourceKey = data.resource.OriginalData.Id as string;
		//@ts-ignore
		return new modelClass(assignment, {resourceId: data.resource.id}, this);
	}
	
	constructor() {
		super();
	}
}

CrmAssignmentStore.initClass();