
export class HelperServiceCase {
	static async addServiceCasesToServiceOrder(serviceCases: Crm.Service.Rest.Model.CrmService_ServiceCase[] | Crm.Service.Rest.Model.ObservableCrmService_ServiceCase[], serviceOrderId: string, serviceOrderTimeId: string, maxPos: number): Promise<Crm.Service.Rest.Model.CrmService_ServiceOrderTime[]> {
		const results = [];
		serviceCases.map(function (serviceCase) {
			return window.Helper.Database.getDatabaseEntity<Crm.Service.Rest.Model.CrmService_ServiceCase>(serviceCase);
		}).forEach(function (serviceCase) {
			if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation") {
				if (serviceOrderTimeId) {
					window.database.attachOrGet(serviceCase);
					serviceCase.ServiceOrderTimeId = serviceOrderTimeId;
					serviceCase.ServiceOrderErrorTypes.forEach(function (errorType) {
						window.database.attachOrGet(errorType);
						errorType.OrderId = serviceOrderId;
						errorType.ServiceOrderTimeId = serviceOrderTimeId;
					})
				} else {
					const newServiceOrderTime =
						window.database.CrmService_ServiceOrderTime.defaultType.create();
					newServiceOrderTime.DiscountType = Crm.Article.Model.Enums.DiscountType.Absolute;
					if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode ===
						"JobPerInstallation") {
						newServiceOrderTime.InstallationId = serviceCase.AffectedInstallationKey;
					}
					newServiceOrderTime.OrderId = serviceOrderId;
					newServiceOrderTime.Description = serviceCase.ServiceCaseNo;
					newServiceOrderTime.PosNo = window.Helper.ServiceOrder.formatPosNo(++maxPos);
					window.database.add(newServiceOrderTime);
					window.database.attachOrGet(serviceCase);
					serviceCase.ServiceOrderTimeId = newServiceOrderTime.Id;
					serviceCase.ServiceOrderErrorTypes.forEach(function (errorType) {
						window.database.attachOrGet(errorType);
						errorType.OrderId = serviceOrderId;
						errorType.ServiceOrderTimeId = newServiceOrderTime.Id;
					})
					results.push(newServiceOrderTime);
				}
			}
			serviceCase.StatusKey = Helper.ServiceCase.defaults.inProgressStatusKey;
		});
		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "OrderPerInstallation" && serviceCases.length === 1) {
			const serviceOrders = await window.database.CrmService_ServiceOrderHead
				.filter("it.Id === this.id", { id: serviceOrderId })
				.toArray();
			if (serviceOrders.length === 1) {
				const serviceOrder = serviceOrders[0];
				window.database.attachOrGet(serviceOrder);
				serviceOrder.ServiceCaseKey = ko.unwrap(serviceCases[0].Id);
			}
		}
		await window.database.saveChanges();
		return results;
	}

	static belongsToClosed(serviceCaseStatus: Crm.Service.Rest.Model.Lookups.CrmService_ServiceCaseStatus): boolean {
		return (serviceCaseStatus.Groups || "").split(",").indexOf("Closed") !== -1;
	}

	static defaults = {
		inProgressStatusKey: 4
	}

	static getCategoryAbbreviation(serviceCase: Crm.Service.Rest.Model.CrmService_ServiceCase | Crm.Service.Rest.Model.ObservableCrmService_ServiceCase, serviceCaseCategories: {[key:string]: Crm.Service.Rest.Model.Lookups.CrmService_ServiceCaseCategory}): string {
		if (!serviceCase){
			return "";
		}
		serviceCase = ko.unwrap(serviceCase);
		const serviceCaseCategoryKey = ko.unwrap(serviceCase.CategoryKey);
		if (serviceCaseCategoryKey) {
			const serviceCaseCategory = (serviceCaseCategories || {})[serviceCaseCategoryKey];
			if (serviceCaseCategory && serviceCaseCategory.Value) {
				return serviceCaseCategory.Value[0];
			}
		}
		return "";
	}

	static getDisplayName(serviceCase: Crm.Service.Rest.Model.CrmService_ServiceCase | Crm.Service.Rest.Model.ObservableCrmService_ServiceCase): string {
		return [ko.unwrap(serviceCase.ServiceCaseNo), ko.unwrap(serviceCase.ErrorMessage)].filter(Boolean).join(" - ");
	}

	static mapForSelect2Display(serviceCase: Crm.Service.Rest.Model.CrmService_ServiceCase): Select2AutoCompleterResult {
		return {
			id: serviceCase.Id,
			item: serviceCase,
			text: Helper.ServiceCase.getDisplayName(serviceCase)
		};
	}

	static setStatus(serviceCases: Crm.Service.Rest.Model.CrmService_ServiceCase | Crm.Service.Rest.Model.ObservableCrmService_ServiceCase | (Crm.Service.Rest.Model.CrmService_ServiceCase | Crm.Service.Rest.Model.ObservableCrmService_ServiceCase)[], status: Crm.Service.Rest.Model.Lookups.CrmService_ServiceCaseStatus): void {
		serviceCases = Array.isArray(serviceCases) ? serviceCases : [serviceCases];
		serviceCases.map(function (serviceCase) {
			return window.Helper.Database.getDatabaseEntity<Crm.Service.Rest.Model.CrmService_ServiceCase>(serviceCase);
		}).forEach(function (serviceCase) {
			serviceCase.StatusKey = status.Key;
			const belongsToClosed = window.Helper.ServiceCase.belongsToClosed(status);
			if (belongsToClosed) {
				serviceCase.CompletionDate = new Date();
				serviceCase.CompletionServiceOrderId = serviceCase.ServiceOrderTime ? serviceCase.ServiceOrderTime.OrderId : null;
				serviceCase.CompletionUser = window.Helper.User.getCurrentUserName();
			} else {
				serviceCase.CompletionDate = null;
				serviceCase.CompletionServiceOrderId = null;
				serviceCase.CompletionUser = null;
			}
		});
	}

	static getOpenStatuses(): Promise<string[]> {
		return window.database.CrmService_ServiceCaseStatus
			.filter(function (x) { return x.Key !== null && x.Groups !== "Closed" })
			.map(function (x) { return x.Key })
			.toArray();
	}

	static async getCountWidgetOptions() {
		const openStatuses = await window.Helper.ServiceCase.getOpenStatuses();
		const caption = window.Helper.String.getTranslatedString("Priority");
		const urlTemplate = function (priorityKey: any) {
			const operator = priorityKey !== null ? "in" : "==";
			const value = priorityKey !== null ? [ priorityKey] : "null";
			return `#/Crm.Service/ServiceCaseList/IndexTemplate?&filters={"PriorityKey":{"Value":${encodeURIComponent(JSON.stringify(value))},"Operator":${encodeURIComponent(JSON.stringify(operator))},"caption":"${caption}: {1}"}}&bookmark={"Key":"OpenServiceCases"}`;
		};
		return {
			query: window.database.CrmService_ServiceCase.filter(function(x) { return x.StatusKey in this.statusKeys; },
				{ statusKeys: openStatuses }),
			groupBy: 'PriorityKey',
			lookupTable: 'CrmService_ServicePriority',
			urlTemplate: urlTemplate
		};
	}

	static getServiceCasesWithoutAssignmentQuery(query = null, openStatuses): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase> {
		return window.database.CrmService_ServiceCase
			.filter(function(x) { return x.ResponsibleUser == null && x.StatusKey in this.statusKeys; }, { statusKeys: openStatuses })
	}

	static getServiceCasesWithoutAssignment(): Promise<$data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>> {
		return window.Helper.ServiceCase.getOpenStatuses()
			.then(function (openStatuses) {
				return window.Helper.ServiceCase.getServiceCasesWithoutAssignmentQuery(null, openStatuses);
			});
	}
	static getServiceCasesWithoutAssignmentCount(): Promise<number> {
		return window.Helper.ServiceCase.getServiceCasesWithoutAssignment().then((query) => query.count());
	}

	static getServiceCasesWithoutActivityQuery(query = null, openStatuses): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase> {
		const date = window.moment().add("-3", "days").toDate();
		const notesFilter = window.database.storageProvider.name === "webSql" ?
			"it.Notes.CreateDate === null || it.Notes.CreateDate <= this.date" :
			"!it.Notes.some(function(it2){ return it2.CreateDate >= this.date })";

		if(!query) {
			query = window.database.CrmService_ServiceCase;
		}
		query = query.filter(notesFilter, {date: date});
		query = query.filter("it.ModifyDate <= this.date", {date: date});
		return query.filter(function(x) { return x.StatusKey in this.statusKeys; }, { statusKeys: openStatuses });
	}

	static getServiceCasesWithoutActivity(): Promise<$data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceCase>> {
		return window.Helper.ServiceCase.getOpenStatuses()
			.then(function (openStatuses) {
				return window.Helper.ServiceCase.getServiceCasesWithoutActivityQuery(null, openStatuses);
			});
	}

	static getServiceCasesWithoutActivityCount(): Promise<number> {
		return window.Helper.ServiceCase.getServiceCasesWithoutActivity().then((query) => query.count());
	}
}