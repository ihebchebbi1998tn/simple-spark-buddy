namespace Crm.Service
{
	using Crm.Library.Modularization.Menu;
	using Library.Model.Authorization.PermissionIntegration;
	using System;
	using System.Runtime.CompilerServices;
	using PermissionGroup = ServicePlugin.PermissionGroup;

	public class ServiceMenuRegistrar : IMenuRegistrar<MaterialMainMenu>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void Initialize(MenuProvider<MaterialMainMenu> menuProvider)
		{
			menuProvider.Register(null, "AdHocServiceOrder", url: "~/Crm.Service/Dispatch/AdHocTemplate", iconClass: "zmdi zmdi-plus-circle", priority: 1500);
			menuProvider.AddPermission(null, "AdHocServiceOrder", PermissionGroup.Adhoc, PermissionName.Create);

			menuProvider.Register(null, "Dispatches", iconClass: "zmdi zmdi-layers", priority: 1200);
			menuProvider.Register("Dispatches", "UpcomingDispatches", priority: 300, url: "~/Crm.Service/ServiceOrderDispatchList/IndexTemplate?status=upcoming");
			menuProvider.AddPermission("Dispatches", "UpcomingDispatches", PermissionGroup.UpcomingDispatches, PermissionName.View);
			menuProvider.Register("Dispatches", "ScheduledDispatches", priority: 200, url: "~/Crm.Service/ServiceOrderDispatchList/IndexTemplate?status=scheduled");
			menuProvider.AddPermission("Dispatches", "ScheduledDispatches", PermissionGroup.ScheduledDispatches, PermissionName.View);
			menuProvider.Register("Dispatches", "ClosedDispatches", priority: 100, url: "~/Crm.Service/ServiceOrderDispatchList/IndexTemplate?status=closed");
			menuProvider.AddPermission("Dispatches", "ClosedDispatches", PermissionGroup.ClosedDispatches, PermissionName.View);

			menuProvider.Register(null, "ServiceProcessing", iconClass: "zmdi zmdi-wrench", priority: 1100);
			menuProvider.Register("ServiceProcessing", "ServiceCases", url: "~/Crm.Service/ServiceCaseList/IndexTemplate", priority: 200);
			menuProvider.AddPermission("ServiceProcessing", "ServiceCases", PermissionGroup.ServiceCase, PermissionName.View);
			menuProvider.Register("ServiceProcessing", "ServiceOrders", priority: 100, url: "~/Crm.Service/ServiceOrderHeadList/IndexTemplate");
			menuProvider.AddPermission("ServiceProcessing", "ServiceOrders", PermissionGroup.ServiceOrder, PermissionName.View);
			menuProvider.AddPermission("ServiceProcessing", "ServiceOrders", PermissionGroup.Dispatch, PermissionName.Create);

			menuProvider.Register(null, "ServiceData", iconClass: "zmdi zmdi-wrench", priority: 1000);
			menuProvider.Register("ServiceData", "ServiceCaseTemplates", url: "~/Crm.Service/ServiceCaseTemplateList/IndexTemplate", priority: 500);
			menuProvider.AddPermission("ServiceData", "ServiceCaseTemplates", PermissionGroup.ServiceCaseTemplate, PermissionName.View);
			menuProvider.Register("ServiceData", "ServiceOrderTemplates", priority: 400, url: "~/Crm.Service/ServiceOrderTemplateList/IndexTemplate");
			menuProvider.AddPermission("ServiceData", "ServiceOrderTemplates", PermissionGroup.ServiceOrderTemplate, PermissionName.View);
			menuProvider.Register("ServiceData", "Installations", url: "~/Crm.Service/InstallationList/IndexTemplate", priority: 300);
			menuProvider.AddPermission("ServiceData", "Installations", PermissionGroup.Installation, PermissionName.View);
			menuProvider.Register("ServiceData", "ServiceObjects", url: "~/Crm.Service/ServiceObjectList/IndexTemplate", priority: 200);
			menuProvider.AddPermission("ServiceData", "ServiceObjects", PermissionGroup.ServiceObject, PermissionName.View);
			menuProvider.Register("ServiceData", "ServiceContracts", url: "~/Crm.Service/ServiceContractList/IndexTemplate", priority: 100);
			menuProvider.AddPermission("ServiceData", "ServiceContracts", PermissionGroup.ServiceContract, PermissionName.View);

			menuProvider.Register(null, "Replenishment", url: "~/Crm.Service/ReplenishmentOrderItemList/IndexTemplate", iconClass: "zmdi zmdi-shopping-cart-plus", priority: 900);
			menuProvider.AddPermission(null, "Replenishment", PermissionGroup.ReplenishmentOrder, PermissionName.View);
		}
	}
}
