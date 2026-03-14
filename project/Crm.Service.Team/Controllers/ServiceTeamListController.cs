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
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Team;

	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;

	public class ServiceTeamListController : UserGroupListController
	{
		public ServiceTeamListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Usergroup>> rssFeedProviders, IEnumerable<ICsvDefinition<Usergroup>> csvDefinitions, IEntityConfigurationProvider<Usergroup> entityConfigurationProvider, IRepository<Usergroup> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}

		protected override GenericListViewModel GetGenericListTemplateViewModel()
		{
			var model = base.GetGenericListTemplateViewModel();
			model.TypeNameOverride = "ServiceTeam";
			return model;
		}
		
		[RequiredPermission(ServiceTeamPlugin.PermissionName.ListServiceTeams, Group = PermissionGroup.UserAdmin)]
		public override ActionResult IndexTemplate()
		{
			return base.IndexTemplate();
		}

		[RequiredPermission(ServiceTeamPlugin.PermissionName.ListServiceTeams, Group = PermissionGroup.UserAdmin)]
		public override ActionResult FilterTemplate()
		{
			return base.FilterTemplate();
		}

		public override ActionResult MaterialItemTemplate()
		{
			return PartialView("../UserGroupList/MaterialItemTemplate");
		}

		[RequiredPermission(MainPlugin.PermissionName.AddUserGroup, Group = PermissionGroup.UserAdmin)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}

		[RenderAction("MaterialUsergroupItemExtensions")]
		public virtual ActionResult MaterialUsergroupItemExtensions()
		{
			return PartialView();
		}
	}
}
