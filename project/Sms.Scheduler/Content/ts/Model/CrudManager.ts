//@ts-nocheck
import {CrudManager, CrudManagerConfig} from "@bryntum/schedulerpro";
import {StoreIds} from "./DispatchStore";

export class CrmCrudManager extends CrudManager {
	context: any = null;
	attachToStore: WeakSet<any>;
	detachFromStore: WeakSet<any>;
	callbackOnError: Function<Promise<void>>;

	constructor(config: Partial<CrudManagerConfig> & {
		context: any
	}, attachToStore: WeakSet<any>, detachFromStore: WeakSet<any>, callbackOnError: Function<Promise<void>>) {
		super(config);
		this.context = config.context;
		this.on({
			thisObj: this,
			beforeSync: this.beforeSync
		});

		this.attachToStore = attachToStore;
		this.detachFromStore = detachFromStore;
		this.callbackOnError = callbackOnError;
	}

	static get $name() {
		return 'CrmCrudManager';
	}

	static get $type() {
		return 'CrmCrudManager';
	}

	static get defaultConfig(): Partial<CrudManagerConfig> {
		return {
			autoSync: true,
			autoLoad: false,
			transport: {
				sync: {
					url: '~/api/$batch'
				}
			}
		}
	}

	beforeSync({source, type, eventName}): boolean {
		return true;
	}

	prepareData(storeId: string) {
		let self = this;
		let store = self.getCrudStore(storeId);
		store.added?.forEach((transientItem) => {
			if (this.attachToStore.delete(transientItem)) {
				return;
			}

			if (transientItem.OriginalData instanceof $data.Entity) {
				window.database.add(transientItem.OriginalData);
			}
		});
		store.modified?.forEach((transientItem) => {
			//Ignore attach if the only changes are parentIndex and orderedParentIndex
			if (transientItem?.meta?.modified && Object.keys(transientItem.meta.modified).every(k => ['parentIndex', 'orderedParentIndex'].includes(k))) {
				return;
			}

			if (transientItem.OriginalData instanceof $data.Entity) {
				window.database.attachOrGet(transientItem.OriginalData);
			}
		});
		store.removed?.forEach((transientItem) => {
			if (this.detachFromStore.delete(transientItem)) {
				return;
			}

			if (transientItem.OriginalData instanceof $data.Entity) {
				window.database.remove(transientItem.OriginalData);
			}
		});
	}

	sync() {
		let self = this;
		if (!self.crudStoreHasChanges()) {
			return new Promise((resolve, reject) => {
				self.trigger("syncCanceled");
				reject(new Error("Sync cancelled"));
			});
		}
		self.clearTimeout("autoSync");
		if (self.activeRequests.sync) {
			self.trigger("syncDelayed");
			self.activeSyncPromise = self.activeSyncPromise.finally(() => self.sync());
			return self.activeSyncPromise;
		}
		self.activeSyncPromise = new Promise((resolve, reject) => {
			self.trigger("syncStart");
			self.activeRequests.sync = true;
			if (self.crudStoreHasChanges(StoreIds.DispatchStore)) {
				self.prepareData(StoreIds.DispatchStore);
			}
			if (self.crudStoreHasChanges(StoreIds.AssignmentStore)) {
				self.prepareData(StoreIds.AssignmentStore);
			}
			if (self.trigger("beforeSync") !== false) {
				self.context.parentViewModel.syncWasOff(false);
				return window.database.saveChanges().then((data) => {
					resolve(data);
				}).catch((error) => {
					const popupItems = [{
						type: 'label',
						cls: 'scheduler-error',
						text: window.Helper.String.getTranslatedString("ErrorOccuredWhileSaving"),
						style: 'width: 100%',
					}];
					if(error.message != null) {
						popupItems.push({
							type: 'label',
							cls: 'scheduler-error',
							text: error.message,
							style: 'width: 100%',
						})
					}
					window.Helper.Scheduler.ShowPopup(popupItems, () => {
						window.Helper.Database.clearTrackedEntities();
						if(error.message === window.Helper.String.getTranslatedString("HttpPreconditionFailedError")) {
							self.callbackOnError();
						}
					});
					reject(error as Error);
				});
			} else {
				self.trigger("syncCanceled");
				reject(new Error("Sync cancelled"));
			}
		}).finally(() => {
			if (self.crudStoreHasChanges()) {
				self.acceptChanges();
				self.activeRequests['sync'] = null;
			}
			self.trigger("save");
		}).catch((error) => {
			if (error && !error.cancelled) {
				window.Log.error(error);
			}
			throw new Error("An error occured while trying to save changes", { cause: error });
		});
		return self.activeSyncPromise;
	}
}

CrmCrudManager.initClass();