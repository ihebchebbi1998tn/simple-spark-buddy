namespace Main.Services
{
	using Autofac;

	using Crm.Library.Helper;

	using Geocoding;
	using Geocoding.Google;
	using Geocoding.MapQuest;
	using Geocoding.Microsoft;
	using Geocoding.Yahoo;

	public class GeocoderModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register<IGeocoder>(c =>
			{
				var appSettingsProvider = c.Resolve<IAppSettingsProvider>();
				var geocoderService = appSettingsProvider.GetValue(MainPlugin.Settings.Geocoder.GeocoderService);
				var googleMapsApiKey = appSettingsProvider.GetValue(MainPlugin.Settings.Geocoder.GoogleMapsApiKey);
				var mapQuestApiKey = appSettingsProvider.GetValue(MainPlugin.Settings.Geocoder.MapQuestApiKey);
				var bingMapsApiKey = appSettingsProvider.GetValue(MainPlugin.Settings.Geocoder.BingMapsApiKey);
				var yahooMapsApiKey = appSettingsProvider.GetValue(MainPlugin.Settings.Geocoder.YahooMapsApiKey);
				var yahooMapsApiSecret = appSettingsProvider.GetValue(MainPlugin.Settings.Geocoder.YahooMapsApiSecret);

				switch (geocoderService.ToLowerInvariant())
				{
					case "mapquest":
						return new MapQuestGeocoder(mapQuestApiKey);
					case "bing":
						return new BingMapsGeocoder(bingMapsApiKey);
					case "yahoo":
						return new YahooGeocoder(yahooMapsApiKey, yahooMapsApiSecret);
					default:
						return string.IsNullOrWhiteSpace(googleMapsApiKey)
						? new GoogleGeocoder()
						: new GoogleGeocoder { ApiKey = googleMapsApiKey };
				}
			});
		}
	}
}
