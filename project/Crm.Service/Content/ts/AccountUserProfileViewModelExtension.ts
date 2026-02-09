export class AccountUserProfileViewModelExtension extends window.Main.ViewModels.AccountUserProfileViewModel {
	selectedStoreId: KnockoutObservable<string>;
	selectedStorageAreaId: KnockoutObservable<string>;
	selectedLocationId: KnockoutObservable<string>;

	constructor() {
		super();
		this.selectedStoreId = ko.observable<string>()
		this.selectedStorageAreaId = ko.observable<string>()
		this.selectedLocationId = ko.observable<string>()
	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params)
		const userExtensionValues = this.user().ExtensionValues()
		if (userExtensionValues.DefaultStoreNo()) {
			const store = await window.database.CrmArticle_Store.filter(function (store) { return store.StoreNo == this.defaultStoreNo },
				{ defaultStoreNo: userExtensionValues.DefaultStoreNo() }).first()
			this.selectedStoreId(store.Id)
		}
		if (userExtensionValues.DefaultStorageAreaNo()) {
			const storageArea = await window.database.CrmArticle_StorageArea.filter(function (storageArea) { return storageArea.StorageAreaNo == this.defaultStorageAreaNo },
				{ defaultStorageAreaNo: userExtensionValues.DefaultStorageAreaNo() }).first()
			this.selectedStorageAreaId(storageArea.Id)
		}
		if (userExtensionValues.DefaultLocationNo()) {
			const location = await window.database.CrmArticle_Location.filter(function (location) { return location.LocationNo == this.defaultLocationNo },
				{ defaultLocationNo: userExtensionValues.DefaultLocationNo() }).first()
			this.selectedLocationId(location.Id)
		}
	}

	locationFilter(query, term: string, storeId?: string, storageAreaId?: string) {
		if (storeId) {
			query = query.filter('it.StoreId == this.storeId', {storeId: storeId});
		}
		if (storageAreaId) {
			query = query.filter('it.StorageAreaId == this.storageAreaId', {storageAreaId: storageAreaId});
		}
		if (term) {
			query = window.Helper.String.contains(query, term, ["LocationNo"]);
		}
		return query;
	};

	storageAreaFilter(query, term: string, storeId?: string) {
		if (storeId) {
			query = query.filter('it.StoreId == this.storeId', {storeId: storeId});
		}
		if (term) {
			query = window.Helper.String.contains(query, term, ["StorageAreaNo"]);
		}
		return query;
	};

	onStoreSelect = async function (userExtensionValues: Default.ObservableMain_User_ExtensionValues, store: Crm.Article.Rest.Model.CrmArticle_Store) {
		this.loading(true);
		if (store != null && store.StoreNo != userExtensionValues.DefaultStoreNo()) {
			const storeLocations = await window.database.CrmArticle_Location
				.filter("it.StoreId === this.storeId", { storeId: store.Id })
				.toArray();
			if (store.StorageAreas.map(x => x.StorageAreaNo).indexOf(userExtensionValues.DefaultStorageAreaNo()) === -1) {
				if (store.StorageAreas.length != 1) {
					userExtensionValues.DefaultStorageAreaNo(null);
					this.selectedStorageAreaId(null)
					if (storeLocations.length == 0 || storeLocations.map(x => x.LocationNo).indexOf(userExtensionValues.DefaultLocationNo()) === -1) {
						userExtensionValues.DefaultLocationNo(null);
						this.selectedLocationId(null)
					} else if (storeLocations.length == 1) {
						userExtensionValues.DefaultLocationNo(storeLocations[0].LocationNo);
						this.selectedLocationId(store.StorageAreas[0].Locations[0].Id)
					}
				}
				else {
					userExtensionValues.DefaultStorageAreaNo(store.StorageAreas[0].StorageAreaNo);
					this.selectedStorageAreaId(store.StorageAreas[0].Id)
					if (store.StorageAreas[0].Locations.length == 1) {
						userExtensionValues.DefaultLocationNo(store.StorageAreas[0].Locations[0].LocationNo);
						this.selectedLocationId(store.StorageAreas[0].Locations[0].Id)
					} else {
						userExtensionValues.DefaultLocationNo(null);
						this.selectedLocationId(null)
					}
				}
			}
		} else if (store == null) {
			userExtensionValues.DefaultStorageAreaNo(null);
			this.selectedStorageAreaId(null)
			userExtensionValues.DefaultLocationNo(null);
			this.selectedLocationId(null)
		}
		userExtensionValues.DefaultStoreNo(store == null ? null : store.StoreNo);
		this.loading(false);
	};

	onStorageAreaSelect = async function (userExtensionValues: Default.ObservableMain_User_ExtensionValues, storageArea: Crm.Article.Rest.Model.CrmArticle_StorageArea) {
		this.loading(true);
		if (storageArea != null && storageArea.StorageAreaNo != userExtensionValues.DefaultStorageAreaNo()) {
			if (storageArea.Locations.map(x => x.LocationNo).indexOf(userExtensionValues.DefaultLocationNo()) === -1) {
				if (storageArea.Locations.length == 1) {
					userExtensionValues.DefaultLocationNo(storageArea.Locations[0].LocationNo);
					this.selectedLocationId(storageArea.Locations[0].Id)
				} else {
					userExtensionValues.DefaultLocationNo(null);
					this.selectedLocationId(null)
				}
			}
		} else if (storageArea == null) {
			userExtensionValues.DefaultLocationNo(null);
			this.selectedLocationId(null)
		}
		userExtensionValues.DefaultStorageAreaNo(storageArea == null ? null : storageArea.StorageAreaNo);
		this.loading(false);
	};

	onLocationSelect = async function (userExtensionValues: Default.ObservableMain_User_ExtensionValues, location: Crm.Article.Rest.Model.CrmArticle_Location) {
		this.loading(true);
		if (location?.StorageArea != null) {
			userExtensionValues.DefaultStorageAreaNo(location.StorageArea.StorageAreaNo);
			this.selectedStorageAreaId(location.StorageArea.Id)
		}
		userExtensionValues.DefaultLocationNo(location?.LocationNo);
		this.loading(false);
	};
}
window.Main.ViewModels.AccountUserProfileViewModel = AccountUserProfileViewModelExtension;