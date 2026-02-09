namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using Main;

	public class CompanyDetailsActionRoleProvider : RoleCollectorBase
	{
		public CompanyDetailsActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.UpgradeProspect, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			AddImport(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.UpgradeProspect, CrmPlugin.PermissionGroup.Company, PermissionName.Edit);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.EditAddress, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			AddImport(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.EditAddress, CrmPlugin.PermissionGroup.Company, PermissionName.Read);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.DeleteAddress, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			AddImport(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.DeleteAddress, CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.EditAddress);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.DeleteCommunication, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.CreateBusinessRelationship, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.DeleteBusinessRelationship, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);

			Add(CrmPlugin.PermissionGroup.Company, MainPlugin.PermissionName.SidebarBackgroundInfo, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.SidebarBravo, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.SidebarClientCompanies, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.SidebarContactInfo, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.SidebarStaffList, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);

			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.RelationshipsTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.NotesTab, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.StaffTab, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.TasksBlock, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
		}
	}
}
