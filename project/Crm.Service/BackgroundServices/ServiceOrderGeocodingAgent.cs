namespace Crm.Service.BackgroundServices
{
	using System;
	using System.Linq;

	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Service.Model;

	using log4net;

	using Main;
	using Main.BackgroundServices;

	using Microsoft.Extensions.Hosting;

	using NHibernate.AdoNet;
	using NHibernate.Impl;

	using Quartz;

	[DisallowConcurrentExecution]
	public class ServiceOrderGeocodingAgent : JobBase
	{
		private readonly IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository;
		private readonly IGeocodingService geocodingService;
		private readonly IAppSettingsProvider appSettings;
		private readonly IScheduler scheduler;
		public const string JobGroup = "Crm.Service";
		public const string JobName = "ServiceOrderGeocodingAgent";

		protected override void Run(IJobExecutionContext context)
		{
			// We exceeded our Geocode quota for today, try next day.
			if (geocodingService.QuotaExceeded)
			{
				return;
			}

			var hasGoogleMapsApiKey = appSettings.GetValue(MainPlugin.Settings.Geocoder.GoogleMapsApiKey).IsNotNullOrEmpty();
			if (geocodingService.GeocoderIsGoogle && !hasGoogleMapsApiKey)
			{
				scheduler.PauseJob(new JobKey("ServiceOrderGeocodingAgent", "Crm.Service"));
				Logger.Warn("Google geocoder is not able to geocode locations anymore without API Key. See config: Geocoder/GoogleMapsApiKey");
				return;
			}

			var entityWithGeocodes = serviceOrderRepository.GetAll().Where(x => x.Latitude == null || x.Longitude == null);
			entityWithGeocodes = entityWithGeocodes.Where(x => x.GeocodingRetryCounter <= 3);
			entityWithGeocodes = entityWithGeocodes.Where(x =>
				x.Street != null && x.Street != string.Empty
				|| x.City != null && x.City != string.Empty
				|| x.ZipCode != null && x.ZipCode != string.Empty
				|| x.CountryKey != null && x.CountryKey != string.Empty);

			var batchSizeConfig = appSettings.GetValue(MainPlugin.Settings.Geocoder.BatchSize);
			var batchSize = batchSizeConfig > 0 ? batchSizeConfig : 50;
			var batch = entityWithGeocodes.Take(batchSize).ToList();
			if ((serviceOrderRepository.Session as SessionImpl)?.Batcher.GetType() != typeof(NonBatchingBatcher))
			{
				serviceOrderRepository.Session.SetBatchSize(batchSize);
			}

			foreach (var entityWithGeocode in batch)
			{
				if (receivedShutdownSignal)
				{
					break;
				}

				if (!geocodingService.TryGeocode(entityWithGeocode))
				{
					continue;
				}

				serviceOrderRepository.Session.Update(entityWithGeocode);
			}

			if (batch.Count == batchSize && !receivedShutdownSignal)
			{
				scheduler.TriggerJob(new JobKey(JobName, JobGroup));
			}
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

		public ServiceOrderGeocodingAgent(ISessionProvider sessionProvider, IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository, IGeocodingService geocodingService, IAppSettingsProvider appSettings, ILog logger, IHostApplicationLifetime hostApplicationLifetime, IScheduler scheduler)
			: base(sessionProvider,
				logger,
				hostApplicationLifetime)
		{
			this.serviceOrderRepository = serviceOrderRepository;
			this.geocodingService = geocodingService;
			this.appSettings = appSettings;
			this.scheduler = scheduler;
		}
	}
}
