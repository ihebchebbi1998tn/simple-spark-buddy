ko.bindingHandlers.infiniteScrollItem = {
	init: function(element, valueAccessor) {
		const infiniteScrollArray = valueAccessor().array;
		const index = ko.unwrap(valueAccessor().index);
		if (!infiniteScrollArray.infiniteScroll) {
			return;
		}

		function handleIntersect(entries) {
			entries.forEach(function(entry) {
				if (entry.intersectionRatio === 0 &&
					infiniteScrollArray.infiniteScroll.visibleIndexes.indexOf(index) !== -1) {
					infiniteScrollArray.infiniteScroll.visibleIndexes.remove(index);
				} else if (entry.intersectionRatio === 1 &&
					infiniteScrollArray.infiniteScroll.visibleIndexes.indexOf(index) === -1) {
					infiniteScrollArray.infiniteScroll.visibleIndexes.push(index);
				}
			});
		}

		const options = {
			root: null,
			rootMargin: "0px",
			threshold: 1
		};

		const observer = new IntersectionObserver(handleIntersect, options);
		// @ts-ignore
		observer.POLL_INTERVAL = 100; // polyfill configuration
		observer.observe(element);
	}
};
// @ts-ignore
ko.extenders.infiniteScroll = function<TItem>(target: KnockoutObservableArrayWithInfiniteScroll<TItem>) {
	const props: InfiniteItemsScrollProps<TItem> = {} as InfiniteItemsScrollProps<TItem>;
	target.infiniteScroll = props;
	props.visibleIndexes = ko.observableArray([]);
	props.isIndexVisible = function(index) {
		return props.visibleIndexes.indexOf(index) !== -1;
	};
	props.isVisible = function(item) {
		return props.visibleItems().indexOf(item) !== -1;
	};
	props.firstVisibleIndex = ko.computed(function() {
		return Math.min.apply(this, props.visibleIndexes());
	});
	props.lastVisibleIndex = ko.computed(function() {
		return Math.max.apply(this, props.visibleIndexes());
	});
	props.visibleItems = ko.pureComputed(function() {
		return target().slice(props.firstVisibleIndex(), props.lastVisibleIndex());
	});
};

interface InfiniteItemsScrollProps<TItem> {
	visibleItems: KnockoutComputed<TItem[]>
	visibleIndexes: KnockoutObservableArray<number>
	firstVisibleIndex: KnockoutComputed<number>
	lastVisibleIndex: KnockoutComputed<number>
	isIndexVisible(index: number): boolean
	isVisible(item: TItem): boolean
}

export interface KnockoutObservableArrayWithInfiniteScroll<TItem> extends KnockoutObservableArray<TItem> {
	infiniteScroll: InfiniteItemsScrollProps<TItem>
}