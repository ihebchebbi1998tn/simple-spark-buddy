import {GridRowModel, Model} from "@bryntum/schedulerpro";

export function isAbsenceOrder(order: Model): order is AbsenceOrder {
	return order != null && order instanceof AbsenceOrder && order.OriginalData instanceof Crm.PerDiem.Rest.Model.Lookups.CrmPerDiem_TimeEntryType;
}
export function isArticleDowntime(order: Model): order is AbsenceOrder {
	return order != null && order instanceof AbsenceOrder && order.OriginalData instanceof Crm.Article.Model.Lookups.CrmArticle_ArticleDowntimeReason;
}
export class AbsenceOrder extends GridRowModel {
	static get $name() {
		return 'AbsenceOrder';
	}
	OriginalData: Crm.PerDiem.Rest.Model.Lookups.CrmPerDiem_TimeEntryType | Crm.Article.Model.Lookups.CrmArticle_ArticleDowntimeReason;
	AbsenceType: string;

	static get fields() {
		return [
			{ name: 'OriginalData', type: 'object'},
			{ name: 'name', type: 'string', dataSource: 'OriginalData.Value' },
			{ name: 'id', type: 'string', dataSource: 'OriginalData.Key' }
		]
	}

	get name() {
		return this.OriginalData.Value;
	}

	constructor(data = null) {
		super();
		this.OriginalData = data;
		this.AbsenceType = data instanceof Crm.PerDiem.Rest.Model.Lookups.CrmPerDiem_TimeEntryType ? window.Helper.getTranslatedString('SmsScheduler_Absence') : window.Helper.getTranslatedString('CrmArticle_ArticleDowntime');
	}
}