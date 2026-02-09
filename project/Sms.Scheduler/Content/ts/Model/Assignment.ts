import {AssignmentModel, type AssignmentModelConfig} from "@bryntum/schedulerpro";
import {namespace} from "@Main/namespace";

export class Assignment extends AssignmentModel {
	static get $name() {
		return 'Assignment';
	}	
	static get $type() {
		return 'Assignment';
	}
	OriginalData: Sms.Scheduler.Rest.Model.SmsScheduler_DispatchPersonAssignment | Sms.Scheduler.Rest.Model.SmsScheduler_DispatchArticleAssignment;

	constructor(data = null, config?: Partial<AssignmentModelConfig>) {
		super({ id: config?.id ?? data?.Id, eventId: config?.eventId ?? data?.DispatchKey, resourceId: config?.resourceId ?? data?.ResourceKey });
		this.OriginalData = data;
	}
}
namespace("Sms.Scheduler.Model").Assignment = Assignment;