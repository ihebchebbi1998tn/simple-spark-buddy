import {namespace} from "./namespace";

export class TemplateGetIcsLinkModalViewModel extends window.Main.ViewModels.ViewModelBase {
	link = ko.observable<string>(null);

	async init(id?: string, params?: any): Promise<void> {
		await super.init(id, params);
		this.link(decodeURIComponent(decodeURIComponent(id)));
	}

	async copyToClipboard(): Promise<void> {
		try {
			await navigator.clipboard.writeText(this.link());
			this.showSnackbar(window.Helper.String.getTranslatedString("LinkCopied"));
		} catch (e) {
			$("#ics-url").focus();
			$("#ics-url").select();
			try {
				if (document.queryCommandSupported('copy') && document.queryCommandEnabled('copy')) {
					document.execCommand('copy');
					this.showSnackbar(window.Helper.String.getTranslatedString("LinkCopied"));
					return;
				}
			} catch (e) {
				this.loading(false);
				window.swal(window.Helper.String.getTranslatedString("LinkCopied_Error"), (e as Error).message, "error");
			}
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("LinkCopied_Error"), (e as Error).message, "error");
		}
	}
}

namespace("Main.ViewModels").TemplateGetIcsLinkModalViewModel = TemplateGetIcsLinkModalViewModel;