namespace Main.Controllers.RssFeedProvider
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.Globalization.Resource;
	using Crm.Library.Model;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Services.Interfaces;

	using Main.Infrastructure;
	using Main.Results;

	using Microsoft.AspNetCore.Routing;

	public class PostingRssFeedProvider : RssFeedProvider<Posting>
	{
		private readonly IUserService userService;
		private readonly IResourceManager resourceManager;

		public PostingRssFeedProvider(IAbsolutePathHelper absolutePathHelper, IUserService userService, IResourceManager resourceManager, Site site, IPluginProvider pluginProvider)
			: base(absolutePathHelper, userService, site, pluginProvider)
		{
			this.userService = userService;
			this.resourceManager = resourceManager;
		}
		protected override SyndicationFeedItemMapper<Posting> GetFeedMapper(Dictionary<string, object> argDictionary)
		{
			return new SyndicationFeedItemMapper<Posting>
				(
				x => String.Format("Posting {0} ({1} {2}) - {3}", x.Id, x.PostingType, x.EntityTypeName, x.PostingState),
				x => String.Format("Posting {0} ({1} {2}) - {3} - of user {4} posted at {5} failed. Posting data: {6}", x.Id, x.PostingType, x.EntityTypeName, x.PostingState, userService.GetUser(x.CreateUser).DisplayName, x.CreateDate.ToShortDateString(), x.SerializedEntity),
				"PostingList",
				"Index",
				x => null,
				x => x.CreateDate);
		}
		protected override SyndicationFeedOptions GetFeedOptions(Dictionary<string, object> argDictionary)
		{
			return new SyndicationFeedOptions
				(
				resourceManager.GetTranslation("Transactions"),
				resourceManager.GetTranslation("Transactions"),
				absolutePathHelper.GetPath("Index", "PostingList", new RouteValueDictionary(new { plugin = "Main" }))
				)
				;
		}
	}
}
