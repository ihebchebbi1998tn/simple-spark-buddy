import {namespace} from "./namespace";
import type {GenericListViewModel} from "./GenericListViewModel";

export interface TopSearchEntity {
	caption: string;
	searchProperties: string[];
	template: string;
	viewModel: GenericListViewModel<any, any>;
}

export class TopSearchViewModel {

	loading = ko.observable<boolean>(true);
	input = ko.observable<string>("");
	lastInput = "";
	delayedInput = ko.pureComputed(this.input).extend({rateLimit: {method: "notifyWhenChangesStop", timeout: 2000}});
	entities = ko.observableArray<TopSearchEntity>([]);
	initialized = false;
	render = ko.observable<boolean>(false);

	constructor(options?: {}) {
		this.delayedInput.subscribe((value) => {
			if (value && value !== this.lastInput) {
				this.search();
			}
		});
	}

	async init(): Promise<void> {
		const promises: Array<Promise<void>> = [];
		for (const entity of this.entities()) {
			entity.viewModel.enableUrlUpdate(false);
			entity.viewModel.infiniteScroll(false);
			entity.viewModel.pageSize(5);
			entity.viewModel.displayOnlyMultiplePages(true);
			const originalApplyFilters = entity.viewModel.applyFilters.bind(entity.viewModel);
			entity.viewModel.applyFilters = (query) => {
				query = originalApplyFilters(query);
				if (this.input()) {
					const inputWords = window.Helper.String.parseWords(this.input());
					inputWords.forEach(inputWord => {
						const filterString = entity.searchProperties.map(x => "it." + x + ".contains(this.value)").join(" || ")
						query = query.filter(filterString, {value: inputWord});
					});
				}
				return query;
			}
			entity.viewModel.registerEventHandlers = () => {
			};
			entity.viewModel.bulkActions([]);
			let promise = entity.viewModel.init();
			promise.then(() => {
				entity.viewModel.bulkActions([]);
				entity.viewModel.loading(false);
			})
			promises.push(promise);
		}
		await Promise.all(promises);
	}

	onKeypress(context: TopSearchViewModel, e: KeyboardEvent) {
		if (e.key == "Enter") {
			this.search();
			return false;
		}
		return true;
	}

	dispose(): void {
	}

	async filter(): Promise<void> {
		await Promise.all(this.entities().map(entity => {
			entity.viewModel.page(1);
			return entity.viewModel.search(false, true, true);
		}))
	}

	async search(): Promise<void> {
		this.lastInput = this.input();
		this.loading(true);
		$("#topSearchModal:not(.in)").modal("show", {viewModel: this});
		if (!this.initialized) {
			await this.init();
			this.initialized = true;
		} else {
			await this.filter();
		}
		this.loading(false);
	}
}

namespace("Main.ViewModels").TopSearchViewModel ||= TopSearchViewModel;