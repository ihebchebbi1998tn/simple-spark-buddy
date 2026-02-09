ko.validation.rules["minArrayLength"] = {
	validator: function (val, minLength) {
		return !$.isArray(val) || val.length == 0 || val.length >= minLength;
	},
	message: 'Please select at least {0} choices.'
};
ko.validation.rules["maxArrayLength"] = {
	validator: function (val, maxLength) {
		return !$.isArray(val) || val.length == 0 || val.length <= maxLength;
	},
	message: 'Please select not more than {0} choices.'
};
(function() {
	var baseRequired = window.ko.validation.rules["required"];
	window.ko.validation.rules["required"] = {
		message: baseRequired.message,
		validator: function(val, required) {
			var result = baseRequired.validator(val, required);
			if (result && val === window.Helper.String.emptyGuid()) {
				return false;
			}
			return result;
		}
	};
})();

ko.validation.rules["number"] = {
	validator: function (value, validate) {
		if (!validate) { return true; }
		return ko.validation.utils.isEmptyVal(value) || !isNaN(value) || !isNaN(window.Globalize.parseNumber(value));
	},
	message: 'Please enter a number.'
};

/*
 * This rules checks the given array of objects/observables and returns 
 * true if at least one of the elements validates agains the the default
 * 'required' rules
 * 
 * Example:
 * 
 *
 * self.mobilePhone.extend({ requiresOneOf: [self.homePhone, self.mobilePhone] });
 * self.homePhone.extend({ requiresOneOf: [self.homePhone, self.mobilePhone] }); 
 *
 * taken from https://github.com/Knockout-Contrib/Knockout-Validation/wiki/User-Contributed-Rules
*/
ko.validation.rules['requiresOneOf'] = {
	validator: function (val, fields) {
		var self = this;
		var anyOne = ko.utils.arrayFirst(fields, function (field) {
			var stringTrimRegEx = /^\s+|\s+$/g;
      var testVal;
			var val = ko.unwrap(field);
			if (val === undefined || val === null) {
				return false;
			}
			testVal = val;
			if (typeof (val) == "string") {
				testVal = val.replace(stringTrimRegEx, '');
			}
			return ((testVal + '').length > 0);
		});
		return !!anyOne;
	},
	message: 'One of these fields is required'
};
ko.validation.rules['unique'] = {
	async: true,
// @ts-ignore
	validator: function (val, parameters, callback) {
		var query = parameters[0];
		var keyProperty = window.Helper.Database.getKeyProperty(query.expression.storageModel.ItemName);
		if (!val){
			callback({ isValid: true });
			return;
		}
		var id = window.ko.unwrap(parameters[2]) || null;
		var idIsValue = window.ko.unwrap(parameters[4]) || false;
		var queryString = idIsValue ? "it." + parameters[1] + " == this.value" : "it." + parameters[1] + " == this.value && it." + keyProperty + " != this.id";
		query = query.filter(queryString, { value: val, id: id, parameters: parameters});
		query.take(1).toArray(
			function (results) {
				if (results.length === 0) {
					callback({ isValid: true });
				} else {
					callback({ isValid: false, message: window.Helper.String.getTranslatedString("RuleViolation.Unique").replace("{0}", window.Helper.String.getTranslatedString(parameters[3] || parameters[1])) });
				}
			}, { parameters: parameters});
	}
};
$(function() {
	// @ts-ignore
	ko.validation.configuration.insertMessages = false;
	ko.validation.registerExtenders();
});
(function() {
	const origGroup = window.ko.validation.group;
	// @ts-ignore
	window.ko.validation.group = function(obj, options) {
		const result = origGroup(obj, options);
		const origShowAllMessages = result.showAllMessages;
		// @ts-ignore
		result.showAllMessages = function(show) {
			const messages = this;
			origShowAllMessages(show);
			if (show !== false) {
				messages.forEach(function (x) {
					if (x.isValid()) {
						return;
					}
					let msg = "Validation: " + x.error();
					msg = x.rules().reduce(function(msg, rule) { return msg + " (rule: " + (rule.propertyName || "unknown property") + " " + rule.rule + ")"; }, msg);
					console.log(msg);
				});
			}
		};
		return result;
	};
})();
