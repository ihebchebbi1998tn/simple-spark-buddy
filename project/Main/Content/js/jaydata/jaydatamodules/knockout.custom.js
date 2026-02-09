;
(function($data) {
	// for backwards compatibility
	$data.KoObservableEntity.prototype.asKoObservable = function() {
		console.warn("manually called asKoObservable on a ko observable");
		return this;
	};
})($data);