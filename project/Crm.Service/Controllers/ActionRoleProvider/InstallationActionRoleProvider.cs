namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main;
	using Main.Model.Lookups;

	public class InstallationActionRoleProvider : RoleCollectorBase
	{
		public InstallationActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ServicePlugin.PermissionGroup.Installation, PermissionName.View, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			Add(ServicePlugin.PermissionGroup.Installation, PermissionName.Index, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			Add(ServicePlugin.PermissionGroup.Installation, PermissionName.Read, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.Installation, PermissionName.Read, ServicePlugin.PermissionGroup.Installation, PermissionName.Index);
			Add(ServicePlugin.PermissionGroup.Installation, PermissionName.Edit, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Installation, PermissionName.Edit, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.Installation, PermissionName.Create, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Installation, PermissionName.Create, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.Installation, PermissionName.Copy, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(ServicePlugin.PermissionGroup.Installation, PermissionName.Delete, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Installation, PermissionName.Delete, ServicePlugin.PermissionGroup.Installation, PermissionName.Edit);

			Add(ServicePlugin.PermissionGroup.InstallationPosition, PermissionName.Index, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.InstallationPosition, PermissionName.Index, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.InstallationPosition, PermissionName.Create, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.InstallationPosition, PermissionName.Create, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.InstallationPosition, PermissionName.Edit, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.InstallationPosition, PermissionName.Edit, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.InstallationPosition, PermissionName.Delete, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.InstallationPosition, PermissionName.Delete, ServicePlugin.PermissionGroup.Installation, PermissionName.Edit);

			Add(ServicePlugin.PermissionGroup.InstallationPositionSerial, PermissionName.Index, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.InstallationPositionSerial, PermissionName.Index, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.InstallationPositionSerial, PermissionName.Create, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.InstallationPositionSerial, PermissionName.Create, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.InstallationPositionSerial, PermissionName.Edit, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.InstallationPositionSerial, PermissionName.Edit, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);

			Add(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.CreateTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			AddImport(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.CreateTag, ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.AssociateTag);
			AddImport(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.CreateTag, ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.RemoveTag);
			Add(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.AssociateTag, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			Add(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.RenameTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			AddImport(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.RenameTag, ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.DeleteTag);
			Add(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.RemoveTag, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.RemoveTag, ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.AssociateTag);
			Add(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.DeleteTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			AddImport(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.DeleteTag, ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.CreateTag);

			Add(ServicePlugin.PermissionGroup.Installation, MainPlugin.PermissionName.DetailsBackgroundInfo, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.Installation, MainPlugin.PermissionName.DetailsBackgroundInfo, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.DetailsContactInfo, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.DetailsContactInfo, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.CreateStandardAction, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.EditStandardAction, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService);
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.TabMaterial, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			AddImport(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.TabMaterial, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.NotesTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.InternalSales);
			AddImport(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.NotesTab, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.RelationshipsTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.InternalSales);
			AddImport(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.RelationshipsTab, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.ServiceOrdersTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Installation, ServicePlugin.PermissionName.ServiceOrdersTab, ServicePlugin.PermissionGroup.ServiceOrder, PermissionName.Read);
			Add(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.TasksBlock, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			AddImport(ServicePlugin.PermissionGroup.Installation, CrmPlugin.PermissionName.TasksBlock, ServicePlugin.PermissionGroup.Installation, PermissionName.Read);

			Add(PermissionGroup.WebApi, nameof(Installation), CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(InstallationHeadStatus), CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(InstallationType), CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(InstallationPos), CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(InstallationPosSerial), CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(DocumentCategory), CrmPlugin.Roles.SalesBackOffice, CrmPlugin.Roles.HeadOfSales, CrmPlugin.Roles.InternalSales, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService, Roles.APIUser);
		}
	}
}
