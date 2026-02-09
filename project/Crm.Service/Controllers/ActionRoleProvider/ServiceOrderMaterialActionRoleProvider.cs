namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main;

	public class ServiceOrderMaterialActionRoleProvider : RoleCollectorBase
	{
		public ServiceOrderMaterialActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.ServiceOrderMaterial,
				PermissionName.Index,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.CreateCost,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.CreateCost, ServicePlugin.PermissionGroup.ServiceOrder, PermissionName.Read);
			
			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.EditCost,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditCost, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.CreateCost);
			
			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.DeleteCost,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.DeleteCost, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditCost);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.DeleteCost, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.CreateCost);
			
			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.CreateMaterial,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.CreateMaterial, ServicePlugin.PermissionGroup.ServiceOrder, PermissionName.Read);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.EditMaterial,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditMaterial, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.CreateMaterial);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.EditMaterialPrePlannedJob,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditMaterialPrePlannedJob, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditMaterial);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.EditMaterialPrePlannedPrice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditMaterialPrePlannedPrice, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditMaterial);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.EditMaterialUsedPrice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.InternalService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditMaterialUsedPrice, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditMaterial);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.DeleteMaterial,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.DeleteMaterial, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditMaterial);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.DeleteMaterial, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.CreateMaterial);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.MaterialSerialsEdit,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.MaterialSerialsEdit, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditMaterial);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.MaterialSerialSave,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService);

			Add(ServicePlugin.PermissionGroup.ServiceOrder,
				ServicePlugin.PermissionName.MaterialSerialRemove,
						ServicePlugin.Roles.HeadOfService,
						ServicePlugin.Roles.ServiceBackOffice,
						ServicePlugin.Roles.InternalService,
						ServicePlugin.Roles.FieldService,
						CrmPlugin.Roles.HeadOfSales,
						CrmPlugin.Roles.SalesBackOffice);
			AddImport(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.MaterialSerialRemove, ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditMaterial);

			Add(PermissionGroup.WebApi, nameof(ServiceOrderMaterial), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(CommissioningStatus), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(NoPreviousSerialNoReason), ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, CrmPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ServiceOrderMaterial), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(CommissioningStatus), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(NoPreviousSerialNoReason), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.ServiceBackOffice);
			Add(ServicePlugin.PermissionGroup.ServiceOrder, ServicePlugin.PermissionName.EditMaterialWhenDispatchClosed, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService);
		}
	}
}
