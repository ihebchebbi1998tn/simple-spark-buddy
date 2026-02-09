namespace Crm.Service.EventHandler
{
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Library.Modularization.Events;
	using Crm.Service.BackgroundServices;
	using Crm.Service.Model;

	using Main;
	using Main.BackgroundServices;

	using Quartz;

	public class ServiceOrderCreatedEventHandler : IEventHandler<EntityCreatedEvent<ServiceOrderHead>>, ISeparateTransactionEventHandler
	{
		private readonly IScheduler scheduler;
		private readonly IGeocodingService geocodingService;
		private readonly IAppSettingsProvider appSettingsProvider;
		public ServiceOrderCreatedEventHandler(IScheduler scheduler, IGeocodingService geocodingService, IAppSettingsProvider appSettingsProvider)
		{
			this.scheduler = scheduler;
			this.geocodingService = geocodingService;
			this.appSettingsProvider = appSettingsProvider;
		}

		public virtual void Handle(EntityCreatedEvent<ServiceOrderHead> e)
		{
			var hasGoogleMapsApiKey = appSettingsProvider.GetValue(MainPlugin.Settings.Geocoder.GoogleMapsApiKey).IsNotNullOrEmpty();
			if (!geocodingService.GeocoderIsGoogle || hasGoogleMapsApiKey)
			{
				ServiceOrderGeocodingAgent.Trigger(scheduler);
			}
		}
	}
}
