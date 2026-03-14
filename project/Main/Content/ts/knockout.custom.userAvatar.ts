;
(function(ko) {
	ko.bindingHandlers.userAvatar = {
		update: function(element, valueAccessor, allBindingsAccessor, viewModel, context) {
			var value = ko.unwrap(valueAccessor());
			if (!value) {
				$(element).hide();
				return;
			}
			var promise;
			if (typeof(value) === "string") {
				promise = window.database.Main_User.include("expandAvatar").find(value);
			} else if (value && value.innerInstance) {
				promise = $.Deferred().resolve(window.Helper.Database.getDatabaseEntity(value)).promise();
			} else {
				throw "unsupported value";
			}
			promise.then(function (user) {
					var avatarUrl = window.Helper.Url.resolveUrl("~/Plugins/Main/Content/img/avatar.jpg");
					var blob = user.Avatar !== null ? window.Helper.String.base64toBlob(user.Avatar) : null;
					if (blob !== null) {
						avatarUrl = window.URL.createObjectURL(blob);
						ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
							window.URL.revokeObjectURL(avatarUrl);
						});
					}
					if (element.tagName === "IMG") {
						$(element).attr("src", avatarUrl);
					} else if (element.tagName === "DIV") {
						$(element).css("background-image", "url(" + avatarUrl + ")");
					}
					$(element).show();
				})
				.fail(function() {
					$(element).hide();
				});
		}
	};
})(window.ko);