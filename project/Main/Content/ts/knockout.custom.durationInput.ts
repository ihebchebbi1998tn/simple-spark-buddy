ko.bindingHandlers.durationInput = {
	init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
		const el = $(element);
		const close = el.parent().find('[data-action="close"]');
		let prevValue = '';
		close.on('click', function () {
			el.parent().trigger('click.bs.dropdown');
		});
		const clear = el.parent().find('[data-action="clear"]');
		clear.on('click', function () {
			valueAccessor()('');
		});
		el.on("change",
			function () {
				valueAccessor()(el.val());
			});
		el.on("keyup",
			function () {
				const v = el.val().toString();
				if (v.length > 2 && !v.includes(':')) {
					let h = v.substring(0,2);
					let m = v.substring(2);
					el.val(h + ':' + m);
				}
				if (!/^\d{0,2}(?::\d{0,2})?$/.test(el.val().toString()) && el.val() != '') {
					el.val(prevValue);
				}
			});
		el.on("keydown",
			function () {
				const v = el.val().toString();
				if (/^\d{0,2}(?::\d{0,2})?$/.test(v) && el.val() != '') {
					prevValue = el.val().toString();
				}
			});
	},
	update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
		const value = ko.unwrap(valueAccessor());
		if (value != null) {
			const options = ko.unwrap(allBindingsAccessor.get("datePickerOptions") || {});
			const config = options.config || {};
			const stepping = config.stepping || 5;
			let duration = window.moment.duration(value);
			const roundedDuration = window.moment.duration();
			roundedDuration.add((Math.round(duration.minutes() / stepping) * stepping), 'm');
			roundedDuration.add(duration.hours(), 'h');
			roundedDuration.add(duration.days(), 'd');
			roundedDuration.add(duration.months(), 'months');
			roundedDuration.add(duration.years(), 'years');
			valueAccessor()(roundedDuration.toString());
			const formattedDuration = roundedDuration.format("HH:mm", {trim: false});
			// @ts-ignore
			$(element).attr('value', roundedDuration);
			$(element).val(formattedDuration);
		}
	}
}