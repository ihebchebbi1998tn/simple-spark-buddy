import { uniq } from "lodash";
import {HelperDate} from "@Main/helper/Helper.Date";

export class TimeEntryIndexViewModelExtension extends window.Crm.PerDiem.ViewModels.TimeEntryIndexViewModel {
	serviceOrderTimePostings = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting>([]);
	serviceOrderExpensePostings = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting>([]);
	canSeeAllUsersDispatches: boolean;

	constructor() {
		super();
	}

	async setItemDescriptions(serviceOrderPostings): Promise<void> {
		let language = await window.Helper.Culture.languageCulture();
		const itemNos: string[] = window._.uniq(serviceOrderPostings.map(x => x.ItemNo()));
		let articleDescriptionsMap = await window.Helper.Article.loadArticleDescriptionsMap(itemNos, language);
		serviceOrderPostings.forEach(function (serviceOrderPosting) {
			serviceOrderPosting.ItemDescription(articleDescriptionsMap[serviceOrderPosting.ItemNo()]);
		});
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		if (window.database.CrmService_ServiceOrderTimePosting) {
			window.Helper.Database.registerEventHandlers(this,
				{
					"CrmService_ServiceOrderTimePosting": {
						"afterCreate": this.refreshServiceOrderTimePostings,
						"afterDelete": async (sender, item) => {
							await this.refreshServiceOrderTimePostings();
							if (item.PerDiemReportId) {
								await window.Helper.PerDiem.refreshPerDiemReport(item.PerDiemReportId);
							}
						},
						"afterUpdate": this.refreshServiceOrderTimePostings
					}
				});
		}
		
		if (window.database.CrmService_ServiceOrderExpensePosting) {
			window.Helper.Database.registerEventHandlers(this,
				{
					"CrmService_ServiceOrderExpensePosting": {
						"afterCreate": this.refreshServiceOrderExpensePostings,
						"afterDelete": async (sender, item) => {
							await this.refreshServiceOrderExpensePostings();
							if (item.PerDiemReportId) {
								await window.Helper.PerDiem.refreshPerDiemReport(item.PerDiemReportId);
							}
						},
						"afterUpdate": this.refreshServiceOrderExpensePostings
					}
				});
		}
		
		this.canSeeAllUsersDispatches = window.AuthorizationManager.isAuthorizedForAction("Dispatch", "SeeAllUsersDispatches");
	}

	async refresh(): Promise<void> {
		await super.refresh();
		if (window.database.CrmService_ServiceOrderTimePosting) {
			await this.refreshServiceOrderTimePostings();
		}
		if (window.database.CrmService_ServiceOrderExpensePosting) {
			await this.refreshServiceOrderExpensePostings();
		}
	};

	async refreshServiceOrderTimePostings(): Promise<void> {
		let setLoading = this.loading() === false;
		if (setLoading) {
			this.loading(true);
		}
		let serviceOrderTimePostings = await window.database.CrmService_ServiceOrderTimePosting
			.include("ServiceOrder")
			.include("ServiceOrderDispatch")
			.include("ServiceOrderDispatch.DispatchedUser")
			.filter(function (serviceOrderTimePosting) {
					return serviceOrderTimePosting.Username === this.username &&
						serviceOrderTimePosting.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Used &&
						serviceOrderTimePosting.To >= this.dateMin &&
						serviceOrderTimePosting.From < this.dateMax;
				},
				{
					dateMin: this.dateFilterFrom(),
					dateMax: this.dateFilterTo(),
					username: this.username()
				})
			.toArray();

		let observableServiceOrderTimePostings = serviceOrderTimePostings.map(x => x.asKoObservable());
		for (const item of observableServiceOrderTimePostings) {
			// @ts-ignore
			item.ItemDescription = ko.observable("");
			// @ts-ignore
			item.canViewCurrentUser = ko.observable(false);
			if (this.canSeeAllUsersDispatches) {
				// @ts-ignore
				item.canViewCurrentUser(true);
			} else {
				let count = await window.database.CrmService_ServiceOrderDispatch
					.filter(function (it) {
							return it.Username === this.username &&
								it.OrderId === this.orderId;
						},
						{username: this.username, orderId: item.OrderId()})
					.count();
				if (count > 0) {
					// @ts-ignore
					item.canViewCurrentUser(true);
				}
			}
		}

		await this.setItemDescriptions(observableServiceOrderTimePostings);
		this.serviceOrderTimePostings(observableServiceOrderTimePostings);
		let user = await window.Helper.User.getCurrentUser();
		await window.Helper.Article.loadArticleDescriptionsMapFromItemNo(this.serviceOrderTimePostings(), user.DefaultLanguageKey);

		if (setLoading) {
			this.loading(false);
		}
	};
	
	async refreshServiceOrderExpensePostings(): Promise<void> {
		let setLoading = this.loading() === false;
		if (setLoading) {
			this.loading(true);
		}

		let serviceOrderExpensePostings = await window.database.CrmService_ServiceOrderExpensePosting
			.include("ServiceOrder")
			.include("ServiceOrderDispatch")
			.include("Article")
			.include("FileResource")
			.filter(function (serviceOrderExpensePosting) {
					return serviceOrderExpensePosting.ResponsibleUser === this.username &&
						serviceOrderExpensePosting.Date >= this.dateMin &&
						serviceOrderExpensePosting.Date < this.dateMax;
				},
				{
					dateMin: this.dateFilterFrom(),
					dateMax: this.dateFilterTo(),
					username: this.username()
				})
			.toArray();

		let observableServiceOrderExpensePostings = serviceOrderExpensePostings.map(x => x.asKoObservable());
		for (const item of observableServiceOrderExpensePostings) {
			// @ts-ignore
			item.ItemDescription = ko.observable("");
			// @ts-ignore
			item.canViewCurrentUser = ko.observable(false);
			if (this.canSeeAllUsersDispatches) {
				// @ts-ignore
				item.canViewCurrentUser(true);
			} else {
				let count = await window.database.CrmService_ServiceOrderDispatch
					.filter(function (it) {
							return it.Username === this.username &&
								it.OrderId === this.orderId;
						},
						{username: this.username, orderId: item.OrderId()})
					.count();
				if (count > 0) {
					// @ts-ignore
					item.canViewCurrentUser(true);
				}
			}
		}
		await this.setItemDescriptions(observableServiceOrderExpensePostings);
		this.serviceOrderExpensePostings(observableServiceOrderExpensePostings);
		let user = await window.Helper.User.getCurrentUser();
		await window.Helper.Article.loadArticleDescriptionsMapFromItemNo(this.serviceOrderTimePostings(), user.DefaultLanguageKey);
		await window.Helper.Article.loadArticleDescriptionsMapFromItemNo(this.serviceOrderExpensePostings(), user.DefaultLanguageKey);

		if (setLoading) {
			this.loading(false);
		}
	};

	getTimesForDate(date: Date): any[] {
		let timePostingsForDate = this.serviceOrderTimePostings().filter(x => HelperDate.dayIsInRange(date, x.From(), x.To()));
		return [].concat(super.getTimesForDate(date)).concat(timePostingsForDate);
	};

	getExpensesForDate(date: Date): any[] {
		let expensePostingsForDate = this.serviceOrderExpensePostings().filter(x => window.moment(x.Date()).isSame(date, 'day'));
		return [].concat(super.getExpensesForDate(date)).concat(expensePostingsForDate);
	};

	getCurrencyKeys(): string[] {
		return uniq(super.getCurrencyKeys().concat(this.serviceOrderExpensePostings().map(x => x.CurrencyKey())));
	}
}

window.Crm.PerDiem.ViewModels.TimeEntryIndexViewModel = TimeEntryIndexViewModelExtension;