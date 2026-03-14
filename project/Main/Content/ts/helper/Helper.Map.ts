export class HelperMap {

	static getPopupItemTemplateInformation(item: any): HTMLDivElement {
		let helperElement = document.createElement("div");
		if (!item) {
			return helperElement;
		}
		const element = $(`#MapItemTemplate`);
		const context = ko.contextFor($(`#${ko.unwrap(item.Id)}`)[0]);
		helperElement.innerHTML = element[0].innerHTML;
		ko.applyBindings(context, helperElement);
		return helperElement;
	}

	static getMapMarkerColor(item: any): string {
		return $(`#${ko.unwrap(item.Id)} > .lv-avatar`).css('background-color');
	}

	static getMapMarkerContent(item: any): string {
		return $(`#${ko.unwrap(item.Id)} > .lv-avatar`).text();
	}
}