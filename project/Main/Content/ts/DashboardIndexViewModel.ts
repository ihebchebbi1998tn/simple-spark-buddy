import {namespace} from "./namespace";

export class DashboardIndexViewModel extends window.Main.ViewModels.ViewModelBase {
	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
	}
}

namespace("Main.ViewModels").DashboardIndexViewModel = DashboardIndexViewModel;