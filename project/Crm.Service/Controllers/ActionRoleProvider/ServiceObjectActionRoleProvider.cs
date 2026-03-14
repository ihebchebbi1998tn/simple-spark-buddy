namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Licensing;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main;

	[Licensing(ModuleId = "FLD03160")]
	public class ServiceObjectActionRoleProvider : RoleCollectorBase
	{
		public ServiceObjectActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.ServiceObject, PermissionName.View, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice);
			Add(ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Index, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice);
			Add(ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Index);
			Add(ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Create, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Edit, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Edit, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Create);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Edit, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Delete, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Delete, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Edit);
			Add(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.CreateAddress, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.CreateAddress, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.CreateAddress, ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.EditAddress);
			Add(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.EditAddress, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.EditAddress, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.DeleteAddress, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.DeleteAddress, ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.EditAddress);

			Add(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.CreateTag, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.CreateTag, ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.AssociateTag);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.CreateTag, ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.RemoveTag);
			Add(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.AssociateTag, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.AssociateTag, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.RenameTag, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.RenameTag, ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.CreateTag);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.RenameTag, ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.DeleteTag);
			Add(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.RemoveTag, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.RemoveTag, ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.AssociateTag);
			Add(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.DeleteTag, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.DeleteTag, ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.CreateTag);

			Add(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.NotesTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.NotesTab, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.StaffTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, CrmPlugin.PermissionName.StaffTab, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.ServiceObject, ServicePlugin.PermissionName.InstallationsTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, ServicePlugin.PermissionName.InstallationsTab, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.ServiceObject, ServicePlugin.PermissionName.ServiceOrdersTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, ServicePlugin.PermissionName.ServiceOrdersTab, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.ServiceObject, ServicePlugin.PermissionName.ServiceContractsTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, ServicePlugin.PermissionName.ServiceContractsTab, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.ServiceObject, ServicePlugin.PermissionName.ServiceCasesTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceObject, ServicePlugin.PermissionName.ServiceCasesTab, ServicePlugin.PermissionGroup.ServiceObject, PermissionName.Read);

			Add(PermissionGroup.WebApi, nameof(ServiceObject), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceObjectCategory), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
		}
	}
}
