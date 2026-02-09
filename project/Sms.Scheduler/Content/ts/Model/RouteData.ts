export class RouteData {
	technician: KnockoutObservable<string>;
	color: KnockoutObservable<string>;

	constructor(technician: KnockoutObservable<string> = null, color: KnockoutObservable<string> = null) {
		this.technician = technician;
		this.color = color;
	}

}