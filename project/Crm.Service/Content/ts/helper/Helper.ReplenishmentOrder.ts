export class HelperReplenishmentOrder {
	static async getOrCreateCurrentReplenishmentOrder(responsibleUser: string, excludeId?: string): Promise<Crm.Service.Rest.Model.ObservableCrmService_ReplenishmentOrder> {
		let replenishmentOrders = await window.database.CrmService_ReplenishmentOrder
			.filter(function (replenishmentOrder) {
				return replenishmentOrder.IsClosed === false && replenishmentOrder.ResponsibleUser === this.responsibleUser && replenishmentOrder.Id !== this.excludeId;
			}, {responsibleUser: responsibleUser, excludeId: excludeId || null})
			.take(1)
			.toArray();
		if (replenishmentOrders.length === 0) {
			const newReplenishmentOrder = window.database.CrmService_ReplenishmentOrder.defaultType.create();
			newReplenishmentOrder.ResponsibleUser = responsibleUser;
			window.database.add(newReplenishmentOrder);
			return newReplenishmentOrder.asKoObservable();
		} else {
			const replenishmentOrder = replenishmentOrders[0];
			window.database.attachOrGet(replenishmentOrder);
			return replenishmentOrder.asKoObservable();
		}
	}
}