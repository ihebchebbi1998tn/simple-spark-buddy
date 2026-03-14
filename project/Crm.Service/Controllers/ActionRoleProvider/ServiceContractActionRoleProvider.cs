namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Licensing;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Model.Relationships;

	using Main;
	using Main.Model.Lookups;

	[Licensing(ModuleId = "FLD03150")]
	public class ServiceContractActionRoleProvider : RoleCollectorBase
	{
		public ServiceContractActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.ServiceContract, PermissionName.View, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice);
			Add(ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Index, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice);
			Add(ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Read, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Read, ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Index);
			Add(ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Create, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Create, ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Edit, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Edit, ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Create);
			AddImport(ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Edit, ServicePlugin.PermissionGroup.ServiceContract, MainPlugin.PermissionName.SetStatus);
			Add(ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Delete, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Delete, ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Edit);
			Add(ServicePlugin.PermissionGroup.ServiceContract, MainPlugin.PermissionName.SetStatus, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);

			Add(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.CreateTag, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.CreateTag, ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.AssociateTag);
			AddImport(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.CreateTag, ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.RemoveTag);
			Add(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.AssociateTag, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.AssociateTag, ServicePlugin.PermissionGroup.ServiceContract, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.RenameTag, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.RenameTag, ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.CreateTag);
			AddImport(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.RenameTag, ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.DeleteTag);
			Add(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.RemoveTag, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.RemoveTag, ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.AssociateTag);
			Add(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.DeleteTag, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.DeleteTag, ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.CreateTag);

			Add(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.TasksBlock, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.NotesTab, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.RelationshipsTab, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceContract, ServicePlugin.PermissionName.InstallationsTab, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceContract, ServicePlugin.PermissionName.MaintenancePlansTab, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.RemoveAddressRelationship, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceContract, ServicePlugin.PermissionName.RemoveInstallationRelationship, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceContract, ServicePlugin.PermissionName.RemoveMaintenancePlan, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceContract, CrmPlugin.PermissionName.EditAddressRelationship, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceContract, ServicePlugin.PermissionName.SaveInstallationRelationship, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceContract, ServicePlugin.PermissionName.SaveMaintenancePlan, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceContract, ServicePlugin.PermissionName.ServiceOrdersTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.ServiceContract, ServicePlugin.PermissionName.DocumentArchive, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);

			Add(PermissionGroup.WebApi, nameof(ServiceContract), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceContractStatus), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceContractType), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceContractAddressRelationship), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceContractAddressRelationshipType), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceContractInstallationRelationship), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceContractLimitType), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(MaintenancePlan), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(PaymentCondition), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(PaymentInterval), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(PaymentType), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(SparePartsBudgetInvoiceType), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(SparePartsBudgetTimeSpanUnit), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
		}
	}
}
