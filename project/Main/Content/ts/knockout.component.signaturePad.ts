import {registerComponent} from "./componentRegistrar";

registerComponent({
	componentName: "signature-pad",
	template: "Main/Template/SignaturePad",
	viewModel: function (params) {
		const viewModel = this;
		viewModel.caption = window.Helper.String.getTranslatedString(params.caption || "Signature");
		viewModel.signature = params.signature;
		viewModel.signatureDate = params.signatureDate;
		viewModel.signatureName = params.signatureName;
		viewModel.signature.subscribe(function (signature) {
			if (!!signature) {
				viewModel.signatureDate(new Date());
			} else {
				viewModel.signatureDate(null);
			}
		});

	}
});