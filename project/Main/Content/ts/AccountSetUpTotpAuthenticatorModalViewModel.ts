import {namespace} from "./namespace";
import type {AccountUserProfileViewModel} from "./AccountUserProfileViewModel";

export class AccountSetUpTotpAuthenticatorModalViewModel extends window.Main.ViewModels.ViewModelBase {
	parentViewModel: AccountUserProfileViewModel;
	errors = ko.validation.group(this, {deep: true});
	resultErrors = ko.observableArray<string>(null);
	newAuthorizationKey = ko.observable<string>(null);
	newAuthorizationUri = ko.observable<string>(null);
	barcodeFormat = ko.observable<string>("QR-Code");
	tokenParts: TokenPartModel[] = new Array();
	tokenIsValid = ko.observable<boolean>(false);

	constructor(parentViewModel: AccountUserProfileViewModel) {
		super();
		this.parentViewModel = parentViewModel;

		for (let i = 0; i < 6; i++) {
			const tokenPart = new TokenPartModel();
			tokenPart.index = i;
			this.tokenParts.push(tokenPart);
		}
	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);

		const newTotpCredentialsResponse = await fetch(window.Helper.Url.resolveUrl("~/Main/Account/GetNewTotpAuthorizationCredentialsForUser"));
		const newTotpCredentials = await newTotpCredentialsResponse.json();
		this.newAuthorizationKey(newTotpCredentials.authorizationKey);
		this.newAuthorizationUri(newTotpCredentials.authorizationUri);
	}

	async copyAuthorizationUriToClipboard(): Promise<void> {
		await navigator.clipboard.writeText(this.newAuthorizationKey());
		this.showSnackbar(window.Helper.String.getTranslatedString("AuthorizationKeyCopied"));
	};

	onTokenInputTextInput(data, e) {
		const key = e.originalEvent.data;
		if (/^\d$/g.test(key)) {
			data.value(key);
			if (data.index < this.tokenParts.length - 1) {
				this.tokenParts[data.index + 1].isFocused(true);
			}
			this.updateTokenIsValid();
			this.resultErrors.removeAll();
		} else if (key !== "Backspace") {
			e.preventDefault();
			return false;
		}
		return true;
	}

	onTokenInputKeyUp(data, e) {
		const key = e.key;

		if (key === "Backspace") {
			data.value("");
			if (data.index > 0) {
				this.tokenParts[data.index - 1].isFocused(true);
			}
			this.updateTokenIsValid();
			this.resultErrors.removeAll();
		}
		return true;
	}

	onTokenInputCut(data, e) {
		e.preventDefault();
	}

	onTokenInputCopy(data, e) {
		e.preventDefault();
	}

	onTokenInputPaste(data, e) {
		e.preventDefault();

		// @ts-ignore
		const clipboardData = e.originalEvent.clipboardData || window.clipboardData;
		const pastedData = clipboardData.getData("Text");
		const token = pastedData.replace(/\D/g, "");

		const focusedIndex = data.index;
		if (focusedIndex !== -1) {
			let lastFilledIndex = focusedIndex;

			for (let i = 0; i < token.length && (focusedIndex + i) < this.tokenParts.length; i++) {
				const tokenPartModel = this.tokenParts[focusedIndex + i];
				tokenPartModel.value(token[i]);
				lastFilledIndex = focusedIndex + i;
			}
			this.tokenParts[lastFilledIndex].isFocused(true);
		}

		this.updateTokenIsValid();
		this.resultErrors.removeAll();
	}

	composeToken(): string {
		return this.tokenParts.map(part => part.value()).join("");
	}

	updateTokenIsValid(): void {
		const isValid = this.composeToken().length === this.tokenParts.length;
		this.tokenIsValid(isValid);
	}

	async save(enable: boolean): Promise<void> {
		let errors = ko.validation.group(this);
		if (errors().length > 0) {
			errors.showAllMessages();
			return;
		}
		this.loading(true);

		try {
			const result = await (await fetch(window.Helper.Url.resolveUrl("~/Main/Account/SetTotpAuthenticationForCurrentUser"), {
				method: "POST",
				headers: {"Content-Type": "application/json"},
				body: JSON.stringify({
					authorizationKey: this.newAuthorizationKey(),
					token: this.composeToken()
				})
			})).json();
			if (result.length > 0) {
				const resultErrors = result.filter(x => x.Key !== "MESSAGE").map(x => x.Value);
				this.resultErrors(resultErrors);
			} else {
				this.resultErrors(null);
			}
			if (this.resultErrors().length === 0) {
				this.parentViewModel.totpIsSetUp(true);
				$(".modal:visible").modal("hide");
				this.showSnackbar(window.Helper.String.getTranslatedString("TotpAuthenticationSuccessfullySetUp"));
			}
			this.loading(false);
		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
			throw e;
		}

		this.loading(false);
	};
}

class TokenPartModel {
	index: number;
	value = ko.observable<string>("");
	isFocused = ko.observable<boolean>(false);
}

namespace("Main.ViewModels").AccountSetUpTotpAuthenticatorModalViewModel = AccountSetUpTotpAuthenticatorModalViewModel;