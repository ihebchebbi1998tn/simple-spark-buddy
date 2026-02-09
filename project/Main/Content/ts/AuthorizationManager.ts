import {HelperUrl} from "./helper/Helper.Url";

export class AuthorizationManager {
	users: { [key: string]: { [key: string]: Main.Rest.Model.PermissionRest } } = {};
	currentUserName: string = null;

	currentUserHasPermission(permissionName: string): boolean {
		const currentUserName = this.currentUserName ?? window.Helper.User.getCurrentUserName();
		return this.userHasPermission(currentUserName, permissionName);
	}

	currentUserIsAuthorizedForAction(permissionGroup: string, permissionName: string): boolean {
		return this.currentUserHasPermission(permissionGroup + "::" + permissionName);
	}

	userIsAuthorizedForAction(username: string, permissionGroup: string, permissionName: string): boolean {
		return this.userHasPermission(username, permissionGroup + "::" + permissionName);
	}

	userHasPermission(username: string, permissionName: string): boolean {
		if (!this.users[username]) {
			throw new Error("AuthorizationManager is not initialized");
		}
		return !!this.users[username] && !!this.users[username][permissionName];
	}

	async initPromise(username?: string): Promise<AuthorizationManager> {
		this.currentUserName = username || window.Helper.User.getCurrentUserName();
		if (this.users.hasOwnProperty(this.currentUserName)) {
			return new Promise(resolve => resolve(this));
		}
		const response = await fetch(HelperUrl.resolveUrl(`~/Main/Authorization/${this.currentUserName}.json`))
		const results = await response.json();
		results.forEach((permission) => {
			if (permission !== null) {
				(this.users[this.currentUserName] ||= {})[permission.Name] = permission;
			}
		});
		return this;
	}

	async init(username?: string): Promise<void> {
		username = username || window.Helper.User.getCurrentUserName();
		if (this.users.hasOwnProperty(username)) {
			return;
		}
		const response = await fetch(HelperUrl.resolveUrl("~/Main/Authorization/" + username + ".json"));
		const results = await response.json();
		this.users[username] = {};
		results.forEach((permission) => {
			this.users[username][permission.Name] = permission;
		});
	}

	hasPermission(...params: string[]): boolean {
		if (params.length === 1) {
			return this.currentUserHasPermission(params[0]);
		}
		if (params.length === 2) {
			return this.userHasPermission(params[0], params[1]);
		}
		throw new Error("Invalid number of arguments");
	}

	isAuthorizedForAction(...params: string[]): boolean {
		if (params.length === 2) {
			return this.currentUserIsAuthorizedForAction(params[0], params[1]);
		}
		if (params.length === 3) {
			return this.userIsAuthorizedForAction(params[0], params[1], params[2]);
		}
		throw new Error("Invalid number of arguments");
	}
}

const authManager = new AuthorizationManager();
export {authManager};

window.AuthorizationManager = authManager;