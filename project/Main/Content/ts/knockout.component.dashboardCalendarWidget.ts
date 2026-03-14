import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "dashboard-calendar-widget",
	template: "Main/Dashboard/CalendarWidgetTemplate",
	viewModel: {
		createViewModel: function (params, componentInfo) {
			const viewModel = new window.Main.ViewModels.DashboardCalendarWidgetViewModel(params);
			viewModel.init();
			return viewModel;
		}
	}
});

$(document).on('click', '#type-select-dropdown', function (e) {
	e.stopPropagation();
});