import {namespace} from "./namespace";

export interface ThirdPartySoftware {
	name: string,
	version: string,
	url: string,
	author: string,
	licenseType: string,
	licenseUrl: string
}
export class SiteThirdPartySoftwareViewModel extends window.Main.ViewModels.ViewModelBase {
	items: ThirdPartySoftware[];
	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let nugetLicenses = await (await fetch(window.Helper.Url.resolveUrl("~/static-dist/licenses-nuget.json"))).json();
		let npmLicenses = await (await fetch(window.Helper.Url.resolveUrl("~/static-dist/licenses-npm.json"))).json();
		this.items = window._.sortedUniqBy(npmLicenses.map(npmLicense => {
			let url = npmLicense.link.replace(/^git\+(.*)\.git$/, "$1").replace(/^git:\/\/(.*)\.git$/, "https://$1");
			return {
				name: npmLicense.name,
				version: npmLicense.installedVersion,
				url: url,
				author: npmLicense.author,
				licenseType: npmLicense.licenseType,
				licenseUrl: url
			}
		}).concat(nugetLicenses.map(nugetLicense => {

			return {
				name: nugetLicense.PackageName,
				version: nugetLicense.PackageVersion,
				url: nugetLicense.PackageUrl,
				author: nugetLicense.Authors.join(", "),
				licenseType: nugetLicense.LicenseType || nugetLicense.LicenseUrl || "License",
				licenseUrl: nugetLicense.LicenseUrl || nugetLicense.PackageUrl
			}
		})), x => x.name);
		
		this.loading(false);
	}
}

namespace("Main.ViewModels").SiteThirdPartySoftwareViewModel = SiteThirdPartySoftwareViewModel;