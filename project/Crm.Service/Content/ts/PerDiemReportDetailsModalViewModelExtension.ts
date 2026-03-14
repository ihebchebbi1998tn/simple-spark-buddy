import { uniq } from "lodash";
import moment from "moment";
import {HelperDate} from "@Main/helper/Helper.Date";

export class PerDiemReportDetailsModalViewModelExtension extends window.Crm.PerDiem.ViewModels.PerDiemReportDetailsModalViewModel {
	serviceOrderTimePostings = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting>([]);
	serviceOrderExpensePostings = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting>([]);

	constructor() {
		super();
		const reportEntries = this.reportEntries;
		this.reportEntries = ko.pureComputed(() => [...reportEntries(), ...this.serviceOrderTimePostings(), ...this.serviceOrderExpensePostings()]);

		this.lookups.serviceOrderTypes = {};
	}

	async loadReportEntries(id?: string, params?: any): Promise<void> {
		await super.loadReportEntries(id, params);
		if (window.database.CrmService_ServiceOrderTimePosting) {
			await this.loadServiceOrderTimePostings(id, params);
		}
		if (window.database.CrmService_ServiceOrderExpensePosting) {
			await this.loadServiceOrderExpensePostings(id, params);
		}
		if (window.database.CrmService_ServiceOrderType) {
			this.lookups.serviceOrderTypes = await window.Helper.Lookup
				.getLocalizedArrayMap("CrmService_ServiceOrderType");		}
	};

	async loadServiceOrderTimePostings(id?: string, params?: any): Promise<void> {
		if (!window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "TimePostingAdd")) {
			return;
		}
		const filter = id
			? function (it) {
				return it.PerDiemReportId === this.id;
			}
			: function (it) {
				return it.Username === this.username && it.From >= this.fromDate && it.To <= this.toDate;
			};
		const filterParams = id ? { id: id } : params;
		let results = await window.database.CrmService_ServiceOrderTimePosting
			.include("Article")
			.include("ServiceOrder")
			.include("ServiceOrder.Company")
			.include("ServiceOrder.Installation")
			.include("ServiceOrderTime.Installation")
			.filter(filter, filterParams)
			.orderBy("it.From")
			.toArray();
		this.serviceOrderTimePostings(results.map(x => {
			let result = x.asKoObservable() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting & { ResponsibleUser: KnockoutObservable<Edm.String> };
			result.ResponsibleUser = result.Username;
			return result;
		}));
	}

	async loadServiceOrderExpensePostings(id?: string, params?: any): Promise<void> {
		if (!window.AuthorizationManager.isAuthorizedForAction("ServiceOrderExpensePosting", "Create")) {
			return;
		}
		const filter = id
			? function (it) {
				return it.PerDiemReportId === this.id;
			}
			: function (it) {
				return it.Username === this.username && it.From >= this.fromDate && it.To <= this.toDate;
			};
		const filterParams = id ? { id: id } : params;
		let results = await window.database.CrmService_ServiceOrderExpensePosting
			.include("Article")
			.include("ServiceOrder")
			.include("ServiceOrder.Company")
			.include("ServiceOrder.Installation")
			.include("ServiceOrderTime.Installation")
			.include("FileResource")
			.filter(filter, filterParams)
			.orderBy("it.Date")
			.toArray();
		this.serviceOrderExpensePostings(results.map(x => {
			let result = x.asKoObservable();
			// @ts-ignore
			result.ResponsibleUser = ko.observable(x.Username);
			return result;
		}));
	}

	getGroupedReportEntries(): { Date: Date, Users: { Duration: moment.Duration, Entries: any[], From: Date, Sums: [string, number][], To: Date, User: Main.Rest.Model.ObservableMain_User }[] }[] {
		const distinctReportEntries = uniq(this.reportEntries().flatMap(function (entry) {
			if ('Date' in entry) {
				return moment(entry.Date()).startOf("day").toDate().getTime();
			} else {
				return HelperDate.getDatesOfRange(Helper.Date.convertLocalDateToTimeZone(entry.From(), entry.TimeZoneFrom()), Helper.Date.convertLocalDateToTimeZone(entry.To(), entry.TimeZoneTo())).map(d => d.getTime());
			}
		}));

		return distinctReportEntries.map(function (time) {
			return window.moment(time).toDate();
		})
			.sort(function (a, b) {
				return a < b ? -1 : a > b ? 1 : 0;
			})
			.map(date => {
				const entries = this.reportEntries().filter(function (x) {
					if ('Date' in x) {
						return window.moment(x.Date()).isSame(date, "day");
					} else {
						return HelperDate.dayIsInRange(date, Helper.Date.convertLocalDateToTimeZone(x.From(), x.TimeZoneFrom()), Helper.Date.convertLocalDateToTimeZone(x.To(), x.TimeZoneTo()));
					}
				});
				var dateEntries = window._.uniq(entries);
				const users = window._.uniq(dateEntries.map(function (x) {
					return x.ResponsibleUser();
				}));
				return {
					Date: date,
					Users: users.map(username => {
						let userDateEntries = dateEntries.filter(function (x) {
							return x.ResponsibleUser() === username;
						});
						userDateEntries = userDateEntries.sort(function (entry1, entry2) {
							if (window.database.CrmPerDiemGermany_PerDiemAllowanceEntry != undefined && window.database.CrmPerDiem_UserExpense != undefined) {
								if (entry1.innerInstance instanceof window.database.CrmPerDiemGermany_PerDiemAllowanceEntry.defaultType || entry1.innerInstance instanceof window.database.CrmPerDiem_UserExpense.defaultType || entry1.innerInstance instanceof window.database.CrmService_ServiceOrderExpensePosting.defaultType) {
									return 1;
								}
								if (entry2.innerInstance instanceof window.database.CrmPerDiemGermany_PerDiemAllowanceEntry.defaultType || entry2.innerInstance instanceof window.database.CrmPerDiem_UserExpense.defaultType || entry1.innerInstance instanceof window.database.CrmPerDiem_UserExpense.defaultType || entry2.innerInstance instanceof window.database.CrmService_ServiceOrderExpensePosting.defaultType) {
									return -1;
								}
								return (new Date(Helper.Date.convertLocalDateToTimeZone(entry1.From(), entry1.TimeZoneFrom())).getTime() - new Date(Helper.Date.convertLocalDateToTimeZone(entry2.From(), entry2.TimeZoneFrom())).getTime());
							} else if (window.database.CrmPerDiemGermany_PerDiemAllowanceEntry == undefined) {
								if (entry1.innerInstance instanceof window.database.CrmPerDiem_UserExpense.defaultType || entry1.innerInstance instanceof window.database.CrmPerDiem_UserExpense.defaultType || entry1.innerInstance instanceof window.database.CrmService_ServiceOrderExpensePosting.defaultType) {
									return 1;
								}
								if (entry2.innerInstance instanceof window.database.CrmPerDiem_UserExpense.defaultType || entry1.innerInstance instanceof window.database.CrmPerDiem_UserExpense.defaultType || entry2.innerInstance instanceof window.database.CrmService_ServiceOrderExpensePosting.defaultType) {
									return -1;
								}
								return (new Date(Helper.Date.convertLocalDateToTimeZone(entry1.From(), entry1.TimeZoneFrom())).getTime() - new Date(Helper.Date.convertLocalDateToTimeZone(entry2.From(), entry2.TimeZoneFrom())).getTime());
							} else {
								if (entry1.innerInstance instanceof window.database.CrmPerDiemGermany_PerDiemAllowanceEntry.defaultType || entry1.innerInstance instanceof window.database.CrmPerDiem_UserExpense.defaultType || entry1.innerInstance instanceof window.database.CrmService_ServiceOrderExpensePosting.defaultType) {
									return 1;
								}
								if (entry2.innerInstance instanceof window.database.CrmPerDiemGermany_PerDiemAllowanceEntry.defaultType || entry1.innerInstance instanceof window.database.CrmPerDiem_UserExpense.defaultType || entry2.innerInstance instanceof window.database.CrmService_ServiceOrderExpensePosting.defaultType) {
									return -1;
								}
								return (new Date(Helper.Date.convertLocalDateToTimeZone(entry1.From(), entry1.TimeZoneFrom())).getTime() - new Date(Helper.Date.convertLocalDateToTimeZone(entry2.From(), entry2.TimeZoneFrom())).getTime());
							}

						})
						return {
							Duration: this.getDurationSum(userDateEntries, date),
							Entries: userDateEntries,
							From: new Date(Math.min(
								...userDateEntries
									.filter(x => !!window.ko.unwrap(x.From))
									.map(x => HelperDate.areOnSameDay(Helper.Date.convertLocalDateToTimeZone(x.From(), x.TimeZoneFrom()), date) ? Helper.Date.convertLocalDateToTimeZone(x.From(), x.TimeZoneFrom()).getTime() : date.getTime()))),
							Sums: this.getExpenseSums(userDateEntries),
							To: new Date(Math.max(
								...userDateEntries
									.filter(x => !!window.ko.unwrap(x.To))
									.map(x => HelperDate.areOnSameDay(Helper.Date.convertLocalDateToTimeZone(x.To(), x.TimeZoneTo()), date) ? Helper.Date.convertLocalDateToTimeZone(x.To(), x.TimeZoneTo()).getTime() : moment(date).add(24, 'hour').toDate().getTime()))),
							User: this.users().find(function (x) {
								return x.Id() === username;
							})
						};
					})
				};
			});
	};

	isFirstServiceOrderEntry(data, entries, index): boolean {
		if (index == 0) {
			return true;
		}
		const firstEntryWithSameServiceOrderNumber = entries.find(function (entry) {
			if (entry.innerInstance instanceof window.database.CrmService_ServiceOrderTimePosting.defaultType || entry.innerInstance instanceof window.database.CrmService_ServiceOrderExpensePosting.defaultType) {
				return data.ServiceOrder().OrderNo() == entry.ServiceOrder().OrderNo();
			}
			return false;
		});

		return firstEntryWithSameServiceOrderNumber == data;
	};

	isSameOrderAsPrevious(data, entries, index): boolean {
		if (index == 0) {
			return true;
		}
		if (this.isFirstServiceOrderEntry(data, entries, index) == true) {
			return true;
		}
		if (entries[index - 1].ServiceOrder == undefined) {
			return false;
		}
		return entries[index - 1].ServiceOrder().OrderNo() == entries[index].ServiceOrder().OrderNo();
	};

}

window.Crm.PerDiem.ViewModels.PerDiemReportDetailsModalViewModel = PerDiemReportDetailsModalViewModelExtension;