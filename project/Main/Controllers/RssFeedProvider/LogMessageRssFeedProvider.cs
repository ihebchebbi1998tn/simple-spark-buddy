namespace Main.Controllers.RssFeedProvider
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.Globalization.Resource;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Services.Interfaces;

	using Main.Infrastructure;
	using Main.Model;
	using Main.Results;

	using Microsoft.AspNetCore.Routing;

	public class LogMessageRssFeedProvider : RssFeedProvider<Log>
	{
		private readonly IResourceManager resourceManager;
		public LogMessageRssFeedProvider(IAbsolutePathHelper absolutePathHelper, IUserService userService, IResourceManager resourceManager, Site site, IPluginProvider pluginProvider)
			: base(absolutePathHelper, userService, site, pluginProvider)
		{
			this.resourceManager = resourceManager;
		}

		protected override SyndicationFeedItemMapper<Log> GetFeedMapper(Dictionary<string, object> argDictionary)
		{
			return new SyndicationFeedItemMapper<Log>
				(
				f => String.Format("{0} - Id: {1}", f.Level, f.Id),
				f => f.Message,
				"LogList",
				"IndexTemplate",
				f => null,
				f => f.CreateDate
				);
		}
		protected override SyndicationFeedOptions GetFeedOptions(Dictionary<string, object> argDictionary)
		{
			var logLevel = argDictionary != null && argDictionary.ContainsKey("LogLevel") ? (string)argDictionary["LogLevel"] : null;
			logLevel = String.IsNullOrEmpty(logLevel) ? resourceManager.GetTranslation("All") : logLevel;
			var logLevelStr = String.Format("{0} = {1}", resourceManager.GetTranslation("LogLevel"), logLevel);
			return new SyndicationFeedOptions
				(
				resourceManager.GetTranslation("ListOfLogs"),
				String.Format("{0}: {1}", resourceManager.GetTranslation("FilteredBy"), logLevelStr), absolutePathHelper.GetPath("IndexTemplate", "LogList", new RouteValueDictionary(new { plugin = "Main" }))
				);
		}
	}
}
