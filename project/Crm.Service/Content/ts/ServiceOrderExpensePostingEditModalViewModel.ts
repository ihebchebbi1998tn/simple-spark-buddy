import {namespace} from "@Main/namespace";
import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";
import type { DispatchDetailsViewModel } from "./DispatchDetailsViewModel";
import 'moment-duration-format';

export class ServiceOrderExpensePostingEditModalViewModel extends window.Main.ViewModels.ViewModelBase {
	isNewServiceOrderExpensePosting = ko.observable<boolean>(true)
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	initialServiceOrderTimeId = ko.observable<string>(null);
	fileResource = ko.observable<Crm.Rest.Model.Crm_FileResource>(null);
	minDate = ko.observable<Date>((!window.AuthorizationManager.isAuthorizedForAction("ServiceOrderExpensePosting", "NoMinDateLimit") && window.Crm.Service.Settings.ServiceOrderTimePosting.MaxDaysAgo) 
		? window.moment().startOf("day").utc(true).add(-parseInt(window.Crm.Service.Settings.ServiceOrderTimePosting.MaxDaysAgo), "days") 
		: false);
	maxDate = ko.observable<Date>(new Date());
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	serviceOrderTime = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime>(null);
	serviceOrderExpensePosting = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderExpensePosting>(null);
	selectedExpenseType = ko.observable<Crm.PerDiem.Rest.Model.Lookups.ObservableCrmPerDiem_ExpenseType>(null);
	parentViewModel: ServiceOrderDetailsViewModel | DispatchDetailsViewModel;
	errors = ko.validation.group(this.serviceOrderExpensePosting, { deep: true });
	validCostCenters = ko.computed(() => {
		return this.selectedExpenseType() ? this.selectedExpenseType().ValidCostCenters() : [];
	});

	lookups: LookupType = {
		installationHeadStatuses: {$tableName: "CrmService_InstallationHeadStatus"},
		serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"},
		servicePriorities: { $tableName: "CrmService_ServicePriority" },
		costCenters: { $tableName: "Main_CostCenter" },
		currencies: { $tableName: "Main_Currency" },
		expenseTypes: { $tableName: "CrmPerDiem_ExpenseType" }
	}

	constructor(parentViewModel: ServiceOrderDetailsViewModel | DispatchDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
		this.dispatch = parentViewModel && parentViewModel.dispatch || window.ko.observable(null);
		this.serviceOrder = parentViewModel && parentViewModel.serviceOrder || window.ko.observable(null);
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups)
		let user = await window.Helper.User.getCurrentUser()
		this.currentUser(user);
		let serviceOrderExpensePosting: Crm.Service.Rest.Model.CrmService_ServiceOrderExpensePosting;
		if (id) {
			serviceOrderExpensePosting = await window.database.CrmService_ServiceOrderExpensePosting
				.include("ServiceOrderTime")
				.find(id);
			this.isNewServiceOrderExpensePosting(false);
		} else {
			this.isNewServiceOrderExpensePosting(true);
			this.serviceOrderTime(this.dispatch() ? this.dispatch().CurrentServiceOrderTime() : null);
			serviceOrderExpensePosting = window.database.CrmService_ServiceOrderExpensePosting.defaultType.create();
			serviceOrderExpensePosting.ArticleId = params.articleId || (this.serviceOrderTime() ? this.serviceOrderTime().ArticleId() : null);
			serviceOrderExpensePosting.OrderId = this.serviceOrder() ? this.serviceOrder().Id() : null;

			serviceOrderExpensePosting.CostCenterKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.costCenters, serviceOrderExpensePosting.CostCenterKey);
			serviceOrderExpensePosting.CurrencyKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.currencies, serviceOrderExpensePosting.CurrencyKey);
			serviceOrderExpensePosting.Date = window.moment(params.selectedDate || new Date()).utc(true).startOf("day").toDate();
			serviceOrderExpensePosting.ExpenseTypeKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.expenseTypes, serviceOrderExpensePosting.ExpenseTypeKey);
			serviceOrderExpensePosting.ResponsibleUser = this.currentUser().Id;;
			serviceOrderExpensePosting.IsClosed = false;
			serviceOrderExpensePosting.DispatchId = this.dispatch() ? this.dispatch().Id() : null;
			serviceOrderExpensePosting.ServiceOrderTimeId = params.serviceOrderTimeId || serviceOrderExpensePosting.ServiceOrderTimeId || (this.dispatch() ? this.dispatch().CurrentServiceOrderTimeId() : null) || null;
		}


		this.serviceOrderExpensePosting(serviceOrderExpensePosting.asKoObservable());


		let fileResource;
		if (serviceOrderExpensePosting.FileResourceKey) {
			const fileResourceFromExpense = await window.database.Crm_FileResource.find(serviceOrderExpensePosting.FileResourceKey);
			fileResource = window.database.attachOrGet(fileResourceFromExpense);
		} else {
			const newFileResource = window.database.Crm_FileResource.defaultType.create();
			fileResource = window.database.add(newFileResource);
		}
		this.fileResource(fileResource.asKoObservable());
		if (id) {
			window.database.attachOrGet(serviceOrderExpensePosting);
		} else {
			window.database.add(serviceOrderExpensePosting)
		}
	};



	dispose(): void {
		window.database.detach(this.serviceOrderExpensePosting().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);
		const fileResource = window.Helper.Database.getDatabaseEntity(this.fileResource);
		if (fileResource && fileResource.ContentType && !fileResource.ContentType.startsWith('image/') && fileResource.ContentType != 'application/pdf') {
			this.loading(false);
			await window.Helper.Confirm.genericConfirm({
				text: window.Helper.String.getTranslatedString("M_SelectedFileNotImage"),
				title: fileResource.Filename,
				type: "warning",
				showCancelButton: false
			});
			return;
		}

		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}

		if (fileResource.entityState === $data.EntityState.Added) {
			if (fileResource.Content) {
				this.serviceOrderExpensePosting().FileResourceKey(fileResource.Id);
			} else {
				window.database.detach(fileResource);
			}
		} else if (fileResource.entityState === $data.EntityState.Modified) {
			if (fileResource.Content) {
				const newFileResource = window.database.Crm_FileResource.defaultType.create();
				window.database.add(newFileResource);
				newFileResource.Content = fileResource.Content;
				newFileResource.Length = fileResource.Length;
				newFileResource.ContentType = fileResource.ContentType;
				newFileResource.Filename = fileResource.Filename;
				this.serviceOrderExpensePosting().FileResourceKey(newFileResource.Id);
				window.database.remove(fileResource);

				const file = window.database.stateManager.trackedEntities[2];
				window.database.stateManager.trackedEntities[2] = window.database.stateManager.trackedEntities[1];
				window.database.stateManager.trackedEntities[1] = file;
			} else {
				this.serviceOrderExpensePosting().FileResourceKey(null);
				window.database.remove(fileResource);
			}
		}

		try {
			await window.database.saveChanges();
			this.loading(false);
			if (this.isNewServiceOrderExpensePosting()) {
				this.showSnackbar(window.Helper.String.getTranslatedString("Added"));
			}
			else {
				this.showSnackbar(window.Helper.String.getTranslatedString("Modified"));
			}
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};

	isJobEditable(): boolean {
		return this.dispatch() == null || !Crm.Service.Settings.Dispatch.RestrictEditingToActiveJob;
	}

	getArticleSelect2Filter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article>, term: string): $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article> {

		query = query.filter(function (it) {
			return it.ArticleTypeKey === "Cost" && it.ExtensionValues.IsHidden === false && it.ExtensionValues.IsServiceOrderExpense;
		})
		return window.Helper.Article.getArticleAutocompleteFilter(query, term, this.currentUser().DefaultLanguageKey);
	};

	getServiceOrderTimeAutocompleteDisplay(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): string {
		return window.Helper.ServiceOrderTime.getAutocompleteDisplay(serviceOrderTime,
			this.dispatch() ? this.dispatch().CurrentServiceOrderTimeId() : null);
	};

	getServiceOrderTimeAutocompleteFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		query = query.filter(function (it) {
				return it.OrderId === this.orderId;
			},
			{ orderId: this.serviceOrderExpensePosting().OrderId()});

		if (term) {
			query = window.Helper.String.contains(query, term, ["Description.toLowerCase()", "ItemNo.toLowerCase()", "PosNo.toLowerCase()"]);
		}
		return query;
	};

	onArticleSelect(article: Crm.Article.Rest.Model.CrmArticle_Article): void {
		if (article) {
			this.serviceOrderExpensePosting().ArticleId(article.Id);
			this.serviceOrderExpensePosting().ItemNo(article.ItemNo);
		} else {
			this.serviceOrderExpensePosting().ArticleId(null);
			this.serviceOrderExpensePosting().ItemNo(null);
		}
	};

	onSelectExpenseType(expenseType: Crm.PerDiem.Rest.Model.Lookups.CrmPerDiem_ExpenseType) {
		this.selectedExpenseType(expenseType !== null ? expenseType.asKoObservable() : null);
		if (expenseType && expenseType.Key !== this.serviceOrderExpensePosting().ExpenseTypeKey()) {
			this.serviceOrderExpensePosting().CostCenterKey(null);
		}
	}

	onJobSelected(serviceOrderTime: Crm.Service.Rest.Model.CrmService_ServiceOrderTime): void {
		if (serviceOrderTime === null) {
			this.serviceOrderTime(null);
			this.serviceOrderExpensePosting().ServiceOrderTime(null);
			this.serviceOrderExpensePosting().ServiceOrderTimeId(null);
		} else {
			this.serviceOrderTime(serviceOrderTime.asKoObservable());
			if (this.serviceOrderExpensePosting().ArticleId() === null) {
				this.serviceOrderExpensePosting().ArticleId(this.serviceOrderTime().ArticleId());
			}
		}
	};


}

namespace("Crm.Service.ViewModels").ServiceOrderExpensePostingEditModalViewModel = ServiceOrderExpensePostingEditModalViewModel;