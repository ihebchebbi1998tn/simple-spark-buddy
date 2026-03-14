interface ValidItemNosAfterSignature {
	validMaterialItemNosAfterCustomerSignature: any[],
	validCostItemNosAfterCustomerSignature: any[]
}
interface EditableItemNosAfterSignature {
	editableMaterialItemNosAfterCustomerSignature: any[],
	editableCostItemNosAfterCustomerSignature: any[]
}

export class HelperServiceOrderMaterial {

	static getActualPositions(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, dispatchId?: string): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[] {
		if (serviceOrderMaterial.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned) {
			let usedPositions = serviceOrderMaterial.ChildServiceOrderMaterials().filter(x => x.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used);
			if (dispatchId) {
				usedPositions = usedPositions.filter(x => x.DispatchId() === dispatchId);
			}
			return usedPositions;
		}

		if (serviceOrderMaterial.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Used && (!dispatchId || dispatchId === serviceOrderMaterial.DispatchId())) {
			return [serviceOrderMaterial];
		}

		if (serviceOrderMaterial.ParentServiceOrderMaterial()) {
			return window.Helper.ServiceOrderMaterial.getActualPositions(serviceOrderMaterial.ParentServiceOrderMaterial(), dispatchId);
		}

		return [];
	}

	static getActualDiscount(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, currency: string): string {
		let actualPositions = window.Helper.ServiceOrderMaterial.getActualPositions(serviceOrderMaterial);
		if (actualPositions.length > 0 && actualPositions.every(x => x.DiscountType() === window.Crm.Article.Model.Enums.DiscountType.Percentage && x.Discount() === actualPositions[0].Discount())) {
			return `${actualPositions[0].Discount()} %`;
		}
		return `${window.Globalize.formatNumber(window.Helper.ServiceOrderMaterial.getActualTotalDiscount(serviceOrderMaterial), {
			maximumFractionDigits: 2,
			minimumFractionDigits: 2
		})} ${currency}`;
	}

	static getActualQuantity(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, dispatchId?: string): number {
		return window.Helper.ServiceOrderMaterial.getActualPositions(serviceOrderMaterial).reduce((result, position) => {
			return result + position.Quantity();
		}, 0);
	}

	static getActualPrice(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, dispatchId?: string): number {
		return window.Helper.ServiceOrderMaterial.getActualPositions(serviceOrderMaterial)[0]?.Price() ?? 0;
	}

	static getActualTotalDiscount(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, dispatchId?: string): number {
		return window.Helper.ServiceOrderMaterial.getActualPositions(serviceOrderMaterial).reduce((result, position) => {
			return result + window.Helper.ServiceOrderMaterial.getTotalDiscount(position);
		}, 0);
	}

	static getActualTotalPrice(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, dispatchId?: string): number {
		return window.Helper.ServiceOrderMaterial.getActualPositions(serviceOrderMaterial).reduce((result, position) => {
			return result + window.Helper.ServiceOrderMaterial.getTotalPrice(position);
		}, 0);
	}

	static getDiscount(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, currency: string): string {
		if (serviceOrderMaterial.DiscountType() == Crm.Article.Model.Enums.DiscountType.Percentage) {
			return `${serviceOrderMaterial.Discount()} %`;
		}
		return `${window.Globalize.formatNumber(serviceOrderMaterial.Discount() * serviceOrderMaterial.Quantity(), {
			maximumFractionDigits: 2,
			minimumFractionDigits: 2
		})} ${currency}`;
	}

	static getEstimatedDiscount(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, currency: string): string {
		let estimatedPosition = window.Helper.ServiceOrderMaterial.getEstimatedPosition(serviceOrderMaterial);
		if (estimatedPosition) {
			return window.Helper.ServiceOrderMaterial.getDiscount(estimatedPosition, currency);
		}
		return `${window.Globalize.formatNumber(0, {maximumFractionDigits: 2, minimumFractionDigits: 2})} ${currency}`;
	}

	static getEstimatedPosition(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial {
		if (serviceOrderMaterial.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned) {
			return serviceOrderMaterial;
		}

		if (serviceOrderMaterial.ParentServiceOrderMaterial()) {
			return window.Helper.ServiceOrderMaterial.getEstimatedPosition(serviceOrderMaterial.ParentServiceOrderMaterial());
		}

		return null;
	}

	static getEstimatedQuantity(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return window.Helper.ServiceOrderMaterial.getEstimatedPosition(serviceOrderMaterial)?.Quantity() ?? 0;
	}

	static getEstimatedPrice(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return window.Helper.ServiceOrderMaterial.getEstimatedPosition(serviceOrderMaterial)?.Price() ?? 0;
	}

	static getEstimatedTotalDiscount(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		let position = window.Helper.ServiceOrderMaterial.getEstimatedPosition(serviceOrderMaterial);
		if (!position) {
			return 0;
		}
		return window.Helper.ServiceOrderMaterial.getTotalDiscount(position);
	}

	static getEstimatedTotalPrice(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		let position = window.Helper.ServiceOrderMaterial.getEstimatedPosition(serviceOrderMaterial);
		if (!position) {
			return 0;
		}
		return window.Helper.ServiceOrderMaterial.getTotalPrice(position);
	}

	static getInvoicePositions(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[] {
		const visited = new Set<string>();
		return HelperServiceOrderMaterial.collectPositions(serviceOrderMaterial, visited);
	}

	static createInvoicePosition(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial {
		let invoiceServiceOrderMaterial = window.Helper.Database.createClone(serviceOrderMaterial.innerInstance);
		invoiceServiceOrderMaterial.Id = window.$data.createGuid().toString().toLowerCase();
		invoiceServiceOrderMaterial.ChildServiceOrderMaterials = [];
		invoiceServiceOrderMaterial.ParentServiceOrderMaterialId = serviceOrderMaterial.Id();
		invoiceServiceOrderMaterial.ServiceOrderMaterialType = window.Crm.Service.ServiceOrderMaterialType.Invoice;
		return invoiceServiceOrderMaterial;
	}

	static collectPositions(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, visited: Set<string>): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[] {
		const id = serviceOrderMaterial.Id();
		if (visited.has(id)) return [];

		visited.add(id);
		if (serviceOrderMaterial.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Invoice) {
			return [serviceOrderMaterial];
		}
		let invoicePositions: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[] = [];
		const parent = serviceOrderMaterial.ParentServiceOrderMaterial?.();
		if (parent) {
			invoicePositions.push(...HelperServiceOrderMaterial.collectPositions(parent, visited));
		}
		for (const child of serviceOrderMaterial.ChildServiceOrderMaterials?.() ?? []) {
			invoicePositions.push(...HelperServiceOrderMaterial.collectPositions(child, visited));
		}
		return invoicePositions;
	}

	static getInvoiceDiscount(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, currency: string): string {
		let invoicePositions = window.Helper.ServiceOrderMaterial.getInvoicePositions(serviceOrderMaterial);
		if (invoicePositions.length > 0 && invoicePositions.every(x => x.DiscountType() === window.Crm.Article.Model.Enums.DiscountType.Percentage && x.Discount() === invoicePositions[0].Discount())) {
			return `${invoicePositions[0].Discount()} %`;
		}
		return `${window.Globalize.formatNumber(window.Helper.ServiceOrderMaterial.getInvoiceTotalDiscount(serviceOrderMaterial), {
			maximumFractionDigits: 2,
			minimumFractionDigits: 2
		})} ${currency}`;
	}

	static getInvoiceQuantity(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return window.Helper.ServiceOrderMaterial.getInvoicePositions(serviceOrderMaterial).reduce((result, position) => {
			return result + position.Quantity();
		}, 0);
	}

	static getInvoicePrice(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return window.Helper.ServiceOrderMaterial.getInvoicePositions(serviceOrderMaterial)[0]?.Price() ?? 0;
	}

	static getInvoiceTotalDiscount(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return window.Helper.ServiceOrderMaterial.getInvoicePositions(serviceOrderMaterial).reduce((result, position) => {
			return result + window.Helper.ServiceOrderMaterial.getTotalDiscount(position);
		}, 0);
	}

	static getInvoiceTotalPrice(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return window.Helper.ServiceOrderMaterial.getInvoicePositions(serviceOrderMaterial).reduce((result, position) => {
			return result + window.Helper.ServiceOrderMaterial.getTotalPrice(position);
		}, 0);
	}

	static getServiceOrderMaterialDescription(material: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial, lookup: any): string {
		return lookup[material.ItemNo()] || material.Description() || "";
	}

	static getTotalDiscount(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return (serviceOrderMaterial.DiscountType() == Crm.Article.Model.Enums.DiscountType.Percentage ? serviceOrderMaterial.Price() * serviceOrderMaterial.Discount() / 100 : serviceOrderMaterial.Discount()) * serviceOrderMaterial.Quantity();
	}

	static getTotalPrice(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return (serviceOrderMaterial.Price() - (serviceOrderMaterial.DiscountType() == Crm.Article.Model.Enums.DiscountType.Percentage ? serviceOrderMaterial.Price() * serviceOrderMaterial.Discount() / 100 : serviceOrderMaterial.Discount())) * serviceOrderMaterial.Quantity();
	}

	static async getValidItemNosAfterSignature(): Promise<ValidItemNosAfterSignature> {
		let items = await window.database.CrmArticle_Article.filter((it) => {
			return (it.ArticleTypeKey === "Material" || it.ArticleTypeKey === "Cost") && it.ExtensionValues.CanBeAddedAfterCustomerSignature && !it.ExtensionValues.IsHidden;
		}).toArray();
		const result = items.reduce((r, o) => {
			r[o.ArticleTypeKey === "Material" ? 'materials' : 'costs'].push(o);
			return r;
		}, {materials: [], costs: []});
		return {
			validMaterialItemNosAfterCustomerSignature: result.materials.map(i => i.ItemNo),
			validCostItemNosAfterCustomerSignature: result.costs.map(i => i.ItemNo)
		}
	}

	static async getEditableItemNosAfterSignature(): Promise<EditableItemNosAfterSignature> {
		let items = await window.database.CrmArticle_Article.filter((it) => {
			return (it.ArticleTypeKey === "Material" || it.ArticleTypeKey === "Cost") && it.ExtensionValues.CanBeEditedAfterCustomerSignature && !it.ExtensionValues.IsHidden;
		}).toArray();
		const result = items.reduce((r, o) => {
			r[o.ArticleTypeKey === "Material" ? 'materials' : 'costs'].push(o);
			return r;
		}, { materials: [], costs: [] });
		return {
			editableMaterialItemNosAfterCustomerSignature: result.materials.map(i => i.ItemNo),
			editableCostItemNosAfterCustomerSignature: result.costs.map(i => i.ItemNo)
		}
	}


	static async loadParentAndChildPositions(material: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[], getQuery: () => $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial>): Promise<void> {
		const loadParentPositions = async (allItems: (Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial)[]): Promise<void> => {
			let itemsWithParent = allItems.filter(x => x.ParentServiceOrderMaterialId() !== null);
			if (itemsWithParent.length === 0) {
				return;
			}
			let parents: Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial[] = [];
			let parentIds = itemsWithParent.map(x => x.ParentServiceOrderMaterialId());
			let queries = window._.chunk(parentIds, 25).map(ids => {
				return {
					queryable: getQuery().filter(function (it) {
						return it.Id in this.ids;
					}, {ids: ids}),
					method: "toArray",
					handler: (results: Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial[]) => {
						parents.push(...results);
					}
				}
			});
			await window.Helper.Batch.Execute(queries);
			let parentsAsKoObservable = parents.map(x => x.asKoObservable());
			for (const itemWithParent of itemsWithParent) {
				let parent = parentsAsKoObservable.find(x => x.Id() === itemWithParent.ParentServiceOrderMaterialId()) || null;
				itemWithParent.ParentServiceOrderMaterial(parent);
			}
			await loadParentPositions(parentsAsKoObservable);
			await loadChildPositions(parentsAsKoObservable);
		}
		const loadChildPositions = async (allItems: (Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial)[]): Promise<void> => {
			if (allItems.length === 0) {
				return;
			}
			let children: Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial[] = [];
			let parentIds = allItems.map(x => x.Id());
			let queries = window._.chunk(parentIds, 25).map(ids => {
				return {
					queryable: getQuery().filter(function (it) {
						return it.ParentServiceOrderMaterialId in this.ids;
					}, {ids: ids}),
					method: "toArray",
					handler: (results: Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial[]) => {
						children.push(...results);
					}
				}
			});
			await window.Helper.Batch.Execute(queries);
			let childrenAsKoObservable = children.map(x => x.asKoObservable());
			for (const item of allItems) {
				let children = childrenAsKoObservable.filter(x => x.ParentServiceOrderMaterialId() === item.Id()) || [];
				item.ChildServiceOrderMaterials(children);
			}
			await loadChildPositions(childrenAsKoObservable);
		}
		let items = Array.isArray(material) ? material : [material];
		await loadParentPositions(items);
		await loadChildPositions(items);
	}

	static quantityValidator(value: number, serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial) {
		const quantityUnitEntry = serviceOrderMaterial.QuantityUnitEntry() ?? serviceOrderMaterial.Article()?.QuantityUnitEntry();
		const qtyStep = quantityUnitEntry?.QuantityStep() ?? 0;
		if (qtyStep === 0) {
			return true; //"any"
		}
		return Math.round(value * 1e6) % Math.round(qtyStep * 1e6) === 0;
	}
}

window.namespace("Crm.Service").ServiceOrderMaterialType ||= {};
window.Crm.Service.ServiceOrderMaterialType.Invoice = "Invoice";
window.Crm.Service.ServiceOrderMaterialType.Preplanned = "Preplanned";
window.Crm.Service.ServiceOrderMaterialType.Used = "Used";



