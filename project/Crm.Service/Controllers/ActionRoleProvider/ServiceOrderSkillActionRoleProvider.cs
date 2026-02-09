namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using Main.Model.Lookups;

	public class ServiceOrderSkillActionRoleProvider : RoleCollectorBase
	{
		public ServiceOrderSkillActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.AddSkill,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.AddSkill, ServicePlugin.PermissionGroup.ServiceOrder, PermissionName.Read);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.RemoveSkill,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.RemoveSkill, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.AddSkill);

			Add(PermissionGroup.WebApi, nameof(Skill),
				ServicePlugin.Roles.FieldService,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService);
		}
	}
}