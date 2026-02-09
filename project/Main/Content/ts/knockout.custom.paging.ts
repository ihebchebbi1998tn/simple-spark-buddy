;
(function(ko) {
	// @ts-ignore
	ko.custom = ko.custom || {};
	ko.custom.paging = function(pageSize, displayedPageCount, page, totalPages) {
		pageSize = pageSize || 10;
		displayedPageCount = displayedPageCount || 5;
		var viewModel: any = {};
		viewModel.page = page || ko.observable();
		viewModel.page(parseInt(viewModel.page()) || 1);
		viewModel.pageSize = ko.observable(pageSize);
		viewModel.pageState = ko.observable(null);
		viewModel.totalItemCount = ko.observable(null);
		viewModel.totalPages = totalPages || ko.pureComputed(function() {
			return Math.ceil(viewModel.totalItemCount() / viewModel.pageSize());
		});
		viewModel.hasPreviousPage = ko.pureComputed(function() {
			return viewModel.page() > 1;
		});
		viewModel.hasNextPage = ko.pureComputed(function() {
			return viewModel.page() < viewModel.totalPages();
		});
		viewModel.displayedPages = ko.pureComputed(function() {
			var displayedPages = [viewModel.page()];
			for (var i = 1; i < displayedPageCount; i++) {
				if (displayedPages.length < displayedPageCount && viewModel.page() - i > 0) {
					displayedPages.unshift(viewModel.page() - i);
				}
				if (displayedPages.length < displayedPageCount && viewModel.page() + i <= viewModel.totalPages()) {
					displayedPages.push(viewModel.page() + i);
				}
			}
			return displayedPages;
		});
		viewModel.goToFirstPage = function() {
			if (viewModel.page() > 1) {
				viewModel.page(1);
			}
		}
		viewModel.goToPreviousPage = function() {
			if (viewModel.page() > 1) {
				viewModel.page(viewModel.page() - 1);
			}
		}
		viewModel.goToNextPage = function() {
			if (viewModel.page() < viewModel.totalPages()) {
				viewModel.page(viewModel.page() + 1);
			}
		}
		viewModel.goToLastPage = function() {
			if (viewModel.page() < viewModel.totalPages()) {
				viewModel.page(viewModel.totalPages());
			}
		}
		viewModel.goToPage = function(page) {
			var newPage = parseInt(page);
			if (viewModel.page() !== newPage) {
				viewModel.page(newPage);
			}
		}
		viewModel.skip = ko.pureComputed(function() {
			return (viewModel.page() - 1) * viewModel.pageSize();
		});
		return viewModel;
	};
})(window.ko);