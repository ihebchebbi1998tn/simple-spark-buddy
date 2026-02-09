import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "status-chooser",
	template: "Main/Template/StatusChooser",
	viewModel: {
		createViewModel: function (params) {
			return new StatusChooser(params);
		}
	}
});

class StatusChooser {
	statuses: Array<any>;
	currentStatusKey: KnockoutObservable<string>;
	settableStatuses: KnockoutComputed<any>;
	currentStatus: KnockoutComputed<any>;
	previousStatuses: KnockoutComputed<any>;
	nextStatuses: KnockoutComputed<any>;
	canSetStatus: KnockoutObservable<boolean>;
	setStatus: Function;
	setStatusRoute: string;
	constructor(params: any) {
		this.statuses = params.statuses;
		this.currentStatusKey = params.currentStatus;
		this.canSetStatus = params.canSetStatus ?? window.ko.observable(false);
		this.setStatus = params.setStatus;
		this.setStatusRoute = params.setStatusRoute;

		this.currentStatus = window.ko.pureComputed(() => {
			return this.statuses.find(s => s.Key === this.currentStatusKey())
		});
		this.settableStatuses = window.ko.pureComputed(() => {
			if(params.settableStatuses) {
				return params.settableStatuses();
			}
			return this.statuses.filter(s => (this.currentStatus().SettableStatuses || "").split(',').includes(s.Key.toString()));
		});
		this.previousStatuses = window.ko.pureComputed(() => {
			return this.settableStatuses().filter(s => s.Key !== this.currentStatusKey() && s.SortOrder < this.currentStatus().SortOrder);
		});
		this.nextStatuses = window.ko.pureComputed(() => {
			return this.settableStatuses().filter(s => s.Key !== this.currentStatusKey() && s.SortOrder >= this.currentStatus().SortOrder);
		});
	}
}