import {namespace} from "@Main/namespace";
import {HelperQuantityUnit} from "../../../Crm.Article/Content/ts/helper/Helper.QuantityUnit";

export class InstallationPositionEditModalViewModel extends window.Main.ViewModels.ViewModelBase {
	lookups: LookupType = {
		quantityUnits: {$tableName: "CrmArticle_QuantityUnit"},
		articleTypes: {$tableName: "CrmArticle_ArticleType"}
	};
	installationPosition = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_InstallationPos>(null);
	quantityUnit = ko.pureComputed<Crm.Article.Rest.Model.Lookups.CrmArticle_QuantityUnit>(() => {
		return this.lookups.quantityUnits.$array.find(x => x.Key === this.installationPosition().QuantityUnitKey());
	});
	quantityUnitGroupKey = ko.pureComputed<string>(() => {
		return this.selectedArticle() ? this.selectedArticle().QuantityUnitEntryKey : null;
	});
	selectedArticle = ko.observable<Crm.Article.Rest.Model.CrmArticle_Article>(null);
	errors = ko.validation.group(this.installationPosition, {deep: true});

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let installationPosition: Crm.Service.Rest.Model.CrmService_InstallationPos;
		if (id) {
			installationPosition = await window.database.CrmService_InstallationPos
				.include("QuantityUnitEntry")
				.find(id);
			window.database.attachOrGet(installationPosition);
		} else {
			installationPosition = window.database.CrmService_InstallationPos.defaultType.create();
			installationPosition.Quantity = 1;
			installationPosition.InstallationId = params.installationId;
			installationPosition.IsInstalled = true;
			window.database.add(installationPosition);
		}
		this.installationPosition(installationPosition.asKoObservable());
		if (!!params.referenceId) {
			this.installationPosition().ReferenceId(params.referenceId);
		}
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
	};

	dispose(): void {
		window.database.detach(this.installationPosition().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}
		try {
			await window.Helper.Installation.updatePosNo(this.installationPosition());
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

	getArticleSelect2Filter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article>, filter: string): $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article> {
		const language = (document.getElementById("meta.CurrentLanguage") as HTMLMetaElement).content;
		query = query.filter(function (it) {
			return it.ArticleTypeKey in ["Material", "Cost", "Service", "Tool"];
		});
		return window.Helper.Article.getArticleAutocompleteFilter(query, filter, language);
	};

	installationPosFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_InstallationPos>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_InstallationPos> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["ItemNo.toLowerCase()", "Description.toLowerCase()"]);
		}
		return query.filter(function (it) {
				return it.InstallationId === this.installationId &&
					it.IsGroupItem === true &&
					it.Id !== this.installationPositionId;
			},
			{
				installationId: this.installationPosition().InstallationId(),
				installationPositionId: this.installationPosition().Id()
			});
	};

	onArticleSelect(article: Crm.Article.Rest.Model.CrmArticle_Article): void {
		this.selectedArticle(article);
		if (article) {
			this.installationPosition().IsSerial(article.IsSerial);
			if (article.IsSerial) {
				this.installationPosition().Quantity(1);
			} else {
				this.installationPosition().SerialNo(null);
			}
			this.installationPosition().Description(window.Helper.Article.getArticleDescription(article));
			this.installationPosition().ItemNo(article.ItemNo);
			this.installationPosition().QuantityUnitEntryKey(article.QuantityUnitEntryKey);
		} else {
			this.installationPosition().Description(null);
			this.installationPosition().ItemNo(null);
			this.installationPosition().QuantityUnitKey(null);
			this.installationPosition().QuantityUnitEntryKey(null);
		}
	};

	onQuantityUnitEntrySelect(quantityUnitEntry : Crm.Article.Rest.Model.CrmArticle_QuantityUnitEntry) {
		if(quantityUnitEntry && this.installationPosition().QuantityUnitEntry()) {
			const conversionRate = HelperQuantityUnit.getConversionRate(this.installationPosition().QuantityUnitEntry, quantityUnitEntry)
			this.installationPosition().Quantity(this.installationPosition().Quantity() * conversionRate)
		}
		if(quantityUnitEntry == null) {
			if(this.installationPosition().ArticleId == null)
				this.installationPosition().QuantityUnitEntry(null)
			else
				this.installationPosition().QuantityUnitEntryKey(this.installationPosition().QuantityUnitEntry().Id())
		}
		else if (this.installationPosition().QuantityUnitEntry() == null || this.installationPosition().QuantityUnitEntry().Id() !== quantityUnitEntry.Id) {
			this.installationPosition().QuantityUnitEntry(quantityUnitEntry.asKoObservable())
			this.installationPosition().QuantityUnitKey(quantityUnitEntry.QuantityUnitKey)
		}
	}

	mapArticleForSelect2Display(article: Crm.Article.Rest.Model.CrmArticle_Article): Select2AutoCompleterResult {
		return {
			id: article.Id,
			item: article.asKoObservable(),
			text: window.Helper.Article.getArticleAutocompleteDisplay(article) + " (" + window.Helper.Lookup.getLookupValue(this.lookups.articleTypes, article.ArticleTypeKey) + ")"
		};
	};

	relatedInstallationFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["InstallationNo.toLowerCase()", "Description.toLowerCase()"]);
		}
		return query.filter(function (it) {
				return it.Id !== this.installationId
			},
			{
				installationId: this.installationPosition().InstallationId()
			});
	};

	onRelatedInstallationSelected(installation: Crm.Service.Rest.Model.CrmService_Installation): void {
		if (!!installation) {
			this.installationPosition().IsGroupItem(false);
		}
	};
}

namespace("Crm.Service.ViewModels").InstallationPositionEditModalViewModel = InstallationPositionEditModalViewModel;