;
(function() {
	const getContactLink = window.Helper.Person.getContactLink;
	window.Helper.Person.getContactLink = function(person) {
		if (person.ParentType() === "ServiceObject" && window.AuthorizationManager.isAuthorizedForAction("ServiceObject", "Read")) {
			return "#/Crm.Service/ServiceObject/DetailsTemplate/" + person.ParentId();
		}
		return getContactLink(person);
	};
})();