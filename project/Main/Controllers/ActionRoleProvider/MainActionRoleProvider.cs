namespace Main.Controllers.ActionRoleProvider
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	using LMobile.Unicore;

	using Main.Model;
	using Main.Model.Lookups;
	using Main.Rest.Model;

	using User = Crm.Library.Model.User;

	public class MainActionRoleProvider : RoleCollectorBase
	{
		public MainActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.Dashboard, PermissionName.View);
			Add(PermissionGroup.Dashboard, PermissionName.Index);

			Add(nameof(Log), PermissionName.Rss);
			Add(PermissionGroup.MaterialDashboard, PermissionName.View);
			Add(PermissionGroup.MaterialDashboard, PermissionName.Index);
			Add(PermissionGroup.MaterialDashboard, PermissionName.Calendar);
			Add(PermissionGroup.MaterialDashboard, PermissionName.FilterableCalendar);

			Add(MainPlugin.PermissionGroup.TopSearch, PermissionName.View);

			Add(PermissionGroup.Login, PermissionName.Backend);
			Add(PermissionGroup.Login, PermissionName.MaterialClientOnline);

			Add(PermissionGroup.Visibility, PermissionName.Edit);

			Add(PermissionGroup.File, PermissionName.GetContent);
			Add(PermissionGroup.File, PermissionName.Delete);
			Add(PermissionGroup.File, PermissionName.ThumbnailImage);
			Add(PermissionGroup.Visibility, PermissionName.SkipCheck, Roles.APIUser);

			Add(PermissionGroup.WebApi, nameof(DocumentGeneratorEntry));
			Add(PermissionGroup.WebApi, nameof(Domain), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(EntityType));
			Add(PermissionGroup.WebApi, nameof(Language), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(LicenseModule));
			Add(PermissionGroup.WebApi, nameof(PaymentCondition), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(PaymentInterval), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(PaymentType), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Permission));
			Add(PermissionGroup.WebApi, nameof(PermissionSchemaRole));
			Add(PermissionGroup.WebApi, nameof(User));
			Add(PermissionGroup.WebApi, nameof(Usergroup), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(UserStatus), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(PasswordStrength), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(MultiFactorAuthenticationMode), Roles.APIUser);

			Add(PermissionGroup.WebApi, nameof(MainPlugin.PermissionGroup.BackgroundService));
			Add(PermissionGroup.WebApi, nameof(MenuEntry));
			AddImport(MainPlugin.PermissionGroup.Site,
				PermissionName.Settings,
				PermissionGroup.WebApi,
				nameof(MenuEntry));

		}
	}
}
