import {namespace} from "@Main/namespace";

export type BarcodeState = {
	isVisible: KnockoutObservable<boolean>
	value: KnockoutObservable<string>
	format: KnockoutObservable<string>
	printOnly: KnockoutObservable<boolean>
}

export class ServiceOrderReportGroup {
	Installation: Crm.Service.Rest.Model.ObservableCrmService_Installation;
	ServiceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime;
	ServiceOrderMaterials: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[];
	ServiceOrderExpensePostings: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting[];
	ServiceOrderTimePostings: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting[];
	ServiceOrderErrorTypes: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType[];
}
export class ServiceOrderReportViewModel {
	customerContact = ko.observable<Crm.Rest.Model.ObservableCrm_Company>(null);
	customerContactAddress = ko.observable<Crm.Rest.Model.ObservableCrm_Address>(null);
	displayedMaterials = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial>([]);
	displayedTimePostings = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting>([]);
	displayedExpensePostings = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting>([]);
	displayedErrorTypes = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType>([]);
	serviceOrderTimes = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime>([]);
	initiator = ko.observable<Crm.Rest.Model.ObservableCrm_Company>(null);
	initiatorPerson = ko.observable<Crm.Rest.Model.ObservableCrm_Person>(null);
	initiatorAddress = ko.observable<Crm.Rest.Model.ObservableCrm_Address>(null);
	initiatorEmail = ko.observable<Crm.Rest.Model.ObservableCrm_Email>(null);
	initiatorPhone = ko.observable<Crm.Rest.Model.ObservableCrm_Phone>(null);
	initiatorMobile = ko.observable<Crm.Rest.Model.ObservableCrm_Phone>(null);
	installation = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_Installation>(null);
	installations = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_Installation>([]);
	invoiceRecipient = ko.observable<Crm.Rest.Model.ObservableCrm_Company>(null);
	loading = ko.observable<boolean>(true);
	maintenanceOrderGenerationMode = ko.observable<string>(null);
	payer = ko.observable<Crm.Rest.Model.ObservableCrm_Company>(null);
	serviceObject = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceObject>(null);
	serviceOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>(null);
	showEstimatedData = ko.observable<boolean>(true);
	showInvoiceData = ko.observable<boolean>(true);
	site = ko.observable<Main.Rest.Model.Main_Site>(null);
	attributeFormItems: Array<any> = [];
	itemNos: string[];
	lookups: LookupType = {
		articleDescriptions: {},
		countries: { $tableName: "Main_Country" },
		currencies: { $tableName: "Main_Currency" },
		expenseTypes: { $tableName: "CrmPerDiem_ExpenseType" },
		installationStatus: {$tableName: "CrmService_InstallationHeadStatus"},
		installationTypes: {$tableName: "CrmService_InstallationType"},
		invoicingTypes: {$tableName: "Main_InvoicingType"},
		manufacturers: {$tableName: "CrmService_Manufacturer"},
		noPreviousSerialNoReasons: {$tableName: "CrmService_NoPreviousSerialNoReason"},
		regions: {$tableName: "Main_Region"},
		quantityUnits: {$tableName: "CrmArticle_QuantityUnit"},
		serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"},
		timePostingUserDisplayNames: {},
		expensePostingUserDisplayNames: {},
		statisticsKeyFaultImages: { $tableName: "CrmService_StatisticsKeyFaultImage" },
		statisticsKeyCauses: { $tableName: "CrmService_StatisticsKeyCause" },
		distanceUnits: { $tableName: "CrmPerDiem_DistanceUnit" },
	}
	headerHeight = ko.observable<number>(0);
	footerHeight = ko.observable<number>(0);

	reportGroups = ko.pureComputed<ServiceOrderReportGroup[]>(() => {
		if (this.loading() === true) {
			return [];
		}
		if (this.maintenanceOrderGenerationMode() === "OrderPerInstallation") {
			return [
				{
					Installation: this.installation(),
					ServiceOrderTime: null,
					ServiceOrderMaterials: this.displayedMaterials(),
					ServiceOrderExpensePostings: this.displayedExpensePostings(),
					ServiceOrderTimePostings: this.displayedTimePostings(),
					ServiceOrderErrorTypes: this.displayedErrorTypes(),
				}
			];
		}
		const groupsArray: ServiceOrderReportGroup[] = [];
		const groups: { [key: string]: ServiceOrderReportGroup } = this.serviceOrderTimes().reduce((acc, serviceOrderTime) => {
			acc[serviceOrderTime.Id()] = {
				Installation: serviceOrderTime.InstallationId() ? this.installations().find(x => x.Id() === serviceOrderTime.InstallationId()) : null,
				ServiceOrderTime: serviceOrderTime,
				ServiceOrderMaterials: [],
				ServiceOrderExpensePostings: [],
				ServiceOrderTimePostings: [],
				ServiceOrderErrorTypes: []
			};
			return acc;
		}, {});
		const positions: (Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType)[] = [].concat(this.displayedMaterials()).concat(this.displayedTimePostings()).concat(this.displayedExpensePostings()).concat(this.displayedErrorTypes());
		if (Object.keys(groups).length === 0 || positions.some(x => x.ServiceOrderTimeId() === null)) {
			groups[""] = {
				Installation: null,
				ServiceOrderTime: null,
				ServiceOrderMaterials: [],
				ServiceOrderExpensePostings: [],
				ServiceOrderTimePostings: [],
				ServiceOrderErrorTypes: []

			};
		}
		this.displayedMaterials().forEach(function (position) {
			groups[position.ServiceOrderTimeId() || ""].ServiceOrderMaterials.push(position);
		});

		this.displayedExpensePostings().forEach(function (position) {
			groups[position.ServiceOrderTimeId() || ""].ServiceOrderExpensePostings.push(position);
		});
		this.displayedTimePostings().forEach(function (position) {
			groups[position.ServiceOrderTimeId() || ""].ServiceOrderTimePostings.push(position);
		});
		this.displayedErrorTypes().forEach(function (position) {
			groups[position.ServiceOrderTimeId() || ""].ServiceOrderErrorTypes.push(position);
		});
		Object.keys(groups).forEach(function (position) {
			groupsArray.push(groups[position]);
		});
		return groupsArray;
	});

	sortedReportGroups = ko.pureComputed<ServiceOrderReportGroup[]>(() => {
		this.reportGroups().forEach(function (group) {
			group.ServiceOrderMaterials.sort(function (left, right) {
				return parseInt(left.PosNo() || "0") - parseInt(right.PosNo() || "0");
			});
			group.ServiceOrderTimePostings.sort(function (left, right) {
				if (left.From() > right.From()) {
					return 1;
				}
				if (left.From() < right.From()) {
					return -1;
				}
				if (left.ItemNo() > right.ItemNo()) {
					return 1;
				}
				if (left.ItemNo() < right.ItemNo()) {
					return -1;
				}
				return 0;
			});
		});
		return this.reportGroups().sort(function (left, right) {
			const leftPosNo = left.ServiceOrderTime ? parseInt(left.ServiceOrderTime.PosNo()) : 0;
			const rightPosNo = right.ServiceOrderTime ? parseInt(right.ServiceOrderTime.PosNo()) : 0;
			return leftPosNo - rightPosNo;
		});
	});
	barcode: BarcodeState

	filterPositions(group: ServiceOrderReportGroup, types: string | string[]): Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting[] | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting[] | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial[] | Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderErrorType[]   {
		const obj = group.ServiceOrderTime ?? this.serviceOrder();
		const ls = {
			IsCostLumpSum: ko.unwrap(obj.IsCostLumpSum),
			IsMaterialLumpSum: ko.unwrap(obj.IsMaterialLumpSum),
			IsTimeLumpSum: ko.unwrap(obj.IsTimeLumpSum),
		}
		types = Array.isArray(types) ? types : [types];
		if (types.length === 1 && types[0] === "Time") {
			if (ls.IsTimeLumpSum) {
				return [];
			}
			return group.ServiceOrderTimePostings;
		}
		if (types.length === 1 && types[0] === "Expense") {
			if (ls.IsCostLumpSum) {
				return [];
			}
			return group.ServiceOrderExpensePostings;
		}
		if (types.includes("Cost") || types.includes("Material")) {
			return group.ServiceOrderMaterials.filter(x => {
				if (ko.unwrap(x.ArticleTypeKey) === "Cost" && ls.IsCostLumpSum || ko.unwrap(x.ArticleTypeKey) === "Material" && ls.IsMaterialLumpSum) {
					return false;
				}
				return true;
			})
		}
		if (types.length === 1 && types[0] === "ErrorType") {
			return group.ServiceOrderErrorTypes;
		}
		return [];
	};

	getActualDuration(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		return window.Helper.ServiceOrderTimePosting.getActualDuration(serviceOrderTimePosting);
	}

	getEstimatedDuration(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		return window.Helper.ServiceOrderTimePosting.getEstimatedDuration(serviceOrderTimePosting);
	}

	getInvoiceDuration(serviceOrderTimePosting: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting): string {
		return window.Helper.ServiceOrderTimePosting.getInvoiceDuration(serviceOrderTimePosting);
	}

	getActualQuantity(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return window.Helper.ServiceOrderMaterial.getActualQuantity(serviceOrderMaterial);
	}

	getEstimatedQuantity(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return window.Helper.ServiceOrderMaterial.getEstimatedQuantity(serviceOrderMaterial);
	}

	getInvoiceQuantity(serviceOrderMaterial: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderMaterial): number {
		return window.Helper.ServiceOrderMaterial.getInvoiceQuantity(serviceOrderMaterial);
	}
	
	getLumpSumPositionTypes(group: ServiceOrderReportGroup): { Time: boolean, Cost: boolean, Material: boolean, Expense: boolean, ErrorType: boolean } {
		const obj = group.ServiceOrderTime ?? this.serviceOrder();
		const ls = {
			IsCostLumpSum: ko.unwrap(obj.IsCostLumpSum),
			IsMaterialLumpSum: ko.unwrap(obj.IsMaterialLumpSum),
			IsTimeLumpSum: ko.unwrap(obj.IsTimeLumpSum),
			IsExpenseLumpSum: ko.unwrap(obj.IsCostLumpSum),
		}
		return {
			Time: ls.IsTimeLumpSum && group.ServiceOrderTimePostings.length > 0,
			Cost: ls.IsCostLumpSum && group.ServiceOrderMaterials.some(x => ko.unwrap(x.ArticleTypeKey) === "Cost"),
			Material: ls.IsMaterialLumpSum && group.ServiceOrderMaterials.some(x => ko.unwrap(x.ArticleTypeKey) === "Material"),
			Expense: ls.IsCostLumpSum && group.ServiceOrderExpensePostings.length > 0,
			ErrorType: group.ServiceOrderErrorTypes.length > 0

		};
	};

	showMaterialsTable(group: ServiceOrderReportGroup) {
		const lumpSumTypes = this.getLumpSumPositionTypes(group);
		return group.ServiceOrderMaterials.length > 0 || lumpSumTypes.Cost || lumpSumTypes.Material || !window.Crm.Service.Settings.Dispatch.SuppressEmptyMaterialsInReport;
	};

	showTimePostingsTable(group: ServiceOrderReportGroup) {
		const lumpSumTypes = this.getLumpSumPositionTypes(group);
		return group.ServiceOrderTimePostings.length > 0 || lumpSumTypes.Time || !window.Crm.Service.Settings.Dispatch.SuppressEmptyTimePostingsInReport;
	};

	showExpensePostingsTable(group: ServiceOrderReportGroup) {
		const lumpSumTypes = this.getLumpSumPositionTypes(group);
		return group.ServiceOrderExpensePostings.length > 0 || lumpSumTypes.Expense || !window.Crm.Service.Settings.Dispatch.SuppressEmptyExpensePostingsInReport;
	};

	showErrorTypesTable(group: ServiceOrderReportGroup) {
		const lumpSumTypes = this.getLumpSumPositionTypes(group);
		return group.ServiceOrderErrorTypes.length > 0 || lumpSumTypes.ErrorType || !window.Crm.Service.Settings.Dispatch.SuppressEmptyErrorTypesInReport;
	};

	showGroupHeader(group: ServiceOrderReportGroup) {
		const jobPerInstallationMode = this.maintenanceOrderGenerationMode() === 'JobPerInstallation';
		return this.showMaterialsTable(group) || this.showTimePostingsTable(group) || this.showErrorTypesTable(group) || this.showExpensePostingsTable(group) || (jobPerInstallationMode && !window.Crm.Service.Settings.Dispatch.SuppressEmptyJobsInReport);
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await window.Helper.Database.initialize();
		await window.Crm.Offline.Bootstrapper.initializeSettings();
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		let site = await window.database.Main_Site.GetCurrentSite().first();
		this.site(site);
		this.maintenanceOrderGenerationMode(window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode);
		let headerHeight = +window.Main.Settings.Report.HeaderHeight + +window.Main.Settings.Report.HeaderSpacing;
		this.headerHeight(headerHeight);
		let footerHeight = +window.Main.Settings.Report.FooterHeight + +window.Main.Settings.Report.FooterSpacing;
		this.footerHeight(footerHeight);

		if (id) {
			let serviceOrder = await window.database.CrmService_ServiceOrderHead
				.include("Company")
				.include2("Company.Addresses.filter(function(a) { return a.IsCompanyStandardAddress === true; })")
				.include("Initiator")
				.include2("Initiator.Addresses.filter(function(a) { return a.IsCompanyStandardAddress === true; })")
				.include("Initiator.Emails")
				.include("Initiator.Phones")
				.include("InitiatorPerson")
				.include("InitiatorPerson.Emails")
				.include("InitiatorPerson.Phones")
				.include("Installation")
				.include("Installation.Company")
				.include2("Installation.Company.Addresses.filter(function(x) { x.IsCompanyStandardAddress === true; })")
				.include("InvoiceRecipient")
				.include("Payer")
				.include("ServiceObject")
				.include("ServiceOrderMaterials")
				.include("ServiceOrderExpensePostings")
				.include("ServiceOrderExpensePostings.ResponsibleUserObject")
				.include("ServiceOrderTimePostings")
				.include("ServiceOrderTimePostings.User")
				.include("ServiceOrderTimes")
				.include("ServiceOrderTimes.Installation")
				.include2("ServiceOrderErrorTypes.filter(function(x) { return x.DispatchId === null; })")
				.include("ServiceOrderErrorTypes.ServiceOrderErrorCauses")
				.find(id);
			this.serviceOrder(serviceOrder.asKoObservable());
		}

		this.barcode = {
			isVisible: ko.observable(site.ExtensionValues.ReportBarcodeShowOnServiceReport),
			value: ko.observable(window.Helper.Barcode.getValue(site.asKoObservable(), ko.toJS(this.serviceOrder))),
			format: ko.observable(site.ExtensionValues.ReportBarcodeFormat),
			printOnly: ko.observable(site.ExtensionValues.ReportBarcodePrintOnly),
		}

		if (this.serviceOrder() && this.serviceOrder().Installation && this.serviceOrder().Installation()) {
			this.installation(this.serviceOrder().Installation());
		}
		if (this.serviceOrder() && this.serviceOrder().ServiceOrderMaterials && this.serviceOrder().ServiceOrderMaterials()) {
			for (const serviceOrderMaterial of this.serviceOrder().ServiceOrderMaterials()) {
				serviceOrderMaterial.ChildServiceOrderMaterials(this.serviceOrder().ServiceOrderMaterials().filter(x => x.ParentServiceOrderMaterialId() === serviceOrderMaterial.Id()));
				if (serviceOrderMaterial.ParentServiceOrderMaterialId() !== null) {
					serviceOrderMaterial.ParentServiceOrderMaterial(this.serviceOrder().ServiceOrderMaterials().find(x => x.Id() === serviceOrderMaterial.ParentServiceOrderMaterialId()) ?? null);
				}
			}
			this.displayedMaterials(this.serviceOrder().ServiceOrderMaterials().filter(material => {
				return material.ServiceOrderMaterialType() === window.Crm.Service.ServiceOrderMaterialType.Preplanned || material.ParentServiceOrderMaterialId() === null;
			}));
		}
		if (this.serviceOrder() && this.serviceOrder().ServiceOrderTimePostings && this.serviceOrder().ServiceOrderTimePostings()) {
			for (const serviceOrderTimePosting of this.serviceOrder().ServiceOrderTimePostings()) {
				serviceOrderTimePosting.ChildServiceOrderTimePostings(this.serviceOrder().ServiceOrderTimePostings().filter(x => x.ParentServiceOrderTimePostingId() === serviceOrderTimePosting.Id()));
				if (serviceOrderTimePosting.ParentServiceOrderTimePostingId() !== null) {
					serviceOrderTimePosting.ParentServiceOrderTimePosting(this.serviceOrder().ServiceOrderTimePostings().find(x => x.Id() === serviceOrderTimePosting.ParentServiceOrderTimePostingId()) ?? null);
				}
			}
			this.displayedTimePostings(this.serviceOrder().ServiceOrderTimePostings().filter(timePosting => {
				return timePosting.ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned || timePosting.ParentServiceOrderTimePostingId() === null;
			}));
		}
		if (this.serviceOrder() && this.serviceOrder().ServiceOrderExpensePostings && this.serviceOrder().ServiceOrderExpensePostings()) {
			this.displayedExpensePostings(this.serviceOrder().ServiceOrderExpensePostings());
		}
		if (this.serviceOrder() && this.serviceOrder().ServiceOrderErrorTypes && this.serviceOrder().ServiceOrderErrorTypes()) {
			this.displayedErrorTypes(this.serviceOrder().ServiceOrderErrorTypes().sort((a, b) => {
				if (a.IsMainErrorType() && !b.IsMainErrorType()) {
					return 1;
				} else if (b.IsMainErrorType() && !a.IsMainErrorType()) {
					return -1;
				} else {
					return 0;
				}
			}));
		}
		if (this.serviceOrder() && this.serviceOrder().Initiator && this.serviceOrder().Initiator()) {
			this.initiator(this.serviceOrder().Initiator());
		}
		if (this.serviceOrder() && this.serviceOrder().InitiatorPerson && this.serviceOrder().InitiatorPerson()) {
			this.initiatorPerson(this.serviceOrder().InitiatorPerson());
		}
		if (this.serviceOrder() && this.serviceOrder().Company) {
			this.customerContact(this.serviceOrder().Company());
		}
		if (this.customerContact() && this.customerContact().Addresses && this.customerContact().Addresses() && this.customerContact().Addresses().length > 0) {
			this.customerContactAddress(this.customerContact().Addresses()[0]);
		}
		if (this.initiator() && this.initiator().Addresses && this.initiator().Addresses() && this.initiator().Addresses().length > 0) {
			this.initiatorAddress(this.initiator().Addresses()[0]);
		}
		if (this.initiator() && this.initiator().Phones && this.initiator().Phones() && this.initiator().Phones().length > 0) {
			this.initiatorPhone(this.initiator().Phones().filter(function (x) {
					return x.TypeKey() !== "PhoneMobile";
				})[0] ||
				null);
			this.initiatorMobile(this.initiator().Phones().filter(function (x) {
					return x.TypeKey() === "PhoneMobile";
				})[0] ||
				null);
		}
		if (this.initiator() && this.initiator().Emails && this.initiator().Emails() && this.initiator().Emails().length > 0) {
			this.initiatorEmail(this.initiator().Emails()[0]);
		}
		if (this.initiatorPerson() && this.initiatorPerson().Phones && this.initiatorPerson().Phones() && this.initiatorPerson().Phones().length > 0) {
			this.initiatorPhone(this.initiatorPerson().Phones().filter(function (x) {
					return x.TypeKey() !== "PhoneMobile";
				})[0] ||
				this.initiatorPhone());
			this.initiatorMobile(this.initiatorPerson().Phones().filter(function (x) {
					return x.TypeKey() === "PhoneMobile";
				})[0] ||
				this.initiatorMobile());
		}
		if (this.initiatorPerson() && this.initiatorPerson().Emails && this.initiatorPerson().Emails() && this.initiatorPerson().Emails().length > 0) {
			this.initiatorEmail(this.initiatorPerson().Emails()[0]);
		}
		if (this.serviceOrder() && this.serviceOrder().InvoiceRecipient && this.serviceOrder().InvoiceRecipient()) {
			this.invoiceRecipient(this.serviceOrder().InvoiceRecipient());
		}
		if (this.serviceOrder() && this.serviceOrder().Payer && this.serviceOrder().Payer()) {
			this.payer(this.serviceOrder().Payer());
		}
		if (this.serviceOrder() && this.serviceOrder().ServiceObject && this.serviceOrder().ServiceObject()) {
			this.serviceObject(this.serviceOrder().ServiceObject());
		}
		if (this.serviceOrder() && this.serviceOrder().ServiceOrderTimes && this.serviceOrder().ServiceOrderTimes()) {
			this.serviceOrderTimes(this.serviceOrder().ServiceOrderTimes());
		}
		if (this.serviceOrderTimes() && this.installations().length === 0) {
			this.installations(this.serviceOrderTimes().filter(function (x) {
				return x.Installation && x.Installation();
			}).map(function (x) {
				return x.Installation();
			}));
		}

		this.lookups.timePostingUserDisplayNames = this.displayedTimePostings().reduce(function (map, x) {
			map[x.Username()] = window.Helper.User.getDisplayName(x.User());
			return map;
		}, {});
		this.lookups.expensePostingUserDisplayNames = this.displayedExpensePostings().reduce(function (map, x) {
			map[x.ResponsibleUser()] = window.Helper.User.getDisplayName(x.ResponsibleUserObject());
			return map;
		}, {});
		let timePostingItemNos = this.displayedTimePostings().map(function (i) {
			return ko.unwrap(i.ItemNo);
		});
		let expensePostingItemNos = this.displayedExpensePostings().map(function (i) {
			return ko.unwrap(i.ItemNo);
		});
		let materialItemNos = this.displayedMaterials().map(function (i) {
			return ko.unwrap(i.ItemNo);
		});
		this.itemNos = [];
		this.itemNos.push.apply(this.itemNos, timePostingItemNos);
		this.itemNos.push.apply(this.itemNos, expensePostingItemNos);
		this.itemNos.push.apply(this.itemNos, materialItemNos);
		let languageKey = await window.Helper.Culture.languageCulture();
		this.lookups.articleDescriptions = await Helper.Article.loadArticleDescriptionsMap(this.itemNos, languageKey);

		if (window.Helper.AttributeForms) {
			this.attributeFormItems.push(this.customerContact());
			if (this.serviceOrder().CustomerContactId() !== this.serviceOrder().InitiatorId()) {
				this.attributeFormItems.push(this.initiator())
			};
			this.attributeFormItems.push(this.initiatorPerson());
			this.attributeFormItems.push(this.serviceOrder());
			this.attributeFormItems.push(this.serviceObject());
			this.attributeFormItems.push(this.installation());
			this.attributeFormItems.push(...this.installations());
			this.attributeFormItems = this.attributeFormItems.filter(x => x);
			for (const item of this.attributeFormItems) {
				window.Helper.AttributeForms.mixinReading(item);
			}
			await this.initAttributeFormItems();
		}
	};
	async initAttributeFormItems(language?: string): Promise<void> {
		if (window.Helper.AttributeForms && this.attributeFormItems?.length) {
			for (const item of this.attributeFormItems) {
				await item.initAttributeFormItems([item], { elementFilter: x => x.ExtensionValues.DisplayOnReports, language: language });
			}
		}
	}
}

namespace("Crm.Service.ViewModels").ServiceOrderReportViewModel = ServiceOrderReportViewModel;