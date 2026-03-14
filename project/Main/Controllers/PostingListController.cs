namespace Main.Controllers
{
	using System.Collections.Generic;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using Main.BackgroundServices;

	using Microsoft.AspNetCore.Mvc;

	using Quartz;

	public class PostingListController : GenericListController<Posting>
	{
		private readonly IScheduler scheduler;

		public PostingListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Posting>> rssFeedProviders, IEnumerable<ICsvDefinition<Posting>> csvDefinitions, IEntityConfigurationProvider<Posting> entityConfigurationProvider, IRepository<Posting> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider, IScheduler scheduler)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
			this.scheduler = scheduler;
		}
		protected override string GetTitle()
		{
			return "PostingService";
		}

		public virtual ActionResult TriggerPostingService()
		{
			PostingService.Trigger(scheduler);
			return RedirectToAction("Index");
		}

		public virtual ActionResult SkipTransactions()
		{
			return PartialView();
		}

		[RenderAction("PostingListTopMenu")]
		public virtual ActionResult TriggerPostingServiceTopMenu()
		{
			return PartialView();
		}
	}
}
