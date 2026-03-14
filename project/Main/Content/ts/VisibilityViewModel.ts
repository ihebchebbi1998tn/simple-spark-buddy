import {uniq} from "lodash";
import {HelperString} from "./helper/Helper.String";

export class VisibilityViewModel {
	loading = ko.observable<boolean>(true);
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	entity: any;
	errors: KnockoutValidationErrors;
	selectedUserGroups = ko.observableArray<Main.Rest.Model.ObservableMain_Usergroup>([]);
	selectedUserGroupIds = ko.observableArray<string>([]);
	selectedUserGroupToDisplay = ko.observableArray<{ id: string, text: string }>([]);
	selectedUserGroupNames: KnockoutComputed<string[]>;
	selectedUsers = ko.observableArray<Main.Rest.Model.ObservableMain_User>([]);
	selectedUsersToDisplay = ko.observableArray<{ id: string, text: string }>([]);
	selectedUserIds = ko.observableArray<string>([]);
	selectedUserDisplayNames: KnockoutComputed<string[]>;
	visibilityQuestion: string;
	UserFilterQuery: (query: $data.Queryable<Main.Rest.Model.Main_User>, term: string) => ($data.Queryable<Main.Rest.Model.Main_User>);


	constructor(entity: any, entityType: string) {
		this.entity = ko.isObservable(entity) ? entity : ko.observable(entity);
		this.errors = ko.validation.group(this.entity, {deep: true});

		this.selectedUserGroupNames = ko.computed(() => $.map(this.selectedUserGroups(), (ug) => ug.Name()));

		this.selectedUserDisplayNames = ko.computed(() => {
			return uniq($.map(this.selectedUsers(), window.Helper.User.getDisplayName));
		});
		if (entityType.indexOf("Rest") !== -1) {
			entityType = entityType.substring(0, entityType.indexOf("Rest"));
		}
		this.visibilityQuestion = HelperString.getTranslatedString("ContactVisibilityQuestion").replace("{0}", HelperString.getTranslatedString("This" + entityType));

		this.UserFilterQuery = (query: $data.Queryable<Main.Rest.Model.Main_User>, term: string) => {
			if (!term) {
				return query;
			}
			return window.Helper.String.contains(query, term, ["LastName.toLowerCase()", "FirstName.toLowerCase()"]);
		};
	}


	addValidationRules() {
		this.entity().Visibility.extend({
			validation: {
				async: true,
				validator: async (val, params, callback) => {
					if (window.AuthorizationManager.currentUserIsAuthorizedForAction("Visibility", "SkipCheck") || val === 1 || val === 2) {
						callback(true);
					} else if (val === 4) {
						let isInSelectedUserGroup = this.currentUser().UsergroupIds.filter(usergroupId => {
							return this.entity().VisibleToUsergroupIds().indexOf(usergroupId) !== -1;
						}).length > 0;
						if (!isInSelectedUserGroup) {
							let user = await window.database.Main_User.find(this.currentUser().Id);
							isInSelectedUserGroup = user.UsergroupIds.filter(usergroupId => {
								return this.entity().VisibleToUsergroupIds().indexOf(usergroupId) !== -1;
							}).length > 0;
						}
						callback(isInSelectedUserGroup);
					} else if (val === 8) {
						callback(window.AuthorizationManager.isAuthorizedForAction("Visibility", "Edit")
							|| this.entity().VisibleToUserIds().indexOf(this.currentUser().Id) !== -1);
					} else {
						throw "Not supported Visibility: " + val;
					}
				},
				message: HelperString.getTranslatedString("RuleViolation.NotVisibleToCurrentUser")
			}
		});
		this.entity().VisibleToUsergroupIds.subscribe(() => {
			this.entity().Visibility.valueHasMutated();
		});
		this.entity().VisibleToUserIds.subscribe(() => {
			this.entity().Visibility.valueHasMutated();
		});
	}

	async init(entity?: any): Promise<void> {
		if (entity) {
			this.entity(ko.unwrap(entity));
		}
		this.entity().Visibility(this.entity().Visibility() || 2);
		this.entity().VisibleToUsergroupIds(this.entity().VisibleToUsergroupIds() || []);
		this.entity().VisibleToUserIds(this.entity().VisibleToUserIds() || []);
		this.entity().Visibility.subscribe(visibility => {
			if (visibility !== 4) {
				this.selectedUserGroups([]);
			}
			if (visibility !== 8) {
				this.selectedUsers([]);
			}
		});
		this.selectedUserGroupIds.subscribe(ids => {
			this.entity().VisibleToUsergroupIds(ids);
		});
		this.selectedUserIds.subscribe(ids => {
			this.entity().VisibleToUserIds(ids);
		});
		const currentUser = await window.Helper.User.getCurrentUser()
		this.currentUser(currentUser);

		if (this.entity().VisibleToUserIds().length > 0) {
			this.selectedUsers(
				(await window.database.Main_User
					.filter(function (x) {
						return x.Id in this.ids;
					}, {ids: this.entity().VisibleToUserIds()})
					.toArray()).map(x => x.asKoObservable())
			);
		}

		if (this.entity().VisibleToUsergroupIds().length > 0) {
			this.selectedUserGroups((await window.database.Main_Usergroup
				.filter(function (x) {
					return x.Id in this.ids;
				}, {ids: this.entity().VisibleToUsergroupIds()})
				.toArray())
				.map(x => x.asKoObservable())
			);
		}

		this.selectedUserIds($.map(this.selectedUsers(), u => u.Id()));
		this.selectedUsersToDisplay($.map(this.selectedUsers(), u => {
				return {
					id: u.Id(),
					text: window.Helper.User.getDisplayName(u)
				}
			}
		));
		this.selectedUserGroupIds($.map(this.selectedUserGroups(), u => u.Id()));
		this.selectedUserGroupToDisplay($.map(this.selectedUserGroups(), ug => {
			return {
				id: ug.Id(),
				text: ug.Name()
			};
		}));
		this.addValidationRules();
		this.loading(false);
	}
}

// @ts-ignore
window.VisibilityViewModel = VisibilityViewModel;