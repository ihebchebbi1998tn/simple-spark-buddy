import {HelperUrl} from "./Helper.Url";
import {HelperString} from "./Helper.String";
import {HelperObject} from "./Helper.Object";
import {HelperDOM} from "./Helper.DOM";
import $ from "jquery";

const getMetadata = (name: string): string => HelperDOM.getMetadata(name);

const resolveUrl = (url: string): string => HelperUrl.resolveUrl(url);

const getTranslatedString = (alienString: string, defaultString?: string, language?: string): string =>
	HelperString.getTranslatedString(alienString, defaultString, language);

const limitStringWithEllipses = (longString: string, maxLength?: number): string =>
	HelperString.limitStringWithEllipses(longString, maxLength);

const objectToString = (obj): string => HelperObject.objectToString(obj);

const htmlEntities = (unsafeText: string): string => HelperString.htmlEntities(unsafeText);

const log = (...params) => console?.log(...params);

const blockElement = (element: JQuery, unblock = false) => HelperDOM.blockElement(element, unblock);

const createLink = (element: JQuery<HTMLElement>, removeClass: string) => HelperDOM.createLink(element, removeClass);

const generateGuid = (): string => {
	return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, (c) => {
		const r = Math.random() * 16 | 0;
		const v = c === "x" ? r : (r & 0x3 | 0x8);
		return v.toString(16);
	});
};



// https://stackoverflow.com/questions/27936772/how-to-deep-merge-instead-of-shallow-merge
/**
 * Simple object check.
 * @param item
 */
export function isObject(item: any): boolean {
	return (item && typeof item === "object" && !Array.isArray(item));
}

/**
 * Deep merge two objects.
 * @param target
 * @param sources
 */
export function mergeDeep(target: any, ...sources: any[]): any {
	if (!sources.length) return target;
	const source = sources.shift();

	if (isObject(target) && isObject(source)) {
		for (const key in source) {
			if (isObject(source[key])) {
				if (!target[key]) Object.assign(target, { [key]: {} });
				mergeDeep(target[key], source[key]);
			} else {
				Object.assign(target, { [key]: source[key] });
			}
		}
	}

	return mergeDeep(target, ...sources);
}


const helperBase = {
	getMetadata,
	resolveUrl,
	getTranslatedString,
	limitStringWithEllipses,
	objectToString,
	htmlEntities,
	mousePosition: {X: 0, Y: 0},
	log,
	blockElement,
	createLink,
	generateGuid,
	mergeDeep
};

export {
	getMetadata,
	resolveUrl,
	getTranslatedString,
	limitStringWithEllipses,
	objectToString,
	htmlEntities,
	log,
	blockElement,
	createLink,
	generateGuid,
	helperBase
};



window.Helper = mergeDeep(window.Helper, helperBase);

$(document).mousemove(function (e) {
	helperBase.mousePosition.X = e.pageX;
	helperBase.mousePosition.Y = e.pageY;
});