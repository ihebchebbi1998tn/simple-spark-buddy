namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Model.Lookups;

	using Main.Model.Lookups;

	public class LookupActionRoleProvider : RoleCollectorBase
	{
		public LookupActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.WebApi, nameof(AddressType), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(BravoCategory), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(BusinessRelationshipType), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(BusinessTitle), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(CompanyGroupFlag1), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(CompanyGroupFlag2), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(CompanyGroupFlag3), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(CompanyGroupFlag4), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(CompanyGroupFlag5), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(CompanyPersonRelationshipType), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(CompanyType), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(Country), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(DepartmentType), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(EmailType), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(FaxType), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(InvoicingType), ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.FieldService);
			Add(PermissionGroup.WebApi, nameof(NoteType), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(PhoneType), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(Region), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(Salutation), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(SourceType), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(TaskType), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(TimeUnit), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(Title), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PermissionGroup.WebApi, nameof(WebsiteType), ServicePlugin.Roles.FieldService, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
		}
	}
}
