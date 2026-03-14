namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Model;

	using Main;

	public class PersonActionRoleProvider : RoleCollectorBase
	{
		public PersonActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(CrmPlugin.PermissionGroup.Person, PermissionName.View, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, PermissionName.Index, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, PermissionName.Read, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, PermissionName.Create, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, PermissionName.Edit, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, PermissionName.Delete, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);

			Add(CrmPlugin.PermissionGroup.Person, MainPlugin.PermissionName.Merge, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.AddTask, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, MainPlugin.PermissionName.ImportFromVcf, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.DeleteBusinessRelationship, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.DeleteCommunication, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.MakeStandardAddress, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.EditAddress, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.DeleteAddress, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);

			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.NotesTab, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.TasksBlock, ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.RelationshipsTab, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);

			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.CreateTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.AssociateTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.RenameTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.RemoveTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.DeleteTag, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice);

			Add(CrmPlugin.PermissionGroup.Person, MainPlugin.PermissionName.PublicHeaderInfo, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, MainPlugin.PermissionName.PrivateHeaderInfo, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, MainPlugin.PermissionName.SidebarBackgroundInfo, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.SidebarBravo, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.SidebarContactInfo, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(CrmPlugin.PermissionGroup.Person, CrmPlugin.PermissionName.SidebarStaffList, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);

			Add(MainPlugin.PermissionGroup.TopSearch, nameof(Person), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
		}
	}
}
