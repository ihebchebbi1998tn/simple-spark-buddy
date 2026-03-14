import {registerComponent} from "./componentRegistrar";

export interface BarcodeScannerResult {
	cancelled: boolean,
	text: string
}

export interface BarcodeScanner {
	scan(success: (result: BarcodeScannerResult) => void, error: (error: any) => void): void;
}

export class BarcodeScannerViewModel {
	barcodeScanner: BarcodeScanner;
	loading = ko.observable(false);
	value: KnockoutObservable<any>;
	source: () => any;
	column: string;
	id: string;

	constructor(barcodeScanner: BarcodeScanner, params: any) {
		this.barcodeScanner = barcodeScanner;
		this.value = params.value;
		this.source = params.source;
		this.column = params.column;
		this.id = params.id;
	}

	scan(): void {
		this.barcodeScanner.scan(async result => {
			if (!!result && !result.cancelled) {
				if (!this.source || !this.column) {
					this.value(result.text);
					return;
				}
				this.loading(true);
				let query = this.source().filter("it." + this.column + " === this.barcode", {barcode: result.text});
				if (this.id) {
					query = query.map("it." + this.id);
				}
				try {
					let value = await query.first();
					this.value(value);
					this.loading(false);
				} catch {
					this.loading(false);
					await window.Helper.Confirm.genericConfirmAsync({
						title: window.Helper.String.getTranslatedString("NotFoundTitle"),
						text: result.text,
						type: "info",
						showCancelButton: false
					});
				}
			}
		}, error => {
			window.Log.warn('error during barcode scanning: ' + error);
		});
	}
}

registerComponent({
	componentName: "barcode-scanner",
	viewModel: {
		createViewModel: function (params, componentInfo) {
			const $element = $(componentInfo.element);
			// @ts-ignore
			let barcodeScanner = window.barcodeScanner || null;
			if (!barcodeScanner) {
				window.Log.debug("cannot find barcode scanner");
				let $inputGroup = $element.parent(".input-group");
				if ($(".input-group-addon", $inputGroup).length === 1) {
					$inputGroup.removeClass("input-group")
				}
				return null;
			}
			return new BarcodeScannerViewModel(barcodeScanner, params);
		},
	},
	template: "Main/Template/BarcodeScanner"
});