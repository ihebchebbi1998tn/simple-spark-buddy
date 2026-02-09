import {namespace} from "@Main/namespace";

type MenuEntryGroupChild = {
	title: string
	priority: number
	dbMenuEntry: Main.Rest.Model.ObservableMain_MenuEntry
}

type MenuEntryGroup = {
	id: string
	title: string
	priority: number
	iconClass: string
	dbMenuEntry: Main.Rest.Model.ObservableMain_MenuEntry
	items: KnockoutObservableArray<MenuEntryGroupChild>
}

export class SiteDetailsMenuEntriesTabViewModel extends window.Main.ViewModels.ViewModelBase {
	isEdit = ko.observable(false)
	menuEntryGroups = ko.observableArray<MenuEntryGroup>([]);

	constructor() {
		super();
	}

	onEdit() {
		this.isEdit(true)
	}

	async reset() {
		this.isEdit(false)
		await this.init();
	}

	async onCancel() {
		await this.reset()
	}

	getSortableOptions(): {
		connectWith?: string;
		items: string;
		handle: string;
		tolerance: string;
		placeholder: string;
		start: (e: any, ui: any) => void;
	} {
		return {
			items: ".sortable-item",
			handle: ".move",
			tolerance: "pointer",
			placeholder: "sortable-placeholder",
			start: function (e, ui) {
				ui.placeholder.width(ui.item.width());
				ui.placeholder.height(ui.item.height());
			}
		};
	}

	afterMenuEntryMove(arg: {
		item: MenuEntryGroup | MenuEntryGroupChild,
		sourceIndex: number,
		sourceParent?: KnockoutObservableArray<MenuEntryGroup | MenuEntryGroupChild>,
		targetIndex: number,
		targetParent: KnockoutObservableArray<MenuEntryGroup | MenuEntryGroupChild>
	}): void {
		const newArr = arg.targetParent();
		const item = newArr[arg.targetIndex]
		const itemBefore = newArr[arg.targetIndex - 1]
		const itemAfter = newArr[arg.targetIndex + 1]
		let newPriority: number;
		if (itemAfter) {
			newPriority = itemAfter.priority + 1;
		} else {
			newPriority = itemBefore.priority - 1;
		}
		window.database.attachOrGet(item.dbMenuEntry)
		item.dbMenuEntry.Priority(newPriority)
		for (let i = 0; i < newArr.length; i++) {
			for (let j = 0; j < newArr.length; j++) {
				const item = newArr[j];
				const nextItem = newArr[j + 1];
				if (!nextItem) {
					continue;
				}
				if (item.dbMenuEntry.Priority() === nextItem.dbMenuEntry.Priority()) {
					window.database.attachOrGet(item.dbMenuEntry)
					item.dbMenuEntry.Priority(item.dbMenuEntry.Priority() + 1);
				}
			}
		}
	}

	async onSubmit() {
		await window.database.saveChanges();
		window.location.reload();
	}

	async init() {
		const menuEntries = await window.database.Main_MenuEntry.toArray()
		const menuEntryGroups = menuEntries.reduce((groups, menuEntry, idx, self) => {
			let group = groups.find(group => group.id === menuEntry.Category || group.id === menuEntry.Title);
			const isParent = menuEntry.Category === null;
			if (!group) {
				const groupParent = self.find(e => e.Category === null &&
					(e.Title === menuEntry.Title || e.Title === menuEntry.Category));
				const newGroup: MenuEntryGroup = {
					id: groupParent.Title,
					title: window.Helper.getTranslatedString(groupParent.Title),
					priority: groupParent.Priority,
					iconClass: groupParent.IconClass,
					items: ko.observableArray([]),
					dbMenuEntry: groupParent.asKoObservable()
				}
				group = newGroup
				groups.push(newGroup)
			}
			if (!isParent) {
				group.items.push({
					title: window.Helper.getTranslatedString(menuEntry.Title),
					priority: menuEntry.Priority,
					dbMenuEntry: menuEntry.asKoObservable()
				})
			}
			return groups;
		}, [] as MenuEntryGroup[]);
		menuEntryGroups.sort((a, b) => b.priority - a.priority)
		menuEntryGroups.forEach(group => {
			group.items.sort((a, b) => b.priority - a.priority)
		})
		this.menuEntryGroups(menuEntryGroups)
	}

}

namespace("Main.ViewModels").SiteDetailsMenuEntriesTabViewModel = SiteDetailsMenuEntriesTabViewModel;