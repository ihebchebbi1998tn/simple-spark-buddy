;
(function() {

	type ServiceOrderTimePostingCallback = (serviceOrderTimePosting: Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting) => void;
	
	function getOverlappingServiceOrderTimePosting(id: string, username: string, from: Date, to: Date, callback: ServiceOrderTimePostingCallback): void {
		if (!username || !from || !to || from > to || !window.database.CrmService_ServiceOrderTimePosting || window.Crm.Service.Settings.ServiceOrderTimePosting.AllowOverlap) {
			callback(null);
			return;
		}
		window.database.CrmService_ServiceOrderTimePosting
			.include("ServiceOrder")
			.first(function(timePosting) {
					return timePosting.Username === this.username &&
						timePosting.From < this.to &&
						timePosting.To > this.from &&
						timePosting.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Used &&
						timePosting.Id !== this.id;
				},
				{ id: id, username: username, from: from, to: to })
			.then(function(result) {
				return window.Helper.Culture.languageCulture().then(function(language) {
					return Helper.Article.loadArticleDescriptionsMap([result.ItemNo], language);
				}).then(function(description) {
					// @ts-ignore
					result.ItemDescription = description[result.ItemNo];
					callback(result);
				});
			})
			.catch(function() {
				callback(null);
			});
	}

	const denyRevalidationWithinMilliseconds = 1000;
	const validatingEntries = new Map();

	var getValidationFunction = window.Helper.TimeEntry.OverlappingTimeEntryValidator.getValidationFunction;
	window.Helper.TimeEntry.OverlappingTimeEntryValidator.getValidationFunction = function(timeEntry) {
		timeEntry = window.ko.unwrap(timeEntry);
		return function (val, params, callback) {
			const id = timeEntry.Id();
			if (validatingEntries.has(id)) {
				const previousAttemptDate = validatingEntries.get(id);
				const diff = new Date().getTime() - previousAttemptDate.getTime();
				if (diff < denyRevalidationWithinMilliseconds) {
					return;
				}
			}
			validatingEntries.set(id, new Date());			

			getOverlappingServiceOrderTimePosting(timeEntry.Id(),
				window.ko.unwrap(timeEntry.ResponsibleUser) || ko.unwrap(timeEntry.Username),
				timeEntry.From(),
				timeEntry.To(),
				function (overlappingTimePosting) {
					validatingEntries.delete(timeEntry.Id());
					if (!overlappingTimePosting) {
						getValidationFunction(timeEntry)(val, params, callback);
					} else {
						var message = window.Helper.String.getTranslatedString("OverlappingByFromOrderNo")
							// @ts-ignore
							.replace("{0}", overlappingTimePosting.ItemDescription)
							.replace("{1}",
								window.Globalize.formatDate(overlappingTimePosting.From, { datetime: "medium" }))
							.replace("{2}", window.Globalize.formatDate(overlappingTimePosting.To, { datetime: "medium" }))
							.replace("{3}", overlappingTimePosting.ServiceOrder.OrderNo);
						callback({ isValid: false, message: message });
					}
				});
		};
	}
})();