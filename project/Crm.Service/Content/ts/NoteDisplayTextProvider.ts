window.Crm.ViewModels.NoteViewModel.registerDisplayTextProvider("ServiceOrderDispatchCompletedNote", function(note) {
	const text = window.Helper.String.getTranslatedString("DispatchCompleted");
	return Promise.resolve(text);
});
window.Crm.ViewModels.NoteViewModel.registerDisplayTextProvider("ServiceOrderDispatchReopenedNote", function(note) {
	const text = window.Helper.String.getTranslatedString("DispatchReopened");
	return Promise.resolve(text);
});
window.Crm.ViewModels.NoteViewModel.registerDisplayTextProvider("ServiceOrderHeadCreatedNote", function(note) {
	const text = window.Helper.String.getTranslatedString("ServiceOrderHeadCreated");
	return Promise.resolve(text);
});
window.Crm.ViewModels.NoteViewModel.registerDisplayTextProvider("ServiceCaseCreatedNote", function (note) {
	const text = window.Helper.String.getTranslatedString("ServiceCaseCreated");
	return Promise.resolve(text);
});
window.Crm.ViewModels.NoteViewModel.registerDisplayTextProvider("ServiceContractStatusChangedNote", function(note) {
	const text = window.Helper.String.getTranslatedString("ServiceContractStatusSet").replace("{0}", note.Text);
	return Promise.resolve(text);
});
window.Crm.ViewModels.NoteViewModel.registerDisplayTextProvider("ServiceCaseStatusChangedNote", function(note) {
	const text = window.Helper.String.getTranslatedString("ServiceCaseStatusSet").replace("{0}", note.Text);
	return Promise.resolve(text);
});
window.Crm.ViewModels.NoteViewModel.registerDisplayTextProvider("ServiceCaseInformationChangedNote", function(note) {
	let text = "";
	
	const obj = JSON.parse(note.Text);
	for (let key in obj) {
		let noteType: string = "ServiceCase" + key + "Changed";
		let value = obj[key];
		if (obj[key] === null) {
			value = window.Helper.String.getTranslatedString("Unspecified");
		}
		text += window.Helper.String.getTranslatedString(noteType).replace("{0}", value) + "\r\n";
	}
	return Promise.resolve(text);
});
window.Crm.ViewModels.NoteViewModel.registerDisplayTextProvider("OrderStatusChangedNote", function(note) {
	const key = note.Text;
	return window.Helper.Lookup.getLocalizedArrayMap("CrmService_ServiceOrderStatus").then(function(map) {
		if (map && map[key]) {
			return window.Helper.String.getTranslatedString("ServiceOrderStatusSetTo").replace("{0}", map[key].Value);
		}
		return "";
	}).then(function(text) {
		if (key === "ReadyForInvoice" && note.Meta) {
			const split = note.Meta.split(";");
			const invoiceReasonKey = split[0];
			const invoiceRemark = split[1];
			text += "\r\n";
			return window.Helper.Lookup.getLocalizedArrayMap("CrmService_ServiceOrderInvoiceReason").then(function(map) {
				if (map && map[invoiceReasonKey]) {
					text += window.Helper.String.getTranslatedString("InvoiceReason");
					text += ": " + map[invoiceReasonKey].Value;
					if (invoiceRemark) {
						text += " (" + invoiceRemark + ")";
					}
				}
				return text;
			});
		}
		return text || key;
	});
});