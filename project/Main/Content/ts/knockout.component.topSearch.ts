import {registerComponent} from "./componentRegistrar";

registerComponent({
    componentName: "top-search",
    template: "Main/Template/TopSearch",
    viewModel: {
        createViewModel: function (params) {
            return params.viewModel;
        },
    },
    permission: "TopSearch::View"
});
$(document).one('Initialized', (e) => {
    const viewModel = new window.Main.ViewModels.TopSearchViewModel();
	$("#top-search, top-search, #topSearchModal").each((i, n) => window.ko.applyBindings(viewModel, n));
	$("#topSearchModal").on("hide.bs.modal", () => $("#top-search-close").trigger("click"));
	$("#topSearchModal").on("show.bs.modal", () => viewModel.render(true));
	$("#topSearchModal").on("hidden.bs.modal", () => viewModel.render(false));
});
$(document).on("click", "#topSearchModal .modal-body a", function (e) {
	e.preventDefault();

	const currentPath = window.location.hash?.split('?')[0];
	const targetPath = $(this).attr("href");

	if (targetPath.indexOf("/") !== -1) {
		e.preventDefault();

		if (currentPath === targetPath) {
			$("#topSearchModal").modal("hide");

			$.growl({
				message: window.Helper.String.getTranslatedString("RequestedViewIsTheCurrent")
			},
			{
				type: "inverse",
				allow_dismiss: true,
				placement: {
					from: "top",
					align: "center"
				},
				z_index: 9999,
				delay: 2500,
				animate: {
					enter: "animated fadeInDown",
					exit: "animated fadeOut"
				}
			});
		} else {
			window.location.href = targetPath;
		}
	}
});