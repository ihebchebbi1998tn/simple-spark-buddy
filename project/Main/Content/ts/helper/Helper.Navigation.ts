export class HelperNavigation {

	static postToUrl(url: string, params: {}): void {
		const $form = $("<form></form>");
		$form.attr("action", url);
		$form.attr("method", "POST");

		for (const key in params) {
			const $input = $("<input type=\"hidden\"></input>");
			$input.attr("name", key);
			$input.val(params[key]);
			$form.append($input);
		}
		$("body").append($form);
		$form.submit();
		$form.remove();
	}

}

