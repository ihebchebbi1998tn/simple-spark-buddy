import { namespace } from "@Main/namespace"

export class FlowRuleListIndexViewModel extends window.Main.ViewModels.GenericListViewModel<Main.Flow.Rest.Model.MainFlow_FlowRule, Main.Flow.Rest.Model.ObservableMainFlow_FlowRule> {
	constructor() {
		super("MainFlow_FlowRule", ["EntityType"], ["ASC"]);
	}

	getItemGroup(item: Main.Flow.Rest.Model.ObservableMainFlow_FlowRule): ItemGroup {
		let entityType = window.ko.unwrap(item.EntityType).split(".");
		return { title: window.Helper.String.getTranslatedString(entityType[entityType.length - 1]) };
	}

	getAvatarColor(action: number): string {
		switch (action) {
			case 0:
				return 'bgm-green'
			case 1:
				return 'bgm-amber'
			case 2:
				return 'bgm-red'
			default:
				return 'bgm-bluegray'
		}
	}

	deleteRule(item: Main.Flow.Rest.Model.ObservableMainFlow_FlowRule): void {
		window.Helper.Confirm.confirmDelete().done(function() {
			window.database.remove(item);
			window.database.saveChanges();
		});
	}
}

namespace("Main.Flow.ViewModels").FlowRuleListIndexViewModel = FlowRuleListIndexViewModel;