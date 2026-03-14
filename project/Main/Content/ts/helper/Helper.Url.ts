import {HelperObject} from "./Helper.Object";
import {isEqual} from "lodash";

export class HelperUrl {

	static appPath: string;

	/**
	 * Resolves the url path for the given parameter, use "~" (tilde) to get the root url.
	 * @example
	 * resolveUrl("~/RoleAndGroup/UserGroupCreate")
	 * // returns "localhost:5000/RoleAndGroup/UserGroupCreate"
	 * @returns {string}
	 */
	static resolveUrl(url = ""): string {
		if (!url.startsWith("~"))
			return url;

		if (!HelperUrl.appPath) {
			const meta = document.getElementById("meta.AppDomainAppVirtualPath");
			HelperUrl.appPath = meta ? meta.getAttribute("content") : "/";
		}

		if (HelperUrl.appPath === "/")
			return url.slice(1, url.length);
		else if (HelperUrl.appPath.endsWith("/"))
			return HelperUrl.appPath.slice(0, HelperUrl.appPath.length - 1) + url.slice(1, url.length);
		else
			return HelperUrl.appPath + url.slice(1, url.length);
	}

	/**
	 * @param action {string}
	 * @param controller {string}
	 * @param options {{ plugin: string; id: string }}
	 */
	static redirectToAction(action: string, controller: string, options: {plugin?: string, id?: string}): void {
		window.location.replace(HelperUrl.action(action, controller, options));
	}

	/**
	 *
	 * @param action {string}
	 * @param controller {string}
	 * @param options {{ plugin: string; id: string }}
	 * @returns {string}
	 */
	static action(action: string, controller: string, options: {plugin?: string, id?: string}): string {
		let url = `/${controller}/${action}`;
		if (!!options.plugin) {
			url = `/${options.plugin}${url}`;
		}
		if (!!options.id) {
			url += `/${options.id}`;
		}
		return HelperUrl.resolveUrl(`~${url}`);
	}

	/**
	 * @param url {string}
	 * @returns {string}
	 */
	static qualifyURL(url: string): string {
		const el = document.createElement("div");
		el.innerHTML = "<a href=\"" + url.split("&").join("&amp;").split("<").join("&lt;").split("\"").join("&quot;") + "\">x</a>";
		return (el.firstChild as HTMLAnchorElement).href;
	}

	/**
	 * @param parameter {string}
	 * @returns {null|boolean|number}
	 */
	static getURLParameter(parameter: string): any {
		if (!parameter) {
			return null;
		}
		parameter = parameter.toLowerCase();
		const searchParameters = window.location.search && window.location.search.length > 1 ? HelperUrl.getParameters(window.location.search.substring(1)) : {};
		const hashParameters = window.location.hash && window.location.hash.length > 1 ? HelperUrl.getParameters(window.location.hash.substring(1)) : {};
		const parameters = $.extend(searchParameters, hashParameters);
		let result = null;
		Object.getOwnPropertyNames(parameters).forEach((param) => {
			if (result == null && param.toLowerCase() === parameter) {
				result = parameters[param];
			}
		});
		if (!isNaN(result) && isFinite(result)) {
			const numericResult = parseFloat(result);
			if (!isNaN(numericResult)) {
				return numericResult;
			}
		}
		if (/^[Tt]rue$/.test(result)) {
			return true;
		}
		if (/^[Ff]alse$/.test(result)) {
			return false;
		}
		return result;
	}

	/**
	 * @param paramString {string} The query string in the form of "a=3&b=5&c=7&c=9"
	 * @summary Gets the parameters from a given string and returns an object
	 * @example
	 * getParameters("a=3&b=5&c=7&c=9"); //yields {a:3,b:5,c:[7,9]}
	 * @returns {*}
	 */
	static getParameters(paramString: string): {} {
		let e;
		const params = {};
		const a = /\+/g;
		const r = /([^&;=]+)=?([^&;]*)/g;
		const d = (s) => decodeURIComponent(s.replace(a, " "));
		while (e = r.exec(paramString)) {
			const key = d(e[1]); const value = d(e[2]);
			if (!params[key]) {
				params[key] = value;
			} else if (Array.isArray(params[key])) {
				params[key][params[key].length] = value;
			} else {
				params[key] = [params[key], value];
			}
		}
		return params;
	}

	/**
	 * Gets the criteria from the hash and query string
	 * <br> Does so by passing them to getParameters function and then merging the results.
	 * @param criteriaId {?string}
	 */
	static getCriteriaFromUrl(criteriaId: string = null): {} {
		let searchString = window.location.search !== "" && window.location.search !== undefined ? window.location.search.substring(1) : "";
		if (!!criteriaId) {
			if (!searchString.includes(criteriaId)) {
				searchString = "";
			} else {
				searchString.split("CID_List=").forEach(function (part) {
					if (new RegExp("^" + criteriaId).exec(part)) {
						searchString = "CID_List=" + part;
					}
				});
			}
		}
		let hash = window.location.hash.substring(1);
		if (hash.includes("?")) {
			hash = hash.substr(hash.indexOf("?") + 1);
		}
		return {...HelperUrl.getParameters(searchString), ...HelperUrl.getParameters(hash)};
	}

	/**
	 * @summary
	 * Puts the criteria to query string (or to the hash if there's
	 * no History API), without reloading the page
	 * Please keep in mind that if there is no History API support,
	 * the additional entry will be created regardless of this parameter.
	 * @param criteria {object} The key-value pair for criteria
	 * @param push {boolean} If set to true, it will create an additional history entry.
	 * @param criteriaId {string}
	 * */
	static saveCriteriaToUrl(criteria: {}, push = false, criteriaId = ""): void {
		const newCriteria = HelperObject.removeEmptyParams(criteria);

		//We know better than History.js... Force the refresh.
		delete newCriteria["_suid"];

		const urlCriteria = HelperUrl.getCriteriaFromUrl();
		let serialized = $.param(newCriteria, true);
		if (isEqual(urlCriteria, newCriteria)) {
			return;
		}
		if (!!criteriaId) {
			let newSerialized = "";
			const searchString = window.location.search.substring(1);
			searchString.split("CID_List=").forEach(function (part) {
				if (part !== "" && !new RegExp("^" + criteriaId).exec(part)) {
					newSerialized += "CID_List=" + (part.endsWith("&") ? part : part + "&");
				}
			});
			newSerialized += "CID_List=" + criteriaId + "&" + serialized;
			serialized = newSerialized;
		}
		if (serialized.length > 0) {
			serialized = "?" + serialized;
		} else if (window.location.search.length < 2) {
			return;
		}
		if (!!push) {
			window.history.pushState({serializedCriteria: serialized}, document.title, serialized);
		} else {
			// try catch is needed for safari
			try {
				window.history.replaceState({ serializedCriteria: serialized }, document.title, serialized);
			} catch (e: unknown) {
				// Safari throws an error when trying to replace the state too many times
				if (e instanceof Error && e.name === 'SecurityError' && e.message.includes('Attempt to use history.replaceState()')) {
					return;
				}
				// any other error should be thrown
				throw e;
			}
		}

		// won't work in internet explorer... will reload the page after the hash is added/modified
		//			else if (serialized !== "" && serialized !== window.location.hash) {
		//				window.location.hash = serialized;
		//				if(window.location.search !== "")
		//					window.location.search = "";
		//			}
	}

	/**
	 * @param url {string}
	 * @returns {string}
	 */
	static prependHttp(url: string): string {
		return /^https?:\/\//i.test(url) ? url : "http://" + url;
	}

	static reloadAndReturnToLocation(): void {
		window.history.replaceState(null, null, window.location.origin + window.location.pathname + "#/Main/Home/Startup?sync=true&redirectUrl=" + encodeURIComponent(window.location.hash.substr(1)));
		$(document).trigger("hashchange");
	}
}

// @ts-ignore
(window.Helper = window.Helper || {}).Url = HelperUrl;