import {namespace} from "@Main/namespace";

export class InstallationPositionListIndexItemViewModel {
	visible: KnockoutObservable<boolean>;
	haveItems: KnockoutObservable<boolean>;
	level: KnockoutObservable<number>;
	parents: KnockoutObservableArray<Crm.Service.Rest.Model.ObservableCrmService_InstallationPos>;
	totalItemCount: KnockoutObservable<number>;
	items: KnockoutObservableArray<Crm.Service.Rest.Model.ObservableCrmService_InstallationPos & InstallationPositionListIndexItemViewModel>;
	bookmarks: KnockoutObservableArray<Bookmark<Crm.Service.Rest.Model.CrmService_InstallationPos>>;
	bookmark: KnockoutObservable<Bookmark<Crm.Service.Rest.Model.CrmService_InstallationPos>>;
	loading: KnockoutObservable<boolean>;
	positionsExpanded: KnockoutObservable<boolean>;
	subPositionsViewModel: InstallationPositionListIndexViewModel;
}
export class InstallationPositionListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Service.Rest.Model.CrmService_InstallationPos, Crm.Service.Rest.Model.ObservableCrmService_InstallationPos, Crm.Service.Rest.Model.ObservableCrmService_InstallationPos & InstallationPositionListIndexItemViewModel> {

	installationId: string;
	currentLevel: number;
	parents = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_InstallationPos>([]);
	positionsExpanded = ko.observable<boolean>(false);

	constructor(installationId: string, level: number, parents: Crm.Service.Rest.Model.ObservableCrmService_InstallationPos[]) {
		super(
			"CrmService_InstallationPos",
			["IsInstalled", "PosNo"],
			["DESC", "ASC"],
			["Article", "RelatedInstallation"]);
		this.installationId = installationId;
		this.currentLevel = !!level ? level : 1;
		this.parents(parents);
		if (!!this.installationId) {
			this.getFilter("InstallationId").extend({filterOperator: "==="})(this.installationId);
		}

		const bookmark = {
			Category: window.Helper.String.getTranslatedString("T_IsInstalled"),
			Name: window.Helper.String.getTranslatedString("InstalledPositions"),
			Key: "InstalledPositions",
			Expression: function (query) {
				return query.filter("it.IsInstalled === true ");
			}
		};
		this.bookmarks.push(bookmark);
		this.bookmark(bookmark);
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("T_IsInstalled"),
			Name: window.Helper.String.getTranslatedString("AllPositions"),
			Key: "AllPositions",
			Expression: function (query) {
				return query.filter("it.IsInstalled === true || it.IsInstalled === false ");
			}
		});
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_InstallationPos>): $data.Queryable<Crm.Service.Rest.Model.CrmService_InstallationPos> {
		query = super.applyFilters(query);
		if (this.filters.hasOwnProperty("ReferenceId")) {
			let filterValue = ko.unwrap(this.filters["ReferenceId"]) ? ko.unwrap(this.filters["ReferenceId"]).Value : null;
			query = query.filter("it.ReferenceId === this.referenceId",
				{referenceId: filterValue});
		}
		return query;
	}

	initItem(item: Crm.Service.Rest.Model.CrmService_InstallationPos): Crm.Service.Rest.Model.ObservableCrmService_InstallationPos & InstallationPositionListIndexItemViewModel {
		let result = super.initItem(item) as (Crm.Service.Rest.Model.ObservableCrmService_InstallationPos & InstallationPositionListIndexItemViewModel);
		result.visible = ko.observable(false);
		result.haveItems = ko.observable(false);
		result.level = ko.observable(this.currentLevel);
		result.parents = ko.observableArray(this.parents());
		return result;
	};

	async toggleBookmark(bookmark: Bookmark<Crm.Service.Rest.Model.CrmService_InstallationPos>): Promise<InstallationPositionListIndexViewModel> {
		return new Promise(resolve => {
			if (this.bookmark() === bookmark) {
				this.bookmark(null);
			} else {
				this.bookmark(bookmark);
			}
			this.page(1);
			this.currentSearch = this.search(false, true);
			this.currentSearch.then(() => {
				this.loading(false);
				resolve(this);
			});
		});
	}
}

namespace("Crm.Service.ViewModels").InstallationPositionListIndexViewModel = InstallationPositionListIndexViewModel;
