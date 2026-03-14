import { namespace } from "@Main/namespace"

export class FlowItemListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Main.Flow.Rest.Model.MainFlow_FlowItem, Main.Flow.Rest.Model.ObservableMainFlow_FlowItem> {

	status: KnockoutObservable<string>;
	users: KnockoutObservableArray<Main.Rest.Model.Main_User>;

	constructor() {
		super("MainFlow_FlowItem", "CreateDate", "DESC")
		this.status = window.ko.observable(window.Helper.Offline ? window.Helper.Offline.status : "online");
		this.users = window.ko.observableArray([]);
		this.bulkActions.push(this.skipRequestsBulkAction());
		this.bulkActions.push(this.retryRequestsBulkAction());
	}

	initItem(item: any) {
		item.SerializedEntityParsed = JSON.parse(item.SerializedEntity);
		item.Name = item.SerializedEntityParsed.Name;
		item.Color = this.getColor(item);
		return item;
	}
	
	async initItems(items: Main.Flow.Rest.Model.ObservableMainFlow_FlowItem[]): Promise<Main.Flow.Rest.Model.ObservableMainFlow_FlowItem[]> {
		const storedUsernames = this.users().map(u => u.Id);
		const usernamesToLoad = items.map(x => x.CreateUser()).filter(x => !storedUsernames.includes(x));
		return window.database.Main_User
			.filter("it.Id in usernames", { usernames: usernamesToLoad})
			.toArray()
			.then((users) => {
				users.forEach(user => this.users.push(user));
				return items;
			});
	}

	getColor(item: Main.Flow.Rest.Model.MainFlow_FlowItem): string {
		switch (item.PostingState) {
			case window.Crm.Library.Model.PostingState.Failed:
				return 'bgm-red';
			case window.Crm.Library.Model.PostingState.Pending:
				return 'bgm-orange';
			case window.Crm.Library.Model.PostingState.Processed:
				return 'bgm-green';
			case window.Crm.Library.Model.PostingState.Skipped:
				return 'bgm-teal';
			default:
				return 'bgm-bluegray';
		}
	}
	
	getEntityTypeName(name: string): string {
		return name.split(".").reverse()[0];
	}

	async trigger(): Promise<void> {
		this.loading(true);
		await $.ajax(window.Helper.Url.resolveUrl("~/Main/BackgroundService/RunNow/FlowProcessingService?group=Main.Flow")).then(() => {
			window.Main.ViewModels.ViewModelBase.prototype.showSnackbar(window.Helper.String.getTranslatedString("TriggeredService").replace("{0}", "Flow Processing Service"));
			(this.currentSearch as any) = this.search(true, true, true)
		});
	}

	bulkActionAllowed(item: Main.Flow.Rest.Model.MainFlow_FlowItem): boolean {
		return [
			window.Crm.Library.Model.PostingState.Failed, 
			window.Crm.Library.Model.PostingState.Skipped, 
			window.Crm.Library.Model.PostingState.Blocked
		].includes(item.PostingState);
	}

	skipRequestsBulkAction() {
		return {
			Name: "SkipRequests",
			Action: async function (arrayOrQueryable) {
				try {
					await window.Helper.Confirm.genericConfirm({
						text: window.Helper.String.getTranslatedString("SkipFlowItemsMsg"),
						type: "warning",
						confirmButtonText: window.Helper.String.getTranslatedString("Skip")
					});
				} catch (e) {
					return;
				}
				let items = [];
				if (Array.isArray(arrayOrQueryable)) {
					items = arrayOrQueryable;
				} else if (arrayOrQueryable instanceof window.$data.Queryable) {
					items = await arrayOrQueryable.toArray();
				}
				try {
					items.forEach((item) => {
						window.database.attachOrGet(item);
						item.PostingState = window.Crm.Library.Model.PostingState.Skipped;
					});
					await window.database.saveChanges();
					window.Main.ViewModels.ViewModelBase.prototype.showSnackbar(window.Helper.String.getTranslatedString("MarkedSkipped").replace("{0}", items.length.toString() ));
				} catch (e) {
					window.Log.error(e);
				}
			}
		}
	}

	retryRequestsBulkAction() {
		return {
			Name: "RetryRequests",
			Action: async function (arrayOrQueryable) {
				try {
					await window.Helper.Confirm.genericConfirm({
						text: window.Helper.String.getTranslatedString("RetryFlowItemsMsg"),
						type: "info",
						confirmButtonText: window.Helper.String.getTranslatedString("Retry")
					});
				} catch (e) {
					return;
				}
				let items = [];
				if (Array.isArray(arrayOrQueryable)) {
					items = arrayOrQueryable;
				} else if (arrayOrQueryable instanceof window.$data.Queryable) {
					items = await arrayOrQueryable.toArray();
				}
				try {
					items.forEach((item) => {
						window.database.attachOrGet(item);
						item.PostingState = window.Crm.Library.Model.PostingState.Pending;
						item.Retries = 0;
						item.RetryAfter = null;
					});
					await window.database.saveChanges();
					window.Main.ViewModels.ViewModelBase.prototype.showSnackbar(window.Helper.String.getTranslatedString("MarkedRetry").replace("{0}", items.length.toString() ));
				} catch (e) {
					window.Log.error(e);
				}
			}
		}
	}
}

namespace("Main.Flow.ViewModels").FlowItemListIndexViewModel = FlowItemListIndexViewModel;