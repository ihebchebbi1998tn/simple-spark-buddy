namespace Main.EventHandler
{
	using System;

	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Modularization.Events;

	using Main.BackgroundServices;
	using Main.Events;

	using Quartz;

	public class PostingSavedEventHandler : IEventHandler<EntityCreatedEvent<Posting>>, IEventHandler<PostingFailedEvent>, ISeparateTransactionEventHandler
	{
		private readonly IScheduler scheduler;
		private readonly IAppSettingsProvider appSettingsProvider;
		public PostingSavedEventHandler(IScheduler scheduler, IAppSettingsProvider appSettingsProvider)
		{
			this.scheduler = scheduler;
			this.appSettingsProvider = appSettingsProvider;
		}
		public virtual void Handle(EntityCreatedEvent<Posting> e)
		{
			PostingService.Trigger(scheduler);
		}
		public virtual void Handle(PostingFailedEvent e)
		{
			if (e.Posting.PostingState == PostingState.Failed && e.Posting.RetryAfter.HasValue)
			{
				PostingService.Trigger(scheduler, e.Posting.RetryAfter.Value.Subtract(DateTime.UtcNow).TotalMilliseconds);
			}
		}
	}
}
