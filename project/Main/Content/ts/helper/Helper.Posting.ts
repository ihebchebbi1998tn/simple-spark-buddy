import {HelperString} from "./Helper.String";
import {HelperUrl} from "./Helper.Url";

function isEditAllowed(posting: Crm.Library.Rest.Model.ObservableMain_Posting): boolean {
	return posting.PostingState() === window.Crm.Library.Model.PostingState.Pending ||
		posting.PostingState() === window.Crm.Library.Model.PostingState.Failed ||
		posting.PostingState() === window.Crm.Library.Model.PostingState.Blocked;
}

async function trigger(): Promise<void> {
	await fetch(HelperUrl.resolveUrl("~/Crm.Offline/Sync/WaitForPostings"));
	window.Main.ViewModels.ViewModelBase.prototype.showSnackbar(HelperString.getTranslatedString("TriggeredService").replace("{0}", "Posting Service"));
}

const HelperPosting = {
	isEditAllowed,
	trigger
};

export {HelperPosting}


