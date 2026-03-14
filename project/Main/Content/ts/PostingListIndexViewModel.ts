import {namespace} from "./namespace";

export class PostingListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Main.Controllers.OData.Transaction, Main.Controllers.OData.ObservableTransaction> {
	users: Main.Rest.Model.Main_User[];

	constructor() {
		super("Main_Posting", "CreateDate", "DESC");
		this.addBookmarks();
		this.selectedItems.subscribe((items) => {
			let actions: BulkAction[] = [];
			actions.push(this.retryTransactionsBulkAction);
			let canUseSkip = items.filter(i => i.TransactionState() !== window.Crm.Library.Model.PostingState.Failed).length === 0;
			if (canUseSkip) {
				actions.push(this.skipTransactionsBulkAction);
			}
			this.bulkActions = ko.observableArray(actions);
		});
	}

	getBaseQuery(): $data.Queryable<any> {
		return window.database.Main_Posting.GetTransactions();
	};

	getFilterQuery(addJoins?: boolean, addOrderBy?: boolean, additionalFilter?: {}): $data.Queryable<any> {
		if (additionalFilter) {
			if (additionalFilter.hasOwnProperty("it.Id in this.ids")) {
				additionalFilter["it.TransactionId in this.ids"] = additionalFilter["it.Id in this.ids"];
				delete additionalFilter["it.Id in this.ids"];
			}
		}
		return super.getFilterQuery(addJoins, addOrderBy, additionalFilter);
	};

	addBookmarks(): void {
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Status"),
			Name: window.Helper.String.getTranslatedString("All"),
			Key: "AllPostings",
			Expression: query => query
		});
		const openPostings = {
			Category: window.Helper.String.getTranslatedString("Status"),
			Name: window.Helper.String.getTranslatedString("OpenTransactions"),
			Key: "OpenTransactions",
			Expression: query => query.filter(function (transaction) {
				return transaction.PostingState in this.states;
			}, {states: [window.Crm.Library.Model.PostingState.Pending, window.Crm.Library.Model.PostingState.Failed, window.Crm.Library.Model.PostingState.Blocked]})
		};
		this.bookmarks.push(openPostings);
		this.bookmark(openPostings);
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.users = await window.database.Main_User.toArray();
	};

	async trigger(): Promise<void> {
		this.loading(true);
		await window.Helper.Posting.trigger();
		this.currentSearch = this.search(false, true);
	};

	bulkActionAllowed(item: Main.Controllers.OData.ObservableTransaction): boolean {
		const status = item.TransactionState();
		return [
			window.Crm.Library.Model.PostingState.Failed,
			window.Crm.Library.Model.PostingState.Skipped,
			window.Crm.Library.Model.PostingState.Blocked
		].includes(status);
	};

	toggleSelectAllItems(): void {
		if (this.allItemsSelected()) {
			super.toggleSelectAllItems();
		} else {
			this.selectedItems([]);
			this.items().filter(this.bulkActionAllowed).forEach(item => {
				this.selectedItems().push(item);
			});
			this.selectedItems.valueHasMutated();
			this.allItemsSelected(true);
		}
	};

	async applySkipTransactionsBulkAction(): Promise<void> {
		this.loading(true);
		const bulkActionParameter = this.allItemsSelected() === true ? await this.getFilterQuery().toArray() : this.selectedItems();
		try {
			await this.skipTransactionsBulkAction.Action(bulkActionParameter)
			this.loading(false);
			$(".modal:visible").modal("hide");
		} catch (e) {
			this.loading(false);
		}
	};

	async skipTransactions(transactionIds: string[]): Promise<void> {
		await window.database.Main_Posting
			.filter(function (posting) {
					return posting.TransactionId in this.transactionIds && posting.PostingState in this.states;
				},
				{transactionIds: transactionIds, states: [window.Crm.Library.Model.PostingState.Failed]})
			.forEach(function (posting) {
				window.database.attachOrGet(posting);
				posting.PostingState = window.Crm.Library.Model.PostingState.Skipped;
			});
		await window.database.saveChanges();
	}

	skipTransactionsBulkAction = {
		Name: "SkipTransactions",
		Action: (array: Main.Controllers.OData.ObservableTransaction[]) => {
			return this.skipTransactions(array.map(function (x) {
				return x.Id();
			}));
		},
		Modal: {
			Target: "#smModal",
			Route: "Main/PostingList/SkipTransactions"
		}
	};

	async retryTransactions(transactionIds: string[]): Promise<void> {
		try {
			let text = transactionIds.length === 1
				? window.Helper.String.getTranslatedString("RetryTransactionConfirmationQuestion")
				: window.Helper.String.getTranslatedString("RetryTransactionsConfirmationQuestion");
			await window.Helper.Confirm.genericConfirm({
				text: text,
				type: "warning",
				confirmButtonText: window.Helper.String.getTranslatedString("Confirm")
			});
		} catch {
			return;
		}
		const postings = await window.database.Main_Posting.filter("it.TransactionId in this.ids", {ids: transactionIds}).toArray();
		postings.forEach((posting) => {
			window.database.attachOrGet(posting);
			posting.PostingState = window.Crm.Library.Model.PostingState.Pending;
			posting.Retries = 0;
			posting.RetryAfter = null;
		});
		await window.database.saveChanges();
		await window.Helper.Posting.trigger();
	}

	retryTransactionsBulkAction = {
		Name: "Retry",
		Action: async (arrayOrQueryable: any) => {
			const items = Array.isArray(arrayOrQueryable) ? arrayOrQueryable : await arrayOrQueryable.toArray();
			const ids = items.map(x => x.Id ?? x.Id());
			return this.retryTransactions(ids);
		}
	};
}

namespace("Main.ViewModels").PostingListIndexViewModel = PostingListIndexViewModel;