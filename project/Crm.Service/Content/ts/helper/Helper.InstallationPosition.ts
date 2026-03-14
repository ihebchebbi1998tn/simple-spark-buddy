export class HelperInstallationPosition {
	static getPositionColor(installationPosition: Crm.Service.Rest.Model.ObservableCrmService_InstallationPos, articleTypes: { [key: string]: Crm.Article.Rest.Model.Lookups.CrmArticle_ArticleType }): string {
		const articleTypeKey = installationPosition.Article()?.ArticleTypeKey();
		let bgmColor = "#9e9e9e";
		if (installationPosition.IsInstalled()) {
			if (!!articleTypes && !!articleTypeKey) {
				const articleType = articleTypes[articleTypeKey];
				bgmColor = articleType?.Color;
			}
		}
		return bgmColor;
	}
}