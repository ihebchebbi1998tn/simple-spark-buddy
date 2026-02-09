import {namespace} from "./namespace";
import { HelperString } from "./helper/Helper.String";

export type Trigger = {
	Name: string;
	Group: string;
	Description: string;
	NextFireTime: string;
	TriggerState: string;
	IsPaused: boolean;

};

export type JobWithTriggers = {
	Job: {
		Key: { Name: string; Group: string };
		Description: string;
		FullName: string;
		JobDataMap: any;
		Durable: boolean;
		PersistJobDataAfterExecution: boolean;
		ConcurrentExecutionDisallowed: boolean;
		RequestsRecovery: boolean;
		JobType: string;
	};
	Triggers: Trigger[];
};

export class BackgroundServiceIndexViewModel extends window.Main.ViewModels.ViewModelBase {

	allJobsData = ko.observableArray<JobWithTriggers>([]);
	allJobs = ko.observableArray<JobWithTriggers>([]);
	searchCriteria = ko.observable<string>(null);
	noMatchingRecords = ko.observable<boolean>(false);
	isFiltered = ko.observable<boolean>(false);
	filteredProperties = ko.observableArray<{
		caption: string,
		displayedValue: any
	}>([]);

	constructor() {
		super();
	}

	async init(id?: string, params?: {[key:string]:string}) {
		await super.init(id, params);
		const response = await this.getAll();
		const data: JobWithTriggers[] = JSON.parse(await response.json());
		this.allJobs(data);
		this.allJobsData(data);
	}

	async getAll(): Promise<any> {
		const url: string = window.Helper.Url.resolveUrl("~/Main/BackgroundService/BackgroundServiceListData");
		return await fetch(url, {
			method: "get",
		});
	}

	async pauseAllTriggers() {
		const url: string = window.Helper.Url.resolveUrl("~/Main/BackgroundService/PauseAll");
		const response = await fetch(url, {
			method: "post",
		});
		if (response.status === 200) {
			const data: JobWithTriggers[] = JSON.parse(await response.json());
			if (this.isFiltered()) {
				const result = data.filter(x => x.Job.Key.Name.toLowerCase().includes(this.searchCriteria()?.toLowerCase()));
				this.allJobs(result);
			} else {
				this.allJobs(data);
			}
			this.showNotification("AllTriggersPause");
			return;
		}
		this.showNotification("FailPauseAllTriggers");
	}

	showNotification(message: string): void {
		this.showSnackbar(window.Helper.String.getTranslatedString(message));
	}

	filter(): void {
		const searchString: string = this.searchCriteria()?.toLowerCase();
		if (!searchString) {
			return;
		}
		const results: JobWithTriggers[] = this.allJobsData().filter(x => x.Job.Key.Name.toLowerCase().includes(searchString));
		if (results.length === 0) {
			this.noMatchingRecords(true);
		} else {
			this.noMatchingRecords(false);
		}
		this.isFiltered(true);
		this.allJobs(results);
		if (this.isFiltered()) {
			this.filteredProperties([]);
			this.filteredProperties.push({
				caption: HelperString.getTranslatedString('Name', 'Name'),
				displayedValue: searchString
			})
		}
	}

	resetFilter(): void {
		this.allJobs(this.allJobsData());
		this.isFiltered(false);
		this.searchCriteria();
		this.noMatchingRecords(false);
	}
}

namespace("Main.ViewModels").BackgroundServiceIndexViewModel = BackgroundServiceIndexViewModel;
