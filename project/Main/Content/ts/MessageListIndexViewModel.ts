import {namespace} from "./namespace";

export class MessageListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Main.Rest.Model.Main_Message, Main.Rest.Model.ObservableMain_Message> {
	currentUserName: string;

	constructor() {
		super("Main_Message", "CreateDate", "DESC");
		this.currentUserName = window.Helper.User.getCurrentUserName();
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("All"),
			Key: "All",
			Expression: query => query
		});
		const pendingBookmark = {
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("Pending"),
			Key: "Pending",
			Expression: query => query.filter("it.State === this.state", {state: window.Main.Model.Enums.MessageState.Pending})
		};
		this.bookmarks.push(pendingBookmark);
		this.bookmark(pendingBookmark);
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("Failed"),
			Key: "Failed",
			Expression: query => query.filter("it.State === this.state", {state: window.Main.Model.Enums.MessageState.Failed})
		});
		this.bulkActions.push({
			Name: "Retry",
			Action: async (arrayOrQueryable: Main.Rest.Model.ObservableMain_Message[] | $data.Queryable<Main.Rest.Model.Main_Message>): Promise<void> => {
				if (Array.isArray(arrayOrQueryable)) {
					arrayOrQueryable.forEach(message => this.retry(message.innerInstance));
					await window.database.saveChanges();
				} else if (arrayOrQueryable instanceof window.$data.Queryable) {
					const pageSize = 25;
					let page = 0;
					const processNextPage = async () => {
						let messages = await arrayOrQueryable
							.orderBy(function (x) {
								return x.Id;
							})
							.skip(page * pageSize)
							.take(pageSize)
							.toArray();
						messages.forEach(message => this.retry(message));
						await window.database.saveChanges();
						if (messages.length === pageSize) {
							page++;
							await processNextPage();
						}
					};
					await processNextPage();
				} else {
					throw "parameter should be array or queryable";
				}
			}
		});
	}

	getStateBackgroundColor(messageState: Main.Model.Enums.MessageState): string {
		if (messageState === window.Main.Model.Enums.MessageState.Pending) {
			return "bgm-gray";
		}
		if (messageState === window.Main.Model.Enums.MessageState.Dispatched) {
			return "bgm-green";
		}
		if (messageState === window.Main.Model.Enums.MessageState.Failed) {
			return "bgm-red";
		}
		return null;
	};

	getStateColor(messageState: Main.Model.Enums.MessageState): string {
		if (messageState === window.Main.Model.Enums.MessageState.Pending) {
			return "c-gray";
		}
		if (messageState === window.Main.Model.Enums.MessageState.Dispatched) {
			return "c-green";
		}
		if (messageState === window.Main.Model.Enums.MessageState.Failed) {
			return "c-red";
		}
		return null;
	};

	retry(message: Main.Rest.Model.Main_Message): void {
		window.database.attachOrGet(message);
		message.ErrorMessage = null;
		message.State = window.Main.Model.Enums.MessageState.Pending;
	};

	async retryAction(message: Main.Rest.Model.ObservableMain_Message): Promise<void> {
		this.loading(true);
		this.retry(message.innerInstance);
		await window.database.saveChanges();
		this.loading(false);
	};

	async trigger(): Promise<void> {
		this.loading(true);
		await $.ajax(window.Helper.Url.resolveUrl("~/Main/BackgroundService/RunNow/EmailAgent?group=Core"));
		this.showSnackbar(window.Helper.String.getTranslatedString("TriggeredService").replace("{0}", "EmailAgent"));
		this.loading(false);
	};
}

namespace("Main.ViewModels").MessageListIndexViewModel = MessageListIndexViewModel;