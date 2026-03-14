namespace Main.Controllers
{
	using System.Collections.Generic;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc;

	public class UserListController : GenericListController<User>
	{
		protected override string GetTitle()
		{
			return "UserAdmin";
		}
		[RenderAction("UserListTopMenu")]
		public virtual ActionResult RefreshUserCacheTopMenu()
		{
			return PartialView();
		}
		[RenderAction("UserListResource")]
		public virtual ActionResult UserListResource()
		{
			return PartialView();
		}
		[RenderAction("UserListHeaderExtension")]
		public virtual ActionResult UserLicenseInfo()
		{
			return PartialView();
		}
		[RenderAction("UserListHeaderExtension")]
		public virtual ActionResult UserMissingActiveDirectoryNames() => PartialView();
		[RequiredPermission(PermissionName.Index, Group = PermissionGroup.UserAdmin)]
		public override ActionResult IndexTemplate() => base.IndexTemplate();
		public override ActionResult MaterialPrimaryAction() => PartialView();
		public UserListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<User>> rssFeedProviders, IEnumerable<ICsvDefinition<User>> csvDefinitions, IEntityConfigurationProvider<User> entityConfigurationProvider, IRepository<User> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
	}
}
