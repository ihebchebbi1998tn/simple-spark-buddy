namespace Main.Controllers
{
	using System.Collections.Generic;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using LMobile.Unicore;

	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;

	public class RoleListController : GenericListController<PermissionSchemaRole>
	{
		protected override string GetTitle()
		{
			return "UserRoles";
		}
		protected override string GetEmptySlate()
		{
			return resourceManager.GetTranslation("UserRoleEmptySlate");
		}
		[RenderAction("RoleListResource")]
		public virtual ActionResult RoleListResource()
		{
			return PartialView();
		}
		protected override GenericListViewModel GetGenericListViewModel()
		{
			var model = GetGenericListTemplateViewModel();
			model.EmptySlate = GetEmptySlate();
			model.TypeNameOverride = "Role";
			return model;
		}
		public RoleListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<PermissionSchemaRole>> rssFeedProviders, IEnumerable<ICsvDefinition<PermissionSchemaRole>> csvDefinitions, IEntityConfigurationProvider<PermissionSchemaRole> entityConfigurationProvider, IRepository<PermissionSchemaRole> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
	}
}
