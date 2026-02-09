import {HelperUrl} from "./Helper.Url";

declare global {
	interface Window {
		defaultLanguage: string | null,
		fetchedStrings: any
	}
}

window.defaultLanguage ??= null;
window.fetchedStrings ??= {};

export function base64toBlob(base64Data: string, contentType: string | null = null): Blob {
	contentType = contentType || "";
	const sliceSize = 1024;
	let byteCharacters: string;
	try {
		byteCharacters = atob(encodeURI(base64Data));
	} catch (e) {
		window.Log.error("Failed converting base64 to Blob: " + base64Data)
		throw e;
	}
	const bytesLength = byteCharacters.length;
	const slicesCount = Math.ceil(bytesLength / sliceSize);
	const byteArrays = new Array(slicesCount);

	for (let sliceIndex = 0; sliceIndex < slicesCount; ++sliceIndex) {
		const begin = sliceIndex * sliceSize;
		const end = Math.min(begin + sliceSize, bytesLength);

		const bytes = new Array(end - begin);
		for (let offset = begin, i = 0; offset < end; ++i, ++offset) {
			bytes[i] = byteCharacters[offset].charCodeAt(0);
		}
		byteArrays[sliceIndex] = new Uint8Array(bytes);
	}
	return new Blob(byteArrays, {type: contentType});
}

export function blobToBase64(blob: Blob): Promise<string> {
	return new Promise<string>(resolve => {
		const reader = new FileReader();
		reader.readAsDataURL(blob);
		reader.onloadend = function () {
			const dataUrl = reader.result.toString();
			const base64 = dataUrl.split(",")[1];
			resolve(base64);
		};
	});
}

export function getTranslatedString(alienString: string, defaultstring?: string, language: string | null = null): string | null {
	if (typeof alienString === "boolean"){
		alienString = (alienString as boolean).toString();
	}
	if (!alienString) {
		return defaultstring;
	}
	language ||= window.defaultLanguage;
	const fetched = !!language && !!window.fetchedStrings[language] ?
		window.fetchedStrings[language][alienString] : window.fetchedStrings[alienString];
	if (!!fetched) {
		return fetched;
	}
	const containsSpecialChars = /[^A-Za-z0-9_.]/.exec(alienString) != null;
	const selectors = [`${alienString}.translation`, `${alienString}-translation`];
	if (alienString.includes(".")) {
		selectors.push(alienString.replace(".", "_") + ".translation", alienString.replace(".", "_") + "-translation");
	}
	const current = !containsSpecialChars ? $(selectors.map(selector => document.getElementById(selector)).filter(x => x !== null)) : $([]);
	if (current?.length > 0) {
		return current.text();
	} else if (!!defaultstring) {
		return defaultstring;
	} else {
		return `Translation Missing: "${alienString}"`;
	}
}


export function format(value: string, ...params: any[]): string {
	return params.reduce((a: string, c, index) => a.replace(new RegExp(`\\{${index}\\}`, "g"), c.toString()), value);
}


export function fetchStringTranslation(alienString: string, language: string | null = null): JQuery.Promise<string> {
	const deferred = $.Deferred();
	if (!!(window.fetchedStrings)[alienString]) {
		deferred.resolve((window.fetchedStrings)[alienString]);
	} else {
		let url = HelperUrl.resolveUrl("~/Main/Resource/" + alienString + ".json");
		if (!!language) {
			url += "?language=" + language;
		}
		$.ajax(url,
			{
				cache: true,
				success: function (data) {
					window.fetchedStrings[alienString] = data.Value;
					deferred.resolve(data.Value);
				}
			});
	}
	return deferred.promise();
}

export function fetchStringTranslationPromise(alienString: string, language: string = null): Promise<string> {
	return new Promise(resolve => {
		if (!!window.fetchedStrings[alienString]) {
			resolve(window.fetchedStrings[alienString]);
		} else {
			let url = HelperUrl.resolveUrl(`~/Main/Resource/${alienString}.json`);
			if (!!language) {
				url += `?language=${language}`;
			}
			fetch(url, {cache: "force-cache"})
				.then(value => value.json())
				.then(value => {
					window.fetchedStrings[alienString] = value;
					resolve(value);
				});
		}

	});
}

export function setDefaultLanguage(language: string): void {
	window.defaultLanguage = language;
}

export function setStringTranslation(resourceKey: string, translation: string, language: string | number = null): void {
	if (!!language) {
		window.fetchedStrings[language] ||= {};
		window.fetchedStrings[language][resourceKey] = translation;
	} else {
		window.fetchedStrings[resourceKey] = translation;
	}
}

export function limitStringWithEllipses(longString: string, maxLength = 9): string {
	if (longString.length > maxLength && maxLength > 0) {
		return longString.substring(0, maxLength - 4) + "...";
	} else {
		return longString;
	}
}

export function htmlEntities(unsafeText: string): string {
	unsafeText = unsafeText.toString();
	if (!unsafeText) return "";
	const temporaryDiv = $("<div></div>").addClass("hide").appendTo("body").text(unsafeText);
	const safeText = temporaryDiv.html();
	temporaryDiv.remove();
	return safeText;
}

export function hash(str: string): number {
	let hash = 0;
	let i;
	let chr;
	let len;
	if (!str) return hash;
	for (i = 0, len = str.length; i < len; i++) {
		chr = str.charCodeAt(i);
		hash = ((hash << 5) - hash) + chr;
		hash |= 0;
	}
	return hash;
}

export function isGuid(str: string): boolean {
	return /^({)?[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}(})?$/gi.test(str);
}

/** @returns {"00000000-0000-0000-0000-000000000000"} */
export function emptyGuid(): string {
	return "00000000-0000-0000-0000-000000000000";
}

export function trim(x: string | KnockoutObservable<string>): string {
	return (window.ko.unwrap(x) || "").trim();
}

export function contains<T extends $data.Entity>(query: $data.Queryable<T>, term: string, columns: string[]): $data.Queryable<T> {
	const words = window.Helper.String.parseWords(term);
	words.forEach(word => {
		const filterExpr = columns.map(col => `it.${col}.contains(this.value)`).join(" || ");
		query = query.filter(filterExpr, { value: word });
	});
	return query;
}
export function replaceLineBreaks(html: string): string {
	return html.replace(/\\r\\n|\\r|\\n/g, "<br>");
}

export function isNullOrEmpty(value: string | KnockoutObservable<string>): boolean {
	if (ko.isObservable(value)) {
		return ko.unwrap(value) == null || ko.unwrap(value) === "";
	}
	return value == null || value === "";
}

// splits (search) string by words, keeping together words encapsulated in quotes
export function parseWords(str: string): string[] {
	const matches = str.match(/[^,\s"]+|"([^"]*)"/g);
	return matches ? matches.map(match => match.replace(/"/g, '')) : [];
}

export function tryExtractErrorMessageValue(errorMessage) {
	try {
		const parsedError = typeof errorMessage === 'object' ? errorMessage : JSON.parse(errorMessage);
		if (parsedError && parsedError.value) {
			return parsedError.value;
		}
		if(parsedError && parsedError.error && parsedError.error.details && Array.isArray(parsedError.error.details)) {
			return parsedError.error.details.map(x => x.message).join(', ');
		}
		if (parsedError && parsedError.errors && parsedError.errors.parameters) {
			return parsedError.errors.parameters;
		}
	} catch { }

	return null;
}

const HelperString = {
	base64toBlob,
	blobToBase64,
	getTranslatedString,
	format,
	fetchStringTranslation,
	fetchStringTranslationPromise,
	setDefaultLanguage,
	setStringTranslation,
	limitStringWithEllipses,
	htmlEntities,
	hash,
	isGuid,
	emptyGuid,
	trim,
	contains,
	replaceLineBreaks,
	isNullOrEmpty,
	parseWords,
	tryExtractErrorMessageValue
};

export {HelperString};

// @ts-ignore
(window.Helper = window.Helper || {}).String = HelperString;

