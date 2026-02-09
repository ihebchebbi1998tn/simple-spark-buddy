export class HelperDistinct {

	static createIndex(array: KnockoutObservableArray<any> | KnockoutComputed<any[]>, path: string): void {
		function distinct(target: any, prop: string) {
			var propParts = prop.split('.');
			target.index = target.index || {};
			target.indexKeys = target.indexKeys || {};

			var targetPropIndex = target.index;
			var targetPropIndexKeys = target.indexKeys;
			for (var i = 0; i < propParts.length; i++) {
				var currentPart = propParts[i];
				if (i == propParts.length - 1) {
					targetPropIndex[currentPart] = ko.observable({});
					targetPropIndexKeys[currentPart] = ko.observable([]);
					targetPropIndex = targetPropIndex[currentPart];
					targetPropIndexKeys = targetPropIndexKeys[currentPart];
				} else {
					targetPropIndex[currentPart] = targetPropIndex[currentPart] || {};
					targetPropIndexKeys[currentPart] = targetPropIndexKeys[currentPart] || {};
					targetPropIndex = targetPropIndex[currentPart];
					targetPropIndexKeys = targetPropIndexKeys[currentPart];
				}
			}

			ko.computed(function () {
				//rebuild index
				var propIndex = {};
				var propIndexKeys = [];

				ko.utils.arrayForEach(target(), function (item) {
					var propValue = item;
					for (var i2 = 0; i2 < propParts.length; i2++) {
						var currentPart2 = propParts[i2];
						propValue[currentPart2] = propValue[currentPart2] || {};
						propValue = propValue[currentPart2];
					}
					var unwrapped = ko.utils.unwrapObservable(propValue) || '';
					// @ts-ignore
					var key = typeof unwrapped.toJSON === 'function' ? unwrapped.toJSON() : unwrapped.toString();
					propIndex[key] = propIndex[key] || [];
					propIndex[key].push(item);
					if (propIndexKeys.indexOf(key) == -1) {
						propIndexKeys.push(key);
					}
				});

				targetPropIndex(propIndex);
				targetPropIndexKeys(propIndexKeys);
			});

			return target;
		}

		distinct(array, path);
	}

	static getIndex<T>(array: KnockoutObservableArray<T> | KnockoutComputed<T[]>, path: string): { [key: string]: Array<T> } {
		if (!window.Helper.Distinct.hasIndex(array, path)) {
			window.Helper.Distinct.createIndex(array, path);
		}
		// @ts-ignore
		return array.index[path]()
	}

	static getIndexKeys(array: KnockoutObservableArray<any> | KnockoutComputed<any[]>, path: string): string[] {
		if (!window.Helper.Distinct.hasIndex(array, path)) {
			window.Helper.Distinct.createIndex(array, path);
		}
		// @ts-ignore
		return array.indexKeys[path]()
	}

	static hasIndex<T>(array: KnockoutObservableArray<T> | KnockoutComputed<T[]>, path: string): boolean {
		// @ts-ignore
		return array.index !== undefined && array.index[path] !== undefined && array.indexKeys !== undefined && array.indexKeys[path] !== undefined;
	}
}