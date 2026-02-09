import {namespace} from "./namespace";

export class BarcodePreviewModalViewModel extends window.Main.ViewModels.ViewModelBase {
	rawString = ko.observable<string>(null);
	barcode = ko.observable<string>(null);

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.rawString.subscribe(this.barcodeGenerator.bind(this));
		this.rawString(id);
	};

	barcodeGenerator(val: string): void {
		// @ts-ignore
		QRCode.toDataURL(val, (err, url) => {
			this.barcode(url);
		});
	};
}

namespace("Main.ViewModels").BarcodePreviewModalViewModel = BarcodePreviewModalViewModel;

