import download from "downloadjs";

export class HelperDownload {
	static async downloadFile(url: string, fileName: string, fileType: string) {
		window.Main.ViewModels.ViewModelBase.prototype.showSnackbar(`${window.Helper.String.getTranslatedString('PleaseWait')}. ${window.Helper.String.getTranslatedString('DownloadingFile').replace('{0}', fileName)}`);
		await fetch(window.Helper.Url.resolveUrl(url))
			.then(response => response.blob())
			.then(blob => window.Helper.String.blobToBase64(blob))
			.then(function (base64data) {
				download(atob(base64data), fileName, fileType);
			})
	}

	static async downloadPdf(url: string, fileName: string) {
		await this.downloadFile(url, fileName, "application/pdf");
	}
}