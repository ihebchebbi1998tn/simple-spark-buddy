import _ from "lodash";

export class HelperDOM {
	static getMetadata = (name: string): string => $("meta[name='" + name + "']").attr("content");

	static blockElement(element: HTMLElement | JQuery<HTMLElement>, unblock = false): void {
		if (unblock === true) {
			$(element).unblock();
			return;
		}
		$(element).block({
			ignoreIfBlocked: true,
			message: null,
			overlayCSS: {backgroundColor: "white"}
		});
		$(element).find("input:focus, select:focus, textarea:focus").blur();
	}

	static getCookie(value: string): string {
		const cookies = document.cookie.split(";");
		let result = null;
		cookies.forEach(cookie => {
			if (cookie.split("=")[0].trim() === value)
				result = cookie.split("=")[1].trim();
		});
		return result;
	}

	static getTextWithoutChildren(element: HTMLElement): string {
		return $(element).clone()
			.children()
			.remove()
			.end()
			.text();
	}

	static createLink(element: JQuery<HTMLElement>, removeClass: string): void {
		const schemas = ["http://", "https://", "ftp://"];
		const target = element.attr("href");
		for (let i = 0; i <= schemas.length; i++) {
			if (target.indexOf(schemas[i]) === 0 || target.indexOf(schemas[i]) === 1) {
				element.attr("href", target).removeClass(removeClass).attr("target", "_blank");
				return;
			}
		}
		if (target.indexOf("@") > 0) {
			element.attr("href", "mailto:" + target).removeClass(removeClass);
		} else if (target.indexOf("www.") === 0) {
			element.attr("href", "http://" + target).removeClass(removeClass).attr("target", "_blank");
		} else if (/[A-Za-z0-9]{2,50}\.(com|net|de|org|co\.uk|fr)/.test(target)) {
			element.attr("href", "http://" + target).removeClass(removeClass).attr("target", "_blank");
		}
	}

	/**
	 * @type {{change: function(*, *=): void}}
	 * @summary
	 * Attaches the function to all events that may cause an input to change its value.
	 * <code>attributes</code> is a space separated list of attributes to look for
	 * i.e. "maxlength type='text' name='justanotherinput' data-a-data-attr"
	 */
	static attachGlobalEvent = {
		change: function (attributes, fn) {
			$("input[" + attributes.replace(/\s/g, "][") + "]")
				.on("keyup keydown drop change mouseup paste mouseleave focus", fn)
				.trigger("change");
		}
	}

	static sortElements(parent, _empty: never, attribute: string, sortAsInt: boolean): void {
		const storedVal = parent.val();
		const getComparator = function (el) {
			const target = $(el);
			return $.trim(!!attribute ? target.attr(attribute) : target.text());
		};
		_(parent.children().detach())
			.sortBy(getComparator)
			.sortBy(function (child) {
				const value = getComparator(child);
				return sortAsInt ? parseInt(value, 10) : value;
			})
			.each(function (child) {
				parent.append(child);
			});
		if (!!storedVal) {
			parent.val(storedVal);
		}
	}

	static updateDiv(element: HTMLElement, url: string): void {
		const $element = $(element);
		HelperDOM.blockElement($element, false);
		$.ajax({
			url: url,
			type: "GET",
			success: function (data) {
				$element.html(data);
				HelperDOM.blockElement($element, true);
			}
		});
	}

	static showModal(): void {
		$("#lgModal").show();
	}

	static hideModal(): void {
		$("#lgModal").hide();
	}

	static dismissModal(): void {
		$("#lgModal").modal("toggle");
	}

}

// @ts-ignore
(window.Helper = window.Helper || {}).DOM = HelperDOM;
