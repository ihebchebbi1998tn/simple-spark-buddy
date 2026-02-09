import {registerComponent} from "./componentRegistrar";
import {GenericListViewModel} from "@Main/GenericListViewModel";

export class GenericListSelectionViewModel extends GenericListViewModel<$data.Entity, any>  {
	disable: boolean;
}

registerComponent({
	componentName: "generic-list-selection",
	template: "Main/Template/GenericListSelection",
	viewModel: {
		createViewModel: function (params) {
			let viewModelType = window.namespace(params.plugin)["ViewModels"][params.controller + "IndexViewModel"];
			let viewModel: GenericListSelectionViewModel = new viewModelType();
			viewModel.disable = params.disable || false;
			viewModel.pageTitle = params.caption;
			viewModel.plugin = params.plugin;
			viewModel.controller = params.controller;
			viewModel.setEditing = null;
			viewModel.enableUrlUpdate(false);
			viewModel.loading(false);
			viewModel.bulkActions([{Name: 'Apply'}]);
			viewModel.selectedItems.subscribe(selectedItems => {
				let selectedItem = selectedItems[0];
				if (selectedItem) {
					if (ko.isWriteableObservable(params.value)) {
						let keyPropertyName = window.Helper.Database.getKeyProperty(selectedItem.innerInstance.getType().name);
						params.value(selectedItem[keyPropertyName]());
					}
					if (params.onSelect) {
						params.onSelect(selectedItem.innerInstance);
					}
					$(".modal:visible").closest(".modal-content").data("keepchanges", true);
					$(".modal:visible").last().modal("hide");
					viewModel.selectedItems([]);
				}
			});
			if (params.customFilter) {
				let applyFilters = viewModel.applyFilters.bind(viewModel);
				viewModel.applyFilters = query => {
					query = params.customFilter(query);
					return applyFilters(query);
				}
			}
			let origSearch = viewModel.search.bind(viewModel);
			viewModel.search = async () => {
				viewModel.search = origSearch;
				viewModel.loading(true);
				await viewModel.init(null, { scrollToTop: "true" });
				viewModel.defaultFilters = {};
				viewModel.loading(false);
			}
			return viewModel;
		},
	}
});