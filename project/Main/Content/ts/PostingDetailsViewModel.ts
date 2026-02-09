import {namespace} from "./namespace";
import {Breadcrumb} from "./breadcrumbs";

export class PostingDetailsViewModel extends window.Main.ViewModels.ViewModelBase {
	tabs = ko.observable<{}>({});
	transactionId = ko.observable<string>();
	transaction = ko.observable<Main.Controllers.OData.ObservableTransaction>();
	canRetryTransaction = window.ko.pureComputed(() => {
		const status = this.transaction().TransactionState();
		return [
			window.Crm.Library.Model.PostingState.Failed,
			window.Crm.Library.Model.PostingState.Skipped,
			window.Crm.Library.Model.PostingState.Blocked
		].includes(status);
	});
	title = ko.observable<string>(null);

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.transactionId(id);
		await this.load();
		this.setBreadcrumbs();
		this.title(id);
	};

	async load(): Promise<void> {
		const transaction = await window.database.Main_Posting
			.GetTransactions()
			.filter("it.TransactionId === this.id", {id: this.transactionId()})
			.orderBy(x => x.CreateDate)
			.first();
		this.transaction(transaction.asKoObservable());
	};

	async reload(): Promise<void> {
		this.loading(true);
		await this.load();
		this.loading(false);
	};

	async retryTransaction(): Promise<void> {
		try {
			await window.Helper.Confirm.genericConfirm({
				text: window.Helper.String.getTranslatedString("RetryTransactionConfirmationQuestion"),
				type: "warning",
				confirmButtonText: window.Helper.String.getTranslatedString("Confirm")
			});
		} catch {
			return;
		}
		const postings = await window.database.Main_Posting.filter("it.TransactionId == this.id && it.PostingState in this.postingStates", {id: this.transaction().Id(), postingStates: [window.Crm.Library.Model.PostingState.Blocked, window.Crm.Library.Model.PostingState.Failed] }).toArray();
		postings.forEach((posting) => {
			window.database.attachOrGet(posting);
			posting.PostingState = window.Crm.Library.Model.PostingState.Pending;
			posting.Retries = 0;
			posting.RetryAfter = null;
			posting.StateDetails = null;
		});
		await window.database.saveChanges();
		await window.Helper.Posting.trigger();
	}
	
	setBreadcrumbs(): void {
		if (!window.breadcrumbsViewModel) {
			return;
		}
		window.breadcrumbsViewModel.setCustomBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("Transactions"), "WebAPI::Posting", "#/Main/PostingList/IndexTemplate"),
			new Breadcrumb(window.Helper.String.getTranslatedString("Transaction"))
		]);
	};
}

namespace("Main.ViewModels").PostingDetailsViewModel = PostingDetailsViewModel;