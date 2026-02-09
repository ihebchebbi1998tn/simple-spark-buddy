import {registerComponent} from "@Main/componentRegistrar";

registerComponent({
	componentName: "servicecase-dashboard-list",
	template: "Crm.Service/ServiceCaseList/ServiceCaseDashboardList",
	viewModel: {
		createViewModel: function (params, componentInfo) {
			const viewModel = new window.Crm.Service.ViewModels.ServiceCaseListIndexViewModel();
			viewModel.infiniteScroll(false);
			viewModel.pageSize(3);
			viewModel.viewModes([viewModel.viewMode()]);
			viewModel.init(null, { context: "dashboard" }).then(function() {
				viewModel.bulkActions([]);
				viewModel.loading(false);
			});
			viewModel.getIcsLinkAllowed = function() {
				return false;
			};
			return viewModel;
		}
	}
});