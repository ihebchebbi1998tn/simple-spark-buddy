namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;

	public class ErrorCauseTypeRelationshipActionRoleProvider : RoleCollectorBase
	{
		public ErrorCauseTypeRelationshipActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var roles = new[]
			{
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService,
				CrmPlugin.Roles.HeadOfSales,
				CrmPlugin.Roles.InternalSales,
				CrmPlugin.Roles.SalesBackOffice,
				Roles.APIUser
			};
			Add(PermissionGroup.WebApi, nameof(ErrorCauseTypeRelationship), roles);
			Add(PermissionGroup.Sync, nameof(ErrorCauseTypeRelationship), roles);
			AddImport(PermissionGroup.Sync, nameof(ErrorCauseTypeRelationship), PermissionGroup.WebApi, nameof(ErrorCauseTypeRelationship));
		}
	}
}
