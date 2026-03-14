/** Class representation for the historical data. */
export class RecentPageHistoryItem {
	/** GUID string for the database. */
	public id: string;
	/** Link for the anchor tag. */
	public link: string;
	/** Create date of the item. */
	public date: number;
	/** Name of the item. */
	public name: string;
	/** Category name for the item. */
    public categorySpecifier: string;
    /** Calculated value for the tooltip, "name (categorySpecifier)". */
	public title: string;

	/**
	 * @param id - GUID {@link RecentPageHistoryItem.id}
	 * @param link - {@link RecentPageHistoryItem.link}
	 * @param name -  {@link RecentPageHistoryItem}
	 * @param date - Date of modification {@link RecentPageHistoryItem.date}
	 * @param categorySpecifier - Category specifier {@link RecentPageHistoryItem.categorySpecifier}
	 */
	constructor(id: string, link: string, name: string, categorySpecifier: string, date = Date.now() ) {
		this.id = id;
		this.link = link.startsWith("#") ? link : `#${link}`;
		this.date = date;
		this.name = name.length > 500 ? name.substring(0, 497) + "..." : name;
		this.categorySpecifier = categorySpecifier;
		this.title = `${name} (${categorySpecifier})`;
	}
}