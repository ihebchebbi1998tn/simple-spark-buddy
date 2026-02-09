namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Model.Relationships;

	public class InstallationCompanyRelationshipActionRoleProvider : RoleCollectorBase
	{
		public InstallationCompanyRelationshipActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var roles = new[] {
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService
			};
			
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.ReadInstallationCompanyRelationship, roles);
			AddImport(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.ReadInstallationCompanyRelationship, ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.RelationshipsTab);
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.CreateInstallationCompanyRelationship, roles);
			AddImport(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.CreateInstallationCompanyRelationship, ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.ReadInstallationCompanyRelationship);
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.EditInstallationCompanyRelationship, roles);
			AddImport(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.EditInstallationCompanyRelationship, ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.CreateInstallationCompanyRelationship);
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.RemoveInstallationCompanyRelationship, roles);
			AddImport(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.RemoveInstallationCompanyRelationship, ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.EditInstallationCompanyRelationship);

			Add(PermissionGroup.WebApi, nameof(InstallationCompanyRelationship), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(InstallationCompanyRelationshipType), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, Roles.APIUser);
		}
	}
}
