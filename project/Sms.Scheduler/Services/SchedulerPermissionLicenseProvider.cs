namespace Sms.Scheduler.Services
{
	using System.Collections.Generic;
	using Crm.Library.Licensing;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;

	public class SchedulerPermissionLicenseProvider : IPermissionLicenseProvider
	{
		public virtual PermissionLicense GetPermissionLicense()
		{
			var licensedPermissions = new List<string>();
			licensedPermissions.Add($"{SchedulerPlugin.PermissionGroup.Scheduler}::{SchedulerPlugin.PermissionName.SchedulerClient}");
			licensedPermissions.Add($"{PermissionGroup.Login}::{SchedulerPlugin.PermissionName.SchedulerClient}");
			
			return new PermissionLicense()
			{
				ModuleId = "FLD01010",
				LicensedPermissions = licensedPermissions
			};
		}
	}
}
