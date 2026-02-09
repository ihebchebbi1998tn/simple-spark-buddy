import {namespace} from "@Main/namespace";
import type {InstallationDetailsViewModel} from "./InstallationDetailsViewModel";
import type {
	InstallationPositionListIndexItemViewModel,
	InstallationPositionListIndexViewModel
} from "./InstallationPositionListIndexViewModel";

export class InstallationDetailsPositionsTabViewModel extends window.Crm.Service.ViewModels.InstallationPositionListIndexViewModel {
	installationId: string;
	treeLevelDisplayLimit = parseInt(window.Crm.Service.Settings.Service.Installation.Position.TreeLevelDisplayLimit);
	pageReferenceId = ko.observableArray<string>();
	levelPage = ko.observable<number>(0);
	topPagePositionsIds = ko.observableArray<string>([]);

	constructor(parentViewModel: InstallationDetailsViewModel) {
		let installationId = parentViewModel.installation().Id();
		super(installationId, 0, []);
		this.installationId = installationId;
		this.getFilter("ReferenceId").extend({filterOperator: "==="})(null);
		this.infiniteScroll(false);
		this.lookups = parentViewModel.lookups || {};
		this.lookups.quantityUnits = this.lookups.quantityUnits || {$tableName: "CrmArticle_QuantityUnit"};
		this.lookups.articleTypes = this.lookups.articleTypes || {$tableName: "CrmArticle_ArticleType"};
	}

	getCurrentPositionContext(item: (Crm.Service.Rest.Model.ObservableCrmService_InstallationPos & InstallationPositionListIndexItemViewModel)): void {
		let uniqCallback = function (item) {
			return item.Id();
		};
		if (item.level() >= this.treeLevelDisplayLimit && (item.level() % this.treeLevelDisplayLimit) === 0) {
			this.pageReferenceId()[this.levelPage()] = !!this.filters["ReferenceId"]() ? this.filters["ReferenceId"]().Value : null;
			this.levelPage(this.levelPage() + 1);
			this.getFilter("ReferenceId").extend({filterOperator: "==="})(item.parents().slice(-1)[0].Id());
			this.topPagePositionsIds()[this.levelPage()] = item.Id();
			this.currentLevel = item.level() + 1;
			this.parents(window._.uniqBy(item.parents().concat([item]), uniqCallback));
			this.filter();
		} else {
			this.currentLevel = item.level() >= this.treeLevelDisplayLimit ? this.currentLevel : 1;
			let subPositionsViewModel = new window.Crm.Service.ViewModels.InstallationPositionListIndexViewModel(this.installationId, item.level() + 1, window._.uniqBy(item.parents().concat([item]), uniqCallback));
			subPositionsViewModel.getFilter("ReferenceId").extend({filterOperator: "==="})(item.Id());
			item.subPositionsViewModel = subPositionsViewModel;
			item.totalItemCount = subPositionsViewModel.totalItemCount;
			item.items = subPositionsViewModel.items;
			subPositionsViewModel.infiniteScroll(false);
			Object.assign(item, subPositionsViewModel);
			let bookmark = window.ko.utils.arrayFirst(item.bookmarks(), i => i.Key === this.bookmark().Key) || null;
			item.bookmark(bookmark);
			item.haveItems(true);
			subPositionsViewModel.init().then(function () {
				item.loading(false);
			});
		}
	}

	async init(): Promise<void> {
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups)
		await super.init();
	};

	async removeInstallationPosition(installationPosition: Crm.Service.Rest.Model.ObservableCrmService_InstallationPos): Promise<void> {
		let confirm = await window.Helper.Confirm.confirmDeleteAsync();
		if (confirm) {
			this.loading(true);
			window.database.remove(window.Helper.Database.getDatabaseEntity(installationPosition));
			await window.database.saveChanges();
		}
	}

	async uninstallInstallationPosition(installationPosition: Crm.Service.Rest.Model.ObservableCrmService_InstallationPos): Promise<void> {
		let confirm = await window.Helper.Confirm.genericConfirmAsync({
			title: window.Helper.String.getTranslatedString("Uninstall"),
			text: window.Helper.String.getTranslatedString("UninstallMaterialConfirmation"),
			type: "warning",
			confirmButtonText: window.Helper.String.getTranslatedString("Uninstall")
		});
		if (confirm) {
			this.loading(true);
			window.database.attachOrGet(installationPosition.innerInstance);
			installationPosition.IsInstalled(false);
			installationPosition.RemoveDate(new Date());
			await window.database.saveChanges();
		}
	};

	async installInstallationPosition(installationPosition: Crm.Service.Rest.Model.ObservableCrmService_InstallationPos): Promise<void> {
		let confirm = await window.Helper.Confirm.genericConfirmAsync({
			title: window.Helper.String.getTranslatedString("Install"),
			text: window.Helper.String.getTranslatedString("InstallMaterialConfirmation"),
			type: "info",
			confirmButtonText: window.Helper.String.getTranslatedString("Install")
		});
		if (confirm) {
			this.loading(true);
			window.database.attachOrGet(installationPosition.innerInstance);
			installationPosition.IsInstalled(true);
			await window.database.saveChanges();
		}
	};

	toggleSelectedGroupedPositions(toggledGroupedPositions: (Crm.Service.Rest.Model.ObservableCrmService_InstallationPos & InstallationPositionListIndexItemViewModel)): void {
		if ($('.collapsing').length > 0) {
			return;
		}
		this.positionsExpanded(false);

		function hide(positionsGroupViewModel: (Crm.Service.Rest.Model.ObservableCrmService_InstallationPos & InstallationPositionListIndexItemViewModel)) {
			if (positionsGroupViewModel.visible() === true) {
				$("#collapse-panel-positions-" + positionsGroupViewModel.Id()).collapse("hide");
				positionsGroupViewModel.visible(false);
				if (!!positionsGroupViewModel.items) {
					(positionsGroupViewModel.items() || []).forEach(function (item) {
						hide(item as (Crm.Service.Rest.Model.ObservableCrmService_InstallationPos & InstallationPositionListIndexItemViewModel));
					});
					positionsGroupViewModel.haveItems(false);
					positionsGroupViewModel.positionsExpanded(false);
				}
			}
		}

		if (toggledGroupedPositions.visible() === true) {
			hide(toggledGroupedPositions);
		} else {
			$("#collapse-panel-positions-" + toggledGroupedPositions.Id()).collapse("show");
			toggledGroupedPositions.visible(true);
			if (toggledGroupedPositions.IsGroupItem()) {
				this.getCurrentPositionContext(toggledGroupedPositions);
			}
		}
	};

	backToPreviousView(): void {
		this.loading(true);
		this.levelPage(this.levelPage() - 1);
		this.getFilter("ReferenceId").extend({filterOperator: "==="})(this.pageReferenceId()[this.levelPage()]);
		this.currentLevel = this.treeLevelDisplayLimit * this.levelPage() + 1;
		if (this.currentLevel === 1) {
			this.pageReferenceId([]);
			this.topPagePositionsIds([]);
			this.parents([]);
		} else {
			for (let i = 1; i < this.treeLevelDisplayLimit; i++) {
				this.parents.remove(this.parents()[this.parents().length - 1]);
			}
		}
		this.filter();
	};

	expandAllPositions(data: InstallationPositionListIndexViewModel): void {
		const position = !!data ? data : this;
		position.items().forEach(item => {
			$("#collapse-panel-positions-" + item.Id()).collapse("show");
			item.visible(true);
			if (item.IsGroupItem()) {
				this.getCurrentPositionContext(item);
			}
		});
		position.positionsExpanded(true);
	}

	closeAllPositions(data: InstallationPositionListIndexViewModel): void {
		const position = !!data ? data : this;
		position.items().forEach(function (item) {
			$("#collapse-panel-positions-" + item.Id()).collapse("hide");
			item.visible(false);
			item.haveItems(false);
		});
		position.positionsExpanded(false);
	}

	async toggleBookmark(bookmark: Bookmark<Crm.Service.Rest.Model.CrmService_InstallationPos>): Promise<InstallationPositionListIndexViewModel> {
		let model = (await super.toggleBookmark(bookmark)) as InstallationDetailsPositionsTabViewModel;
		if (model.positionsExpanded()) {
			model.expandAllPositions(model);
		}
		return model;
	}
}

namespace("Crm.Service.ViewModels").InstallationDetailsPositionsTabViewModel = InstallationDetailsPositionsTabViewModel;