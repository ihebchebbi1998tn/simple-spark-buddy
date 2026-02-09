;
(function () {
	var getContactLink = window.Helper.Note.getContactLink;
	window.Helper.Note.getContactLink = function (note) {
		if (note.ContactType() === "Dispatch" && window.AuthorizationManager.isAuthorizedForAction("Dispatch", "Read")) {
			let link = "#/Crm.Service/Dispatch/DetailsTemplate/" + note.ExtensionValues().DispatchId();
			if (note.NoteType() == "UserNote")
				return link + '?tab=tab-notes';
			return link;
		}
		if (note.ContactType() === "ServiceOrderTemplate" && window.AuthorizationManager.isAuthorizedForAction("ServiceOrderTemplate", "Read")) {
			let link = "#/Crm.Service/ServiceOrderTemplate/Details/" + note.ContactId();
			if (note.NoteType() == "UserNote")
				return link + '?tab=tab-notes';
		}
		return getContactLink.call(this, note);
	};
})();