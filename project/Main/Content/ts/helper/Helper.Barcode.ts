export class HelperBarcode {
	static getPreviewValue = (site: Main.Rest.Model.ObservableMain_Site | Main.Multitenant.Rest.Model.ObservableMainMultitenant_Domain) => {
		let val = "";
		if (site.ExtensionValues().ReportBarcodePrefix()) val = `${site.ExtensionValues().ReportBarcodePrefix()}-${val}`
		if (site.ExtensionValues().ReportBarcodeSuffix()) val = `${val}-${site.ExtensionValues().ReportBarcodeSuffix()}`
		return val || window.Helper.getTranslatedString('BarcodePlaceholder');
	}

	static getValue = (site: Main.Rest.Model.ObservableMain_Site | Main.Multitenant.Rest.Model.ObservableMainMultitenant_Domain, serviceOrder: Crm.Service.Rest.Model.CrmService_ServiceOrderHead) => {
		const serviceOrderColumnNames = (site.ExtensionValues().ReportBarcodeRoot() || "")
			.split(",")
			.filter(columnName => !!serviceOrder[columnName]);
		let val = "";

		serviceOrderColumnNames.forEach((columnName, idx) => {
			const columnValue = serviceOrder[columnName]
			if (!columnValue) return;
			if (idx > 0) val += "-"
			val += columnValue;
		});

		if (site.ExtensionValues().ReportBarcodePrefix()) val = `${site.ExtensionValues().ReportBarcodePrefix()}-${val}`
		if (site.ExtensionValues().ReportBarcodeSuffix()) val = `${val}-${site.ExtensionValues().ReportBarcodeSuffix()}`
		return val
	}
}