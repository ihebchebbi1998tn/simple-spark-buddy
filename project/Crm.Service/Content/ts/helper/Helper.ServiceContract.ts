import type {Moment} from "moment";
import type {unitOfTime} from "moment/moment";

export class HelperServiceContract {
	static getTypeAbbreviation(serviceContract: Crm.Service.Rest.Model.CrmService_ServiceContract | Crm.Service.Rest.Model.ObservableCrmService_ServiceContract, serviceContractTypes: { [key:string]: Crm.Service.Rest.Model.Lookups.CrmService_ServiceContractType}): string {
		if (!serviceContract){
			return "";
		}
		serviceContract = ko.unwrap(serviceContract);
		const serviceContractTypeKey = ko.unwrap(serviceContract.ContractTypeKey);
		if (serviceContractTypeKey) {
			const serviceContractType = (serviceContractTypes || {})[serviceContractTypeKey];
			if (serviceContractType && serviceContractType.Value) {
				return serviceContractType.Value[0];
			}
		}
		return "";
	}

	static getDisplayName(serviceContract: Crm.Service.Rest.Model.CrmService_ServiceContract | Crm.Service.Rest.Model.ObservableCrmService_ServiceContract): string {
		if (!serviceContract) {
			return "";
		}
		return ko.unwrap(serviceContract.ContractNo);
	}
	
	static getNextXGenerationDates(maintenancePlan: Crm.Service.Rest.Model.ObservableCrmService_MaintenancePlan, numberOfDates): string[]{
		if(!maintenancePlan.FirstDate() || ! maintenancePlan.RhythmValue() || !maintenancePlan.RhythmUnitKey()){
			return null
		}
		var nextDates: string[] = [];
		var nextDate: Moment;

		if(!maintenancePlan.NextDate() || window.moment(maintenancePlan.NextDate()).isBefore(window.moment(maintenancePlan.FirstDate()), 'day')){
			nextDate = window.moment(maintenancePlan.FirstDate());
			while(nextDate.isBefore(window.moment(), 'day')){
				nextDate = this.addTimeSpan(nextDate, maintenancePlan.RhythmValue(), maintenancePlan.RhythmUnitKey());
			}
		} else {
			nextDate = window.moment(maintenancePlan.NextDate());
		}
		nextDates.push(window.Globalize.formatDate(nextDate.toDate(), { date: "full" }));
		
		for(let i = 0; i < numberOfDates - 1; i++){
			nextDate = this.addTimeSpan(nextDate, maintenancePlan.RhythmValue(), maintenancePlan.RhythmUnitKey());
			nextDates.push(window.Globalize.formatDate(nextDate.toDate(), { date: "full" }));
		}
		return nextDates;
	}
	
	static addTimeSpan(date: Moment, val: number, key: string){
		let unit = key.toLowerCase() + "s" as unitOfTime.DurationConstructor;
		return date.add(val, unit);
	}
}