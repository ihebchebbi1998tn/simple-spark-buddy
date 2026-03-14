import {AbstractViewModel} from "./abstractViewModel";
import {Breadcrumb} from "./breadcrumbs";
import {registerComponent} from "./componentRegistrar";
import {RecentPageHistoryItem} from "./recentPageHistoryItem";
import {namespace} from "./namespace";

registerComponent({
	componentName: "breadcrumbs",
	template: "Main/Template/Breadcrumbs",
	viewModel: {
		createViewModel: function(params) {
			return params.viewModel;
		},
	},
	permission: "WebAPI::RecentPage"
});

export class MenuBreadcrumbsViewModel extends AbstractViewModel {
	activeMenu: string;
	activeSubmenu: string;
	historyPaths: KnockoutObservableArray<RecentPageHistoryItem>;
	readonly home: Breadcrumb;
	breadcrumbs: Breadcrumb[];
	readonly breadcrumbPaths: KnockoutObservableArray<Breadcrumb>;
	readonly maxHistory: number;
	customBreadcrumbs: Breadcrumb[];
	readonly currentUser: string;
	todayDate: Date;
	yesterdaysDate: Date;
	readonly backupPageTitle = document.title;
	readonly lastBreadcrumbText: KnockoutObservable<string>;
	/** This is only created for JayData, because using 'this' in that context is not valid for a Typescript Abstract
	 * Syntax Tree. Without this the compiler optimizes it away, therefore breaking the JayData.
	 * See the usage at: {@link MenuBreadcrumbsViewModel.persistRecentPage} */
	link: string;

	constructor(rootNode = null) {
		super(rootNode);
		this.activeMenu = "";
		this.activeSubmenu = "";
		this.historyPaths = ko.observableArray([]);
		this.home = new Breadcrumb(window.Helper.String.getTranslatedString("Home"), null, "#/Main/Dashboard/IndexTemplate");
		this.breadcrumbs = [];
		this.breadcrumbPaths = ko.observableArray([this.home]);
		this.maxHistory = 15;
		this.calculateDates();
		this.customBreadcrumbs = [];
		this.currentUser = window.Helper.User.getCurrentUserName();
		this.loadHistory();
		this.lastBreadcrumbText = ko.observable("");
	}

	/** Calculates the dates for the session.*/
	calculateDates(): void {
		const now = new Date();
		this.todayDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
		this.yesterdaysDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 1);
	}

	/**
	 * Updates the breadcrumbs, first removes all of them, then repopulates it.
	 *
	 * @param useExistent - This specifies, if a previously set Breadcrumb should be loaded. If true, then
	 * {@link MenuBreadcrumbsViewModel.breadcrumbs} will be used. This overrides any Breadcrumb would be calculated.
	 * @param customBreadcrumbs - This specifies if the custom Breadcrumb should be loaded. If true, then the
	 * {@link MenuBreadcrumbsViewModel.customBreadcrumbs} will be used as an addition!
	 */
	async update(useExistent = false, customBreadcrumbs = false): Promise<void> {
		if (!useExistent) {
			this.activeSubmenu = $(".main-menu .sub-menu li a.active").text();
			this.activeMenu = $(".main-menu li.active a").text().trim();
			this.activeMenu = this.activeMenu.split(/\n/)[0];
		}
		this.breadcrumbPaths.removeAll();
		this.lastBreadcrumbText("");
		if (useExistent || (this.activeMenu || this.activeSubmenu)) {
			this.breadcrumbPaths.push(this.home);
		}
		if (useExistent) {
			this.breadcrumbPaths.push(...this.breadcrumbs);
			const categorySpecifier = this.breadcrumbPaths()[this.breadcrumbPaths().length - 2].name;
			const lastItem: Breadcrumb = this.breadcrumbPaths()[this.breadcrumbPaths().length - 1];
			document.title = `${this.backupPageTitle} - ${lastItem.name}`;
			if (!customBreadcrumbs) {
				this.lastBreadcrumbText(this.breadcrumbPaths.pop().name);
			}
			await this.addHistory(lastItem.id, lastItem.name, lastItem.link, categorySpecifier);
		} else {
			if (this.activeSubmenu) {
				document.title = `${this.backupPageTitle} - ${this.activeSubmenu}`;
				this.breadcrumbPaths.push(new Breadcrumb(this.activeSubmenu, null, window.location.hash));
			} else if (this.activeMenu) {
				document.title = `${this.backupPageTitle} - ${this.activeMenu}`;
				if (!window.location.hash.includes("Dashboard"))
				{
					this.breadcrumbPaths.push(new Breadcrumb(this.activeMenu, null, window.location.hash));
				}
			}
		}
		if (customBreadcrumbs) {
			if (this.customBreadcrumbs.length > 0 && this.customBreadcrumbs[0].name === null) {
				this.breadcrumbPaths()[this.breadcrumbPaths().length - 1].onClick = this.customBreadcrumbs[0].onClick;
				this.customBreadcrumbs.shift();
			}
			this.breadcrumbPaths.push(...this.customBreadcrumbs);
		}
		// It removes the last item from the breadcrumbs to make it a text, not an anchor
		if (!this.lastBreadcrumbText() && this.breadcrumbPaths.length)
		{
			this.lastBreadcrumbText(this.breadcrumbPaths.pop().name);
		}
	}

	/** Gets the RecentPages which are from today available in the current history.*/
	today(): RecentPageHistoryItem[] {
		return this.historyPaths()
		.filter(e => this.todayDate.getTime() <= e.date)
		.sort((a, b) => MenuBreadcrumbsViewModel.compareHistoryItems(a, b));
	}

	/** Gets the RecentPages which are from yesterday available in the current history.*/
	yesterday(): RecentPageHistoryItem[] {
		return this.historyPaths()
		.filter(e => this.yesterdaysDate.getTime() <= e.date && e.date < this.todayDate.getTime())
		.sort((a, b) => MenuBreadcrumbsViewModel.compareHistoryItems(a, b));
	}

	/** Gets the RecentPages which are older than two days available in the current history.*/
	older(): RecentPageHistoryItem[] {
		return this.historyPaths()
		.filter(e => e.date < this.yesterdaysDate.getTime())
		.sort((a, b) => MenuBreadcrumbsViewModel.compareHistoryItems(a, b));
	}

	/** Compares two {@link RecentPageHistoryItem} by date in reverse order. */
	static compareHistoryItems(a: RecentPageHistoryItem, b: RecentPageHistoryItem): number {
		return a.date > b.date ? -1 : 1;
	}

	/** Adds a new HistoryItem to the list and persists it into the database.*/
	async addHistory(id: string, name: string, link: string, categorySpecifier: string): Promise<void> {
		if (!link) return;
		let title: string;
		if (categorySpecifier) {
			title = `${name} (${categorySpecifier})`;
		} else {
			title = name;
		}
		const items = this.historyPaths().filter(e => e.title === title && e.name === name);
		if (items.length) this.historyPaths.splice(this.historyPaths().indexOf(items[0]), 1);
		while (this.historyPaths().length >= this.maxHistory) {
			this.historyPaths.pop();
		}
		const historyItem = new RecentPageHistoryItem(id, link, name, categorySpecifier, Date.now());
		this.historyPaths.unshift(historyItem);
		return this.persistRecentPage(historyItem).then().catch(reason => {
			if (reason) window.Log.warn(reason);
		});
	}

	/**Loads the RecentPages for the current user from the database.*/
	loadHistory(): Promise<void> {
		if (!window.database.Main_RecentPage) return Promise.resolve();
		return window.database.Main_RecentPage
		.filter(page => page.ModifyUser === this.currentUser, {currentUser: this.currentUser})
		.take(this.maxHistory)
		.toArray()
		.then(arr => {
			const sortedItems: Crm.Library.Rest.Model.Main_RecentPage[] = arr.sort((a, b) => a.ModifyDate.valueOf() - b.ModifyDate.valueOf());
			for (const element of sortedItems) {
				const historyItem = new RecentPageHistoryItem(element.Id, element.Url, element.Title, element.Category, element.ModifyDate.valueOf());
				this.historyPaths.unshift(historyItem);
			}
		});
	}

	/**
	 * Persists the data into the database. If the HistoryItem is already present in the database the counter
	 * will be incremented, and updates the modified date. If the HistoryItem does not exist yet, it will create e new one.
	 */
	async persistRecentPage(historyItem: RecentPageHistoryItem): Promise<void> {
		const link = historyItem.link.substring(1).split("?")[0];
		if (!window.database.Main_RecentPage) return;
		const recentPages: any[] = await window.database.Main_RecentPage
		.filter(page => page.Url === this.link
			&& page.ModifyUser === this.currentUser, {link, currentUser: this.currentUser})
		.take(1).toArray();
		if (recentPages.length > 0) {
			const recentPage: Crm.Library.Rest.Model.Main_RecentPage = recentPages[0];
			window.database.attachOrGet(recentPage);
			recentPage.Count++;
		} else {
			const recentPage = window.database.Main_RecentPage.defaultType.create();
			recentPage.Username = this.currentUser;
			recentPage.Url = link;
			recentPage.Count = 1;
			recentPage.IsActive = true;
			recentPage.Title = historyItem.name;
			recentPage.Category = historyItem.categorySpecifier;
			window.database.Main_RecentPage.add(recentPage);
		}
		await window.database.saveChanges();
	}

	/** This is used externally by other pages to push their Breadcrumbs in case by case basis.*/
	async setBreadcrumbs(breadcrumbs: Breadcrumb[]) {
		this.breadcrumbs = breadcrumbs;
		await this.update(true);
	}

	/**
	 * This is used externally, but with this, you can implement sub Breadcrumbs which are used in a page.
	 * Currently, this provides a solution for the Configurator since it has an inline Breadcrumbs which uses
	 * Knockout functionality to map certain parts of the page.
	 */
	setCustomBreadcrumbs(arr: Breadcrumb[]) {
		this.customBreadcrumbs = arr;
		this.update(false, true);
	}

	/**
	 * This is the general event handler for the Breadcrumbs. This is bound to the **click** event.
	 * If {@link Breadcrumb.onClick} is a function, it will be called and its return value will be propagated back,
	 * else true returned, allowing the default behaviour to run.
	 */
	goBack(data: Breadcrumb): boolean {
		return data.onClick ? data.onClick() : true;
	}
}

namespace("Main.ViewModels").MenuBreadcrumbsViewModel = MenuBreadcrumbsViewModel;
