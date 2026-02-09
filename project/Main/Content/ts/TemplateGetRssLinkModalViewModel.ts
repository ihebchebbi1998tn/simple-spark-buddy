import {namespace} from "./namespace";

export class TemplateGetRssLinkModalViewModel extends window.Main.ViewModels.ViewModelBase {

	url = ko.observable<string>(null);

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		this.url(decodeURIComponent(params.url).replace("application/xml+rss", encodeURIComponent("application/xml+rss")));
	}

	async copyToClipboard(): Promise<void> {
		try {
			await navigator.clipboard.writeText(this.url());
			this.showSnackbar(window.Helper.String.getTranslatedString("LinkCopied"));
		} catch (e) {
			$("#rss-feed-url").focus();
			$("#rss-feed-url").select();
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

namespace("Main.ViewModels").TemplateGetRssLinkModalViewModel = TemplateGetRssLinkModalViewModel;