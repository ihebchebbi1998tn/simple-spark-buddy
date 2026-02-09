import {namespace} from "./namespace";
import {HelperUrl} from "@Main/helper/Helper.Url";
import {HelperLookup} from "@Main/helper/Helper.Lookup";

export interface License {
	DomainId: string,
	ContractNo: string,
	InstallationLocation: string,
	InstallationType: string,
	ProjectId: string,
	Expires: Date,
	IsTrialLicense: boolean,
	Modules: Map<string, number>,
	UsedModules: Map<string, number>
}

export class SiteDetailsLicenseTabViewModel extends window.Main.ViewModels.ViewModelBase {
	public license: License;
	private activeUsers: number;
	lookups: LookupType = {
		licenseModules: {$tableName: "Main_LicenseModule"}
	};
	private sortedModuleIds: string[] = [];
	
	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.license = await fetch(HelperUrl.resolveUrl("~/Main_Site/GetLicense"), { cache: "no-cache" }).then(x => x.json());
		this.activeUsers = await (window.Helper.User.getActiveUsers(window.database.Main_User) as $data.Queryable<Main.Rest.Model.Main_User>).count();
		await HelperLookup.getLocalizedArrayMaps(this.lookups);
		this.sortModuleIds();
	}
	
	private sortModuleIds(): void {
		const sortedModules = Object.keys(this.license.Modules).sort((x, y) => x.localeCompare(y));
		while(sortedModules.length != this.sortedModuleIds.length) {
			sortedModules.forEach(moduleId => {
				if(this.sortedModuleIds.indexOf(moduleId) !== -1) {
					return;
				}
				const parentModuleId = this.getParentModuleId(moduleId);
				if (!parentModuleId) {
					this.sortedModuleIds.push(moduleId);
					return;
				}
				const parentIndex = this.sortedModuleIds.indexOf(parentModuleId);
				if(parentIndex === -1) {
					return;
				}
				const parentSubIndex = window._.findLastIndex(this.sortedModuleIds, m => this.getParentModuleId(m) === parentModuleId);
				this.sortedModuleIds.splice((parentSubIndex > parentIndex ? parentSubIndex : parentIndex)+1, 0, moduleId);
			});
		}
	}
	
	private getParentModuleId(moduleId: string): string {
		const licenseModule = this.lookups.licenseModules[moduleId];
		if(licenseModule) {
			return licenseModule.ParentModuleId;
		}
		return null;
	}
	
	private getUsedModuleCount(moduleId: string): number {
		if(moduleId == "FLD05030") {
			return this.activeUsers;
		}
		
		return this.license.UsedModules[moduleId] ?? 0;
	}
	
	private getColor(moduleId: string): string {
		const used = this.getUsedModuleCount(moduleId);
		const percentage = used / this.license.Modules[moduleId];
		if(used === 0) {
			return "text-warning";
		}
		if(percentage == 1.0) {
			return "text-danger";
		}
		if(percentage >= 0.8) {
			return "text-warning";
		}
		return "text-success";
	}
}

namespace("Main.ViewModels").SiteDetailsLicenseTabViewModel = SiteDetailsLicenseTabViewModel;