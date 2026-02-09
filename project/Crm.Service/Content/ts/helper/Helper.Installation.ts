
export class HelperInstallation {
	static getDisplayName(installation: Crm.Service.Rest.Model.CrmService_Installation | Crm.Service.Rest.Model.ObservableCrmService_Installation): string {
		if (!installation) {
			return "";
		}
		let displayName = [ko.unwrap(installation.InstallationNo), ko.unwrap(installation.Description)]
			.filter(Boolean).join(" - ");
		if (ko.unwrap(installation.LegacyInstallationId)) {
			displayName += " (" + ko.unwrap(installation.LegacyInstallationId) + ")";
		}
		return displayName;
	}

	static getTypeAbbreviation(installation: Crm.Service.Rest.Model.CrmService_Installation | Crm.Service.Rest.Model.ObservableCrmService_Installation, installationTypes: {[key:string]: Crm.Service.Rest.Model.Lookups.CrmService_InstallationType}): string {
		if (!installation){
			return "";
		}
		installation = ko.unwrap(installation);
		const installationTypeKey = ko.unwrap(installation.InstallationTypeKey);
		if (installationTypeKey) {
			const installationType = (installationTypes || {})[installationTypeKey];
			if (installationType && installationType.Value) {
				return installationType.Value[0];
			}
		}
		return "";
	}

	static mapForSelect2Display(installation: Crm.Service.Rest.Model.CrmService_Installation): Select2AutoCompleterResult {
		return {
			id: installation.Id,
			item: installation,
			text: Helper.Installation.getDisplayName(installation)
		};
	}

	static mapInstallationPositionForSelect2Display(installationPosition: Crm.Service.Rest.Model.CrmService_InstallationPos, articleTypes: {[key:string]: Crm.Article.Rest.Model.Lookups.CrmArticle_ArticleType}): Select2AutoCompleterResult {
		const displayedObject = {
			id: installationPosition.Id,
			text: installationPosition.PosNo +
				". " +
				installationPosition.ItemNo +
				" - " +
				installationPosition.Description,
			item: installationPosition
		};
		if (!!articleTypes && !!installationPosition.Article) {
			displayedObject.text += " (" + window.Helper.Lookup.getLookupValue(articleTypes, installationPosition.Article?.ArticleTypeKey) + ")";
		}
		return displayedObject;
	}

	static async updatePosNo(installationPosition: Crm.Service.Rest.Model.ObservableCrmService_InstallationPos): Promise<void> {
		if (installationPosition.PosNo()) {
			return;
		}
		let posNo = 1;
		let results = await window.database.CrmService_InstallationPos.filter(
			function (it) {
				return it.InstallationId === this.installationId;
			},
			{installationId: installationPosition.InstallationId()})
			.orderByDescending("it.PosNo")
			.take(1)
			.toArray();
		if (results.length > 0) {
			posNo = Math.max(posNo, parseInt(results[0].PosNo) + 1);
		}
		installationPosition.PosNo(posNo < 10000
			? ("0000" + posNo.toString()).slice(-4)
			: posNo.toString());
	}

	static getInstallationAutocompleteFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation>, term: string) : $data.Queryable<Crm.Service.Rest.Model.CrmService_Installation> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["InstallationNo","Description"]);
		}
		return query;
	}
}