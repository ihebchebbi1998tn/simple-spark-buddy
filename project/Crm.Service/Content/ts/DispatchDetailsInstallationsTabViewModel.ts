import {namespace} from "@Main/namespace";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";

export class DispatchDetailsInstallationsTabViewModel extends window.Crm.ViewModels.ContactListViewModel<Crm.Service.Rest.Model.CrmService_Installation, Crm.Service.Rest.Model.ObservableCrmService_Installation> {
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	lookups: LookupType;
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	firstInstallationActive = ko.pureComputed<boolean>(() => {
		if (this.items().length === 1) {
			return true;
		}
		if (this.dispatch() && this.dispatch().CurrentServiceOrderTime() &&
			this.dispatch().CurrentServiceOrderTime().InstallationId()) {
			return this.items()[0].Id() === this.dispatch().CurrentServiceOrderTime().InstallationId();
		}
		return false;
	});

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super("CrmService_Installation",
			["InstallationNo"],
			["ASC"],
			["Address"]);
		this.dispatch = parentViewModel.dispatch;
		this.lookups = parentViewModel.lookups;
		this.lookups.installationHeadStatuses = this.lookups.installationHeadStatuses || {$tableName: "CrmService_InstallationHeadStatus"};
		this.lookups.installationTypes = this.lookups.installationTypes || {$tableName: "CrmService_InstallationType"};
		this.lookups.manufacturers = this.lookups.manufacturers || {$tableName: "CrmService_Manufacturer"};
		this.serviceOrder = parentViewModel.serviceOrder;
		if (this.serviceOrder().InstallationId()) {
			this.getFilter("Id").extend({filterOperator: "==="})(this.serviceOrder().InstallationId());
		}
		this.infiniteScroll(true);
	}

	async init(): Promise<void> {
		await super.init();
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
	};

	async initItems(items: any[]): Promise<any[]> {
		items = await super.initItems(items);
		if (Helper.AttributeForms) {
			for (const item of items) {
				Helper.AttributeForms.mixinEditing(item, "Installation");
				await item.updateAttributeForms.call(item);
				item.loading(false);
			}
		}
		return items;
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		query = window.Crm.Service.ViewModels.InstallationListIndexViewModel.prototype.applyFilters.call(this, query);
		if (this.serviceOrder().InstallationId()) {
			return query;
		}
		return query.filter("filterByServiceOrderTimes", {orderId: this.serviceOrder().Id()});
	};

	applyOrderBy(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		let id = null;
		if (this.dispatch() && this.dispatch().CurrentServiceOrderTime() && this.dispatch().CurrentServiceOrderTime().InstallationId()) {
			id = this.dispatch().CurrentServiceOrderTime().InstallationId();
		}
		// @ts-ignore
		query = query.orderByDescending("orderByCurrentServiceOrderTime", {currentServiceOrderTimeId: id});
		return super.applyOrderBy(query);
	};

	getAvatarColor(installation: Crm.Service.Rest.Model.ObservableCrmService_Installation): string {
		return "#9E9E9E";
	};

	getAvatarText(installation: Crm.Service.Rest.Model.ObservableCrmService_Installation): string {
		const installationTypeKey = ko.unwrap(installation.InstallationTypeKey);
		if (installationTypeKey) {
			const installationType = this.lookups.installationTypes[installationTypeKey];
			if (installationType && installationType.Value) {
				return installationType.Value[0];
			}
		}
		return "";
	};
}

namespace("Crm.Service.ViewModels").DispatchDetailsInstallationsTabViewModel = DispatchDetailsInstallationsTabViewModel;