import {namespace} from "./namespace";
import type {PostingDetailsViewModel} from "./PostingDetailsViewModel";

export class PostingDetailsPostingsTabViewModel extends window.Main.ViewModels.GenericListViewModel<Crm.Library.Rest.Model.Main_Posting, Crm.Library.Rest.Model.ObservableMain_Posting> {
	parentViewModel: PostingDetailsViewModel;
	transactionId = ko.observable<string>(null);
	users: Main.Rest.Model.Main_User[] = [];

	constructor(parentViewModel: PostingDetailsViewModel) {
		super("Main_Posting", "Id", "ASC");
		this.parentViewModel = parentViewModel;
	}

	async init(): Promise<void> {
		this.transactionId(this.parentViewModel.transactionId());
		this.getFilter("TransactionId").extend({filterOperator: "=="})(this.transactionId());
		await super.init();
		this.users = await window.database.Main_User.toArray();
	};

	reload(): void {
		this.currentSearch = this.search(false, true);
	};

	registerEventHandlers = function () {
	};
}

namespace("Main.ViewModels").PostingDetailsPostingsTabViewModel = PostingDetailsPostingsTabViewModel;