import JsBarcode, {type BaseOptions} from "jsbarcode";
import {registerComponent} from "@Main/componentRegistrar";
import QRCode from "qrcode";

interface BarcodeParams {
	value: KnockoutObservable<string>
	format: KnockoutObservable<BarcodeFormat>
	width?: number
	printOnly?: KnockoutObservable<boolean>

	onValidate?(res: ValidationResult): void
}

type ValidationResult = { isValid: boolean, errorMessage: string }

interface BarcodeViewModelParams extends BarcodeParams {
	barcodeElementId: string
}

const BARCODE_FORMAT = {
	CODE39: "CODE39",
	CODE128: "CODE128",
	["QR-Code"]: "QR-Code"
}
export type BarcodeFormat = keyof typeof BARCODE_FORMAT

function assertNever(value: never) {
	throw new Error("Unexpected value: " + value);
}

type BarcodeValidatorFn = (value: string, format: BarcodeFormat) => ValidationResult

const FORMAT_MAX_LENGTH_MAP: Record<BarcodeFormat, number> = {
	CODE39: 43,
	CODE128: 48,
	["QR-Code"]: 256
}

export const validateLength: BarcodeValidatorFn = (val: string, format: BarcodeFormat): ValidationResult => {
	let isValid = true
	let errorMessage = ""

	switch (format) {
		case "CODE39":
			isValid = val.length <= FORMAT_MAX_LENGTH_MAP[format];
			break;
		case "CODE128":
			isValid = val.length <= FORMAT_MAX_LENGTH_MAP[format];
			break;
		case "QR-Code":
			isValid = val.length <= FORMAT_MAX_LENGTH_MAP[format];
			break;
	}

	if (!isValid) {
		errorMessage = window.Helper.String.getTranslatedString("RuleViolation.MaxLengthWithNumber")
			.replace("{0}", window.Helper.String.getTranslatedString("Value"))
			.replace("{1}", FORMAT_MAX_LENGTH_MAP[format].toString());
	}

	return {
		isValid,
		errorMessage
	}
}

export const validateContent: BarcodeValidatorFn = (val: string, format: BarcodeFormat): ValidationResult => {
	let isValid = true;
	let errorMessage = "";
	let invalidCharacters: string[] = []
	let validCharsRegex: RegExp = / /;
	let invalidCharsRegex: RegExp = / /;

	switch (format) {
		case "CODE39":
			validCharsRegex = /[a-zA-Z0-9\-\.\ \$\/\+%]/g;
			invalidCharsRegex = /[^a-zA-Z0-9\-\.\ \$\/\+%]/g
			invalidCharacters = val.match(invalidCharsRegex) ?? [];
			isValid = val.match(validCharsRegex)?.length === val.length;
			break;
		case "CODE128":
			validCharsRegex = /[\x20-\x7E]/g;
			invalidCharsRegex = /[^\x20-\x7E]/g
			invalidCharacters = val.match(invalidCharsRegex) ?? [];
			isValid = val.match(validCharsRegex)?.length === val.length;
			break;
		case "QR-Code":
			validCharsRegex = /[\x20-\x7E]/g;
			invalidCharsRegex = /[^\x20-\x7E]/g
			invalidCharacters = val.match(invalidCharsRegex) ?? [];
			isValid = val.match(validCharsRegex)?.length === val.length;
			break;
	}

	if (invalidCharacters.length > 0) {
		const invalidCharactersList = invalidCharacters
			.filter((char, idx, self) => self.indexOf(char) === idx)
			.reduce((cur, char) => {
				cur += `\n • ${char}`
				return cur;
			}, "").toString();
		errorMessage = window.Helper.String.getTranslatedString("InvalidCharacters")
			.replace(`{0}`, invalidCharactersList);
	}

	return {
		isValid,
		errorMessage
	}
}

export class BarcodeViewModel {
	value: KnockoutObservable<string>
	format: KnockoutObservable<BarcodeFormat>
	printOnly: KnockoutObservable<boolean> | undefined
	barcodeElementId: string
	isInvalid = ko.observable(false)
	onValidate?: (res: ValidationResult) => void
	width?: number;

	constructor({ barcodeElementId, value, format, width, printOnly, onValidate}: BarcodeViewModelParams) {
		this.format = format;
		this.width = width;
		this.printOnly = printOnly;
		this.barcodeElementId = barcodeElementId;
		this.value = value;
		this.onValidate = onValidate;

		value.subscribe(() => {
			this.validateAndRender(value(), this.format())
		});
		this.format.subscribe(() => {
			this.validateAndRender(value(), this.format())
		});

		this.validateAndRender(value(), this.format())
	}

	validateAndRender(value: string, format: BarcodeFormat) {
		if (!value) {
			window.Log.warn("No barcode value has been provided, skipped rendering.")
			return
		}
		const {isValid, errorMessage} = this.validate();
		if (!isValid) {
			window.Log.warn(`Validation of barcode value failed, skipped rendering. The reason was: ${errorMessage}`)
			return
		}
		this.renderBarcode(value, format);
	}

	validate(): ValidationResult {
		const val = this.value();
		const format = this.format();
		{
			const res = validateLength(val, format);
			if (!res.isValid) {
				this.isInvalid(true);
				this.onValidate?.(res);
				return res;
			}
		}
		{
			const res = validateContent(val, format)
			if (!res.isValid) {
				this.isInvalid(true);
				this.onValidate?.(res);
				return res;
			}
		}
		this.isInvalid(false);
		const res = {
			isValid: true,
			errorMessage: "",
		}
		this.onValidate?.(res);
		return res;
	}

	renderBarcode(barcodeValue: string, format: BarcodeFormat) {
		const {barcodeElementId} = this;
		const options: BaseOptions = {displayValue: false, width: this.width ?? 2};

		// QR-Code rendering sets fixed width and height which causes issues when format gets changed and component re-renders.
		// Thus, reset to auto before rendering.
		document.getElementById(barcodeElementId).style.height = "auto"
		document.getElementById(barcodeElementId).style.width = "auto"

		switch (format) {
			case "CODE128":
				JsBarcode(`#${barcodeElementId}`)
					.CODE128(barcodeValue, options)
					.render()
				break;
			case "CODE39":
				JsBarcode(`#${barcodeElementId}`)
					.CODE39(barcodeValue, options)
					.render()
				break;
			case "QR-Code":
				QRCode.toCanvas(document.getElementById(barcodeElementId), barcodeValue, {width: this.width ?? 100});
				break;
			default:
				assertNever(format);
				break;
		}
	}
}

registerComponent({
	componentName: "barcode",
	viewModel: {
		createViewModel: function (params: BarcodeParams, componentInfo) {
			const elementId = "barcode"
			const element = (componentInfo.element as HTMLElement).querySelector("canvas")
			element.setAttribute("id", elementId)
			return new BarcodeViewModel({
				barcodeElementId: elementId,
				...params
			});
		},
	},
	template: "Main/Template/Barcode"
});
