import {registerComponent} from "./componentRegistrar";
import type {JobWithTriggers, Trigger} from "./BackgroundServiceIndexViewModel";

registerComponent({
	componentName: "job-details",
	template: "BackgroundService/JobDetails",
	viewModel: {
		createViewModel: function (params: { data: JobWithTriggers }) {
			return new JobDetails(params.data);
		}
	},
});

export default class JobDetails extends window.Main.ViewModels.ViewModelBase {
	avatarChar: KnockoutObservable<string>;
	jobId: KnockoutComputed<string>;
	jobKey: KnockoutObservable<string>;
	jobGroup: KnockoutObservable<string>;
	jobType: KnockoutObservable<string>;
	jobDescription: KnockoutObservable<string>;
	triggers: KnockoutObservableArray<Trigger>;
	haveTrigger: KnockoutObservable<string>;
	fullName: KnockoutObservable<string>;

	constructor(params: JobWithTriggers) {
		super();
		this.jobId = ko.computed(() => {
			return `backserv_${params.Job.Key.Name}`;
		});
		this.jobKey = ko.observable(params.Job.Key.Name);
		this.jobGroup = ko.observable(params.Job.Key.Group);
		this.jobType = ko.observable(params.Job.JobType.split(",")[0]);

		this.fullName = ko.observable(params.Job.Key.Name);
		this.jobDescription = ko.observable(params.Job.Description);
		this.avatarChar = ko.observable();
		this.triggers = ko.observableArray(params.Triggers);

		this.haveTrigger = ko.pureComputed(function () {
			switch (params.Triggers.length > 0) {
				case true:
					this.avatarChar(params.Job.Key.Name.charAt(0));
					return "bgm-green c-white";
				case false:
					this.avatarChar(params.Job.Key.Name.charAt(0));
					return "bgm-gray c-white";
			}
		}, this);
	}

	async requestData(method: string, data: any, url: string): Promise<any> {
		const response = await fetch(url, {
			method: method,
			headers: {
				"Content-Type": "application/x-www-form-urlencoded;charset=UTF-8"
			},
			body: new URLSearchParams(data),
		});

		return response;
	}

	async runJob(): Promise<any> {
		const data: any = {id: this.jobKey(), group: this.jobGroup()};
		const url: string = window.Helper.Url.resolveUrl("~/Main/BackgroundService/RunNow");
		const response = await this.requestData("POST", data, url);

		if (response.status === 200) {
			this.showNotification("BackgroundJobTriggered");
			return;
		}
		this.showNotification("BackgroundJobTriggeredFail");
	}

	async pauseTrigger(obj: any): Promise<any> {
		const data: any = {
			id: obj.triggers()[0].Name,
			group: obj.triggers()[0].Group,
			jobName: this.jobKey(),
			jobGroup: this.jobGroup()
		};
		const url: string = window.Helper.Url.resolveUrl("~/Main/BackgroundService/PauseTrigger");

		const response = await this.requestData("POST", data, url);
		const result = JSON.parse(await response.json());

		this.updateTriggers(result);
		this.showNotification("JobTriggerPaused");
	}

	updateTriggers(data): void {
		this.triggers(data);
	}

	showNotification(message: string): void {
		this.showSnackbar(window.Helper.String.getTranslatedString(message));
	}
}
