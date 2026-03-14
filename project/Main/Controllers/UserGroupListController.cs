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
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc;

	public class UserGroupListController : GenericListController<Usergroup>
	{
		[RequiredPermission(MainPlugin.PermissionName.ListUsergroups, Group = PermissionGroup.UserAdmin)]
		public override ActionResult IndexTemplate()
		{
			return base.IndexTemplate();
		}

		[RequiredPermission(MainPlugin.PermissionName.ListUsergroups, Group = PermissionGroup.UserAdmin)]
		public override ActionResult FilterTemplate()
		{
			return base.FilterTemplate();
		}

		[RequiredPermission(MainPlugin.PermissionName.AddUserGroup, Group = PermissionGroup.UserAdmin)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}

		public UserGroupListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Usergroup>> rssFeedProviders, IEnumerable<ICsvDefinition<Usergroup>> csvDefinitions, IEntityConfigurationProvider<Usergroup> entityConfigurationProvider, IRepository<Usergroup> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
	}
}
