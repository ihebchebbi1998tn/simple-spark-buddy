import { registerComponent } from "./componentRegistrar";

interface License {
	DomainId: string,
	Expires: string
}

registerComponent({
	componentName: "licensing-alert",
	template: "Main/Template/LicensingAlert",
	viewModel: function (params) {
		return new LicensingViewModel(params);
	}
})

class LicensingViewModel {
	private license: KnockoutObservable<License> = ko.observable<License>();
	private expireInDays: KnockoutComputed<number>;
	constructor(params = null) {
		this.license(JSON.parse(window.Helper.Database.getFromLocalStorage("license")));
		this.expireInDays = ko.pureComputed(() => window.Helper.Date.diffInDays(new Date(this.license().Expires), new Date()));
	}
}

$(document).one('Initialized', (e) => {
	window.ko.applyBindings(new LicensingViewModel(), $("licensing-alert")[0]);
});