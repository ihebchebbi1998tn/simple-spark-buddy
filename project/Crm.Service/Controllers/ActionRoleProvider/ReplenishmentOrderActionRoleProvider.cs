namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Licensing;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service;
	using Crm.Service.Model;

	using Main;

	[Licensing(ModuleId= "FLD03190")]
	public class ReplenishmentOrderActionRoleProvider : RoleCollectorBase
	{
		public ReplenishmentOrderActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.ReplenishmentOrder, PermissionName.View, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.ReplenishmentOrder, PermissionName.Index, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.ReplenishmentOrder, PermissionName.Read, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ReplenishmentOrder, PermissionName.Read, ServicePlugin.PermissionGroup.ReplenishmentOrder, PermissionName.Index);
			Add(ServicePlugin.PermissionGroup.ReplenishmentOrder, MainPlugin.PermissionName.Close, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ReplenishmentOrder, MainPlugin.PermissionName.Close, ServicePlugin.PermissionGroup.ReplenishmentOrder, PermissionName.Read);

			Add(ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.CreateItem, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.EditItem, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.EditItem, ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.CreateItem);
			Add(ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.DeleteItem, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.DeleteItem, ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.EditItem);
			Add(ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.ReplenishmentsFromOtherUsersSelectable, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.ReplenishmentsFromOtherUsersSelectable, ServicePlugin.PermissionGroup.ReplenishmentOrder, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.SeeClosedReplenishmentOrders, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.SendReplenishmentOrder, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ReplenishmentOrder, ServicePlugin.PermissionName.SendReplenishmentOrder, ServicePlugin.PermissionGroup.ReplenishmentOrder, PermissionName.Read);

			Add(PermissionGroup.WebApi, nameof(ReplenishmentOrder), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ReplenishmentOrderItem), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, Roles.APIUser);
		}
	}
}
