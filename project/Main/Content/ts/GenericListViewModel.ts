import { ViewModelBase } from "./ViewModelBase";
import { namespace } from "./namespace";
import { HelperString } from "./helper/Helper.String";
import { HelperUrl } from "./helper/Helper.Url";
import { HelperObject } from "./helper/Helper.Object";
import { HelperDatabase } from "./helper/Helper.Database";
import type { TimeLineEvent } from "./DashboardCalendarWidgetViewModel";
import { HelperConfirm } from "./helper/Helper.Confirm";
import moment from "moment";
import { uniq } from "lodash";
import type {KnockoutObservableArrayWithInfiniteScroll} from "@Main/knockout.custom.infiniteScroll";

export class GenericListViewModel<TModel extends $data.Entity, TObservableModel extends $data.Entity, TItemViewModel extends TObservableModel = TObservableModel> extends window.Main.ViewModels.ViewModelBase {

	// region general
	booleanOptions: ({ Value: boolean | null; Text: string })[];
	bookmark = ko.observable<Bookmark<TModel>>(null);
	bookmarks = ko.observableArray<Bookmark<TModel>>([]);
	bulkActions = ko.observableArray<BulkAction>([]);
	controller: string = null;
	currentSearch: Promise<void> = null;
	entityType: string;
	groupBys = ko.observableArray<string>([]);
	infiniteScroll = ko.observable<boolean>(false);
	isEditing = ko.observable<boolean>(null);
	joins = ko.observableArray<string | {Selector: string, Operation: string, eventHandlers?: {}}>([]);
	map = ko.observable<(it: TModel) => TModel>(null);
	plugin: string = null;
	query = ko.observable<$data.Queryable<TModel>>(null);
	enableUrlUpdate = ko.observable<boolean>(true);
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	isTabViewModel = ko.observable<boolean>(false);
	getDefaultSortProperties = ko.observableArray<string>([]);
	getDefaultSortPropertyNames: KnockoutComputed<string>;
	items = ko.observableArray<TItemViewModel>([]);
	selectedItems = ko.observableArray<TItemViewModel>([]);
	allItemsSelected = ko.observable<boolean>(false);
	lookups: LookupType = {};
	pageTitle: string;
	replicationHintInfo: {SettingName: string, HintTranslationKey: string} | null = null;
	replicationHint = ko.observable<string>(null);
	// endregion general

	// region sorting
	orderBy = ko.observable<string | string[]>(null);
	orderByDirection = ko.observable<"ASC" | "DESC" | string[]>(null);
	orderByParameters = ko.observable<string | any>(null);
	// endregion

	// region paging
	page = ko.observable<number>(1);
	pageSize = ko.observable<number>(25);
	pageState: KnockoutComputed<string>;
	totalItemCount = ko.observable<number>(null);
	totalPages: KnockoutComputed<number>;
	hasPreviousPage: KnockoutComputed<boolean>;
	hasNextPage: KnockoutComputed<boolean>;
	displayedPages: KnockoutComputed<number[]>;
	displayOnlyMultiplePages = ko.observable<boolean>(false);
	// endregion paging

	// region filtering
	filters: any = {};
	filteredProperties = ko.observableArray<{
		name: string,
		caption: string,
		value: string,
		displayedValue: any
	}>([]);
	isFiltered = ko.observable<boolean>(false);
	defaultFilters = {};
	// endregion filtering

	// region view modes
	viewModes = ko.observableArray<{ Key: string, Value: string }>([]);
	viewMode = ko.observable<{ Key: string; Value: string }>(null);
	// endregion view modes

	// region calendar view
	timelineProperties = ko.observableArray<{ Start: string, End: string, Caption: string }>([]);
	timelineProperty = ko.observable<{ Start: string, End: string, Caption: string }>(null);
	timelineStart = ko.observable<string>(null);
	timelineEnd = ko.observable<string>(null);
	calendarDefaultView = ko.observable<string>("month");

	// endregion calendar view

	constructor(entityType: string, orderBy?: string | string[], orderByDirection?: "ASC" | "DESC" | string[], joins?: Array<string | {Selector: string, Operation: string, eventHandlers?: {}}>, groupBys?: string[], map?: (it: any) => any) {
		super();

		// region general
		this.booleanOptions = [
			{Value: null, Text: "All"}, {Value: true, Text: "True"}, {Value: false, Text: "False"}
		];
		window.Helper.Distinct.createIndex(this.bookmarks, "Category");
		this.currentSearch = null;
		this.entityType = entityType;
		this.groupBys(groupBys || []);
		this.joins(joins || []);
		this.map(map || null);

		this.getDefaultSortPropertyNames = ko.pureComputed(() => {
			let name = "";
			this.getDefaultSortProperties().forEach((value, index) => {
				if (index)
					name += ", ";
				let parts = value.split(".");
				let lastPart = parts[parts.length - 1];
				name += HelperString.getTranslatedString(lastPart);
			})
			return name;
		});
		if (Array.isArray(orderBy) && orderBy.length > 1) {
			this.getDefaultSortProperties(orderBy);
		}
		this.selectedItems.subscribe(() => {
			this.allItemsSelected(false);
		});
		// endregion general

		// region sorting
		this.orderBy(orderBy || null);
		this.orderByDirection(orderByDirection || null);
		this.orderBy.subscribe((value) => {
			if (!!value && !this.orderByDirection()) {
				this.orderByDirection("ASC");
			}
			if (!value && !!this.orderByDirection()) {
				this.orderByDirection(null);
			}
		});
		// endregion

		// region paging
		this.pageState = ko.pureComputed(() => {
			if (this.infiniteScroll()) {
				return null;
			}
			let firstElementOnPage = (this.page() - 1) * this.pageSize() + 1;
			let lastElementOnPage = (this.page() - 1) * this.pageSize() + this.items().length;
			return HelperString.getTranslatedString("PageStateFormat")
				.replace("{0}", firstElementOnPage.toString())
				.replace("{1}", lastElementOnPage.toString())
				.replace("{2}", String(this.totalItemCount()));
		});
		this.totalPages = ko.pureComputed(() => {
			return Math.ceil(this.totalItemCount() / this.pageSize());
		});
		this.hasPreviousPage = ko.pureComputed(() => {
			return this.page() > 1;
		});
		this.hasNextPage = ko.pureComputed(() => {
			return this.page() < this.totalPages();
		});
		this.displayedPages = ko.pureComputed(() => {
			const displayedPageCount = 5;
			const displayedPages = [this.page()];
			for (let i = 1; i < displayedPageCount; i++) {
				if (displayedPages.length < displayedPageCount && this.page() - i > 0) {
					displayedPages.unshift(this.page() - i);
				}
				if (displayedPages.length < displayedPageCount && this.page() + i <= this.totalPages()) {
					displayedPages.push(this.page() + i);
				}
			}
			return displayedPages;
		});
		// endregion paging

		// region view modes
		this.viewModes([{
			Key: "List",
			Value: HelperString.getTranslatedString("List")
		}]);
		this.viewMode(this.viewModes()[0]);
		// endregion view modes

		// region calendar view
		const calendarViewMode = {Key: "Calendar", Value: HelperString.getTranslatedString("Calendar")};
		this.timelineProperties.subscribe(values => {
			this.viewModes.remove(calendarViewMode);
			if (values.length === 0) {
				this.timelineProperty(null);
			} else {
				this.timelineProperty(values[0]);
				this.viewModes.push(calendarViewMode);
			}
		});
		// endregion calendar view

		// region events
		this.triggerEvent = eventName => {
			if ((navigator.userAgent.match(/(iPad|iPhone|iPod).*/g))) {
				// @ts-ignore
				$.event.trigger(eventName);
			}
		}
		// endregion events
	}

	// region general
	getBaseQuery(): $data.Queryable<TModel> {
		return window.database[this.entityType];
	}

	getFilterQuery(addJoins?: boolean, addOrderBy?: boolean, additionalFilter?: {}): $data.Queryable<TModel> {
		addJoins = addJoins !== false;
		addOrderBy = addOrderBy !== false;
		let query = this.getBaseQuery();
		if (addJoins) {
			query = this.applyJoins(query);
		}
		query = this.applyFilters(query);
		if (additionalFilter) {
			Object.keys(additionalFilter).forEach(function (key) {
				query = query.filter(key, additionalFilter[key]);
			});
		}
		query = this.applyBookmark(query);
		if (addOrderBy) {
			query = this.applyOrderBy(query);
		}
		if (addJoins) {
			query = this.applyMapAndGroupBy(query);
		}
		return query;
	};

	async getSearchResults(resetTotalCount?: boolean): Promise<TModel[]> {
		this.query(this.getFilterQuery(false, true));
		if (resetTotalCount || !this.totalItemCount()) {
			this.query(this.query().withInlineCount());
		}
		let idPropertyName = HelperDatabase.getKeyProperty(this.entityType);
		const ids = await this.query()
			.map("it." + idPropertyName)
			.skip((this.page() - 1) * this.pageSize())
			.take(this.pageSize())
			.toArray();

		// @ts-ignore
		if (ids.totalCount >= 0) {
			// @ts-ignore
			this.totalItemCount(ids.totalCount);
		}
		// @ts-ignore
		if (ids.totalCount === 0) {
			return [];
		}
		if (this.timelineProperty() !== null && this.timelineStart() != null && this.timelineEnd() != null && this.viewMode().Key === "Calendar") {
			let additionalFilter = {};
			additionalFilter["it." + this.timelineProperty().Start + " != null && (it." + this.timelineProperty().End + " != null && it." + this.timelineProperty().End + " >= this.dateStart || it." + this.timelineProperty().Start + " >= this.dateStart) && it." + this.timelineProperty().Start + " <= this.dateEnd"] = {
				dateStart: this.timelineStart(),
				dateEnd: this.timelineEnd()
			};
			return this.getFilterQuery(true, true, additionalFilter).toArray();
		}
		let additionalFilter = {};
		additionalFilter["it." + idPropertyName + " in this.ids"] = {ids: ids.slice()};
		return await this.getFilterQuery(true, true, additionalFilter).toArray();

	};

	async search(scrollToTop: boolean, resetTotalCount?: any, setLoading?: boolean): Promise<void> {
		setLoading = setLoading !== false;
		if (setLoading) {
			this.loading(true);
		}
		if (this.infiniteScroll() && resetTotalCount) {
			this.page(1);
		}
		if (!this.infiniteScroll() || scrollToTop || resetTotalCount) {
			this.selectedItems([]);
		}
		$("input:focus, select:focus, textarea:focus").blur();
		const results = await this.getSearchResults(resetTotalCount);
		const initializedItems = results.map(result => this.initItem(result));
		const items = await this.initItems(initializedItems);

		if (this.infiniteScroll() && !scrollToTop && !resetTotalCount) {
			items.forEach(item => {
				this.items().push(item);
			});
			this.items.valueHasMutated();
		} else {
			this.items(items);
		}
		scrollToTop = scrollToTop && this.viewMode().Key !== "Calendar";
		if (scrollToTop === true) {
			const $listContainer = $(".generic-list:visible").first();
			if ($listContainer.length > 0) {
				$listContainer.get(0).scrollIntoView({block: "start", behavior: "smooth"});
			}
		}

		const filtersWithValue = this.getFiltersWithValue();

		const filteredPropertyNames = ko.utils.arrayFilter(filtersWithValue, (filter) => {
			if (this.defaultFilters.hasOwnProperty(filter) && ko.toJSON(ko.unwrap(this.defaultFilters[filter])) === ko.toJSON(ko.unwrap(this.filters[filter]))) {
				return false;
			}
			if (ko.unwrap(this.filters[filter]) == null && !this.defaultFilters.hasOwnProperty(filter)) {
				return false;
			}
			if (ko.unwrap(this.filters[filter]) === "" && !this.defaultFilters.hasOwnProperty(filter)) {
				return false;
			}
			return true;
		})
		this.isFiltered(filteredPropertyNames.length > 0);
		if (this.isFiltered()) {
			this.filteredProperties([]);
			filteredPropertyNames.forEach(name => {
				if (Object.prototype.hasOwnProperty.call(this.filters, name)) {
					let val;
					if (this.filters[name]) {
						val = this.filters[name]();
					}
					if (val) {
						const caption = !!val.caption ? val.caption : name;
						let displayedValue = val.displayedValue || (val.getDisplayedValue ? val.getDisplayedValue(val.Value) : undefined);
						if (val.Value === "none-filter-value"){
							displayedValue = window.Helper.String.getTranslatedString("None");
						}
						this.filteredProperties.push({
							name: name,
							caption: HelperString.getTranslatedString(caption, caption),
							value: val.Value,
							displayedValue: displayedValue
						});
					}
				}
			});
		}

		if (this.currentSearch) {
			if (this.enableUrlUpdate()) {
				this.saveParamsToUrl();
			}
			if (setLoading) {
				this.loading(false);
			}
		}
		this.triggerEvent("genericlist.searched");
	}

	getFiltersWithValue(): string[] {
		return Object.entries(this.filters)
			.filter(filter => !!filter[1] && !!(filter[1] as KnockoutObservable<any>)() && (filter[1] as KnockoutObservable<any>)().Value != null && (filter[1] as KnockoutObservable<any>)().Value.length !== 0 && (!(filter[1] as KnockoutObservable<any>)().Exclude || (filter[1] as KnockoutObservable<any>)().Exclude === "False"))
			.map(filter => filter[0]);
	}

	// endregion general

	// region sorting
	setOrderBy(newOrderByValue: string): void {
		if (this.entityType) {
			const extensionValuesDefinition = window.database[this.entityType].elementType.getMemberDefinition("ExtensionValues");
			if (extensionValuesDefinition && extensionValuesDefinition.dataType.getMemberDefinition(newOrderByValue)) {
				newOrderByValue = "ExtensionValues." + newOrderByValue;
			}
		}
		if (this.orderBy() !== newOrderByValue) {
			this.orderBy(newOrderByValue);
			this.page(1);
			this.currentSearch = this.search(true);
		}
	}

	setOrderByDirection(newOrderByDirection: "ASC" | "DESC"): void {
		if (this.orderByDirection() !== newOrderByDirection) {
			this.orderByDirection(newOrderByDirection);
			this.page(1);
			this.currentSearch = this.search(true);
		}
	}

	// endregion

	// region paging
	goToFirstPage(): void {
		if (this.page() > 1) {
			this.page(1);
			this.currentSearch = this.search(true);
		}
	}

	goToPreviousPage(): void {
		if (this.page() > 1) {
			this.page(this.page() - 1);
			this.currentSearch = this.search(true);
		}
	}

	goToNextPage(): void {
		if (this.page() < this.totalPages()) {
			this.page(this.page() + 1);
			this.currentSearch = this.search(true);
		}
	}

	goToLastPage(): void {
		if (this.page() < this.totalPages()) {
			this.page(this.totalPages());
			this.currentSearch = this.search(true);
		}
	}

	goToPage(page: number): void {
		if (this.page() !== page) {
			this.page(page);
			this.currentSearch = this.search(true);
		}
	}

	// endregion paging

	// region filtering
	getFilter(filterName: string): any {
		if (!this.filters[filterName]) {
			this.filters[filterName] = ko.observable(null);
		}
		return this.filters[filterName];
	}

	filter(): void {
		this.page(1);
		this.currentSearch = this.search(true, true);
	}

	// endregion filtering

	// region view modes
	setViewMode(viewMode: { Key: string; Value: string }) {
		this.viewMode(viewMode);
	}

	// endregion view modes

	// region calendar view
	setTimelineProperty(timelineProperty: { Start: string, End: string, Caption: string }) {
		this.timelineProperty(timelineProperty);
	};

	setCalendarDefaultView(timePeriod: "month" | "agendaWeek" | "agendaDay" | "basicWeek" | "basicDay"): void {
		const validTimePeriods = ["month", "agendaWeek", "agendaDay", "basicWeek", "basicDay"];
		if (validTimePeriods.indexOf(timePeriod) > -1) {
			this.calendarDefaultView(timePeriod);
		}
	}

	// endregion calendar view

	// region ics
	getFilterExpression(filter: string): string {
		const filterObj = this.filters[filter]();
		const filterKey = filter.trim().split('.').pop();
		if (Array.isArray(filterObj) && filterObj.length >= 1 && (filterObj[0].Value instanceof Date)) {
			const fromDate = filterObj.find(function (x) {
				return x.Operator == ">="
			}).Value;
			const toDate = filterObj.find(function (x) {
				return x.Operator == "<="
			}).Value;
			return "DF_" + filterKey + "_FromTo&XDF_" + filterKey + "_FromDate=" + fromDate.toISOString() + "&XDF_" + filterKey + "_ToDate=" + toDate.toISOString();
		} else if (filterObj instanceof Object && filterObj.Operator != null) {
			return "DF_" + filterKey + "=" + filterObj.Value;
		} else if (filterObj instanceof Object && filterObj.Operator == null) {
			return null;
		} else {
			return "DF_" + filterKey + "=" + filterObj;
		}
	}

	// endregion ics

	// region events
	triggerEvent(eventName) {
		if ((navigator.userAgent.match(/(iPad|iPhone|iPod).*/g))) {
			// @ts-ignore
			$.event.trigger(eventName);
		}
	}

	// endregion events

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		if (this.infiniteScroll()) {
			this.items.extend({infiniteScroll: true});

			(this.items as KnockoutObservableArrayWithInfiniteScroll<any>).infiniteScroll.visibleIndexes.subscribe(() => {
				if (this.items.peek().length - (this.items as KnockoutObservableArrayWithInfiniteScroll<any>).infiniteScroll.lastVisibleIndex.peek() < this.pageSize() && this.page() < this.totalPages() && !this.loading()) {
					this.page(this.page() + 1);
					this.currentSearch = this.search(false);
				}
			});
		}
		this.defaultFilters = JSON.parse(ko.toJSON(this.filters));
		params = params || {};
		Object.getOwnPropertyNames(params).forEach(param => {
			if (param.indexOf("filter.") === 0) {
				const filterName = param.substr("filter.".length);
				if (!!params[param] && params[param] !== '') {
					this.getFilter(filterName)({
						Value: params[param],
						Operator: 'contains',
						caption: filterName.substring(filterName.lastIndexOf('.') + 1, filterName.length),
						displayedValue: params[param]
					});
				}
			}
		});

		if (!!params.filters) {
			const filters = JSON.parse(decodeURIComponent(params.filters));
			Object.getOwnPropertyNames(filters).forEach(filterName => {
				this.getFilter(filterName)(filters[filterName]);
			});
		}

		if (!!params.bookmark) {
			const bookmarkParam = JSON.parse(decodeURIComponent(params.bookmark));
			if (bookmarkParam) {
				const bookmarks = this.bookmarks().filter(x => x.Key === bookmarkParam.Key);
				if (bookmarks.length === 1) {
					this.bookmark(bookmarks[0]);
				}
			}
		}

		if (this.bookmark() && this.bookmark().ApplyFilters) {
			this.bookmark().ApplyFilters();
		}
		if (!!params.orderBy) {
			this.orderBy(JSON.parse(decodeURIComponent(params.orderBy)));
		}
		if (!!params.orderByDirection) {
			this.orderByDirection(JSON.parse(decodeURIComponent(params.orderByDirection)));
		}
		if (!!params.page && !this.infiniteScroll()) {
			this.page(parseInt(decodeURIComponent(params.page)));
		}
		if (!!params.viewMode) {
			this.viewMode(JSON.parse(params.viewMode));
		}
		this.viewMode.subscribe(() => this.filter());
		this.timelineEnd.subscribe(() => {
			this.filter();
		});
		this.timelineProperty.subscribe(() => {
			this.filter();
		});

		this.registerEventHandlers();

		const user = await window.Helper.User.getCurrentUser();
		this.currentUser(user);
		this.currentSearch = this.search(params.scrollToTop === "true", false, false);
		return this.currentSearch;
	};

	getEventHandlerDefinition(): any {
		const handlerDefinition = {};
		handlerDefinition[this.entityType] = {
			"afterCreate": window._.debounce(() => {
				this.currentSearch = this.search(false, true);
			}),
			"afterDelete": window._.debounce(() => {
				if (ko.unwrap(this.items).length === 1 && this.page() > 1) {
					const prevPage = this.page() - 1;
					this.page(prevPage);
					this.currentSearch = this.search(true, true);
				} else {
					this.currentSearch = this.search(false, true);
				}
			}),
			"afterUpdate": window._.debounce(() => {
				this.currentSearch = this.search(false, true);
			})
		}
		return handlerDefinition;
	}

	registerEventHandlers(): void {
		if (this.entityType) {
			const handlerDefinition = this.getEventHandlerDefinition();

			function getEntityType(entityType, join) {
				let joinParts;
				if (typeof join === "string") {
					joinParts = join.split(".");
				} else {
					joinParts = join.Selector.split(".");
				}
				const memberDefinition = entityType.getMemberDefinition(joinParts[0]);
				let result = memberDefinition.elementType || memberDefinition.dataType;
				if (joinParts.length > 1) {
					return getEntityType(result, joinParts.slice(1).join("."));
				}
				while (result.inheritsFrom !== $data.Entity) {
					result = result.inheritsFrom;
				}
				return result;
			}

			this.joins().forEach(join => {
				const entityType = getEntityType(window.database[this.entityType].elementType, join);
				if (!handlerDefinition[entityType.name]) {
					if (typeof join !== "string" && join.eventHandlers){
						handlerDefinition[entityType.name] = join.eventHandlers;
					} else {
						handlerDefinition[entityType.name] = {
							"afterCreate": () => {
								if (!this.loading()) {
									this.currentSearch = this.search(false, true);
								}
							},
							"afterDelete": () => {
								if (!this.loading()) {
									this.currentSearch = this.search(false, true);
								}
							},
							"afterUpdate": () => {
								if (!this.loading()) {
									this.currentSearch = this.search(false, true);
								}
							}
						}	
					}
				}
			});
			HelperDatabase.registerEventHandlers(this, handlerDefinition);
		}
	}

	applyBookmark(query: $data.Queryable<TModel>): $data.Queryable<TModel> {
		if (this.bookmark() !== null && this.bookmark().Expression) {
			query = this.bookmark().Expression(query);
		}
		return query;
	}

	async applyBulkAction(bulkAction: { Name: string, Action?: any, Modal?: { Target: string, Route: string } }): Promise<void> {
		this.loading(true);
		const bulkActionParameter = this.allItemsSelected() === true ? this.getFilterQuery() : this.selectedItems();
		try {
			await bulkAction.Action(bulkActionParameter)
		} finally {
			this.loading(false)
		}
	}

	applyMapAndGroupBy(query: $data.Queryable<TModel>): $data.Queryable<TModel> {
		if (this.map()) {
			query = query.map(this.map());
		}
		this.groupBys().forEach(groupBy => {
			query = query.groupBy(groupBy);
		});
		return query;
	}

	applyJoins(query: $data.Queryable<TModel>): $data.Queryable<TModel> {
		this.joins().forEach((join: string | {Selector: string, Operation: string}) => {
			if (typeof join === "string") {
				query = query.include(join);
			} else {
				query = query.include2(join.Selector + "." + join.Operation);
			}
		});
		return query;
	}

	applyFilter(query: $data.Queryable<TModel>, filterValue: any, filterName?: string): $data.Queryable<TModel> {
		const column = filterValue.Column || filterName;
		if (filterValue.Value) {
			filterValue.Value = ko.unwrap(filterValue.Value);
		}
		if (typeof filterValue === "boolean" || typeof filterValue.Value === "boolean") {
			query = query.filter("it." + column + " == this.value", {value: typeof filterValue.Value == "boolean" ? filterValue.Value : filterValue});
		} else if (filterValue instanceof Date) {
			const dateStart = moment(filterValue).startOf("day").toDate();
			const dateEnd = moment(filterValue).endOf("day").toDate();
			query = query.filter("it." + column + " >= this.dateStart && it." + column + " <= this.dateEnd",
				{dateStart: dateStart, dateEnd: dateEnd});
		} else if (filterValue.Operator && filterValue.Operator.type === "some" && filterValue.Value) {
			if (window.database.storageProvider.name === "oData") {
				query = filterValue.Value === "none-filter-value" ? query.filter("it." + filterValue.Operator.collectionName + ".some(function(it2) { return it2." + filterValue.Operator.collectionProperty + " !== null; }) === false") : query.filter("it." + filterValue.Operator.collectionName + ".some(function(it2) { return it2." + filterValue.Operator.collectionProperty + " === this.id; })", {id: filterValue.Value});
			} else {
				query = filterValue.Value === "none-filter-value" ? query.filter("it." + filterValue.Operator.collectionName + "." + filterValue.Operator.collectionProperty + " === null") : query.filter("it." + filterValue.Operator.collectionName + "." + filterValue.Operator.collectionProperty + " === this.id", {id: filterValue.Value});
			}
		} else if (filterValue.Operator && filterValue.Value == "null") {
			query = query.filter("it." + column+ " " + filterValue.Operator + "null");
		} else if (filterValue.Operator && filterValue.Value !== null && filterValue.Value.length !== 0) {
			if (filterValue.Operator === "contains") {
				const filterValueWords = window.Helper.String.parseWords(filterValue.Value);
				filterValueWords.forEach(filterValueWord => {
					query = query.filter("it." + column + ".contains(this.value)", {value: filterValueWord});
				});
			} else {
				let value = Array.isArray(filterValue.Value) ? filterValue.Value.map(x => x === "none-filter-value" ? null : x) : filterValue.Value === "none-filter-value" ? null : filterValue.Value;
				query = query.filter("it." + column + " " + filterValue.Operator + " this.value",
					{value: value});
			}
		} else if (filterValue) {
			if (filterValue.Type && filterValue.Type === "date") {
				let dateFrom = null;
				let dateTo = null;
				if (!!filterValue.Value) {
					if (filterValue.Value === "FromTo") {
						dateFrom = filterValue.DateFrom;
						dateTo = filterValue.DateTo;
					} else {
						const fromTo = window.Helper.Date.getFromAndToFromString(filterValue.Value);
						dateFrom = fromTo.dateFrom;
						dateTo = fromTo.dateTo;
					}
				}
				if (dateFrom) {
					query = query.filter(column, ">=", dateFrom);
				}
				if (dateTo) {
					query = query.filter(column, "<=", dateTo);
				}
			} else if (filterValue.Type && (filterValue.Type === "scale" || filterValue.Type === "timeRange")) {
				if (filterValue.Min) {
					query = query.filter(column, ">=", filterValue.Min);
				}
				if (filterValue.Max) {
					query = query.filter(column, "<=", filterValue.Max);
				}
			}
		}
		return query;
	};

	applyFilters(query: $data.Queryable<TModel>): $data.Queryable<TModel> {
		for (const filterName in this.filters) {
			if (this.filters.hasOwnProperty(filterName)) {
				let filterValues = ko.unwrap(this.filters[filterName]);
				filterValues = Array.isArray(filterValues) ? filterValues : [filterValues];
				for (let i = 0; i < filterValues.length; i++) {
					const filterValue = filterValues[i];
					if (filterValue === undefined || filterValue === null || filterValue.Operator === null || (filterValue.Operator === null && filterValue.Value === null) || (!!filterValue.Exclude && filterValue.Exclude === "True")) {
						continue;
					}
					query = this.applyFilter(query, filterValue, filterName);
				}
			}
		}
		if (this.timelineProperty() !== null && this.timelineStart() != null && this.timelineEnd() != null && this.viewMode().Key === "Calendar") {
			query = query.filter("it." + this.timelineProperty().Start + " != null && (it." + this.timelineProperty().End + " != null && it." + this.timelineProperty().End + " >= this.dateStart || it." + this.timelineProperty().Start + " >= this.dateStart) && it." + this.timelineProperty().Start + " <= this.dateEnd", {
				dateStart: this.timelineStart(),
				dateEnd: this.timelineEnd()
			});
		}
		return query;
	}

	applyOrderBy(query: $data.Queryable<TModel>): $data.Queryable<TModel> {
		if (!!this.orderBy()) {
			const orderBys = Array.isArray(this.orderBy()) ? this.orderBy() : [this.orderBy()];
			const orderByDirections = Array.isArray(this.orderByDirection()) ? this.orderByDirection() : [this.orderByDirection()];
			const orderByParameters = Array.isArray(this.orderByParameters()) ? this.orderByParameters() : [this.orderByParameters()];
			for (let i = 0; i < orderBys.length; i++) {
				const orderBy = typeof orderBys[i] === "string" ? "it." + orderBys[i] : orderBys[i];
				const orderByParameter = orderByParameters.length > i ? orderByParameters[i] : undefined;
				// TODO: add 'orderByDescending' with 2 parameters to JayData 
				// @ts-ignore
				query = orderByDirections[i] === "DESC" ? query.orderByDescending(orderBy, orderByParameter) : query.orderBy(orderBy, orderByParameter);
			}
		}
		return query;
	}

	async bulkSelectionHasMultipleSelectedValues(propertyName: string): Promise<boolean> {
		const selectedValues = uniq(this.selectedItems().map(x => x[propertyName]()));
		if (selectedValues.length > 1) {
			return true;
		} else if (this.allItemsSelected() === true) {
			const additionalFilter = {};
			additionalFilter["it." + propertyName + " !== this.selectedValue"] = {
				selectedValue: selectedValues[0]
			};
			const count = await this.getFilterQuery(false, false, additionalFilter).count()
			return count > 0;
		} else {
			return false;
		}
	}

	getCsvLink(): string {
		const url = this.getQueryUrl();
		if (!url) {
			return null;
		}
		return url + "&$format=text/csv";
	}

	getDisplayedValueFromLookups(lookupTable: string, keys: string[]): string | null {
		let lookupName = Object.getOwnPropertyNames(this.lookups).find(lookupName => this.lookups[lookupName].$tableName === lookupTable);
		if (lookupName){
			this.lookups[lookupName]["none-filter-value"] ||= {Value: Helper.String.getTranslatedString("None")};
			return keys.map(key => this.lookups[lookupName][key].Value).join(", ");
		}
		return null;
	}

	getIcsLinkAllowed(): boolean {
		return false;
	}

	getIcsLink(): string | null {
		const url = this.getQueryUrl();
		if (!url) {
			return null;
		}
		return url + "&token=" + this.currentUser().GeneralToken + "&$format=text/calendar";
	}

	getQueryUrl(): string {

		let query = window.oDataDatabase[this.entityType];
		query = this.applyFilters(query);
		query = this.applyBookmark(query);
		query = this.applyOrderBy(query);
		if (!query) {
			return null;
		}
		query = query.withInlineCount(() => null);
		// @ts-ignore
		const toArrayExpression = $data.Container.createToArrayExpression(query.expression);
		// @ts-ignore
		const preparator = $data.Container.createQueryExpressionCreator(query.entityContext);
		const expression = preparator.Visit(toArrayExpression);
		// @ts-ignore
		const queryable = $data.Container.createQueryable(query, expression);
		// @ts-ignore
		query = new $data.Query(queryable.expression, queryable.defaultType, queryable.entityContext);
		// @ts-ignore
		const sql = oDataDatabase.storageProvider._compile(query);
		return HelperUrl.qualifyURL(HelperUrl.resolveUrl("~/api" + sql.queryText));
	}

	getRssLink(): string {
		const url = this.getQueryUrl();
		if (!url) {
			return null;
		}
		return url + "&token=" + this.currentUser().GeneralToken + "&$top=100&$format=application/xml+rss";
	}

	getTimelineEvent(item: any, timelineProperty: { Start: string, End: string, Caption: string }): TimeLineEvent {
		let timeLineEvent: TimeLineEvent = {
			allDay: false,
			end: undefined,
			start: undefined,
			title: ""
		};
		timelineProperty = ko.unwrap(timelineProperty || this.timelineProperty);
		if (timelineProperty) {
			timeLineEvent.start = ko.unwrap(item[timelineProperty.Start]);
			timeLineEvent.end = ko.unwrap(item[timelineProperty.End]);
		}
		if (timeLineEvent.start && timeLineEvent.end && timeLineEvent.start.getTime() === timeLineEvent.end.getTime()) {
			timeLineEvent.allDay = true;
		}
		return timeLineEvent;
	}

	initItem(item: TModel): TItemViewModel {
		// @ts-ignore
		const result = item.asKoObservable && !item.innerInstance ? item.asKoObservable() : item;
		result.itemGroup = this.getItemGroup(result);
		return result;
	}

	async initItems(items: TItemViewModel[]): Promise<TItemViewModel[]> {
		return items;
	}

	getItemGroup(item: TItemViewModel): ItemGroup {
		return null;
	}

	getParamsObject(): { filters: string, bookmark: string, orderBy: string, orderByDirection: string, viewMode: string, page?: string } {
		let params = {
			filters: JSON.stringify(HelperObject.removeEmptyParams(ko.toJS(this.filters))),
			bookmark: JSON.stringify(this.bookmark()),
			orderBy: JSON.stringify(this.orderBy()),
			orderByDirection: JSON.stringify(this.orderByDirection()),
			viewMode: JSON.stringify(this.viewMode()),
		}
		if (!this.infiniteScroll()) {
			params["page"] = this.page();
		}
		return $.extend({}, HelperUrl.getCriteriaFromUrl(), params);
	}

	getParamsUrl(): string {
		const params = this.getParamsObject();
		const paramStrings = [];
		Object.getOwnPropertyNames(params).forEach(param => {
			paramStrings.push(param + "=" + encodeURIComponent(params[param]));
		});
		return paramStrings.join("&");
	}

	resetFilter(): void {
		Object.getOwnPropertyNames(this.filters).forEach(filterName => {
			if (this.filters.hasOwnProperty(filterName)) {
				this.filters[filterName](null);
			}
		});
		Object.getOwnPropertyNames(this.defaultFilters).forEach(filterName => {
			this.getFilter(filterName)(this.defaultFilters[filterName]);
		});
		this.page(1);
		this.currentSearch = this.search(true, true);
	}

	resetSingleFilter(filterName: string): void {

		if (this.filters.hasOwnProperty(filterName)) {
			this.filters[filterName](null);
		}

		if (this.defaultFilters.hasOwnProperty(filterName)) {
			this.getFilter(filterName)(this.defaultFilters[filterName]);
		} else {
			this.getFilter(filterName)(null);
		}

		this.page(1);
		this.currentSearch = this.search(true, true);
	}

	saveParamsToUrl(): void {
		let url = window.location.href;
		if (url.indexOf("?") !== -1) {
			url = url.substring(0, url.indexOf("?"));
		}
		const paramsUrl = this.getParamsUrl();
		url += "?" + paramsUrl;
		window.history.replaceState(null, null, url);
	};

	toggleBookmark(bookmark: Bookmark<TModel>): void {
		this.bookmark(bookmark);
		if (bookmark.ApplyFilters) {
			bookmark.ApplyFilters();
		}
		this.page(1);
		this.currentSearch = this.search(true, true);
	}

	toggleSelectAllItems(): void {

		if (this.allItemsSelected()) {
			this.selectedItems([]);
			this.allItemsSelected(false);
		} else {
			this.selectedItems([]);
			this.items().forEach(item => {
				this.selectedItems().push(item);
			});
			this.selectedItems.valueHasMutated();
			this.allItemsSelected(true);
		}
	}

	getEventClick(event?: any): any {
		return;
	}

	async setEditing(entity: TItemViewModel): Promise<void> {
		if (this.isEditing() && window.Helper.Database.hasPendingChanges()) {
			try {
				await HelperConfirm.confirmContinue();
				window.Helper.Database.clearTrackedEntities();
			} catch (_) {
				return;
			}
		}
		// @ts-ignore
		this.isEditing(ko.unwrap(entity.Id));
	}
}

namespace("Main.ViewModels").GenericListViewModel = GenericListViewModel;

$(document).on("press", ".lv-item", function () {
	if ($(".dropdown.open", this).length === 0) {
		$(".bulk-control input", this).trigger("click");
	}
});
