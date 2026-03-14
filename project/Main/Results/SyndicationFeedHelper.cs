namespace Main.Results
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.ServiceModel.Syndication;

	using Crm.Library.Extensions;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Interfaces;

	using Main.Infrastructure;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Routing;

	/// <summary>
	/// Class to create an syndication feed.
	/// </summary>
	/// <typeparam name="TFeedItem">Accepts the custom feed item type.</typeparam>
	public class SyndicationFeedHelper<TFeedItem>
		where TFeedItem : class
	{
		// Members
		private const string Id = "id";
		private readonly ControllerContext context;
		private readonly SyndicationFeedItemMapper<TFeedItem> feedItemMapper;
		private readonly IEnumerable<TFeedItem> feedItems;
		private readonly SyndicationFeedOptions feedOptions;
		private readonly IAbsolutePathHelper absolutePathHelper;
		private readonly IList<SyndicationItem> syndicationItems;
		private readonly Site site;
		private readonly IPluginProvider pluginProvider;
		private SyndicationFeed syndicationFeed;

		/// <summary>
		/// Constructs the SyndicationFeedHelper.
		/// </summary>
		/// <param name="context">Accepts the current controller context.</param>
		/// <param name="feedItems">Accepts a list of feed items.</param>
		/// <param name="syndicationFeedItemMapper">Accepts a <see cref="SyndicationFeedItemMapper"/>.</param>
		/// <param name="syndicationFeedOptions">Accepts a <see cref="SyndicationFeedOptions"/>.</param>
		/// <param name="absolutePathHelper">Accepts a <see cref="IAbsolutePathHelper"/>.</param>
		/// <param name="site">Accepts a <see cref="Site"/>.</param>
		/// <param name="pluginProvider">Accepts a <see cref="IPluginProvider"/>.</param>
		public SyndicationFeedHelper
			(
			ControllerContext context,
			IEnumerable<TFeedItem> feedItems,
			SyndicationFeedItemMapper<TFeedItem> syndicationFeedItemMapper,
			SyndicationFeedOptions syndicationFeedOptions,
			IAbsolutePathHelper absolutePathHelper,
			Site site,
			IPluginProvider pluginProvider)
		{
			this.context = context;
			this.feedItems = feedItems;
			feedItemMapper = syndicationFeedItemMapper;
			feedOptions = syndicationFeedOptions;
			this.absolutePathHelper = absolutePathHelper;
			this.site = site;
			this.pluginProvider = pluginProvider;
			syndicationItems = new List<SyndicationItem>();
			SetUpFeedOptions();
		}

		// Properties
		private ControllerContext Context
		{
			get { return context; }
		}
		private IEnumerable<TFeedItem> FeedItems
		{
			get { return feedItems; }
		}
		private SyndicationFeedItemMapper<TFeedItem> FeedItemMapper
		{
			get { return feedItemMapper; }
		}
		private SyndicationFeedOptions SyndicationFeedOptions
		{
			get { return feedOptions; }
		}

		/// <summary>
		/// Creates and returns a <see cref="System.ServiceModel.Syndication.SyndicationFeed"/>.
		/// </summary>
		/// <returns><see cref="System.ServiceModel.Syndication.SyndicationFeed"/></returns>
		public SyndicationFeed GetFeed()
		{
			feedItems.ToList().ForEach(feedItem =>
				{
					var syndicationItem = CreateSyndicationItem(feedItem);
					AddItemAuthor(feedItem, syndicationItem);
					AddPublishDate(feedItem, syndicationItem);
					syndicationItems.Add(syndicationItem);
				});
			syndicationFeed.Items = syndicationItems;
			return syndicationFeed;
		}
		private void SetUpFeedOptions()
		{
			syndicationFeed = new SyndicationFeed
				(
				SyndicationFeedOptions.Title,
				SyndicationFeedOptions.Description,
				new Uri(SyndicationFeedOptions.Url)
				);
			AddFeedId();
			AddCopyrightStatement();
			AddLanguageIsoCode();
			AddLastUpdateDateTime();
		}
		private void AddLastUpdateDateTime()
		{
			if (SyndicationFeedOptions.LastUpdated != default(DateTimeOffset))
			{
				syndicationFeed.LastUpdatedTime = SyndicationFeedOptions.LastUpdated;
			}
		}
		private void AddLanguageIsoCode()
		{
			if (!string.IsNullOrEmpty(SyndicationFeedOptions.Language))
			{
				syndicationFeed.Language = SyndicationFeedOptions.Language;
			}
		}
		private void AddCopyrightStatement()
		{
			if (!string.IsNullOrEmpty(SyndicationFeedOptions.Copyright))
			{
				syndicationFeed.Copyright = new TextSyndicationContent(SyndicationFeedOptions.Copyright);
			}
		}
		private void AddFeedId()
		{
			if (!string.IsNullOrEmpty(SyndicationFeedOptions.FeedId))
			{
				syndicationFeed.Id = SyndicationFeedOptions.FeedId;
			}
		}
		private void AddPublishDate(TFeedItem feedItem, SyndicationItem syndicationItem)
		{
			var publishDate = FeedItemMapper.DatePublished.Invoke(feedItem);
			syndicationItem.PublishDate = publishDate;
		}
		private void AddItemAuthor(TFeedItem feedItem, SyndicationItem syndicationItem)
		{
			var authorEmail = FeedItemMapper.AuthorEmail == null ? string.Empty : FeedItemMapper.AuthorEmail.Invoke(feedItem);
			var authorName = FeedItemMapper.AuthorName == null ? string.Empty : FeedItemMapper.AuthorName.Invoke(feedItem);
			var authorUrl = FeedItemMapper.AuthorUrl == null ? string.Empty : FeedItemMapper.AuthorUrl.Invoke(feedItem);
			var syndicationPerson = new SyndicationPerson();
			if (string.IsNullOrEmpty(authorName))
			{
				syndicationPerson.Name = authorName;
			}
			if (string.IsNullOrEmpty(authorEmail))
			{
				syndicationPerson.Email = authorEmail;
			}
			if (string.IsNullOrEmpty(authorUrl))
			{
				syndicationPerson.Uri = authorUrl;
			}
			if (!string.IsNullOrEmpty(syndicationPerson.Name) || !string.IsNullOrEmpty(syndicationPerson.Email)
			    || !string.IsNullOrEmpty(syndicationPerson.Uri))
			{
				syndicationItem.Authors.Add(syndicationPerson);
			}
		}
		private SyndicationItem CreateSyndicationItem(TFeedItem feedItem)
		{
			var syndicationItem = new SyndicationItem
				(
				FeedItemMapper.Title.Invoke(feedItem),
				FeedItemMapper.Content.Invoke(feedItem),
				GetUrl(feedItem)
				);
			return syndicationItem;
		}
		private Uri GetUrl(TFeedItem feedItem)
		{
			var routeValues = FeedItemMapper.RouteValues != null ? FeedItemMapper.RouteValues.Invoke(feedItem) : new RouteValueDictionary();
			routeValues.Add(Id, FeedItemMapper.Id.Invoke(feedItem));
			var action = FeedItemMapper.Action == null
				             ? FeedItemMapper.ActionString
				             : FeedItemMapper.Action.Invoke(feedItem);
			var controller = FeedItemMapper.Controller == null
				                 ? FeedItemMapper.ControllerString
				                 : FeedItemMapper.Controller.Invoke(feedItem);
			var url = absolutePathHelper.GetPath(action, controller, routeValues, true);
			url = url != null ? Uri.UnescapeDataString(url) : url;
			
			if (url == null)
			{
				var plugin = pluginProvider.FindPluginDescriptor(typeof(TFeedItem)).PluginName;
				url = $"{site.GetExtension<DomainExtension>().HostUri.ToString().AppendIfMissing("/")}Main/Home/MaterialIndex#/{plugin}/{controller}/{action}";
				var routeValue = routeValues[Id];
				if (routeValue != null)
				{
					url = $"{url}/{routeValue}";
				}
			}
			return new Uri(url);
		}
	}
}
