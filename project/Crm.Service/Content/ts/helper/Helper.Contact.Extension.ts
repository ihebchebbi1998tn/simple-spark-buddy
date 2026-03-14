;
(function() {
	const getLink = window.Helper.Contact.getLink;
	window.Helper.Contact.getLink = function(contactId, contactType) {
		if (contactType === "Installation" && window.AuthorizationManager.isAuthorizedForAction("Installation", "Read")) {
			return "#/Crm.Service/Installation/DetailsTemplate/" + contactId;
		}
		if (contactType === "ServiceOrder" && window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "Read")) {
			return "#/Crm.Service/ServiceOrder/DetailsTemplate/" + contactId;
		}
		if (contactType === "ServiceCase" && window.AuthorizationManager.isAuthorizedForAction("ServiceCase", "Read")) {
			return "#/Crm.Service/ServiceCase/DetailsTemplate/" + contactId;
		}
		if (contactType === "ServiceContract" && window.AuthorizationManager.isAuthorizedForAction("ServiceContract", "Read")) {
			return "#/Crm.Service/ServiceContract/DetailsTemplate/" + contactId;
		}
		return getLink.call(this, contactId, contactType);
	};
})();