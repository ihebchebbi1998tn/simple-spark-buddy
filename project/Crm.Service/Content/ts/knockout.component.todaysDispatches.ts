import {registerComponent} from "@Main/componentRegistrar";

registerComponent({
	componentName: "todays-dispatches",
	template: "Crm.Service/ServiceOrderDispatchList/TodaysDispatches",
	viewModel: {
		createViewModel: function (params, componentInfo) {
			var viewModel = new window.Crm.Service.ViewModels.ServiceOrderDispatchListIndexViewModel(null);
			viewModel.enableUrlUpdate(false);
			viewModel.infiniteScroll(false);
			viewModel.pageSize(3);
			viewModel.mapViewModel.toggleMap = null;
			viewModel.viewModes([viewModel.viewMode()]);
			const dateIndex = viewModel.orderBy().indexOf('Date');
			if (dateIndex > -1) { // @ts-ignore
				viewModel.orderByDirection()[dateIndex] = 'ASC';
			}
			viewModel.init(null, {context: "dashboard"}).then(() => viewModel.loading(false));
			viewModel.downloadIcs = null;
			viewModel.getIcsLinkAllowed = function () {
				return false;
			};
			return viewModel;
		}
	}
});
