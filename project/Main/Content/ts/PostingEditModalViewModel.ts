import type {PostingDetailsViewModel} from "./PostingDetailsViewModel";
import {namespace} from "./namespace";

(function (ko) {
	ko.validation.rules["validJson"] = {
		validator: function (val, params) {
			try {
				JSON.parse(val);
			} catch (e) {
				return false;
			}
			return true;
		},
		message: () => window.Helper.String.getTranslatedString("InvalidJson")
	};
	ko.validation.registerExtenders();
})(window.ko);

export class PostingEditModalViewModel extends window.Main.ViewModels.ViewModelBase {

	postingDetailsViewModel: PostingDetailsViewModel;
	posting = ko.observable<Crm.Library.Rest.Model.ObservableMain_Posting>(null);
	originalSerializedEntity: string = null;
	serializedEntity = ko.observable<string>(null);
	errors = ko.validation.group(this.posting);

	constructor(postingDetailsViewModel: PostingDetailsViewModel) {
		super();
		this.postingDetailsViewModel = postingDetailsViewModel;
	}

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		const posting = await window.database.Main_Posting.find(id);
		this.originalSerializedEntity = posting.SerializedEntity;
		window.database.attach(posting);
		this.posting(posting.asKoObservable());
		this.posting().SerializedEntity.extend({validJson: true});
		this.serializedEntity(JSON.stringify(JSON.parse(posting.SerializedEntity), null, "  "));
		this.serializedEntity.subscribe(x => this.posting().SerializedEntity(x));
	};

	dispose(): void {
		window.database.detach(this.posting());
	};

	async save(reset: boolean): Promise<void> {
		this.loading(true);
		const posting = this.posting().innerInstance;
		if (posting.PostingState === window.Crm.Library.Model.PostingState.Blocked || posting.PostingState === window.Crm.Library.Model.PostingState.Failed) {
			posting.PostingState = window.Crm.Library.Model.PostingState.Pending;
			posting.StateDetails = null;
		}
		posting.Retries = 0;
		posting.RetryAfter = null;
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}
		if (reset === true) {
			posting.SerializedEntity = this.originalSerializedEntity;
		} else {
			posting.SerializedEntity = JSON.stringify(JSON.parse(this.serializedEntity()), null, "");
		}
		await window.database.Main_Posting
			.filter(function (transaction) {
				return transaction.TransactionId === this.transactionId && transaction.Id !== this.postingId;
			}, {transactionId: posting.TransactionId, postingId: posting.Id})
			.forEach(x => {
				window.database.attach(x);
				if (x.PostingState === window.Crm.Library.Model.PostingState.Blocked || posting.PostingState === window.Crm.Library.Model.PostingState.Failed) {
					x.PostingState = window.Crm.Library.Model.PostingState.Pending;
					x.StateDetails = null;
				}
				x.Retries = 0;
				x.RetryAfter = null;
			});
		await window.database.saveChanges();
		if (this.postingDetailsViewModel.reload) {
			await this.postingDetailsViewModel.reload();
		}
		const postingTab = ko.unwrap(this.postingDetailsViewModel.tabs()["tab-postings"]);
		if (postingTab && postingTab.reload) {
			await postingTab.reload();
		}
		this.loading(false);
		$(".modal:visible").modal("hide");
		$(".tab-nav a[href='#tab-postings']").tab("show");
	};

	async skip(): Promise<void> {
		const posting = this.posting().innerInstance;
		posting.PostingState = window.Crm.Library.Model.PostingState.Skipped;
	posting.StateDetails = "Skipped by " + window.Helper.User.getCurrentUserName() + " on " + new Date();
		await this.save(true);
	}
}

namespace("Main.ViewModels").PostingEditModalViewModel = PostingEditModalViewModel;