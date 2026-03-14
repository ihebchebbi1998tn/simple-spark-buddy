import {namespace} from "@Main/namespace";
import type {ReplenishmentOrderItemListIndexViewModel} from "./ReplenishmentOrderItemListIndexViewModel";
import {HelperQuantityUnit} from "../../../Crm.Article/Content/ts/helper/Helper.QuantityUnit";

export class ReplenishmentOrderItemEditModalViewModel extends window.Main.ViewModels.ViewModelBase {
	articleAutocomplete = ko.observable<string>("");
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	lookups: LookupType;
	replenishmentOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ReplenishmentOrder>(null);
	replenishmentOrderItem = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ReplenishmentOrderItem>(null);
	quantityUnit = ko.pureComputed<Crm.Article.Rest.Model.Lookups.CrmArticle_QuantityUnit>(() => {
		return this.lookups.quantityUnits.$array.find(x => x.Key === this.replenishmentOrderItem().QuantityUnitKey());
	});
	quantityUnitGroupKey = ko.pureComputed<string>(() => {
		return this.replenishmentOrderItem().Article() ? this.replenishmentOrderItem().Article().QuantityUnitEntryKey() : null;
	});
	errors = ko.validation.group(this.replenishmentOrderItem, {deep: true});


	constructor(parentViewModel: ReplenishmentOrderItemListIndexViewModel) {
		super();
		this.lookups = parentViewModel.lookups || {};
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		const replenishmentOrderId = params.replenishmentOrderId;
		let user = await window.Helper.User.getCurrentUser()
		this.currentUser(user);
		let replenishmentOrder = await window.database.CrmService_ReplenishmentOrder
			.include("ResponsibleUserObject")
			.find(replenishmentOrderId);
		this.replenishmentOrder(replenishmentOrder.asKoObservable());
		let replenishmentOrderItem: Crm.Service.Rest.Model.CrmService_ReplenishmentOrderItem;
		if (id) {
			replenishmentOrderItem = await window.database.CrmService_ReplenishmentOrderItem
				.include("Article")
				.include("QuantityUnitEntry")
				.include("ServiceOrderMaterials")
				.find(id);
			window.database.attachOrGet(replenishmentOrderItem);
		} else {
			replenishmentOrderItem = window.database.CrmService_ReplenishmentOrderItem.defaultType.create();
			replenishmentOrderItem.Quantity = 1;
			replenishmentOrderItem.ReplenishmentOrderId = this.replenishmentOrder().Id();
			window.database.add(replenishmentOrderItem);
		}
		this.replenishmentOrderItem(replenishmentOrderItem.asKoObservable());
		this.lookups.quantityUnits = this.lookups.quantityUnits || (await window.Helper.Lookup.getLocalizedArrayMap("CrmArticle_QuantityUnit"));
	};

	dispose(): void {
		window.database.detach(this.replenishmentOrder().innerInstance);
		window.database.detach(this.replenishmentOrderItem().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);

		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}

		try {
			await window.database.saveChanges();
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};

	getArticleSelect2Filter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article>, term: string): $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article> {
		query = query.filter("it.ArticleTypeKey === 'Material' && it.IsWarehouseManaged === true");
		return window.Helper.Article.getArticleAutocompleteFilter(query, term, this.currentUser().DefaultLanguageKey);
	};

	onArticleSelect(article: Crm.Article.Rest.Model.CrmArticle_Article): void {
		if (article) {
			if(this.replenishmentOrderItem().Article() == null || article.Id !== this.replenishmentOrderItem().Article().Id()) {
				this.replenishmentOrderItem().Article(article.asKoObservable());
				this.replenishmentOrderItem().Description(window.Helper.Article.getArticleDescription(article));
				this.replenishmentOrderItem().MaterialNo(article.ItemNo);
				this.replenishmentOrderItem().QuantityUnitKey(article.QuantityUnitKey);
				this.replenishmentOrderItem().QuantityUnitEntryKey(article.QuantityUnitEntryKey);
			}
		} else {
			this.replenishmentOrderItem().Article(null);
			this.replenishmentOrderItem().Description(null);
			this.replenishmentOrderItem().MaterialNo(null);
			this.replenishmentOrderItem().QuantityUnitKey(null);
			this.replenishmentOrderItem().QuantityUnitEntryKey(null);
		}
	};

	onQuantityUnitEntrySelect(quantityUnitEntry : Crm.Article.Rest.Model.CrmArticle_QuantityUnitEntry) {
		if(quantityUnitEntry && this.replenishmentOrderItem().QuantityUnitEntry()) {
			const conversionRate = HelperQuantityUnit.getConversionRate(this.replenishmentOrderItem().QuantityUnitEntry, quantityUnitEntry)
			this.replenishmentOrderItem().Quantity(this.replenishmentOrderItem().Quantity() * conversionRate)
		}
		if(quantityUnitEntry == null) {
			if(this.replenishmentOrderItem().ArticleId == null)
				this.replenishmentOrderItem().QuantityUnitEntry(null)
			else
				this.replenishmentOrderItem().QuantityUnitEntryKey(this.replenishmentOrderItem().QuantityUnitEntry().Id())
		}
		else if (this.replenishmentOrderItem().QuantityUnitEntry() == null || this.replenishmentOrderItem().QuantityUnitEntry().Id() !== quantityUnitEntry.Id) {
			this.replenishmentOrderItem().QuantityUnitEntry(quantityUnitEntry.asKoObservable())
			this.replenishmentOrderItem().QuantityUnitKey(quantityUnitEntry.QuantityUnitKey)
		}
	}
}

namespace("Crm.Service.ViewModels").ReplenishmentOrderItemEditModalViewModel = ReplenishmentOrderItemEditModalViewModel;