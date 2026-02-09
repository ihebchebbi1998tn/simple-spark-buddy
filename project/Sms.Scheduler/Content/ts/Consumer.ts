//based on https://stackoverflow.com/a/56784618
interface Factory<TEvent> { (TEvent): Promise<void>; }

export class Consumer<T> {
	queue: T[] = [];
	isConsuming = false;
	consume: Factory<T>;

	constructor(consume: Factory<T>) {
		this.consume = consume;
	}

	enqueue(e: T) {
		this.queue.push(e);
	}

	notify() {
		if (this.isConsuming) {
			return;
		}
		this.consumeNext();
	}

	async consumeNext() {
		this.isConsuming = true;
		if (this.queue.length > 0) {
			//consume one item
			await this.consume(this.queue.shift());

			//consume next item
			this.consumeNext();
		}
		else {
			this.isConsuming = false;
		}
	}
}