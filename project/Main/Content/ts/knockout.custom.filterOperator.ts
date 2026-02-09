// @ts-ignore
ko.extenders.filterOperator = function(target, operator) {
	const result = ko.pureComputed({
		read: target,
		write: function (newValue: any) {
			let current = null;
			let valueToWrite = null;
			const hasAdditionalProperties = operator && operator.additionalProperties;
			if (hasAdditionalProperties) {
				current = target.peek() || {Value: null, Operator: operator.operator};
				valueToWrite = {Value: null, Operator: operator.operator};
				Object.getOwnPropertyNames(operator.additionalProperties).forEach(function (propertyName) {
					current[propertyName] = operator.additionalProperties[propertyName];
					valueToWrite[propertyName] = operator.additionalProperties[propertyName];
				});
			} else {
				current = target.peek() || {Value: null, Operator: operator};
				valueToWrite = {Value: null, Operator: operator};
			}
			newValue = newValue && newValue.hasOwnProperty("Value") ? newValue.Value : newValue;
			if (newValue === "" || newValue === undefined) {
				newValue = null;
			}
			valueToWrite.Value = newValue;
			if (valueToWrite.Value === null && target.peek() !== null) {
				target(null);
			} else if (valueToWrite.Value !== current.Value) {
				target(valueToWrite);
			}
		}
	}).extend({notify: "always"});
	result(target());
	return result;
};

$(document).on("click", ".modal .input-group-addon a[data-target='#lgModal']", function (e) {
	e.preventDefault();
	const currentModal = $(this).closest('.modal');

	if (currentModal.length) {
		const previousModalId = currentModal.attr('id');
		const previousModalContent = currentModal.find('.modal-content').html();
		const currentUrl = window.location.href;

		$('#lgModal').data('previousModal', {
			id: previousModalId,
			content: previousModalContent,
			url: currentUrl
		});
		currentModal.css('display', 'none');
		$('.modal-backdrop').first().css('display', 'none');

		$('#lgModal').modal('show');
	} else {
		$('#lgModal').removeData('previousModal');
		$('#lgModal').modal('show');
	}

	return false;
});
$(document).on('hidden.bs.modal', '#lgModal', function () {
	const previousModalData = $(this).data('previousModal');
	if (previousModalData) {
		const currentUrl = window.location.href;

		if (currentUrl === previousModalData.url) {
			const $previousModal = $('#' + previousModalData.id);

			if ($previousModal.find('.modal-content').html().trim() === '') {
				$previousModal.find('.modal-content').html(previousModalData.content);
			}

			$previousModal.css('display', 'block').addClass('in');
			$('body').addClass('modal-open');

			if ($('.modal-backdrop').length === 0) {
				$('<div class="modal-backdrop fade in"></div>').appendTo('body');
			} else {
				$('.modal-backdrop').first().css('display', 'block');
			}
		}

		$(this).removeData('previousModal');
	}
});
