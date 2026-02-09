/**  Breadcrumbs implementation for the site header. */
export class Breadcrumb {

	/** Text used on the anchor tag.*/
	public name: string;
	/** Navigation link for the anchor tag.*/
	public link: string;
	/** A function pointer, can be anonymous, if set this will be called on the click of the Breadcrumb.*/
	public onClick: () => boolean;
	/** GUID string for the database, this is also part of the link.*/
	public id: string;
	/** The permission that is required to access the page pointed by the link. */
	public requiredPermission: string;

	/**
	 * Create a new breadcrumb
	 *
	 * @param id: GUID string, see: {@link Breadcrumb.id}
	 * @param name: Name of the page, this shown in the breadcrumb, see: {@link Breadcrumb.name}
	 * @param link: Link for the anchor tag, see: {@link Breadcrumb.link}
	 * @param onClick: Function pointer, see: {@link Breadcrumb.onClick}
	 * @param requiredPermission: Permission for link, see: {@link Breadcrumb.requiredPermission}
	 */
	constructor(name: string, requiredPermission: string = null, link = "#", onClick = null, id: string = null) {
		this.name = name;
		this.link = link;
		this.onClick = onClick;
		this.id = id;
		this.requiredPermission = requiredPermission;
	}

}

window.Breadcrumb = Breadcrumb;
