namespace Main.Flow.Events
{
	using System;

	using Crm;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Modularization.Events;

	using Main.Flow.BackgroundServices;
	using Main.Flow.Model;

	using Quartz;

	public class FlowItemFailedEventHandler : IEventHandler<FlowItemFailedEvent>
	{
		private readonly IRepository<FlowItem> flowItemRepository;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly IScheduler scheduler;

		public FlowItemFailedEventHandler(IRepository<FlowItem> flowItemRepository, IAppSettingsProvider appSettingsProvider, IScheduler scheduler)
		{
			this.flowItemRepository = flowItemRepository;
			this.appSettingsProvider = appSettingsProvider;
			this.scheduler = scheduler;
		}

		public virtual void Handle(FlowItemFailedEvent e)
		{
			var item = e.FlowItem;
			var ex = e.Exception;
			if(++item.Retries < appSettingsProvider.GetValue(FlowPlugin.Settings.System.MaxRetries))
			{
				item.PostingState = PostingState.Failed;
				item.RetryAfter = DateTime.UtcNow.AddMinutes(appSettingsProvider.GetValue(MainPlugin.Settings.Posting.RetryAfter));
			}
			else
			{
				item.PostingState = PostingState.Blocked;
				item.RetryAfter = null;
			}
			item.StateDetails = ex.Message;
			while (ex.InnerException != null)
			{
				item.StateDetails += Environment.NewLine + ex.InnerException.Message;
				ex = ex.InnerException;
			}
			flowItemRepository.SaveOrUpdate(item);
			flowItemRepository.Session.Flush();
			
			if (e.FlowItem.PostingState == PostingState.Failed && e.FlowItem.RetryAfter.HasValue)
			{
				FlowProcessingService.Trigger(scheduler, e.FlowItem.RetryAfter.Value.Subtract(DateTime.UtcNow).TotalMilliseconds);
			}
		}
	}
}
