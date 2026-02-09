import {namespace} from "./namespace";
import type {BarcodeFormat} from "@Main/knockout.component.barcode";
import {HelperString} from "@Main/helper/Helper.String";
import {MultiFactorAuthenticationMode} from "@Main/helper/Helper.MultiFactorAuthentication";

export type BarcodeViewState = {
	format: KnockoutObservable<string>
	prefix: KnockoutObservable<string>
	root: KnockoutObservable<string>
	suffix: KnockoutObservable<string>
	printOnly: KnockoutObservable<boolean>
	showOnServiceReport: KnockoutObservable<boolean>
	showOnChecklistPdf: KnockoutObservable<boolean>
	value: KnockoutComputed<string>
	rootDisplay: KnockoutComputed<string>
}

type BarcodeEditState = Omit<BarcodeViewState, "rootDisplay"> & {
	_errorMessage: KnockoutObservable<string>
	errorMessage: KnockoutComputed<string>
}

export class SiteEditViewModel extends window.Main.ViewModels.ViewModelBase {
	site = ko.observable<Main.Rest.Model.ObservableMain_Site>(null);
	themes = ko.observableArray<Main.Rest.Model.ObservableTheme>([]);
	selectedTheme = ko.observable<Main.Rest.Model.ObservableTheme>(null);
	activePlugins = ko.observableArray<string>([]);
	availablePlugins = ko.observableArray<Main.Rest.Model.PluginDescriptor>([]);
	selectedPlugins = ko.observableArray<string>([]);
	selectedColumnsForBarcodeRoot = ko.observableArray<string>([]);
	locales = ko.observableArray<string>([]);
	logoUrl = ko.observable<string>(null);
	logoBlob = ko.observable<Blob>(null);
	logoContent = ko.observable<string>(null);
	reportLogoUrl = ko.observable<string>(null);
	reportLogoBlob = ko.observable<Blob>(null);
	reportLogoContent = ko.observable<string>(null);
	reportFooterImageUrl = ko.observable<string>(null);
	reportFooterImageBlob = ko.observable<Blob>(null);
	reportFooterImageContent = ko.observable<string>(null);
	barcode: BarcodeEditState;
	passwordStrengths = ko.observable<{ [key: string]: Main.Model.Lookups.Main_PasswordStrength }>({});
	currentMultiFactorAuthenticationModeKey: string;

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		const viewModel = this;
		let site = await window.database.Main_Site.GetCurrentSite().first();
		window.database.attachOrGet(site);
		let localesData = await fetch(window.Helper.Url.resolveUrl("~/Main/Resource/ListLocales?format=json")).then(x => x.json());
		viewModel.locales(localesData);
		let themes = await window.database.Main_Site.GetMaterialThemes().toArray();
		viewModel.themes(themes.map(x => x.asKoObservable()));
		let plugins = await window.database.Main_Site.GetPlugins().toArray();
		viewModel.availablePlugins(plugins);
		viewModel.site(site.asKoObservable());
		this.selectedColumnsForBarcodeRoot((site.ExtensionValues.ReportBarcodeRoot || "").split(",").filter(x=>!!x))
		let activePlugins = await window.database.Main_Site.GetActivePluginNames().toArray();
		viewModel.activePlugins(activePlugins);
		viewModel.selectedPlugins(activePlugins);
		viewModel.selectedTheme(ko.utils.arrayFirst(viewModel.themes(), function (theme) {
			return theme.Name() === viewModel.site().ExtensionValues().MaterialTheme();
		}));
		let lookup = await window.Helper.Lookup.getLocalizedArrayMap("Main_PasswordStrength");
		this.passwordStrengths(lookup.$array.filter((x) => { return x.Key !== null }));
		
		this.initImages(this.site());
		this.initValidations(this.site());
		this.initBarcode(this.site());
		this.currentMultiFactorAuthenticationModeKey = viewModel.site().ExtensionValues().MultiFactorAuthenticationModeKey() ?? "MandatoryForSpecificUserGroups";
		this.site().ExtensionValues().DefaultLocale.subscribe(val => {
			if (val === undefined) {
				this.site().ExtensionValues().DefaultLocale(null);
			}
		});
	}

	initImages(site: Main.Rest.Model.ObservableMain_Site | Main.Multitenant.Rest.Model.ObservableMainMultitenant_Domain): void {
		const viewModel = this;

		const blob = site.ExtensionValues().MaterialLogoBase64() != null ? window.Helper.String.base64toBlob(site.ExtensionValues().MaterialLogoBase64()) : null;
		if (blob !== null) {
			viewModel.logoBlob(blob);
			viewModel.logoContent(site.ExtensionValues().MaterialLogoBase64());
			viewModel.logoUrl(window.URL.createObjectURL(blob));
		} else {
			viewModel.logoBlob(null);
			viewModel.logoContent(null);
			viewModel.logoUrl(null);
		}
		const reportLogoBlob = site.ExtensionValues().ReportLogoBase64() != null ? window.Helper.String.base64toBlob(site.ExtensionValues().ReportLogoBase64()) : null;
		if (reportLogoBlob !== null) {
			viewModel.reportLogoBlob(reportLogoBlob);
			viewModel.reportLogoContent(site.ExtensionValues().ReportLogoBase64());
			viewModel.reportLogoUrl(window.URL.createObjectURL(reportLogoBlob));
		} else {
			viewModel.reportLogoBlob(null);
			viewModel.reportLogoContent(null);
			viewModel.reportLogoUrl(null);
		}
		const reportFooterImageBlob = site.ExtensionValues().ReportFooterImageBase64() != null ? window.Helper.String.base64toBlob(site.ExtensionValues().ReportFooterImageBase64()) : null;
		if (reportFooterImageBlob !== null) {
			viewModel.reportFooterImageBlob(reportFooterImageBlob);
			viewModel.reportFooterImageContent(site.ExtensionValues().ReportFooterImageBase64());
			viewModel.reportFooterImageUrl(window.URL.createObjectURL(reportFooterImageBlob));
		} else {
			viewModel.reportFooterImageBlob(null);
			viewModel.reportFooterImageContent(null);
			viewModel.reportFooterImageUrl(null);
		}

		viewModel.logoBlob.extend({
			validation: {
				validator: function (val) {
					return val == null || !val.name || val.type.indexOf("image/") !== -1;
				},
				message: window.Helper.String.getTranslatedString("M_SelectedFileNotImage")
			}
		});
		viewModel.reportLogoBlob.extend({
			validation: {
				validator: function (val) {
					return val == null || !val.name || val.type.indexOf("image/") !== -1;
				},
				message: window.Helper.String.getTranslatedString("M_SelectedFileNotImage")
			}
		});
		viewModel.reportFooterImageBlob.extend({
			validation: {
				validator: function (val) {
					return val == null || !val.name || val.type.indexOf("image/") !== -1;
				},
				message: window.Helper.String.getTranslatedString("M_SelectedFileNotImage")
			}
		});
	}

	initValidations(site: Main.Rest.Model.ObservableMain_Site | Main.Multitenant.Rest.Model.ObservableMainMultitenant_Domain): void {
		site.Name.extend({
			validation:
				[
					{
						rule: "required",
						message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Name")),
						params: true
					}
				]
		});
		site.ExtensionValues().CompanyName.extend({
			validation:
				[
					{
						rule: "required",
						message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("DataProtectionCompanyName")),
						params: true
					}
				]
		});
		site.ExtensionValues().ResponsibleAddress.extend({
			validation:
				[
					{
						rule: "required",
						message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("DataProtectionResponsibleAddress")),
						params: true
					}
				]
		});
		site.ExtensionValues().DataProtectionOfficer.extend({
			validation:
				[
					{
						rule: "required",
						message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("DataProtectionOfficer")),
						params: true
					}
				]
		});
		site.ExtensionValues().Host.extend({
			validation:
				[
					{
						rule: "required",
						message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Host")),
						params: true
					}
				]
		});
		site.ExtensionValues().ReportBarcodePrefix.extend({
			validation: {
				validator: (val: string) =>  {
					return !val || val.length === 0 || this.selectedColumnsForBarcodeRoot().length > 0
				},
				message: window.Helper.String.getTranslatedString("PrefixRequiresRoot"),
			}
		});
		site.ExtensionValues().ReportBarcodeSuffix.extend({
			validation:
				{
					validator: (val: string) => {
						return !val || val.length === 0 || this.selectedColumnsForBarcodeRoot().length > 0
					},
					message: window.Helper.String.getTranslatedString("SuffixRequiresRoot"),
				}
		});
		site.ExtensionValues().MultiFactorAuthenticationModeKey.extend({
			validation:
				[
					{
						rule: "required",
						message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("MultiFactorAuthentication")),
						params: site instanceof Main.Rest.Model.ObservableMain_Site
					}
				]
		});
	}

	handleBarcodeValidation = ({isValid, errorMessage}) => {
		if (!isValid) this.barcode._errorMessage(errorMessage)
		else this.barcode._errorMessage("");
	}

	initBarcode(site: Main.Rest.Model.ObservableMain_Site | Main.Multitenant.Rest.Model.ObservableMainMultitenant_Domain): void {

		this.barcode = {
			format: site.ExtensionValues().ReportBarcodeFormat,
			prefix: site.ExtensionValues().ReportBarcodePrefix,
			root: site.ExtensionValues().ReportBarcodeRoot,
			suffix: site.ExtensionValues().ReportBarcodeSuffix,
			printOnly: site.ExtensionValues().ReportBarcodePrintOnly,
			showOnServiceReport: site.ExtensionValues().ReportBarcodeShowOnServiceReport,
			showOnChecklistPdf: site.ExtensionValues().ReportBarcodeShowOnChecklistPdf,
			value: ko.pureComputed(() => {
				return window.Helper.Barcode.getPreviewValue(site);
			}),
			_errorMessage: ko.observable(""),
			errorMessage: ko.pureComputed(() => {
				const maxLength = 25
				let maxLengthErrorMessage = null
				if (this.barcode.format() as BarcodeFormat !== "QR-Code" && this.barcode.value().length > maxLength) {
					maxLengthErrorMessage = window.Helper.String.getTranslatedString("RuleViolation.MaxLengthWithNumber")
						.replace("{0}", window.Helper.String.getTranslatedString("PrefixAndSuffix"))
						.replace("{1}", maxLength.toString())
				}
				return maxLengthErrorMessage || this.barcode._errorMessage()
			}),
		}

		// To trigger prefix and suffix validation.
		// See `initValidations`-fn.
		this.barcode.root.subscribe(() => {
			this.barcode.prefix.valueHasMutated()
			this.barcode.suffix.valueHasMutated()
		});
	}

	onColorSelected(colorData: Main.Rest.Model.ObservableTheme): void {
		const viewModel = this;
		viewModel.selectedTheme(colorData);
		viewModel.site().ExtensionValues().MaterialTheme(colorData.Name());
	}

	async submit(): Promise<void> {
		const viewModel = this;
		let errors = window.ko.validation.group(viewModel, {deep: true});
		if (errors().length > 0 || this.barcode.errorMessage().length > 0) {
			errors.showAllMessages();
			return;
		}

		viewModel.loading(true);
		try {
			viewModel.site().ExtensionValues().ReportBarcodeRoot(this.selectedColumnsForBarcodeRoot().toString());
			viewModel.site().ExtensionValues().MaterialLogoBase64(viewModel.logoContent());
			viewModel.site().ExtensionValues().ReportLogoBase64(viewModel.reportLogoContent());
			viewModel.site().ExtensionValues().ReportFooterImageBase64(viewModel.reportFooterImageContent());
			if (viewModel.reportFooterImageContent()) {
				viewModel.site().ExtensionValues().ReportFooterCol1(null);
				viewModel.site().ExtensionValues().ReportFooterCol2(null);
				viewModel.site().ExtensionValues().ReportFooterCol3(null);
			}
			await window.database.saveChanges();

			const newMultiFactorAuthenticationModeKey = viewModel.site().ExtensionValues().MultiFactorAuthenticationModeKey();
			const handleMultiFactorAuthenticationModeChange = this.currentMultiFactorAuthenticationModeKey !== newMultiFactorAuthenticationModeKey;
			if (handleMultiFactorAuthenticationModeChange) {
				await window.database.Main_Site.HandleMultiFactorAuthenticationModeChange({
					PreviousModeKey: this.currentMultiFactorAuthenticationModeKey
				}).first();
			}
			let pluginsChanged = !window._.isEqual(this.activePlugins().sort(), this.selectedPlugins().sort());
			if (pluginsChanged) {
				await window.database.Main_Site.SetActivePlugins({PluginNames: this.selectedPlugins()}).toArray();
				await this.waitForSiteToRestart();
			}
			window.database.attachOrGet(viewModel.site().innerInstance);
			viewModel.loading(false);
			window.location.hash = "/Main/Site/Details";
			if (pluginsChanged) {
				window.location.reload();
			}
		} catch (e) {
			viewModel.loading(false);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
		}
	}

	cancel(): void {
		const viewModel = this;
		window.database.detach(viewModel.site().innerInstance);
		window.history.back();
	}

	mapBarcodeRootColumnForSelect2Display(columnName: string):{ id: string, text: string }{
		return{
			id: columnName,
			text: window.Helper.getTranslatedString(columnName)
		}
	}

	mapPluginForSelect2Display(plugin: Main.Rest.Model.PluginDescriptor): { id: string, text: string } {
		let text = plugin.PluginName;
		if (plugin.RequiredPluginNames.length > 0) {
			text += " " + window.Helper.String.getTranslatedString("PluginRequires").replace("{0}", plugin.RequiredPluginNames.join(", "));
		}
		return {
			id: plugin.PluginName,
			text: text
		};
	}

	getSelectedColumnsForBarcodeRoot(columnNames: string[]): { id: string, text: string }[]{
		return columnNames.map(this.mapBarcodeRootColumnForSelect2Display);
	}

	passwordStrengthSliderValueFormat(lookup: Main.Model.Lookups.Main_PasswordStrength): string {
		return lookup.Key;
	}
	passwordStrengthSliderTooltipFormat(lookup: Main.Model.Lookups.Main_PasswordStrength): string {
		return `${lookup.Value} (${HelperString.getTranslatedString('ExampleAbbreviation')} ${lookup.Example})`;
	}

	getSelectedPlugins(pluginNames: string[]): { id: string, text: string }[] {
		let viewModel = this;

		const plugins = viewModel.getSelectedPluginsRec(pluginNames);

		viewModel.selectedPlugins(plugins);
		viewModel.selectedPlugins.valueHasMutated();
		const pluginsWithDependencies = viewModel.availablePlugins().filter(x => plugins.indexOf(x.PluginName) !== -1);

		return pluginsWithDependencies.map(viewModel.mapPluginForSelect2Display);
	}

	getSelectedPluginsRec(pluginNames: string[]): string[] {
		let viewModel = this;
		let result = pluginNames;
		let pluginsWithDependencies = viewModel.availablePlugins()
			.filter(x => pluginNames.indexOf(x.PluginName) !== -1);

		for (let plugin of pluginsWithDependencies) {
			if (plugin.RequiredPluginNames.some(x => result.indexOf(x) === -1)) {
				result = result.concat(plugin.RequiredPluginNames);
				return viewModel.getSelectedPluginsRec(result);
			}
		}
		return result.filter(function (value, index, self) {
			return self.indexOf(value) === index;
		});
	}

	onMultiFactorAuthenticationModeSelect(): void {
		if (!this.site().ExtensionValues().MultiFactorAuthenticationModeKey()) {
			this.site().ExtensionValues().MultiFactorAuthenticationModeKey(MultiFactorAuthenticationMode.MandatoryForSpecificUserGroupsKey);
		}
	};

	async waitForSiteToRestart(): Promise<void> {
		await new Promise(resolve => {
			setTimeout(resolve, 1000);
		});
		let request = new Request(window.Helper.resolveUrl("~/Main/Account/Login"), {method: "HEAD"});
		let response = await fetch(request);
		if (response.status === 503) {
			await this.waitForSiteToRestart();
		}
	}
}

namespace("Main.ViewModels").SiteEditViewModel = SiteEditViewModel;