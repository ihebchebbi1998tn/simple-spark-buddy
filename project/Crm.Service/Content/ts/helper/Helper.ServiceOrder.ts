import {HelperCreateServiceOrderData} from "./Helper.ServiceOrder.createServiceOrderData";

export class HelperServiceOrder {

	static CreateServiceOrderData = HelperCreateServiceOrderData;
	static belongsToClosed(serviceOrderStatus: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus): boolean {
		return (serviceOrderStatus.Groups || "").split(",").indexOf("Closed") !== -1;
	}

	static async canEditActualQuantities(serviceOrderId: string, serviceOrderStatusOrKey?: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus | string): Promise<boolean> {
		return await Helper.ServiceOrder.isInStatusGroup(serviceOrderId, ["InProgress", "PostProcessing"], serviceOrderStatusOrKey);
	}

	static async canEditEstimatedQuantities(serviceOrderId: string, serviceOrderStatusOrKey?: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus | string): Promise<boolean> {
		return await Helper.ServiceOrder.isInStatusGroup(serviceOrderId, ["Preparation", "Scheduling"], serviceOrderStatusOrKey);
	}

	static canEditEstimatedQuantitiesSync(serviceOrderStatus: string, statusGroups: {[key:string]: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus}): boolean {
		return Helper.ServiceOrder.isInStatusGroupSync(serviceOrderStatus, statusGroups, ["Preparation", "Scheduling"]);
	}

	static async canEditInvoiceQuantities(serviceOrderId: string, serviceOrderStatusOrKey?: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus | string): Promise<boolean> {
		return Helper.ServiceOrder.isInStatusGroup(serviceOrderId, "PostProcessing", serviceOrderStatusOrKey);
	}

	static formatPosNo(posNo: string | number): string {
		if (typeof posNo !== "number") {
			return posNo;
		}
		return posNo < 10000 ? ("0000" + posNo.toString()).slice(-4) : posNo.toString();
	}

	static getDisplayName(serviceOrder: Crm.Service.Rest.Model.CrmService_ServiceOrderHead | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead): string {
		return [ko.unwrap(serviceOrder.OrderNo), ko.unwrap(serviceOrder.ErrorMessage)].filter(Boolean).join(" - ");
	}

	static getServiceOrderPositionItemGroup(serviceOrderPosition: { ServiceOrderTime: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime>}): ItemGroup {
		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "OrderPerInstallation") {
			return null;
		}
		if (serviceOrderPosition.ServiceOrderTime() === null) {
			return {title: window.Helper.String.getTranslatedString("ServiceOrder")};
		}
		let title = serviceOrderPosition.ServiceOrderTime().PosNo();
		if (serviceOrderPosition.ServiceOrderTime().Description()) {
			title += ": " + serviceOrderPosition.ServiceOrderTime().Description();
		}
		const itemGroup:{title:string, subtitle?:string} = {title: title};
		if (serviceOrderPosition.ServiceOrderTime().Installation()) {
			itemGroup.subtitle =
				window.Helper.Installation.getDisplayName(serviceOrderPosition.ServiceOrderTime().Installation());
		}
		return itemGroup;
	}

	static getServiceOrderTemplateAutocompleteFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderHead> {
		query = query.filter(function (it) {
			return it.IsTemplate === true;
		});
		if (term) {
			query = window.Helper.String.contains(query, term, ["OrderNo", "ErrorMessage"]);
		}
		return query;
	}

	static async isInStatusGroup(serviceOrderId: string, statusGroupOrGroups: string | string[], serviceOrderStatusOrKey?: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus | string): Promise<boolean> {
		statusGroupOrGroups = Array.isArray(statusGroupOrGroups) ? statusGroupOrGroups : [statusGroupOrGroups];
		if (!serviceOrderStatusOrKey) {
			serviceOrderStatusOrKey = await window.database.CrmService_ServiceOrderHead
				.map("it.StatusKey")
				.find(serviceOrderId);
		}
		let serviceOrderStatus = typeof serviceOrderStatusOrKey === "string" ? await window.Helper.Lookup.getLookupByKeyQuery("CrmService_ServiceOrderStatus", serviceOrderStatusOrKey).first() : serviceOrderStatusOrKey;
		let result = false;
		const groups = (serviceOrderStatus.Groups || "").split(",");
		statusGroupOrGroups.forEach(function (statusGroup) {
			result = result || groups.indexOf(statusGroup) !== -1;
		});
		return result;
	}

	static isInStatusGroupSync(serviceOrderStatus: string | KnockoutObservable<string>, statusGroupsHaystack:{[key:string]: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus}, statusGroupsNeedles: string | string[]): boolean {
		serviceOrderStatus = ko.unwrap(serviceOrderStatus);
		const status = statusGroupsHaystack[serviceOrderStatus];
		const haystack = (status.Groups || "").split(",");
		const needles = Array.isArray(statusGroupsNeedles) ? statusGroupsNeedles : [statusGroupsNeedles];
		for (const needle of needles) {
			if (haystack.indexOf(needle) !== -1) {
				return true;
			}
		}
		return false;
	}

	static mapForSelect2Display(serviceOrder: Crm.Service.Rest.Model.CrmService_ServiceOrderHead): Select2AutoCompleterResult {
		return {
			id: serviceOrder.Id,
			item: serviceOrder,
			text: Helper.ServiceOrder.getDisplayName(serviceOrder)
		};
	}

	static queryServiceOrderType(query: $data.Queryable<Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderType>, term: string): $data.Queryable<Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderType> {
		if (!window.AuthorizationManager.isAuthorizedForAction("ServiceOrderType", "SelectNonMobileLookupValues")) {
			query = query.filter("it.ShowInMobileClient === true");
		}
		return window.Helper.Lookup.queryLookup(query, term);
	}

	static setStatus(serviceOrders: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead[], status: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus): void {
		serviceOrders = Array.isArray(serviceOrders) ? serviceOrders : [serviceOrders];
		serviceOrders.map(function (serviceOrder) {
			return window.Helper.Database.getDatabaseEntity<Crm.Service.Rest.Model.CrmService_ServiceOrderHead>(serviceOrder);
		}).forEach(function (serviceOrder) {
			serviceOrder.StatusKey = status.Key;
			if (serviceOrder.StatusKey !== "Closed" && !serviceOrder.IsTemplate) {
				serviceOrder.NoInvoiceReasonKey = null;
			}
			const belongsToClosed = window.Helper.ServiceOrder.belongsToClosed(status);
			if (belongsToClosed) {
				serviceOrder.Closed = new Date();
			} else {
				serviceOrder.Closed = null;
			}
		});
	}

	static async getMaxPosNo(serviceOrderId: string): Promise<number> {
		let posNo = 0;
		let serviceOrderTimes = await window.database.CrmService_ServiceOrderTime.filter(function (it) {
				return it.OrderId === this.orderId;
			},
			{orderId: serviceOrderId})
			.orderByDescending("it.PosNo")
			.take(1)
			.toArray();
		if (serviceOrderTimes.length > 0) {
			posNo = Math.max(posNo, parseInt(serviceOrderTimes[0].PosNo, 10));
		}
		let serviceOrderMaterials = await window.database.CrmService_ServiceOrderMaterial.filter(function (it) {
				return it.OrderId === this.orderId;
			},
			{orderId: serviceOrderId})
			.orderByDescending("it.PosNo")
			.take(1)
			.toArray();
		if (serviceOrderMaterials.length > 0) {
			posNo = Math.max(posNo, parseInt(serviceOrderMaterials[0].PosNo, 10));
		}
		return posNo;
	}

	static async getNextPosNo(serviceOrderId: string): Promise<string> {
		let maxPosNo = await window.Helper.ServiceOrder.getMaxPosNo(serviceOrderId)
		return window.Helper.ServiceOrder.formatPosNo(maxPosNo + 1);
	}

	static async getNextMaterialPosNo(serviceOrderId: string): Promise<string> {
		let posNo = 0;
		let results = await window.database.CrmService_ServiceOrderMaterial.filter(function (it) {
				return it.OrderId === this.orderId;
			},
			{orderId: serviceOrderId})
			.orderByDescending("it.PosNo")
			.take(1)
			.toArray();
		if (results.length > 0) {
			posNo = Math.max(posNo, parseInt(results[0].PosNo, 10));
		}
		return window.Helper.ServiceOrder.formatPosNo(posNo + 1);
	}

	static async getNextJobPosNo(serviceOrderId: string): Promise<string> {
		let posNo = 0;
		let results = await window.database.CrmService_ServiceOrderTime.filter(function (it) {
				return it.OrderId === this.orderId;
			},
			{orderId: serviceOrderId})
			.orderByDescending("it.PosNo")
			.take(1)
			.toArray();
		if (results.length > 0) {
			posNo = Math.max(posNo, parseInt(results[0].PosNo, 10));
		}
		return window.Helper.ServiceOrder.formatPosNo(posNo + 1);
	}

	static getTypeAbbreviation(serviceOrder: Crm.Service.Rest.Model.CrmService_ServiceOrderHead | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead, serviceOrderTypes: {[key:string]: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderType}): string {
		if (!serviceOrder){
			return "";
		}
		serviceOrder = ko.unwrap(serviceOrder);
		const serviceOrderTypeKey = ko.unwrap(serviceOrder.TypeKey);
		if (serviceOrderTypeKey) {
			const serviceOrderType = (serviceOrderTypes || {})[serviceOrderTypeKey];
			if (serviceOrderType && serviceOrderType.Value) {
				return serviceOrderType.Value[0];
			}
		}
		return "";
	}

	static async transferTemplateData(serviceOrderTemplate: Crm.Service.Rest.Model.CrmService_ServiceOrderHead | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead, serviceOrder: Crm.Service.Rest.Model.CrmService_ServiceOrderHead | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead, revertTemplate:boolean): Promise<void> {
		if (!revertTemplate) {
			serviceOrderTemplate = window.Helper.Database.getDatabaseEntity(serviceOrderTemplate);
		}
		serviceOrder = window.Helper.Database.getDatabaseEntity(serviceOrder);
		// @ts-ignore
		if (serviceOrderTemplate.ExtensionValues) {
			// @ts-ignore
			const extensionValues = Object.getOwnPropertyNames(JSON.parse(JSON.stringify(serviceOrderTemplate.ExtensionValues)));
			extensionValues.forEach(function(value){
				// @ts-ignore
				serviceOrder.ExtensionValues[value] = serviceOrderTemplate.ExtensionValues[value];
			});
		}
		serviceOrder.InvoicingTypeKey = serviceOrderTemplate.InvoicingTypeKey;
		serviceOrder.PreferredTechnician = serviceOrderTemplate.PreferredTechnician;
		serviceOrder.PreferredTechnicianUsergroupKey = serviceOrderTemplate.PreferredTechnicianUsergroupKey;
		serviceOrder.PriorityKey = serviceOrderTemplate.PriorityKey;
		serviceOrder.RequiredSkillKeys = serviceOrderTemplate.RequiredSkillKeys;
		serviceOrder.RequiredAssetKeys = serviceOrderTemplate.RequiredAssetKeys;
		serviceOrder.UserGroupKey = serviceOrderTemplate.UserGroupKey;
			if (!!serviceOrderTemplate.ResponsibleUser) {
				serviceOrder.ResponsibleUser = serviceOrderTemplate.ResponsibleUser;
			}
		serviceOrder.StatusKey = serviceOrderTemplate.StatusKey;
		serviceOrder.TypeKey = serviceOrderTemplate.TypeKey;
		if (revertTemplate) {
			serviceOrder.ServiceOrderTemplate = null;
			serviceOrder.ServiceOrderTemplateId = null;
		}
	}

	static async updatePosNo(serviceOrderPosition: Crm.Service.Rest.Model.CrmService_ServiceOrderTime | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime | Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): Promise<void> {
		serviceOrderPosition = window.Helper.Database.getDatabaseEntity<Crm.Service.Rest.Model.CrmService_ServiceOrderTime | Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial>(serviceOrderPosition);
		if (serviceOrderPosition.PosNo) {
			return;
		}
		let posNo = await window.Helper.ServiceOrder.getNextPosNo(serviceOrderPosition.OrderId);
		serviceOrderPosition.PosNo = posNo;
	}

	static async updateJobPosNo(serviceOrderPosition: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime | Crm.Service.Rest.Model.CrmService_ServiceOrderTime): Promise<void> {
		serviceOrderPosition = window.Helper.Database.getDatabaseEntity<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>(serviceOrderPosition);
		if (serviceOrderPosition.PosNo) {
			return;
		}
		let posNo = await window.Helper.ServiceOrder.getNextJobPosNo(serviceOrderPosition.OrderId);
		serviceOrderPosition.PosNo = posNo;
	}

	static async updateMaterialPosNo(serviceOrderPosition: Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): Promise<void> {
		serviceOrderPosition = window.Helper.Database.getDatabaseEntity<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial>(serviceOrderPosition);
		if (serviceOrderPosition.PosNo) {
			return;
		}
		let posNo = await window.Helper.ServiceOrder.getNextMaterialPosNo(serviceOrderPosition.OrderId);
		serviceOrderPosition.PosNo = posNo;
	}

	static getRelatedInstallations(serviceOrderHead: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead): Crm.Service.Rest.Model.ObservableCrmService_Installation[] {
		const installations: Crm.Service.Rest.Model.ObservableCrmService_Installation[] = [];
		if (!!serviceOrderHead.Installation() && serviceOrderHead.Installation().IsActive()) {
			installations.push(ko.unwrap(serviceOrderHead.Installation));
		}
		serviceOrderHead.ServiceOrderTimes().forEach(function (x) {
			if (!!x.Installation() && x.Installation().IsActive()) {
				if (installations.map(function (i) {
					return i.Id();
				}).indexOf(x.Installation().Id()) === -1) {
					installations.push(ko.unwrap(x.Installation));
				}
			}
		});
		return installations;
	}

	static calculateServiceDateExceedanceLevelStyle(date: Date, time: string, gracePeriodInDays: number): string {
		const exceedanceLevel = HelperServiceOrder.calculateServiceDateExceedanceLevel(date, time, gracePeriodInDays);
		switch (exceedanceLevel) {
			case ServiceDateExceedanceLevel.NotExceeded:
				return null;
			case ServiceDateExceedanceLevel.WithinGracePeriod:
				return "text-warning";
			case ServiceDateExceedanceLevel.BeyondGracePeriod:
				return "text-danger";
		}
	};

	static calculateServiceDateExceedanceLevel(date: Date, time: string, gracePeriodInDays: number): ServiceDateExceedanceLevel {
		let adjustedDate = window.moment(date);
		if (time) {
			const duration = window.moment.duration(time);
			adjustedDate.set({
				'hour': duration.hours(),
				'minute': duration.minutes(),
				'second': duration.seconds(),
				'millisecond': duration.milliseconds()
			});
		} else {
			adjustedDate.set({
				'hour': 23,
				'minute': 59,
				'second': 59,
				'millisecond': 999
			});
		}

		const currentAndMaxPlannedDateDiffInMilliseconds = window.moment().diff(adjustedDate);
		if (currentAndMaxPlannedDateDiffInMilliseconds > 0) {
			const gracePeriodInMilliseconds = gracePeriodInDays * 864E5;
			if (currentAndMaxPlannedDateDiffInMilliseconds > gracePeriodInMilliseconds) {
				return ServiceDateExceedanceLevel.BeyondGracePeriod;
			} else {
				return ServiceDateExceedanceLevel.WithinGracePeriod;
			}
		} else {
			return ServiceDateExceedanceLevel.NotExceeded;
		}
	}
	
	static isServiceOrderEditable(serviceOrderObservable: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>, currentUserObservable: KnockoutObservable<Main.Rest.Model.Main_User>, serviceOrderStatuses) {
		const serviceOrder = ko.unwrap(serviceOrderObservable)
		const currentUser = ko.unwrap(currentUserObservable)
		let hasPermission = window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "Edit");
		let autonomousPlanningPermission = window.AuthorizationManager.isAuthorizedForAction("Dispatch", "AutonomousDispatchPlanning")
		hasPermission = hasPermission ?? autonomousPlanningPermission ?? serviceOrder.CreateUser() === currentUser.Id;
		if (hasPermission && serviceOrder && serviceOrderStatuses) {
			const status = serviceOrderStatuses[serviceOrder.StatusKey()];
			if (status && status.Groups.split(",").indexOf("Closed") === -1) {
				return true;
			}
		}
		return false;
	}
	
	static dispatchesCanBeAdded(serviceOrderObservable: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>, serviceOrderStatuses)  {
		if (!serviceOrderObservable()) {
			return false;
		}
		const serviceOrderStatus = serviceOrderStatuses.$array.find(x => x.Key === serviceOrderObservable().StatusKey());
		return window._.intersection((serviceOrderStatus.Groups || "").split(","), [
			"Scheduling", "InProgress"
		]).length > 0;
	}
}

enum ServiceDateExceedanceLevel {
	NotExceeded,
	WithinGracePeriod,
	BeyondGracePeriod
}