namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using Main;

	public class BusinessRelationshipActionRoleProvider : RoleCollectorBase
	{
		public BusinessRelationshipActionRoleProvider(IPluginProvider pluginProvider) :
			base(pluginProvider)
		{
			Add(CrmPlugin.PermissionGroup.BusinessRelationship, PermissionName.Edit, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
		}
	}
}
