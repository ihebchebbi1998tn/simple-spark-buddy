
export class HelperObject {

	static removeEmptyParams = (obj: {}): {} => {
		const newObj = {};
		Object.keys(obj).forEach((prop) => {
			if (obj[prop] || obj[prop] === false) {
				newObj[prop] = obj[prop];
			}
		});
		return newObj;
	};

	static objectToString = (obj: {}): string => Object.values(obj).length > 0 ? `,${Object.values(obj).join(",")}` : "";

	static sortObject(object: {}): {} {
		const sortedObj = {};
		const keys = Object.keys(object);
		keys.sort((key1, key2) => key1.toLowerCase().localeCompare(key2.toLowerCase()));
		for (const key of keys) {
			const value = object[key];
			if (value !== null && value !== undefined && typeof value === "object" && !(value instanceof Array)) {
				sortedObj[key] = HelperObject.sortObject(value);
			} else {
				sortedObj[key] = value;
			}
		}
		return sortedObj;
	}
}
