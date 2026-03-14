; (function (ko) {
	ko.bindingHandlers.userDisplayName = {
		update: async function (element, valueAccessor, allBindingsAccessor, viewModel, context) {
			var options = ko.unwrap(valueAccessor());
			var username = null;
			var users = [];
			if (typeof options === "string") {
				username = options;
			} else if (options) {
				username = ko.unwrap(options.UserName);
				users = ko.unwrap(options.Users) || [];
			}
			if (!username) {
				$(element).text("");
				return;
			}
			try {
				let user = window.ko.utils.arrayFirst(users, function (u) {
					return ko.unwrap(u.Id) === username;
				});
				if (!user) {
					user = await window.database.Main_User
						.filter(function (user) {
							return user.Id === this.username;
						}, {username: username})
						.toArray()
						.then(function (arr) {
							return arr[0];
						});
				}
				if (!user) {
					$(element).text(username);
				} else {
					$(element).text(window.Helper.User.getDisplayName(user));
				}
			} catch {
				$(element).text("");
			}
		}
	};
})(window.ko);
