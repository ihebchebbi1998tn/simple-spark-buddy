namespace Main.EventHandler
{
	using System;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Modularization.Events;

	using Main.Events;

	public class PostingFailedEventHandler : IEventHandler<PostingFailedEvent>
	{
		private readonly IRepository<Posting> postingRepository;
		private readonly IAppSettingsProvider appSettingsProvider;

		public PostingFailedEventHandler(IRepository<Posting> postingRepository, IAppSettingsProvider appSettingsProvider)
		{
			this.postingRepository = postingRepository;
			this.appSettingsProvider = appSettingsProvider;
		}

		public virtual void Handle(PostingFailedEvent e)
		{
			var posting = e.Posting;
			var ex = e.Exception;
			if(++posting.Retries < appSettingsProvider.GetValue(MainPlugin.Settings.Posting.MaxRetries))
			{
				posting.PostingState = PostingState.Failed;
				posting.RetryAfter = DateTime.UtcNow.AddMinutes(appSettingsProvider.GetValue(MainPlugin.Settings.Posting.RetryAfter));
			}
			else
			{
				posting.PostingState = PostingState.Blocked;
				posting.RetryAfter = null;
			}
			posting.StateDetails = ex.Message;
			while (ex.InnerException != null)
			{
				posting.StateDetails += Environment.NewLine + ex.InnerException.Message;
				ex = ex.InnerException;
			}
			postingRepository.SaveOrUpdate(posting);
			postingRepository.Session.Flush();
		}
	}
}
