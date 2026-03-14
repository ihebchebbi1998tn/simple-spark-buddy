namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Model;
	using Crm.Service;

	using Main;

	public class CompanyActionRoleProvider : RoleCollectorBase
	{
		public CompanyActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(CrmPlugin.PermissionGroup.Company, ServicePlugin.PermissionName.InstallationsTab, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, ServicePlugin.PermissionName.ServiceContractsTab, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, ServicePlugin.PermissionName.ServiceOrdersTab, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, PermissionName.View, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, PermissionName.Index, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, PermissionName.Read, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, PermissionName.Create, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, PermissionName.Delete, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, PermissionName.Edit, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);

			Add(CrmPlugin.PermissionGroup.Company, MainPlugin.PermissionName.ToggleActive, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, MainPlugin.PermissionName.Merge, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.MakeStandardAddress, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);

			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.CreateTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.AssociateTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.RenameTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.RemoveTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Company, CrmPlugin.PermissionName.DeleteTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);

			Add(PermissionGroup.MaterialDashboard, nameof(Company), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(MainPlugin.PermissionGroup.TopSearch, nameof(Company), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
		}
	}
}
