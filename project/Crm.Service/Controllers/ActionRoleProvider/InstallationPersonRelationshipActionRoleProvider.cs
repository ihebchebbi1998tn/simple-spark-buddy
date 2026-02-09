namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Model.Relationships;

	public class InstallationPersonRelationshipActionRoleProvider : RoleCollectorBase
	{
		public InstallationPersonRelationshipActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var roles = new[] {
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService
			};
			
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.ReadInstallationPersonRelationship, roles);
			AddImport(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.ReadInstallationPersonRelationship, ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.RelationshipsTab);
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.CreateInstallationPersonRelationship, roles);
			AddImport(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.CreateInstallationPersonRelationship, ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.ReadInstallationPersonRelationship);
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.EditInstallationPersonRelationship, roles);
			AddImport(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.EditInstallationPersonRelationship, ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.CreateInstallationPersonRelationship);
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.RemoveInstallationPersonRelationship, roles);
			AddImport(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.RemoveInstallationPersonRelationship, ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.EditInstallationPersonRelationship);

			Add(PermissionGroup.WebApi, nameof(InstallationPersonRelationship), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(InstallationPersonRelationshipType), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, Roles.APIUser);
		}
	}
}
