///<reference types="async"/>

import {HelperDOM} from "./Helper.DOM";
import {HelperUrl} from "./Helper.Url";

declare global {
	interface Window {
		Globalize: any;
	}
}

export class HelperCulture {
	static initialized = false;
	static locale: string;
	static language: string;

	static getGlobalizationData = function (): JQuery.Promise<any> {
		return $.ajax(HelperUrl.resolveUrl("~/Main/Resource/GetGlobalizationData?format=json"), {
			cache: true,
			method: "GET"
		});
	}

	static async languageCulture(): Promise<string> {
		if ($("#meta\\.CurrentLanguage").attr("content")) {
			return $("#meta\\.CurrentLanguage").attr("content");
		} else {
			let result = await HelperCulture.getGlobalizationData()
			return result.languageCultureName;
		}
	}

	static async culture(): Promise<string> {
		if ($("#meta\\.CurrentCulture").attr("content")) {
			return $("#meta\\.CurrentCulture").attr("content");
		} else {
			let result = await HelperCulture.getGlobalizationData();
			return result.cultureName;
		}
	}
	static async initialize(): Promise<void> {
		if (HelperCulture.initialized){
			return;
		}
		const cultureName = await HelperCulture.culture();
		const languageName = await HelperCulture.languageCulture();
		const jsonFiles = [];
		const ajaxOptions = {cache: true};
		const cldrjsPaths = [
			"~/static-dist/cldr-data/supplemental/likelySubtags.json",
			"~/static-dist/cldr-data/main/{0}/ca-gregorian.json",
			"~/static-dist/cldr-data/main/{0}/currencies.json",
			"~/static-dist/cldr-data/main/{0}/dateFields.json",
			"~/static-dist/cldr-data/main/{0}/languages.json",
			"~/static-dist/cldr-data/main/{0}/localeDisplayNames.json",
			"~/static-dist/cldr-data/main/{0}/numbers.json",
			"~/static-dist/cldr-data/main/{0}/scripts.json",
			"~/static-dist/cldr-data/main/{0}/territories.json",
			"~/static-dist/cldr-data/main/{0}/timeZoneNames.json",
			"~/static-dist/cldr-data/main/{0}/variants.json",
			"~/static-dist/cldr-data/supplemental/currencyData.json",
			"~/static-dist/cldr-data/supplemental/likelySubtags.json",
			"~/static-dist/cldr-data/supplemental/numberingSystems.json",
			"~/static-dist/cldr-data/supplemental/ordinals.json",
			"~/static-dist/cldr-data/supplemental/plurals.json",
			"~/static-dist/cldr-data/supplemental/timeData.json",
			"~/static-dist/cldr-data/supplemental/weekData.json"
		];
		const culturesToLoad = new Set([cultureName, languageName]);
		for (const culture of culturesToLoad) {
			for (const path of cldrjsPaths) {
				try {
					let result = await $.ajax(HelperUrl.resolveUrl(path.replace("{0}", culture)), ajaxOptions);
					jsonFiles.push(result);
				} catch (err) {
					console.error(`Failed to load ${path} for culture ${culture}`, err);
				}
			}
		}
		window.Globalize.load(jsonFiles);
		window.Globalize.locale(cultureName);
		HelperCulture.initialized = true;
		HelperCulture.locale = await HelperCulture.culture();
		HelperCulture.language = await HelperCulture.languageCulture();
	}
	static setMapLanguage(url: string): string {
		if (url.includes("google")) {
			url = url + "&hl=" + document.getElementById("meta.CurrentLanguage").getAttribute("content");;
		}
		return url;
	}
	static getDisplayName(bcp47Tag: string, showScript = false, showVariant = false): string {
		// @ts-ignore
		const cldr = new Cldr(bcp47Tag);
		const language = cldr.attributes.language;
		const script = cldr.attributes.script;
		const territory = cldr.attributes.territory;
		let variant = cldr.attributes.variant;
		window.Globalize.locale(HelperCulture.language);
		const languageDisplay = window.Globalize.cldr.main("localeDisplayNames/languages/" + language);
		const territoryDisplay = window.Globalize.cldr.main("localeDisplayNames/territories/" + territory);
		const localeSeparator = window.Globalize.cldr.main("localeDisplayNames/localeDisplayPattern/localeSeparator");
		const pattern = window.Globalize.cldr.main("localeDisplayNames/localeDisplayPattern/localePattern");
		let result = pattern.replace("{0}", languageDisplay).replace("{1}", territoryDisplay);
		if (showScript === true && script) {
			const scriptDisplay = window.Globalize.cldr.main("localeDisplayNames/scripts/" + script);
			result = localeSeparator.replace("{0}", result).replace("{1}", scriptDisplay);
		}
		if (showVariant === true && variant) {
			if (variant.indexOf("-") === 0) {
				variant = variant.substr(1);
			}
			const variantDisplay = window.Globalize.cldr.main("localeDisplayNames/variants/" + variant);
			result = localeSeparator.replace("{0}", result).replace("{1}", variantDisplay);
		}
		window.Globalize.locale(HelperCulture.locale);
		return result;
	}
	static initializeLocaleSelect(element: string, target: string): void {
		const $select = $(element);
		if ($select.length === 0) {
			return;
		}
		const $container = $select.parent("label");
		HelperDOM.blockElement($container);
		HelperCulture.initialize().then(function() {
			return $.get(HelperUrl.resolveUrl("~/Main/Resource/ListLocales?format=json"));
		}).then(function (locales) {
			const options = locales.map(function (locale) {
				return {
					key: locale,
					language: locale.indexOf("-") === -1 ? locale : locale.substring(0, locale.indexOf("-")),
					value: HelperCulture.getDisplayName(locale)
				};
			}).sort(function (a, b) {
				return a.value.localeCompare(b.value);
			});
			const selectedValue = $select.data("value");
			options.forEach(function (option) {
				const selected = selectedValue === option.key ? " selected" : "";
				$select.append("<option value='" + option.key + "' class='" + option.language + "'" + selected + ">" + option.value + "</option>");
			});
			// @ts-ignore
			$select.chained(target);
			HelperDOM.blockElement($container, true);
		});
	}
}