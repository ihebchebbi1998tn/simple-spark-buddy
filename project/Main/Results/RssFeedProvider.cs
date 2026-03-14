namespace Main.Results
{
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

	using Crm.Library.Model;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Services.Interfaces;

	using Main.Infrastructure;

	using Microsoft.AspNetCore.Mvc;

	public abstract class RssFeedProvider<T> : IRssFeedProvider<T>
		where T : class
	{
		protected readonly CultureInfo CultureInfo;

		protected readonly IAbsolutePathHelper absolutePathHelper;
		private readonly Site site;
		private readonly IPluginProvider pluginProvider;
		protected abstract SyndicationFeedItemMapper<T> GetFeedMapper(Dictionary<string, object> argDictionary = null);
		protected abstract SyndicationFeedOptions GetFeedOptions(Dictionary<string, object> argDictionary = null);
		public virtual IQueryable<T> Eager(IQueryable<T> items)
		{
			return items;
		}

		public virtual RssActionResult GetFeed(ControllerContext controllerContext, IEnumerable<T> feedItems, Dictionary<string, object> argDictionary = null)
		{
			var feedHelper = new SyndicationFeedHelper<T>
				(
				controllerContext,
				feedItems,
				GetFeedMapper(argDictionary),
				GetFeedOptions(argDictionary),
				absolutePathHelper,
				site,
				pluginProvider
				);

			return new RssActionResult(feedHelper.GetFeed());
		}

		protected RssFeedProvider(IAbsolutePathHelper absolutePathHelper, IUserService userService, Site site, IPluginProvider pluginProvider)
		{
			this.absolutePathHelper = absolutePathHelper;
			this.site = site;
			this.pluginProvider = pluginProvider;
			CultureInfo = CultureInfo.GetCultureInfo("en");
			if (userService.CurrentUser != null)
			{
				CultureInfo = CultureInfo.GetCultureInfo(userService.CurrentUser.DefaultLanguageKey);
			}
		}
	}
}
