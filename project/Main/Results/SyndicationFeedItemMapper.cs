namespace Main.Results
{
	using System;

	using Microsoft.AspNetCore.Routing;

	public class SyndicationFeedItemMapper<TFeedItem> where TFeedItem : class
	{
		// Members
		readonly Func<TFeedItem, string> title;
		readonly Func<TFeedItem, string> content;
		readonly Func<TFeedItem, string> controller;
		readonly Func<TFeedItem, string> action;
		readonly Func<TFeedItem, string> id;
		readonly Func<TFeedItem, RouteValueDictionary> routeValues;
		readonly Func<TFeedItem, DateTimeOffset> datePublished;
		readonly string controllerString;
		readonly string actionString;

		// Properties
		public Func<TFeedItem, string> Title
		{
			get
			{
				return title;
			}
		}
		public Func<TFeedItem, string> Content
		{
			get
			{
				return content;
			}
		}
		public Func<TFeedItem, string> Controller
		{
			get
			{
				return controller;
			}
		}
		public Func<TFeedItem, string> Action
		{
			get
			{
				return action;
			}
		}
		public Func<TFeedItem, string> Id
		{
			get
			{
				return id;
			}
		}
		public Func<TFeedItem, DateTimeOffset> DatePublished
		{
			get
			{
				return datePublished;
			}
		}
		public Func<TFeedItem, RouteValueDictionary> RouteValues
		{
			get
			{
				return routeValues;
			}
		}
		public string ControllerString
		{
			get
			{
				return controllerString;
			}
		}
		public string ActionString
		{
			get
			{
				return actionString;
			}
		}
		public Func<TFeedItem, string> AuthorName { get; set; }
		public Func<TFeedItem, string> AuthorEmail { get; set; }
		public Func<TFeedItem, string> AuthorUrl { get; set; }
    
		// Constructor
		public SyndicationFeedItemMapper
			(
			Func<TFeedItem, string> title,
			Func<TFeedItem, string> content,
			string controller,
			string action,
			Func<TFeedItem, string> id,
			Func<TFeedItem, DateTimeOffset> datePublished
			)
			: this(title, content, id, datePublished)
		{
			controllerString = controller;
			actionString = action;
		}
		public SyndicationFeedItemMapper
			(
			Func<TFeedItem, string> title,
			Func<TFeedItem, string> content,
			Func<TFeedItem, string> controller,
			Func<TFeedItem, string> action,
			Func<TFeedItem, string> id,
			Func<TFeedItem, DateTimeOffset> datePublished
			)
			: this(title, content, id, datePublished)
		{
			this.controller = controller;
			this.action = action;
		}
		public SyndicationFeedItemMapper
			(
			Func<TFeedItem, string> title,
			Func<TFeedItem, string> content,
			Func<TFeedItem, string> controller,
			Func<TFeedItem, string> action,
			Func<TFeedItem, string> id,
			Func<TFeedItem, RouteValueDictionary> routeValues,
			Func<TFeedItem, DateTimeOffset> datePublished
			)
			: this(title, content, id, datePublished)
		{
			this.controller = controller;
			this.action = action;
			this.routeValues = routeValues;
		}
		protected SyndicationFeedItemMapper
			(
			Func<TFeedItem, string> title,
			Func<TFeedItem, string> content,
			Func<TFeedItem, string> id,
			Func<TFeedItem, DateTimeOffset> datePublished
			)
		{
			this.title = title;
			this.content = content;
			this.id = id;
			this.datePublished = datePublished;
		}
	}
}
