import {registerComponent} from "@Main/componentRegistrar";

registerComponent({
	componentName: "statisticskey-info",
	permission: "StatisticsKey::View",
	template: "Crm.Service/StatisticsKey/InfoTemplate",
	viewModel: {
		createViewModel: function (params, componentInfo) {
			return new window.Crm.Service.ViewModels.StatisticsKeyInfoViewModel(params);
		}
	}
});