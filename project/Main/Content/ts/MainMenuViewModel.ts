import {namespace} from "./namespace";

export class MainMenuViewModel {
	isMainMenuVisible = ko.pureComputed<boolean>(() => !this.loading());
	loading = ko.observable<boolean>(true);
}

namespace("Main.ViewModels").MainMenuViewModel = MainMenuViewModel;