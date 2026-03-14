interface Factory<TResult> { (): TResult; }

export class Lazy<T> {
	private _value: T = undefined;
	private _isValueCreated: boolean = false;
	private _factory: Factory<T>;

	constructor(valueFactory: Factory<T>) {
		this._factory = valueFactory;
	}

	public get value() {
		if (!this._isValueCreated) {
			this._value = this._factory();
			this._isValueCreated = true;
		}
		return this._value;
	}

	public get isValueCreated() {
		return this._isValueCreated;
	}

	public toString(): string {
		return this._isValueCreated ? this.value?.toString() : Lazy.Lazy_ToString_ValueNotCreated;
	}

	public static readonly Lazy_ToString_ValueNotCreated = "Lazy_ToString_ValueNotCreated";
}

export class LazyOfTTMetadata<T, TMetadata> extends Lazy<T>{
	private readonly _metadata: TMetadata;

	constructor(valueFactory: Factory<T>, metadata: TMetadata) {
		super(valueFactory);
		this._metadata = metadata;
	}

	public get Metadata(): TMetadata {
		return this._metadata;
	}
}