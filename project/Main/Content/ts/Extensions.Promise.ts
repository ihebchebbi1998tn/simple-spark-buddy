(function () {
	if (!Promise || !Promise.prototype) {
		return;
	}
	// @ts-ignore
	if (!Promise.prototype.fail) {
		// @ts-ignore
		Promise.prototype.fail = Promise.prototype.catch; //for backwards compatibility to $.Deferred (jQuery.fn.jquery 2.1.4)
	}
	// @ts-ignore
	if (!Promise.prototype.done) {
		// @ts-ignore
		Promise.prototype.done = Promise.prototype.then; //for backwards compatibility to $.Deferred (jQuery.fn.jquery 2.1.4)
	}
	// @ts-ignore
	if (!Promise.prototype.pipe) {
		// @ts-ignore
		Promise.prototype.pipe = Promise.prototype.then; //for backwards compatibility to $.Deferred (jQuery.fn.jquery 2.1.4)
	}
})();