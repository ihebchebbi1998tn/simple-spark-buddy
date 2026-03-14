using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Autofac;
using Crm.Library.BackgroundServices;
using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Library.Data.NHibernateProvider;
using Crm.Library.Extensions;
using Crm.Library.Helper;
using Crm.Library.Model;
using log4net;
using Main.Flow.Model;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace Main.Flow.BackgroundServices
{
	using System.Collections.Generic;

	using Crm.Library.Modularization.Events;

	using Main.Flow.Events;

	[DisallowConcurrentExecution]
	public class FlowProcessingService : JobBase
	{
		private readonly VirtualRequestHandler virtualRequestHandler;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly IScheduler scheduler;
		private readonly ILog logger;
		public const string JobName = "FlowProcessingService";
		public const string JobGroup = "Main.Flow";

		public FlowProcessingService(ISessionProvider sessionProvider, ILog logger, VirtualRequestHandler virtualRequestHandler, IAppSettingsProvider appSettingsProvider, IScheduler scheduler, IHostApplicationLifetime hostApplicationLifetime)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.virtualRequestHandler = virtualRequestHandler;
			this.appSettingsProvider = appSettingsProvider;
			this.scheduler = scheduler;
			this.logger = logger;
		}

		protected override void Run(IJobExecutionContext context)
		{
			virtualRequestHandler.BeginRequest();
			var repo = virtualRequestHandler.GetLifetimeScope().Resolve<IRepository<FlowItem>>();
			var pendingItems = repo.GetAll()
				.Where(x => x.PostingState == PostingState.Pending || x.PostingState == PostingState.Failed)
				.Where(x => x.RetryAfter == null || x.RetryAfter <= DateTime.UtcNow)
				.OrderBy(x => x.CreateDate)
				.ToList();

			var failedEvents = new List<FlowItemFailedEvent>();

			using var client = new HttpClient();

			foreach (var item in pendingItems)
			{
				try
				{
					var content = new StringContent(item.SerializedEntity, Encoding.UTF8, "application/json");
					if(item.Rule.Username.IsNotNullOrEmpty() && item.Rule.Password.IsNotNullOrEmpty())
					{
						client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{item.Rule.Username}:{item.Rule.Password}")));
					}
					var response = client.PostAsync(item.Rule.Endpoint, content).Result;

					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						item.PostingState = PostingState.Processed;
						item.StateDetails = string.Empty;
						virtualRequestHandler.GetLifetimeScope().Resolve<IRepository<FlowItem>>().SaveOrUpdate(item);
					}
					else
					{
						throw new Exception($"Endpoint responded with {response.StatusCode} instead of {System.Net.HttpStatusCode.OK}");
					}
				}
				catch (Exception ex)
				{
					failedEvents.Add(new FlowItemFailedEvent(item, ex));
					logger.Error($"There was an exception while calling {item.Rule.Endpoint}", ex);
				}
			}
			virtualRequestHandler.EndRequest();
			
			virtualRequestHandler.BeginRequest();
			foreach (var failedEvent in failedEvents)
			{
				virtualRequestHandler.GetLifetimeScope().Resolve<IEventAggregator>().Publish(failedEvent);
			}
			virtualRequestHandler.EndRequest();

		}
		protected override JobFailureMode JobFailureMode
		{
			get { return JobFailureMode.Continue; }
		}

		public static async void Trigger(IScheduler scheduler, double delay = 0)
		{
			if (!scheduler.IsStarted)
			{
				await scheduler.Start();
			}

			var nextTrigger = DateTime.Now.AddMilliseconds(delay);
			var alreadyTriggered = scheduler.GetTriggersOfJob(new JobKey(JobName, JobGroup)).Result.OfType<ISimpleTrigger>().Any(x => scheduler.GetTriggerState(x.Key).Result != TriggerState.Complete && x.GetNextFireTimeUtc() <= nextTrigger.AddMilliseconds(500) && x.GetNextFireTimeUtc() >= nextTrigger.AddMilliseconds(-500));
			if (alreadyTriggered)
			{
				return;
			}
			var trigger = TriggerBuilder
				.Create()
				.ForJob(JobName, JobGroup)
				.StartAt(nextTrigger)
				.Build();
			await scheduler.ScheduleJob(trigger);
		}
	}
}
