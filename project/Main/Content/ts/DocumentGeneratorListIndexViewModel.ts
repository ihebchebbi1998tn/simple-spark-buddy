import {namespace} from "./namespace";
import type {GenericListViewModel} from "./GenericListViewModel";

export class DocumentGeneratorListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Main.Rest.Model.Main_DocumentGeneratorEntry, Main.Rest.Model.ObservableMain_DocumentGeneratorEntry> {
	generatorServices = ko.observableArray<{ Type: string, Name: string }>([]);
	generatorServiceViewModels = ko.observableArray<GenericListViewModel<Main.Rest.Model.Main_DocumentGeneratorEntry, Main.Rest.Model.ObservableMain_DocumentGeneratorEntry>>([]);
	pageState = ko.pureComputed<string>(() => null);

	constructor() {
		super("Main_DocumentGeneratorEntry", "CreateDate", "DESC");
		this.page.subscribe(page => {
			this.generatorServiceViewModels().forEach(generatorServiceViewModel => {
				generatorServiceViewModel.page(page);
			});
		})
		this.pageSize(10);
		this.bookmark.subscribe(bookmark => {
			this.bulkActions.remove(x => x.Name === "Retry");
			if (bookmark && bookmark.Key === "Failed") {
				this.bulkActions.push({
					Name: "Retry",
					Action: (items) => {
						return items instanceof Array ? this.retry(items) : this.retry(this.items());
					}
				});
			}
		});
		this.addBookmarks();
		this.search = async function (scrollToTop, resetTotalCount, setLoading): Promise<void> {
			this.generatorServiceViewModels().forEach(generatorServiceViewModel => {
				generatorServiceViewModel.currentSearch = generatorServiceViewModel.search(scrollToTop, resetTotalCount, setLoading);
			});
			this.selectedItems([]);
			return null;
		}
		let computedItems = ko.pureComputed(() => {
			let items = [];
			this.generatorServiceViewModels().forEach(generatorServiceViewModel => {
				items = items.concat(generatorServiceViewModel.items());
			});
			return items;
		});
		computedItems.subscribe(items => {
			items.forEach(item => this.initItem(item));
			this.items(items);
			this.totalItemCount(Math.max(...this.generatorServiceViewModels().map(x => x.totalItemCount())));
		});
		let computedLoading = ko.pureComputed(() => {
			return this.generatorServiceViewModels().some(generatorServiceViewModel => generatorServiceViewModel.loading());
		});
		computedLoading.subscribe(loading => {
			this.loading(loading);
		});
	}
	
	addBookmarks(): void {
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("Pending"),
			Key: "Pending",
			Expression: function (query) {
				return query;
			}
		});
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("Failed"),
			Key: "Failed",
			Expression: function (query) {
				return query;
			}
		});
		this.bookmark(this.bookmarks()[1]);
	}

	getItemGroup(item: Main.Rest.Model.ObservableMain_DocumentGeneratorEntry): ItemGroup {
		// @ts-ignore
		return {title: item.Name ? window.Helper.String.getTranslatedString(item.Name, item.Name) : window.Helper.String.getTranslatedString(item.GeneratorService(), item.GeneratorService())};
	}

	initItem(item): any {
		let generatorService = this.generatorServices().filter(service => service.Type === ko.unwrap(item.GeneratorService))[0];
		if (generatorService)
			item.Name = generatorService.Name;
		return super.initItem(item);
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<any> {
		// @ts-ignore
		this.generatorServices(await window.database.Main_DocumentGeneratorEntry.GetGeneratorServices().toArray());
		for (const generatorService of this.generatorServices()) {
			let generatorServiceViewModel = new window.Main.ViewModels.GenericListViewModel<Main.Rest.Model.Main_DocumentGeneratorEntry, Main.Rest.Model.ObservableMain_DocumentGeneratorEntry>("Main_DocumentGeneratorEntry", "CreateDate", "DESC");
			generatorServiceViewModel.getBaseQuery = () => {
				return this.bookmark().Key === "Failed" ? window.database.Main_DocumentGeneratorEntry.GetFailed(generatorService.Type) : window.database.Main_DocumentGeneratorEntry.GetPending(generatorService.Type);
			}
			generatorServiceViewModel.bulkActions = this.bulkActions;
			let getSearchResults = generatorServiceViewModel.getSearchResults.bind(generatorServiceViewModel);
			generatorServiceViewModel.getSearchResults = (resetTotalCount) => {
				let filter = this.getFilter("GeneratorService")();
				return !filter || filter.Value === generatorService.Type ? getSearchResults(resetTotalCount) : Promise.resolve([]);
			}
			generatorServiceViewModel.pageSize(this.pageSize());
			this.generatorServiceViewModels.push(generatorServiceViewModel);
			await generatorServiceViewModel.init();
		}
		await super.init(id, params);
	}

	async retry(items: Main.Rest.Model.ObservableMain_DocumentGeneratorEntry[]) {
		await window.database.RetryDocumentGeneration(items.map(x => window.Helper.Database.getDatabaseEntity(x)));
		this.currentSearch = this.search(true, true);
	}

	typeFilterDisplay(item) {
		return window.Helper.String.getTranslatedString(item.Type, item.Type);
	}

	typeFilterFilter(query, term) {
		query = window.database.Main_DocumentGeneratorEntry.GetGeneratorServices(term);
		return query;
	}
}

namespace("Main.ViewModels").DocumentGeneratorListIndexViewModel = DocumentGeneratorListIndexViewModel;