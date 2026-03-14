import {namespace} from "./namespace";

export class OnlineTopMenuViewModel {
	isVisible = ko.pureComputed<boolean>(this.isVisibleFunc.bind(this));

	isVisibleFunc(): boolean {
		if (Helper.Offline && Helper.Offline.status === "offline") {
			return false;
		}
		return true;
	};

	async init(): Promise<void> {
	};

	reload(): void {
		window.Helper.Url.reloadAndReturnToLocation();
	};

	onInitialized(): void {
		const reload = document.querySelector("#reload-top-menu");
		if (reload) {
			if (!ko.contextFor(reload)) {
				const viewModel = new window.Main.ViewModels.OnlineTopMenuViewModel();
				viewModel.init().then(function () {
					ko.applyBindings(viewModel, reload);
				});
			}
		}
	};
}

namespace("Main.ViewModels").OnlineTopMenuViewModel = OnlineTopMenuViewModel;

document.addEventListener("Initialized", window.Main.ViewModels.OnlineTopMenuViewModel.prototype.onInitialized);
