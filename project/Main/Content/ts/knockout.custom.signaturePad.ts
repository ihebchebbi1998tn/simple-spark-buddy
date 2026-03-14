$(function () {
	$(window).on('resize', function () {
		$('.pad').trigger('padresized');
	});
	$(document).on('shown.bs.tab', function () {
		$('.pad').trigger('padresized');
	});
	$(document).on('shown.bs.modal', function () {
		$('.pad').trigger('padresized');
	});
	const signatureLinePercentFromBottom = 20;
	const defaultMaxWidth = 2000;
	const widthHeightRatio = 3;
	const padding = 30;

	function getCanvas(container) {
		var pad = document.createElement('canvas');
		pad.setAttribute('class', 'pad');
		var maxWidth = $(container).data("maxWidth") || defaultMaxWidth;
		var width = Math.min(maxWidth, Math.min($(container).width(), $(document).width() - padding));
		var height = Math.round(width / widthHeightRatio);
		pad.setAttribute('width', width.toString());
		pad.setAttribute('height', height.toString());
		return pad;
	}

	function createDrawItSignaturePad(container) {
		var pad = $('.pad', container);
		var height = +pad.attr('height');
		var signatureLineFromTopMultiplier = 1 - 0.01 * signatureLinePercentFromBottom;
		var lineTop = Math.round(height * signatureLineFromTopMultiplier);
		if ($(container).data()) {
			$(container).data()[$.fn.jquery === "1.11.3" ? "pluginSignaturePad" : "plugin-signaturePad"] = null;
		}
		return $(container).signaturePad({ defaultAction: "drawIt", lineTop: lineTop });
	}

	function resetSignaturePad(container) {
		var newPad = getCanvas(container);
		var oldPad = $('.pad', container);
		$(oldPad).replaceWith(newPad);
		createDrawItSignaturePad(container);
	}

	function resizeSignaturePad(pad) {
		var container = $(pad).parent();
		var output = $('.output', container);
		var outputValue = output.val() as string;
		if (outputValue == null || outputValue == '') {
			resetSignaturePad(container);
		} else {
			var maxWidth = $(container).data("maxWidth") || defaultMaxWidth;
			var width = Math.min(maxWidth,  Math.min($(container).width(), $(document).width() - padding));
			var height = Math.round(width / widthHeightRatio);
			pad.get(0).setAttribute('width', width);
			pad.get(0).setAttribute('height', height);
			if (pad.get(0).tagName.toLowerCase() === 'canvas') {
				var sigApi = createDrawItSignaturePad(container);
				sigApi.regenerate(outputValue);
			}
		}
	}

	ko.bindingHandlers.signaturePad = {
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var overrideWidth = $(element).data("signWidth");
			element.setAttribute('class', 'output');
			var container = document.createElement('div');
			$(element).replaceWith(container);
			$(container).data("maxWidth", overrideWidth ? parseFloat(overrideWidth) : defaultMaxWidth);
			var pad = getCanvas(container);

			var signatureJson = window.ko.utils.unwrapObservable(valueAccessor());
			if (!signatureJson || signatureJson.length == 0) {
				signatureJson = element.value;
			}
			var signature = $.parseJSON(signatureJson || null);
			var signatureWidth = 0;
			var signatureHeight = 0;
			if (signature != null) {
				signature.forEach(function (signaturePoint) { signatureWidth = Math.max(signatureWidth, signaturePoint.lx, signaturePoint.mx); });
				signature.forEach(function (signaturePoint) { signatureHeight = Math.max(signatureHeight, signaturePoint.ly, signaturePoint.my); });
			}

			$(container).append(element);
			$(container).append(pad);

			var clearButton = $(".clearButton", $(container).parent()).first();
			if (clearButton.length === 0) {
				clearButton = $(document.createElement('a'));
				clearButton.attr('class', 'clearButton');
				clearButton.attr('href', '#');
			}
			if (!!$(element).data('signature-clear-text')) {
				$(clearButton).css("margin-top", "5px");
				$(clearButton).text($(element).data('signature-clear-text'));
			} else {
				$(clearButton).addClass('hide');
			}
			$(container).append(clearButton);
			$(clearButton).on('click touchend', function (e) {
				e.preventDefault();
				$(element).val('');
				$(element).trigger('change');
				resetSignaturePad(container);
				return false;
			});


			if (signature == null) {
				createDrawItSignaturePad(container);
				setTimeout(function () {
					resizeSignaturePad(pad);
				}, 0);
			} else {
				pad.setAttribute('width', (signatureWidth + 5).toString());
				pad.setAttribute('height', (signatureHeight + 5).toString());
				if ($(container).width() < signatureWidth) {
					signatureHeight = Math.round(signatureHeight * ($(container).width() / signatureWidth));
					signatureWidth = $(container).width();
				}
				var penColour = $(element).data('sigpencolour') || '#145394';
				var bgColour = $(element).data('sigbgcolour') || '#dddddd';
				var borderColour = $(element).data('sigbordercolour') || '#000000';
				var sigApi = $(container).signaturePad({ displayOnly: true, lineWidth: 0, penColour: penColour, bgColour: bgColour });
				sigApi.regenerate(signatureJson);
				var imgData = sigApi.getSignatureImage();
				var sigImg = document.createElement('img');
				sigImg.setAttribute('class', 'pad');
				sigImg.setAttribute('width', signatureWidth.toString());
				sigImg.setAttribute('height', signatureHeight.toString());
				sigImg.setAttribute('src', imgData);
				sigImg.setAttribute('style', 'border-color: ' + borderColour);
				$(pad).replaceWith(sigImg);
				setTimeout(function () {
					// @ts-ignore
					valueAccessor(signatureJson);
				}, 300);
				setTimeout(function () {
					resizeSignaturePad($(sigImg));
					}, 0);
			}
			ko.bindingHandlers.value.init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
		},
		update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			ko.bindingHandlers.value.update(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
		}
	};
	$(document).on('mousedown touchstart dragstart', '.pad', function () {
		var pad = this;
		$(document).one('mouseup touchend dragend', function () {
			$('.output', $(pad).parent()).trigger('change');
		});
	});
	$(document).on('padresized', '.pad', function () {
		var pad = $(this);
		setTimeout(function () {
			resizeSignaturePad(pad);
		}, 0);
	});
});