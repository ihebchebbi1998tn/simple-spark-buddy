namespace Sms.Scheduler
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Menu;

	using PermissionGroup = SchedulerPlugin.PermissionGroup;

	public class SchedulerMenuRegistrar : IMenuRegistrar<MaterialUserProfileMenu>
	{
		public virtual void Initialize(MenuProvider<MaterialUserProfileMenu> menuProvider)
		{
			menuProvider.Register(null, "WebSchedulerOnline", url: "~/Sms.Scheduler/Scheduler/DetailsTemplate", iconClass: "zmdi zmdi-desktop-windows", priority: Int32.MaxValue - 20, htmlAttributes: new Dictionary<string, object> { { "data-bind", "if: Helper.Offline?.status !== 'offline'" } });
			menuProvider.AddPermission(null, "WebSchedulerOnline", PermissionGroup.Scheduler, PermissionName.View);
			menuProvider.Register(null, "WebSchedulerOffline", url: "~/Main/Home/Startup/online?redirectUrl=/Sms.Scheduler/Scheduler/DetailsTemplate", iconClass: "zmdi zmdi-desktop-windows", priority: Int32.MaxValue - 20, htmlAttributes: new Dictionary<string, object> { { "data-bind", "if: Helper.Offline?.status === 'offline'" } });
			menuProvider.AddPermission(null, "WebSchedulerOffline", PermissionGroup.Scheduler, PermissionName.View);
		}
	}
}
