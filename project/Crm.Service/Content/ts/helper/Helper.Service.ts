function setupService() {
	function applyTimeEntryFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting>, userId: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting> {
		return query.filter(function(it) {
			return it.Username === this.currentUser && (it.IsClosed === false && it.PerDiemReportId === null);
		}, { currentUser: userId });
	}

	function applyExpenseEntryFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderExpensePosting>, userId: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderExpensePosting> {
		return query.filter(function (it) {
			return it.ResponsibleUser === this.currentUser && (it.IsClosed === false && it.PerDiemReportId === null);
		}, { currentUser: userId });
	}
	function configureOdataGetTimePostingDistinctDates() {
		if (!window.database.CrmService_ServiceOrderTimePosting) {
			return;
		}
		if (!window.database.CrmService_ServiceOrderTimePosting.GetDistinctServiceOrderTimePostingDates) {
			throw "CrmService_ServiceOrderTimePosting.GetDistinctServiceOrderTimePostingDates must be defined at this point";
		}
		const origGetDistinctServiceOrderTimePostingDates = window.database.CrmService_ServiceOrderTimePosting.GetDistinctServiceOrderTimePostingDates;
		// @ts-ignore
		window.database.CrmService_ServiceOrderTimePosting.GetDistinctServiceOrderTimePostingDates = function(userId) {
			// @ts-ignore
			return applyTimeEntryFilter(origGetDistinctServiceOrderTimePostingDates.call(this), userId);
		};
	}

	function configureOdataGetExpensePostingDistinctDates() {
		if (!window.database.CrmService_ServiceOrderExpensePosting) {
			return;
		}
		if (!window.database.CrmService_ServiceOrderExpensePosting.GetDistinctServiceOrderExpensePostingDates) {
			throw "CrmService_ServiceOrderExpensePosting.GetDistinctServiceOrderExpensePostingDates must be defined at this point";
		}
		const origGetDistinctServiceOrderExpensePostingDates = window.database.CrmService_ServiceOrderExpensePosting.GetDistinctServiceOrderExpensePostingDates;
		// @ts-ignore
		window.database.CrmService_ServiceOrderExpensePosting.GetDistinctServiceOrderExpensePostingDates = function (userId) {
			// @ts-ignore
			return applyExpenseEntryFilter(origGetDistinctServiceOrderExpensePostingDates.call(this), userId);
		};
	}
	function configureWebSqlGetTimePostingDistinctDates() {
		if (!window.database.CrmService_ServiceOrderTimePosting) {
			return;
		}
		if (window.database.CrmService_ServiceOrderTimePosting.GetDistinctServiceOrderTimePostingDates) {
			throw "CrmService_ServiceOrderTimePosting.GetDistinctServiceOrderTimePostingDates must be undefined at this point";
		}
		window.database.CrmService_ServiceOrderTimePosting.GetDistinctServiceOrderTimePostingDates = function(userId) {
			return applyTimeEntryFilter(window.database.CrmService_ServiceOrderTimePosting, userId)
				.map(function(it) { return it.From; })
				.distinct();
		};
	}

	function configureWebSqlGetExpensePostingDistinctDates() {
		if (!window.database.CrmService_ServiceOrderExpensePosting) {
			return;
		}
		if (window.database.CrmService_ServiceOrderExpensePosting.GetDistinctServiceOrderExpensePostingDates) {
			throw "CrmService_ServiceOrderExpensePosting.GetDistinctServiceOrderExpensePostingDates must be undefined at this point";
		}
		window.database.CrmService_ServiceOrderExpensePosting.GetDistinctServiceOrderExpensePostingDates = function (userId) {
			return applyExpenseEntryFilter(window.database.CrmService_ServiceOrderExpensePosting, userId)
				.map(function (it) { return it.Date; })
				.distinct();
		};
	}

	document.addEventListener("DatabaseInitialized", function() {
		if (window.database.storageProvider.name === "webSql") {
			configureWebSqlGetTimePostingDistinctDates();
			configureWebSqlGetExpensePostingDistinctDates();
		} else if (window.database.storageProvider.name === "oData") {
			configureOdataGetTimePostingDistinctDates();
			configureOdataGetExpensePostingDistinctDates();
		}
	});
}
setupService();

export class HelperService {
	static getLumpSumString(obj: Crm.Service.Rest.Model.CrmService_ServiceOrderTime | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime | Crm.Service.Rest.Model.CrmService_ServiceOrderHead | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead): string {
		return [
			ko.unwrap(obj.IsCostLumpSum) ? "Cost" : null,
			ko.unwrap(obj.IsMaterialLumpSum) ? "Material" : null,
			ko.unwrap(obj.IsTimeLumpSum) ? "Time" : null,
		].filter(Boolean)
			.map(function(x) { return window.Helper.String.getTranslatedString(x); })
			.join(", ");
	}
	static onInvoicingTypeSelected(obj: Crm.Service.Rest.Model.CrmService_ServiceOrderTime | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime | Crm.Service.Rest.Model.CrmService_ServiceOrderHead | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead, invoicingType: Main.Rest.Model.Lookups.Main_InvoicingType): void {
		var src = invoicingType ?? { Key: undefined, ExtensionValues: { IsCostLumpSum: false, IsMaterialLumpSum: false, IsTimeLumpSum: false } };
		obj = ko.unwrap(obj);
		if (obj instanceof window.database.CrmService_ServiceOrderTime.defaultType || obj instanceof window.database.CrmService_ServiceOrderHead.defaultType) {
			obj.InvoicingTypeKey = src.Key;
			obj.IsCostLumpSum = src.ExtensionValues.IsCostLumpSum;
			obj.IsMaterialLumpSum = src.ExtensionValues.IsMaterialLumpSum;
			obj.IsTimeLumpSum = src.ExtensionValues.IsTimeLumpSum;
		} else {
			obj.InvoicingTypeKey(src.Key);
			obj.IsCostLumpSum(src.ExtensionValues.IsCostLumpSum);
			obj.IsMaterialLumpSum(src.ExtensionValues.IsMaterialLumpSum);
			obj.IsTimeLumpSum(src.ExtensionValues.IsTimeLumpSum);
		}
	}
	static async resetInvoicingType(entity: Crm.Service.Rest.Model.CrmService_ServiceOrderTime | Crm.Service.Rest.Model.CrmService_ServiceOrderHead): Promise<void> {
		if (entity instanceof window.database.CrmService_ServiceOrderTime.defaultType) {
			const order = await window.database.CrmService_ServiceOrderHead.find(entity.OrderId);
			if (!order.InvoicingTypeKey) {
				return;
			}
			for (const key of ["InvoicingTypeKey", "IsCostLumpSum", "IsMaterialLumpSum", "IsTimeLumpSum"]) {
				if (entity[key] !== order[key]) {
					entity[key] = order[key];
				}
			}
		}
	}
	static async resetPositions(entity: Crm.Service.Rest.Model.CrmService_ServiceOrderTime | Crm.Service.Rest.Model.CrmService_ServiceOrderHead): Promise<void> {
		const changedProperties = (entity.changedProperties || []).reduce((map, p) => { map[p.name] = true; return map; }, {});
		if (entity.entityState === $data.EntityState.Modified
			&& entity instanceof window.database.CrmService_ServiceOrderHead.defaultType
			&& ["InvoicingTypeKey", "IsCostLumpSum", "IsMaterialLumpSum", "IsTimeLumpSum"].some(x => changedProperties[x])) {
			const queries = [];
			let invoicingType = null;
			if (entity.InvoicingTypeKey) {
				queries.push({
					queryable: window.Helper.Lookup.getLookupByKeyQuery("Main_InvoicingType", entity.InvoicingTypeKey),
					method: "first",
					handler: x => invoicingType = x
				});
			}
			queries.push({
				queryable: window.database.CrmService_ServiceOrderTime.filter("OrderId", "===", entity.Id),
				method: "toArray",
				handler: times => {
					for (const x of times) {
						window.database.attachOrGet(x);
						x.InvoicingTypeKey = invoicingType ? invoicingType.Key : null;
						x.IsCostLumpSum = invoicingType ? invoicingType.ExtensionValues.IsCostLumpSum : false;
						x.IsMaterialLumpSum = invoicingType ? invoicingType.ExtensionValues.IsMaterialLumpSum : false;
						x.IsTimeLumpSum = invoicingType ? invoicingType.ExtensionValues.IsTimeLumpSum : false;
					}
				}
			});
			await window.Helper.Batch.Execute(queries);
		}
	}
	static async resetInvoicingIfLumpSumSettingsChanged(obj: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead): Promise<void> {
		const entity = ko.unwrap(obj).innerInstance;
		await Helper.Service.resetInvoicingType(entity);
		await Helper.Service.resetPositions(entity);
	}
	static getErrorCodeAutocompleteOptions() {
		const options = window.Helper.Lookup.getAutocompleteOptions("CrmService_ErrorCode");
		options.mapDisplayObject = function (lookup) {
			return {
				id: lookup.Key,
				item: lookup,
				text: lookup.Key + " - " + lookup.Value
			};
		};
		options.customFilter = function (query, term) {
			if (!!term) {
				return window.Helper.Lookup.getLocalizedQuery("CrmService_ErrorCode", null, 'it.Key.toLowerCase().contains(this.term)||it.Value.toLowerCase().contains(this.term)', { term: term })
			}
			return window.Helper.Lookup.getLocalizedQuery("CrmService_ErrorCode");
		}
		return options;
	}
}