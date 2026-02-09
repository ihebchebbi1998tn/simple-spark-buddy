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

	using Main.Model;

	using Microsoft.AspNetCore.Mvc;

	public class MessageListController : GenericListController<Message>
	{
		protected override string GetEmptySlate()
		{
			return resourceManager.GetTranslation("NoMessages");
		}

		protected override string GetTitle()
		{
			return "EmailMessages";
		}

		[RequiredPermission(nameof(Message), Group = PermissionGroup.WebApiRead)]
		public override ActionResult IndexTemplate()
		{
			return base.IndexTemplate();
		}

		[RenderAction("MessageListTopMenu")]
		public virtual ActionResult TriggerEmailAgentTopMenu()
		{
			return PartialView();
		}
		public MessageListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Message>> rssFeedProviders, IEnumerable<ICsvDefinition<Message>> csvDefinitions, IEntityConfigurationProvider<Message> entityConfigurationProvider, IRepository<Message> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
	}
}
