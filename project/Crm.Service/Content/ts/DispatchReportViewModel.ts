import {namespace} from "@Main/namespace";

export class DispatchReportViewModel extends window.Crm.Service.ViewModels.ServiceOrderReportViewModel {
	dispatch = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>(null);
	dispatchedUser = ko.observable<Main.Rest.Model.ObservableMain_User>(null);

	constructor() {
		super();
		this.showEstimatedData(true);
		this.showInvoiceData(false);
	}

	async init(id?: string, params?: {[key:string]:string}) {
		await window.Helper.Database.initialize();
		let dispatch = await window.database.CrmService_ServiceOrderDispatch
			.include("DispatchedUser")
			.include("ServiceOrder")
			.include("ServiceOrder.InvoiceRecipient")
			.include("ServiceOrder.Payer")
			.include("ServiceOrder.ServiceObject")
			.find(id);
		this.dispatch(dispatch.asKoObservable());
		this.serviceOrder(this.dispatch().ServiceOrder());
		const queries = [];
		queries.push({
			queryable: window.database.CrmService_ServiceOrderTime
				.include("Installation")
				.filter("it.OrderId === this.orderId", {orderId: this.dispatch().OrderId()}),
			method: "toArray",
			handler: x => {
				const serviceOrderTimes = x.map(function (i) {
					return i.asKoObservable();
				});
				this.dispatch().ServiceOrder().ServiceOrderTimes(serviceOrderTimes);
				this.serviceOrderTimes(serviceOrderTimes);
			}
		});
		queries.push({
			queryable: window.database.CrmService_ServiceOrderMaterial
				.include("ChildServiceOrderMaterials")
				.filter("it.DispatchId === this.dispatchId && it.ServiceOrderMaterialType === '" + window.Crm.Service.ServiceOrderMaterialType.Used + "'", {dispatchId: this.dispatch().Id()}),
			method: "toArray",
			handler: x => {
				const dispatchMaterials = x.map(function (i) {
					return i.asKoObservable();
				});
				this.dispatch().ServiceOrderMaterial(dispatchMaterials);
				this.displayedMaterials(this.dispatch().ServiceOrderMaterial() || []);
			}
		});
		queries.push({
			queryable: window.database.CrmService_ServiceOrderTimePosting
				.include("User")
				.include("ChildServiceOrderTimePostings")
				.filter("it.DispatchId === this.dispatchId && it.Username != null && it.ServiceOrderTimePostingType === '" + window.Crm.Service.ServiceOrderTimePostingType.Used + "'", {dispatchId: this.dispatch().Id()}),
			method: "toArray",
			handler: x => {
				const dispatchTimePostings = x.map(function (i) {
					return i.asKoObservable();
				});
				this.dispatch().ServiceOrderTimePostings(dispatchTimePostings);
				this.displayedTimePostings(this.dispatch().ServiceOrderTimePostings() || []);
			}
		});
		queries.push({
			queryable: window.database.CrmService_ServiceOrderExpensePosting
				.include("ResponsibleUserObject")
				.filter("it.DispatchId === this.dispatchId && it.ResponsibleUser != null", { dispatchId: this.dispatch().Id() }),
			method: "toArray",
			handler: x => {
				const dispatchExpensePostings = x.map(function (i) {
					return i.asKoObservable();
				});
				this.dispatch().ServiceOrderExpensePostings(dispatchExpensePostings);
				this.displayedExpensePostings(this.dispatch().ServiceOrderExpensePostings() || []);
			}
		});
		queries.push({
			queryable: window.database.CrmService_ServiceOrderErrorType
				.include("ServiceOrderErrorCauses")
				.filter("it.OrderId === this.orderId && it.DispatchId === this.dispatchId", { orderId: this.dispatch().ServiceOrder().Id(), dispatchId: this.dispatch().Id() })
				.orderByDescending(it => it.IsMainErrorType),
			method: "toArray",
			handler: x => {
				const dispatchErrorTypes = x.map(function (i) {
					return i.asKoObservable();
				});
				this.dispatch().ServiceOrderErrorTypes(dispatchErrorTypes);
				this.displayedErrorTypes(this.dispatch().ServiceOrderErrorTypes() || []);
			}
		});
		queries.push({
			queryable: window.database.Crm_Company
				.include2("Addresses.filter(function(x) { x.IsCompanyStandardAddress === true; })")
				.filter("it.Id === this.id", {id: this.dispatch().ServiceOrder().CustomerContactId()}),
			method: "first",
			handler: this.dispatch().ServiceOrder().Company
		});
		if (this.dispatch().ServiceOrder().InitiatorId()) {
			queries.push({
				queryable: window.database.Crm_Company
					.include2("Addresses.filter(function(x) { x.IsCompanyStandardAddress === true; })")
					.include("Phones")
					.include("Emails")
					.filter("it.Id === this.id", {id: this.dispatch().ServiceOrder().InitiatorId()}),
				method: "first",
				handler: this.dispatch().ServiceOrder().Initiator
			});
		}
		if (this.dispatch().ServiceOrder().InstallationId()) {
			queries.push({
				queryable: window.database.CrmService_Installation
					.include("Company")
					.include2("Company.Addresses.filter(function(x) { x.IsCompanyStandardAddress === true; })")
					.filter("it.Id === this.id", {id: this.dispatch().ServiceOrder().InstallationId()}),
				method: "first",
				handler: this.dispatch().ServiceOrder().Installation
			});
		}
		if (this.dispatch().ServiceOrder().InitiatorPersonId()) {
			queries.push({
				queryable: window.database.Crm_Person
					.include("Emails")
					.include("Phones")
					.filter("it.Id === this.id", {id: this.dispatch().ServiceOrder().InitiatorPersonId()}),
				method: "first",
				handler: this.dispatch().ServiceOrder().InitiatorPerson
			});
		}
		await window.Helper.Batch.Execute(queries);
		this.lookups.timePostingUserDisplayNames = this.displayedTimePostings().reduce(function (map, x) {
			map[x.Username()] = window.Helper.User.getDisplayName(x.User());
			return map;
		}, {});
		this.lookups.expensePostingUserDisplayNames = this.displayedExpensePostings().reduce(function (map, x) {
			map[x.ResponsibleUser()] = window.Helper.User.getDisplayName(x.ResponsibleUserObject());
			return map;
		}, {});

		const timePostingItemNos = this.displayedTimePostings().map(function (i) {
			return window.ko.unwrap(i.ItemNo);
		});
		var expensePostingItemNos = this.displayedExpensePostings().map(function (i) {
			return window.ko.unwrap(i.ItemNo);
		});
		const materialItemNos = this.displayedMaterials().map(function (i) {
			return window.ko.unwrap(i.ItemNo);
		});
		this.itemNos = [];
		this.itemNos.push.apply(this.itemNos, timePostingItemNos);
		this.itemNos.push.apply(this.itemNos, expensePostingItemNos);
		this.itemNos.push.apply(this.itemNos, materialItemNos);
		await super.init(null, params);
		if (this.dispatch() && this.dispatch().ServiceOrderMaterial && this.dispatch().ServiceOrderMaterial()) {
			let preplannedType = window.Crm.Service.ServiceOrderMaterialType.Preplanned;
			let serviceOrderMaterialsQuery = () => window.database.CrmService_ServiceOrderMaterial
				.filter("it.OrderId === this.serviceOrderId", { serviceOrderId: this.serviceOrder().Id() })
				.filter("it.ServiceOrderMaterialType === this.preplannedType", { preplannedType })
			await window.Helper.ServiceOrderMaterial.loadParentAndChildPositions(this.dispatch().ServiceOrderMaterial(), serviceOrderMaterialsQuery);
			this.displayedMaterials(this.dispatch().ServiceOrderMaterial()
				.filter(function (x) {
					return x.Quantity() > 0;
				}));
		}
		if (this.dispatch() && this.dispatch().ServiceOrderTimePostings && this.dispatch().ServiceOrderTimePostings()) {
			this.displayedTimePostings(this.dispatch().ServiceOrderTimePostings());
		}
		if (this.dispatch() && this.dispatch().ServiceOrderExpensePostings && this.dispatch().ServiceOrderExpensePostings()) {
			this.displayedExpensePostings(this.dispatch().ServiceOrderExpensePostings());
		}
		if (this.dispatch() && this.dispatch().ServiceOrderErrorTypes && this.dispatch().ServiceOrderErrorTypes()) {
			this.displayedErrorTypes(this.dispatch().ServiceOrderErrorTypes());
		}
		if (this.dispatch() && this.dispatch().DispatchedUser && this.dispatch().DispatchedUser()) {
			this.dispatchedUser(this.dispatch().DispatchedUser());
		}

	};
}

namespace("Crm.Service.ViewModels").DispatchReportViewModel = DispatchReportViewModel;