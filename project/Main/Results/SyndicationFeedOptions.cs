namespace Main.Results
{
	using System;

	public class SyndicationFeedOptions
	{
		// Properties
		public SyndicationFeedOptions(string title, string description, string url)
		{
			Title = title;
			Description = description;
			Url = url;
		}

		public string Title { get; private set; }
		public string Description { get; private set; }
		public string Url { get; private set; }

		public string FeedId { get; set; }
		public DateTimeOffset LastUpdated { get; set; }
		public string Copyright { get; set; }
		public string Language { get; set; }

		// Constructor
	}
}
