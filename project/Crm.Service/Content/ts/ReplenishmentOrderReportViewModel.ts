import {namespace} from "@Main/namespace";
import {HelperQuantityUnit} from "../../../Crm.Article/Content/ts/helper/Helper.QuantityUnit";

export class ReplenishmentOrderReportViewModel {
	replenishmentOrder = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ReplenishmentOrder>(null);
	site = ko.observable<Main.Rest.Model.Main_Site>(null);
	lookups: LookupType = {
		quantityUnits: {$tableName: "CrmArticle_QuantityUnit"}
	}
	headerHeight = ko.observable<number>(0);
	footerHeight = ko.observable<number>(0);

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await window.Helper.Database.initialize()
		await window.Crm.Offline.Bootstrapper.initializeSettings();
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		let replenishmentOrder = await window.database.CrmService_ReplenishmentOrder
			.include("Items")
			.include("Items.Article.QuantityUnitEntry")
			.include("Items.QuantityUnitEntry")
			.include("Items.ServiceOrderMaterials")
			.include("Items.ServiceOrderMaterials.ServiceOrderHead")
			.include("ResponsibleUserObject")
			.find(id);
		this.replenishmentOrder(replenishmentOrder.asKoObservable());
		this.replenishmentOrder().Items(this.replenishmentOrder().Items().sort((x, y) => x.MaterialNo().localeCompare(y.MaterialNo())));
		let site = await window.database.Main_Site.GetCurrentSite().first();
		this.site(site);
		if (window.Main &&
			window.Main.Settings &&
			window.Main.Settings.Report) {
			var headerHeight = +window.Main.Settings.Report.HeaderHeight +
				+window.Main.Settings.Report.HeaderSpacing;
			this.headerHeight(headerHeight);
			var footerHeight = +window.Main.Settings.Report.FooterHeight +
				+window.Main.Settings.Report.FooterSpacing;
			this.footerHeight(footerHeight);
		}
	};

	sumQty(materialNo: string): number {
		let sum = 0;
		this.replenishmentOrder().Items().forEach(item => {
			if (item.MaterialNo() == materialNo)
				if (item.Article() && item.QuantityUnitEntry() && item.Article().QuantityUnitEntry)
					sum += HelperQuantityUnit.getConversionRate(item.QuantityUnitEntry, item.Article().QuantityUnitEntry) * item.Quantity();
				else
					sum += item.Quantity();
		});
		return sum;
	}
}

namespace("Crm.Service.ViewModels").ReplenishmentOrderReportViewModel = ReplenishmentOrderReportViewModel;
namespace("Crm.Service.ViewModels").ReplenishmentOrderReportPreviewModalViewModel = ReplenishmentOrderReportViewModel;