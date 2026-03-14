/**
 * AbstractViewModel is common class for all the view models,
 * where the focus is to apply the view model right away
 */
export abstract class AbstractViewModel {

	/**
	 * @param rootNode - Html node to apply the view model
	 * @remark If you don't create a component the binding will not work,
	 * therefore the {@link @param rootNode} should be null, and after the
	 * super call the {@link applyViewModel} should be called manually.
	 */
	protected constructor(rootNode: Element | null) {
		if (rootNode) {
			this.applyViewModel(rootNode);
		}
	}

	protected applyViewModel(rootNode: Element): void {
		window.ko.applyBindings(this, rootNode);
	}

}
