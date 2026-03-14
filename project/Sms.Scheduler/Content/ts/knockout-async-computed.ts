type _KoStatic = typeof ko;

interface AsyncExtended<T> {
	getter: KnockoutComputed<Promise<T>>;
	inProgress: KnockoutObservable<boolean>;
	dispose: () => void;
}

export const computedPromise = <T>(ko: _KoStatic, getter: KnockoutComputed<Promise<T>>, defaultValue: T): KnockoutObservable<T> & AsyncExtended<T> => {
	const innerObservable = ko.observable(defaultValue) as KnockoutObservable<T> & AsyncExtended<T>;
	innerObservable.inProgress = ko.observable(false);
	innerObservable.getter = getter;

	let existingPromiseReject: ((reason?: any) => void) | null;

	const runGetterAndEnsureIsPromise = () => {
		const promise = getter();
		if (promise != null && promise.then) {
			return promise
		} else {
			throw new Error("Computed must return Promise or be async function")
		}
	}

	const rejectAndGetValue = () => {
		if (existingPromiseReject) {
			existingPromiseReject();
		}

		innerObservable.inProgress(true);

		const promise = runGetterAndEnsureIsPromise();

		// Wrap the source Promise, so that we can cancel it if it's still in progress when a new value becomes needed
		new Promise<T>((resolve, reject) => {
			existingPromiseReject = reject;

			promise.then(v => resolve(v))
			promise.catch(err => reject(err))
		})
			.then(v => {
				innerObservable.inProgress(false);
				innerObservable(v);
				existingPromiseReject = null
			})
			.catch((err) => {
				existingPromiseReject = null
			})
	}

	rejectAndGetValue();
	const subscription = getter.subscribe(p => {
		return rejectAndGetValue()
	});

	innerObservable.dispose = () => {
		subscription.dispose();
		existingPromiseReject?.();
		getter.dispose();
	}

	return innerObservable;
}

const createExtender = (ko: _KoStatic) => <T>(computed: KnockoutComputed<Promise<T>>, defaultValue: T) => computedPromise(ko, computed, defaultValue)


export function registerAsyncComputed(ko: _KoStatic) {
	(ko.extenders as any).async = createExtender(ko)
}

export const asyncComputed = <T>(getPromise: () => Promise<T>, defaultValue: T) => computedPromise(ko, ko.computed(() => getPromise()), defaultValue)