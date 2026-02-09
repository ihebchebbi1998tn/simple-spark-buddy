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

	using LMobile.Unicore;

	using Microsoft.AspNetCore.Mvc;

	public class PermissionSchemaRoleListController : GenericListController<PermissionSchemaRole>
	{
		protected override string GetTitle()
		{
			return "UserRoles";
		}
		[RequiredPermission(MainPlugin.PermissionName.ListRoles, Group = PermissionGroup.UserAdmin)]
		public override ActionResult IndexTemplate() => base.IndexTemplate();
		[RequiredPermission(MainPlugin.PermissionName.CreateRole, Group = PermissionGroup.UserAdmin)]
		public override ActionResult MaterialPrimaryAction() => PartialView();
		public PermissionSchemaRoleListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<PermissionSchemaRole>> rssFeedProviders, IEnumerable<ICsvDefinition<PermissionSchemaRole>> csvDefinitions, IEntityConfigurationProvider<PermissionSchemaRole> entityConfigurationProvider, IRepository<PermissionSchemaRole> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
	}
}
