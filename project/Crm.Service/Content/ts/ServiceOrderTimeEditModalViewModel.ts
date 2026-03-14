import {namespace} from "@Main/namespace";
import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";

export class ServiceOrderTimeEditModalViewModel extends window.Main.ViewModels.ViewModelBase {
	chooseJob = ko.observable<boolean>(false);
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	serviceObject: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceObject>;
	serviceOrderTime = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime>(null);
	installationEditDisabled: KnockoutComputed<boolean>;
	templateJob = ko.pureComputed<boolean>(() => {
		return this.serviceOrder().IsTemplate();
	});
	isInvoicingTypeEditAllowed = ko.pureComputed<boolean>(() => {
		return !this.serviceOrder().InvoicingTypeKey() && window.AuthorizationManager.currentUserHasPermission("ServiceOrder::SetInvoicingType")
	});
	canEditEstimatedDuration = ko.observable<boolean>(false);
	canEditInvoiceDuration = ko.observable<boolean>(false);
	errors = ko.validation.group(this.serviceOrderTime, {deep: true});
	lookups: LookupType = {
		currencies: {$tableName: "Main_Currency"},
	}

	constructor(parentViewModel: ServiceOrderDetailsViewModel | DispatchDetailsViewModel) {
		super();
		this.dispatch = parentViewModel.dispatch;
		this.serviceOrder = parentViewModel.serviceOrder;
		this.serviceObject = parentViewModel.serviceObject;
		this.installationEditDisabled = ko.pureComputed<boolean>(() => {
			if (this.serviceOrderTime()) {
				let jobs = parentViewModel.tabs()["tab-jobs"]().items().filter((job) => {
					return job.Id === this.serviceOrderTime().Id();
				});
				if (jobs[0])
					return !jobs[0].isEmpty();
			}
			return false;
		});
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let serviceOrderTime: Crm.Service.Rest.Model.CrmService_ServiceOrderTime;
		if (id) {
			serviceOrderTime = await window.database.CrmService_ServiceOrderTime.find(id);
			window.database.attachOrGet(serviceOrderTime);
		} else {
			serviceOrderTime = window.database.CrmService_ServiceOrderTime.defaultType.create();
			serviceOrderTime.DiscountType = Crm.Article.Model.Enums.DiscountType.Absolute;
			serviceOrderTime.OrderId = this.serviceOrder().Id();
			serviceOrderTime.InvoicingTypeKey = this.serviceOrder().InvoicingTypeKey();
			window.database.add(serviceOrderTime);
		}
		this.serviceOrderTime(serviceOrderTime.asKoObservable());
		let result = await window.Helper.ServiceOrder.canEditEstimatedQuantities(this.serviceOrderTime().OrderId());
		this.canEditEstimatedDuration(result);
		result = await window.Helper.ServiceOrder.canEditInvoiceQuantities(this.serviceOrderTime().OrderId());
		this.canEditInvoiceDuration(result);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		if (!this.serviceOrder().CurrencyKey()) {
			this.serviceOrderTime().DiscountType(window.Crm.Article.Model.Enums.DiscountType.Percentage);
		}
	};

	getArticleSelect2Filter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article>, filter: string): $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article> {
		const language = (document.getElementById("meta.CurrentLanguage") as HTMLMetaElement).content;
		query = query.filter(function (it) {
			return it.ArticleTypeKey === "Service";
		});
		return window.Helper.Article.getArticleAutocompleteFilter(query, filter, language);
	};

	dispose(): void {
		window.database.detach(this.serviceOrderTime().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);

		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}
		await window.Helper.Service.resetInvoicingIfLumpSumSettingsChanged(this.serviceOrderTime());
		if (window.Crm.Service.Settings.PosNoGenerationMethod == "MixedMaterialAndTimes") {
			await window.Helper.ServiceOrder.updatePosNo(this.serviceOrderTime());
		} else {
			await window.Helper.ServiceOrder.updateJobPosNo(this.serviceOrderTime());
		}
		try {
			await window.database.saveChanges();
			if (this.chooseJob()){
				await window.Helper.Dispatch.toggleCurrentJob(this.dispatch, this.serviceOrderTime().Id());
			}
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};

	installationFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["LegacyId", "InstallationNo", "Description"]);
		}
		if (this.serviceOrder().ServiceObjectId()) {
			const serviceObjectCondition = "(this.serviceObjectId === null || it.FolderId === this.serviceObjectId)";
			query = query.filter(serviceObjectCondition,
				{
					serviceObjectId: this.serviceOrder().ServiceObjectId()
				});
		}
		if (window.Crm.Service.Settings.ServiceOrder.OnlyInstallationsOfReferencedCustomer) {
			const companyCondition = "(this.customerContactId === null || it.LocationContactId === this.customerContactId)";
			query = query.filter(companyCondition,
				{
					customerContactId: this.serviceOrder().CustomerContactId() || null
				});
		}
		return query;
	};

	onArticleSelect(article: Crm.Article.Rest.Model.CrmArticle_Article): void {
		if (article === null) {
			this.serviceOrderTime().ItemNo(null);
			this.serviceOrderTime().Description(null);
		} else {
			const currentItemNo = this.serviceOrderTime().ItemNo();
			const currentDescription = this.serviceOrderTime().Description();
			const articleDescription = window.Helper.Article.getArticleDescription(article);
			if (currentItemNo !== article.ItemNo || !currentDescription) {
				this.serviceOrderTime().Description(articleDescription);
			}
			this.serviceOrderTime().ItemNo(article.ItemNo);
		}
	}


	toggleDiscountType(): void {
		if (this.serviceOrderTime().DiscountType() === window.Crm.Article.Model.Enums.DiscountType.Percentage) {
			this.serviceOrderTime().DiscountType(window.Crm.Article.Model.Enums.DiscountType.Absolute);
		} else {
			this.serviceOrderTime().DiscountType(window.Crm.Article.Model.Enums.DiscountType.Percentage);
		}
	};
}

namespace("Crm.Service.ViewModels").ServiceOrderTimeEditModalViewModel = ServiceOrderTimeEditModalViewModel;