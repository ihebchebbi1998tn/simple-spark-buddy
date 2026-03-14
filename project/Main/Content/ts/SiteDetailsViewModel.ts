import {namespace} from "./namespace";
import type {BarcodeViewState} from "@Main/SiteEditViewModel";

export class SiteDetailsViewModel extends window.Main.ViewModels.ViewModelBase {
	tabs = ko.observable<{}>({});
	site = ko.observable<Main.Rest.Model.ObservableMain_Site>(null);
	activePlugins = ko.observableArray<string>([]);
	country = ko.observable<Main.Rest.Model.Lookups.Main_Country>(null);
	title = ko.observable<string>(null);
	barcode: BarcodeViewState
	passwordStrengths = ko.observable<{ [key: string]: Main.Model.Lookups.Main_PasswordStrength }>({});
	lookups: LookupType = {
		passwordStrengths: { $tableName: "Main_PasswordStrength" },
		multiFactorAuthenticationModes: { $tableName: "Main_MultiFactorAuthenticationMode" },
	};
	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let site = await window.database.Main_Site.GetCurrentSite().first();
		this.site(site.asKoObservable());
		let activePlugins = await window.database.Main_Site.GetActivePluginNames().toArray();
		this.activePlugins(activePlugins);
		if (site.ExtensionValues.DefaultCountryKey) {
			this.country(await window.Helper.Lookup.queryLookupValue("Main_Country", site.ExtensionValues.DefaultCountryKey, null));
		}
		this.title = this.site().Name;
		this.barcode = {
			format: this.site().ExtensionValues().ReportBarcodeFormat,
			prefix: this.site().ExtensionValues().ReportBarcodePrefix,
			root: this.site().ExtensionValues().ReportBarcodeRoot,
			rootDisplay: ko.pureComputed(() => {
				return (this.barcode.root() || "")
					.split(",")
					// filter out empty string
					.filter(x => !!x)
					.map(columnName => window.Helper.getTranslatedString(columnName))
					.toString()
			}),
			suffix: this.site().ExtensionValues().ReportBarcodeSuffix,
			printOnly: this.site().ExtensionValues().ReportBarcodePrintOnly,
			showOnServiceReport: this.site().ExtensionValues().ReportBarcodeShowOnServiceReport,
			showOnChecklistPdf:this. site().ExtensionValues().ReportBarcodeShowOnChecklistPdf,
			value: ko.pureComputed(() => {
				return window.Helper.Barcode.getPreviewValue(this.site());
			}),
		}
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
	}
}

namespace("Main.ViewModels").SiteDetailsViewModel = SiteDetailsViewModel;