ko.bindingHandlers.dateText = {
	update: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
		var value = ko.unwrap(valueAccessor());
		var pattern = { date: "medium" };
		var useTimeAgo = null;
		var defaultText = "";
		let timeZone = null;
		if (value) {
			if (value.hasOwnProperty("pattern")) {
				pattern = ko.unwrap(value.pattern) || pattern;
			}
			if (value.hasOwnProperty("useTimeAgo")) {
				useTimeAgo = ko.unwrap(value.useTimeAgo) || useTimeAgo;
			}
			if (value.hasOwnProperty("defaultText")) {
				defaultText = ko.unwrap(value.defaultText) || defaultText;
			}
			if (value.hasOwnProperty("timeZone")) {
				timeZone = ko.unwrap(value.timeZone);
			}
			if (value.hasOwnProperty("value")) {
				value = ko.unwrap(value.value);
			}
		}

		if (value) {
			var date = null;
			if (typeof (value) == 'string') {
				var parsedDate = new Date(value);
				if (!isNaN(parsedDate.getTime())) {
					date = parsedDate;
				} else {
					var timestampRegex = /(\d+\.)?(\d+):(\d+):(\d+)/gi;
					var timestampRegexMatches = timestampRegex.exec(value);
					if (timestampRegexMatches != null) {
						date = new Date();
						date.setHours(parseInt(timestampRegexMatches[2]));
						date.setMinutes(parseInt(timestampRegexMatches[3]));
						date.setSeconds(parseInt(timestampRegexMatches[4]));
					}
				}
			} else {
				date = value;
			}
			if (!date) {
				$(element).text(value);
			} else {
				if(timeZone) {
					date = Helper.Date.convertLocalDateToTimeZone(date, timeZone);
				}
				if (!window.Globalize.locale()) {
					window.Globalize.locale(window.Helper.getMetadata("CurrentCulture"));
				}
				if (useTimeAgo) {
					var dateText = timeAgo(date, useTimeAgo);
					if(dateText){
						$(element).text(dateText)
					}else{
						$(element).text(window.Globalize.formatDate(date, pattern));
					}
				} else {
					$(element).text(window.Globalize.formatDate(date, pattern));
				}
			}
		} else {
			$(element).text(defaultText);
		}
	}
};
function timeAgo(date, inPeriod) {
	var diff = moment.duration(moment().diff(date));
	if (diff.asSeconds() < 1) {
		return window.Helper.String.getTranslatedString("TimeAgoRightNow");
	}
	if (diff.asSeconds() == 1) {
		return window.Helper.String.getTranslatedString("TimeAgoOneSecond");
	}
	if (diff.asMinutes() < 1) {
		return window.Helper.String.getTranslatedString("TimeAgoSeconds").replace("{0}", Math.floor(diff.asSeconds()).toString());
	}
	if (diff.asMinutes() < 2) {
		return window.Helper.String.getTranslatedString("TimeAgoOneMinute");
	}
	if (diff.asMinutes() < 60) {
		return window.Helper.String.getTranslatedString("TimeAgoMinutes").replace("{0}", Math.floor(diff.asMinutes()).toString());
	}
	if(inPeriod === "Hour"){
		return null;
	}
	if (diff.asMinutes() < 120) {
		return window.Helper.String.getTranslatedString("TimeAgoOneHour");
	}
	if (diff.asHours() < 24) {
		return window.Helper.String.getTranslatedString("TimeAgoHours").replace("{0}", Math.floor(diff.asHours()).toString());
	}
	if(inPeriod === "Day"){
		return null;
	}
	if (diff.asDays() == 1) {
		return window.Helper.String.getTranslatedString("TimeAgoYesterday");
	}
	if (diff.asDays() < 7) {
		return window.Helper.String.getTranslatedString("TimeAgoDays").replace("{0}", Math.floor(diff.asDays()).toString());
	}
	if(inPeriod === "Week"){
		return null;
	}
	if (diff.asDays() < 14) {
		return window.Helper.String.getTranslatedString("TimeAgoLastWeek");
	}
	if (diff.asDays() < 21) {
		return window.Helper.String.getTranslatedString("TimeAgoTwoWeeksAgo");
	}
	if (diff.asDays() < 28) {
		return window.Helper.String.getTranslatedString("TimeAgoThreeWeeksAgo");
	}
	if(inPeriod === "Month"){
		return null;
	}
	if (diff.asMonths() < 2) {
		return window.Helper.String.getTranslatedString("TimeAgoLastMonth");
	}
	if (diff.asYears() < 1) {
		return window.Helper.String.getTranslatedString("TimeAgoMonths").replace("{0}", Math.floor(diff.asMonths()).toString());
	}
	if(inPeriod === "Year"){
		return null;
	}
	if (diff.asYears() < 2) {
		return window.Helper.String.getTranslatedString("TimeAgoLastYear");
	}
	return window.Helper.String.getTranslatedString("TimeAgoYears").replace("{0}", Math.floor(diff.asYears()).toString());
}