import {registerComponent} from "@Main/componentRegistrar";
import type {BarcodeFormat} from "@Main/knockout.component.barcode";

export const FORMAT_DISPLAY_MAP: Record<BarcodeFormat, string> = {
	"CODE39": "Code-39",
	"CODE128": "Code-128",
	"QR-Code": "QR-Code",
}

class BarcodeFormatViewModel {

	format: string
	displayFormat: string
	active: boolean
	onClick: () => void

	constructor({format, active, onClick}: BarcodeFormatParams) {
		this.format = format;
		this.displayFormat = FORMAT_DISPLAY_MAP[format];
		this.active = active;
		this.onClick = onClick;
	}

}

type BarcodeFormatParams = {
	format: BarcodeFormat
	active: boolean
	onClick(): void
}

registerComponent({
	componentName: "barcode-format",
	template: "Main/Template/BarcodeFormat",
	viewModel: {
		createViewModel: function (params: BarcodeFormatParams) {
			return new BarcodeFormatViewModel(params);
		},
	},
});
