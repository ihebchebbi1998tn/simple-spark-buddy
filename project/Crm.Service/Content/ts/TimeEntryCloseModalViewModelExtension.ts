import type {TimeEntryIndexViewModel} from "@Crm.PerDiem/TimeEntryIndexViewModel";

export class TimeEntryCloseModalViewModelExtension extends window.Crm.PerDiem.ViewModels.TimeEntryCloseModalViewModel {
	serviceOrderTimePostings = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting>([]);
	serviceOrderExpensePostings = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting>([]);

	constructor(parentViewModel: TimeEntryIndexViewModel) {
		super(parentViewModel);
		let reportEntries = this.reportEntries;
		this.reportEntries = ko.pureComputed(() => [...reportEntries(), ...this.serviceOrderTimePostings(), ...this.serviceOrderExpensePostings()]);
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
	}

	async getDistinctDates(): Promise<void> {
		await super.getDistinctDates();
		this.loading(true);

		if (window.database.CrmService_ServiceOrderTimePosting) {
			let dates = await window.database.CrmService_ServiceOrderTimePosting.GetDistinctServiceOrderTimePostingDates(this.selectedUser()).toArray();
			for (const date of dates) {
				if (this.distinctDates.indexOf(date) === -1) {
					this.distinctDates.push(date);
				}
			}
		}
		if (window.database.CrmService_ServiceOrderExpensePosting) {
			let dates = await window.database.CrmService_ServiceOrderExpensePosting.GetDistinctServiceOrderExpensePostingDates(this.selectedUser()).toArray();
			for (const date of dates) {
				if (this.distinctDates.indexOf(date) === -1) {
					this.distinctDates.push(date);
				}
			}
		}
		this.loading(false);
	}

	async refreshServiceOrderTimePostings(): Promise<void> {
		const id = this.perDiemReportId();
		if (!window.database.CrmService_ServiceOrderTimePosting) {
			return;
		}
		if (!this.perDiemReport().From() || !this.perDiemReport().To()) {
			this.serviceOrderTimePostings([]);
			return;
		}
		let query = window.database.CrmService_ServiceOrderTimePosting
			.include("Article")
			.include("User");
		query = id
			? query.filter(function (it) {
				return it.PerDiemReportId === this.id;
			},
				{ id: id })
			: query.filter(function (it) {
				return it.Username === this.selectedUser &&
					it.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Used &&
					it.IsClosed === false &&
					it.To >= this.from &&
					it.From <= this.to &&
					it.PerDiemReportId === null;
			},
				{
					selectedUser: this.selectedUser(),
					from: window.moment(this.perDiemReport().From()).startOf("day").toDate(),
					to: window.moment(this.perDiemReport().To()).endOf("day").toDate()
				});
		let serviceOrderTimePostings = await query
			.orderBy("it.From")
			.orderBy("it.ItemNo")
			.toArray();
		let observableServiceOrderTimePostings = serviceOrderTimePostings.map(x => x.asKoObservable());
		for (const item of observableServiceOrderTimePostings) {
			// @ts-ignore
			item.ItemDescription = ko.observable("");
		}
		await this.setItemDescriptions(observableServiceOrderTimePostings);
		this.serviceOrderTimePostings(observableServiceOrderTimePostings);

	}

	async refreshServiceOrderExpensePostings(): Promise<void> {
		const id = this.perDiemReportId();
		if (!window.database.CrmService_ServiceOrderExpensePosting) {
			return;
		}
		if (!this.perDiemReport().From() || !this.perDiemReport().To()) {
			this.serviceOrderExpensePostings([]);
			return;
		}
		let query = window.database.CrmService_ServiceOrderExpensePosting
			.include("Article")
			.include("ResponsibleUserObject");
		query = id
			? query.filter(function (it) {
				return it.PerDiemReportId === this.id;
			},
				{ id: id })
			: query.filter(function (it) {
				return it.ResponsibleUser === this.selectedUser &&
					it.IsClosed === false &&
					it.Date >= this.from &&
					it.Date <= this.to &&
					it.PerDiemReportId === null;
			},
				{
					selectedUser: this.selectedUser(),
					from: this.perDiemReport().From(),
					to: this.perDiemReport().To()
				});
		let serviceOrderExpensePostings = await query
			.orderBy("it.Date")
			.orderBy("it.ItemNo")
			.toArray();
		let observableServiceOrderExpensePostings = serviceOrderExpensePostings.map(x => x.asKoObservable());
		for (const item of observableServiceOrderExpensePostings) {
			// @ts-ignore
			item.ItemDescription = ko.observable("");
		}
		await this.setItemDescriptions(observableServiceOrderExpensePostings);
		this.serviceOrderExpensePostings(observableServiceOrderExpensePostings);

	}
	async refresh(): Promise<void> {
		await super.refresh();
		await this.refreshServiceOrderTimePostings();
		await this.refreshServiceOrderExpensePostings();
		
		let user = await window.Helper.User.getCurrentUser();
		await Helper.Article.loadArticleDescriptionsMapFromItemNo(this.serviceOrderTimePostings(), user.DefaultLanguageKey);
		await Helper.Article.loadArticleDescriptionsMapFromItemNo(this.serviceOrderExpensePostings(), user.DefaultLanguageKey);
	};
}

window.Crm.PerDiem.ViewModels.TimeEntryCloseModalViewModel = TimeEntryCloseModalViewModelExtension;
