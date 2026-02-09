export class HelperServiceOrderTimePosting {
	static getActualDuration(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned) {
			let usedPositions = serviceOrderTimePosting.ChildServiceOrderTimePostings().filter(x => x.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used);
			let usedPositionsDuration = usedPositions.reduce((result, position) => {
				return result.add(window.moment.duration(position.Duration()));
			}, window.moment.duration());
			return usedPositionsDuration.toString();
		}

		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used) {
			return serviceOrderTimePosting.Duration();
		}


		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Invoice) {
			return serviceOrderTimePosting.ParentServiceOrderTimePosting()?.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used ? serviceOrderTimePosting.ParentServiceOrderTimePosting().Duration() : "PT0M";
		}

		throw new Error("unknown type " + serviceOrderTimePosting.ServiceOrderTimePostingType());
	}

	static getEstimatedDuration(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned) {
			return serviceOrderTimePosting.Duration();
		}

		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used) {
			if (serviceOrderTimePosting.ParentServiceOrderTimePosting()?.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned) {
				return serviceOrderTimePosting.ParentServiceOrderTimePosting().Duration();
			}
			if (serviceOrderTimePosting.ServiceOrderTime()) {
				return serviceOrderTimePosting.ServiceOrderTime().EstimatedDuration();
			}
			return "PT0M";
		}

		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Invoice) {
			if (serviceOrderTimePosting.ParentServiceOrderTimePosting()?.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned) {
				return serviceOrderTimePosting.ParentServiceOrderTimePosting().Duration();
			} else if (serviceOrderTimePosting.ParentServiceOrderTimePosting()?.ParentServiceOrderTimePosting()?.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned) {
				return serviceOrderTimePosting.ParentServiceOrderTimePosting().ParentServiceOrderTimePosting().Duration();
			} else if (serviceOrderTimePosting.ServiceOrderTime()) {
				return serviceOrderTimePosting.ServiceOrderTime().EstimatedDuration();
			} else {
				return "PT0M";
			}
		}

		throw new Error("unknown type " + serviceOrderTimePosting.ServiceOrderTimePostingType());
	}

	static getInvoiceDuration(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned) {
			let invoicePositions = serviceOrderTimePosting.ChildServiceOrderTimePostings().filter(x => x.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Invoice);
			for (const childServiceOrderMaterial of serviceOrderTimePosting.ChildServiceOrderTimePostings()) {
				invoicePositions.push(...childServiceOrderMaterial.ChildServiceOrderTimePostings().filter(x => x.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Invoice));
			}
			let invoicePositionsDuration = invoicePositions.reduce((result, position) => {
				return result.add(window.moment.duration(position.Duration()));
			}, window.moment.duration());
			return invoicePositionsDuration.toString();
		}

		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Used) {
			let invoicePositions = serviceOrderTimePosting.ChildServiceOrderTimePostings().filter(x => x.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Invoice);
			let invoicePositionsDuration = invoicePositions.reduce((result, position) => {
				return result.add(window.moment.duration(position.Duration()));
			}, window.moment.duration());
			return invoicePositionsDuration.toString();
		}

		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Invoice) {
			return serviceOrderTimePosting.Duration();
		}

		throw new Error("unknown type " + serviceOrderTimePosting.ServiceOrderTimePostingType());
	}

	static isPrePlanned(timePosting: Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): boolean {
		return ko.unwrap(timePosting.ServiceOrderTimePostingType) === window.Crm.Service.ServiceOrderTimePostingType.Preplanned;
	}

	static wasPrePlanned(timePosting: Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): boolean {
		return ko.unwrap(timePosting.ServiceOrderTimePostingType) === window.Crm.Service.ServiceOrderTimePostingType.Used && ko.unwrap(timePosting.ParentServiceOrderTimePostingId) !== null;
	}

	static createInvoicePosition(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting):  Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting {
		let invoiceServiceOrderTimePosting = window.Helper.Database.createClone(serviceOrderTimePosting.innerInstance);
		invoiceServiceOrderTimePosting.Id = window.$data.createGuid().toString().toLowerCase();
		invoiceServiceOrderTimePosting.ChildServiceOrderTimePostings = [];
		invoiceServiceOrderTimePosting.ParentServiceOrderTimePostingId = serviceOrderTimePosting.Id();
		invoiceServiceOrderTimePosting.ServiceOrderTimePostingType = window.Crm.Service.ServiceOrderTimePostingType.Invoice;
		return invoiceServiceOrderTimePosting;
	}

	static getInvoicePositions(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting[] {
		const visited = new Set<string>();
		return HelperServiceOrderTimePosting.collectPositions(serviceOrderTimePosting, visited);
	}

	static collectPositions(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting, visited: Set<string>): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting[] {
		const id = serviceOrderTimePosting.Id();
		if (visited.has(id)) return [];

		visited.add(id);
		if (serviceOrderTimePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderMaterialType.Invoice) {
			return [serviceOrderTimePosting];
		}
		let invoicePositions: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting[] = [];
		const parent = serviceOrderTimePosting.ParentServiceOrderTimePosting?.();
		if (parent) {
			invoicePositions.push(...HelperServiceOrderTimePosting.collectPositions(parent, visited));
		}
		for (const child of serviceOrderTimePosting.ChildServiceOrderTimePostings?.() ?? []) {
			invoicePositions.push(...HelperServiceOrderTimePosting.collectPositions(child, visited));
		}
		return invoicePositions;
	}
}

// @ts-ignore
window.Crm.Service.ServiceOrderTimePostingType ||= {};
window.Crm.Service.ServiceOrderTimePostingType.Invoice = "Invoice";
window.Crm.Service.ServiceOrderTimePostingType.Preplanned = "Preplanned";
window.Crm.Service.ServiceOrderTimePostingType.Used = "Used";
