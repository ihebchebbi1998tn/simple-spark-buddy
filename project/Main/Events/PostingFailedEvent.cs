namespace Main.Events
{
	using System;

	using Crm.Library.Model;
	using Crm.Library.Modularization.Events;

	public class PostingFailedEvent : IEvent
	{
		public Posting Posting { get; set; }
		public Exception Exception { get; set; }
		public PostingFailedEvent(Posting posting, Exception exception)
		{
			Posting = posting;
			Exception = exception;
		}
	}
}
