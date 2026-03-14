export class HomeStartupViewModelExtension extends window.Main.ViewModels.HomeStartupViewModel {
	init(onlineStatus?: string, params?: any) {
		if (params.redirectUrl === "/Sms.Scheduler/Scheduler/DetailsTemplate" && window.Helper.Offline?.status === "offline" && onlineStatus !== "online") {
			window.location.hash = window.location.hash.split("?")[0] + "/online?" + window.location.hash.split("?")[1]
			window.location.reload();
		}
		return super.init(onlineStatus, params);
	}
}

window.Main.ViewModels.HomeStartupViewModel = HomeStartupViewModelExtension;

