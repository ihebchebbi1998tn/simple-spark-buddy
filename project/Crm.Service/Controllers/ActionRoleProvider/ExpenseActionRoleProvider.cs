namespace Crm.Service.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.PerDiem;
	using Crm.PerDiem.Model;
	using Crm.PerDiem.Model.Lookups;
	using Crm.Service;

	using Main;
	using Main.Model.Lookups;

	public class ExpenseActionRoleProvider : RoleCollectorBase
	{
		public ExpenseActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var serviceRoleNames = new[] { ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.FieldService };

			Add(PerDiemPlugin.PermissionGroup.Expense, PermissionName.Create, serviceRoleNames);
			Add(PerDiemPlugin.PermissionGroup.Expense, PermissionName.Edit, serviceRoleNames);
			Add(PerDiemPlugin.PermissionGroup.Expense, PermissionName.Delete, serviceRoleNames);
			Add(PerDiemPlugin.PermissionGroup.Expense, PermissionName.Index, serviceRoleNames);
			Add(PerDiemPlugin.PermissionGroup.Expense, PermissionName.View, serviceRoleNames);
			Add(PerDiemPlugin.PermissionGroup.Expense, PerDiemPlugin.PermissionName.SeeAllUsersExpenses, ServicePlugin.Roles.HeadOfService, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.ServiceBackOffice);
			Add(PerDiemPlugin.PermissionGroup.Expense, MainPlugin.PermissionName.DownloadAsPdf, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);
			Add(PerDiemPlugin.PermissionGroup.Expense, PerDiemPlugin.PermissionName.NoMinDateLimit, ServicePlugin.Roles.ServiceBackOffice, ServicePlugin.Roles.InternalService, ServicePlugin.Roles.HeadOfService);

			Add(PermissionGroup.WebApi, nameof(UserExpense), serviceRoleNames);
			Add(PermissionGroup.WebApi, nameof(ExpenseType), serviceRoleNames);
			Add(PermissionGroup.WebApi, nameof(CostCenter), serviceRoleNames);
			Add(PermissionGroup.WebApi, nameof(Currency), serviceRoleNames);
		}
	}
}
