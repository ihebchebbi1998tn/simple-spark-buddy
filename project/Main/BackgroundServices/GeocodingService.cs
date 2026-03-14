namespace Main.BackgroundServices
{
	using System;
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	using Geocoding;
	using Geocoding.Google;

	using log4net;

	using Main.Model.Lookups;

	using Microsoft.Extensions.Caching.Distributed;

	public interface IGeocoderCountryCache : ISingletonDependency
	{
		string GetCountryCode(string countryKey);
		void SetCountryCode(string countryKey, string countryCode);
	}

	public interface IGeocoderCache : ISingletonDependency
	{
		void CacheAddresses(string cacheString, Location[] addresses);
		Location[] GetAddresses(string cacheString);
		bool IsQuotaExceeded();
		void SetQuotaExceeded();
	}

	public interface IGeocoderFormattedAddressCache : ISingletonDependency
	{
		void CacheFormattedAddresses(string cacheString, string[] formattedAddresses);
		string[] GetFormattedAddresses(string cacheString);
	}

	public class GeocoderCountryCache : Cache<string>, IGeocoderCountryCache
	{
		private const string GeocodingCountry = "GeocodingCountry";
		public GeocoderCountryCache(IDistributedCache cache, IRestSerializer serializer)
			: base(nameof(GeocoderCountryCache), cache, serializer)
		{
		}
		public virtual void SetCountryCode(string countryKey, string countryCode) => DictSet(GeocodingCountry, countryKey, countryCode);
		public virtual string GetCountryCode(string countryKey) => DictGet(GeocodingCountry, countryKey);
	}


	public class GeocoderCache : Cache<Location[]>, IGeocoderCache
	{
		private const string Geocoding = "Geocoding";
		private const string GeocoderQuotaExceeded = "GeocoderQuotaExceeded";
		public GeocoderCache(IDistributedCache cache, IRestSerializer serializer)
			: base(nameof(GeocoderCache), cache, serializer)
		{
		}
		public virtual void CacheAddresses(string cacheString, Location[] addresses) => DictSet(Geocoding, cacheString, addresses);
		public virtual Location[] GetAddresses(string cacheString) => DictGet(Geocoding, cacheString);
		public virtual bool IsQuotaExceeded() => Get<bool>(GeocoderQuotaExceeded) == true;
		public virtual void SetQuotaExceeded() => Set(GeocoderQuotaExceeded, true, new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) });
	}

	public class GeocoderFormattedAddressCache : Cache<string[]>, IGeocoderFormattedAddressCache
	{
		private const string GeocodingFormattedAddress = "GeocodingFormattedAddress";
		public virtual void CacheFormattedAddresses(string cacheString, string[] formattedAddresses) =>
			DictSet(GeocodingFormattedAddress,
				cacheString,
				formattedAddresses);
		public virtual string[] GetFormattedAddresses(string cacheString) => DictGet(GeocodingFormattedAddress, cacheString);
		public GeocoderFormattedAddressCache(IDistributedCache cache, IRestSerializer serializer)
			: base(nameof(GeocoderFormattedAddressCache),
				cache,
				serializer)
		{
		}
	}

	public class GeocodingService : IGeocodingService
	{
		private readonly ILookupManager lookupManager;
		private readonly IGeocoderCache geocoderCache;
		private readonly IGeocoderFormattedAddressCache geocoderFormattedAddressCache;
		private readonly IGeocoderCountryCache geocoderCountryCache;
		private readonly ILog logger;

		public GeocodingService(ILookupManager lookupManager, IGeocoder geocoder, IGeocoderCache geocoderCache, ILog logger, IGeocoderFormattedAddressCache geocoderFormattedAddressCache, IGeocoderCountryCache geocoderCountryCache)
		{
			this.lookupManager = lookupManager;
			this.geocoder = geocoder;
			this.geocoderCache = geocoderCache;
			this.logger = logger;
			this.geocoderFormattedAddressCache = geocoderFormattedAddressCache;
			this.geocoderCountryCache = geocoderCountryCache;
		}

		public virtual bool QuotaExceeded => geocoderCache.IsQuotaExceeded();
		public virtual bool GeocoderIsGoogle => geocoder.GetType().Name == typeof(GoogleGeocoder).Name;
		public virtual IGeocoder geocoder { get; set; }

		public virtual bool TryGeocode(IEntityWithGeocode entityWithGeocode)
		{
			if (QuotaExceeded)
			{
				return false;
			}

			var countryKey = entityWithGeocode.CountryKey ?? lookupManager.GetFavoriteKey<Country>().ToString();
			var countryCode = geocoderCountryCache.GetCountryCode(countryKey);
			if (countryCode == null)
			{
				var countryLookup = lookupManager.Get<Country>(countryKey);
				if (countryLookup != null)
				{
					countryCode = countryLookup.Iso2Code ?? countryLookup.Value;
					geocoderCountryCache.SetCountryCode(countryKey, countryCode);
				}
			}

			if (countryCode == null)
			{
				entityWithGeocode.GeocodingRetryCounter = 4;
				var countryKeyLogging = countryKey?.Replace("\n", "_").Replace("\r", "_");
				logger.WarnFormat("Missing country lookup entry for key {0}, please add accordingly", countryKeyLogging);
				return false;
			}

			try
			{
				var cacheString = $"street:{entityWithGeocode.Street},city:{entityWithGeocode.City},zip:{entityWithGeocode.ZipCode},country:{countryCode}";
				var cachedResult = geocoderCache.GetAddresses(cacheString);
				var addresses = cachedResult ?? geocoder.GeocodeAsync(entityWithGeocode.Street, entityWithGeocode.City, null, entityWithGeocode.ZipCode, countryCode).Result.Select(x => x.Coordinates).ToArray();
				if (cachedResult == null && addresses.Any())
				{
					geocoderCache.CacheAddresses(cacheString, addresses);
				}
				if (addresses.Any())
				{
					var address = addresses.First();
					entityWithGeocode.Latitude = address.Latitude;
					entityWithGeocode.Longitude = address.Longitude;
					entityWithGeocode.IsGeocoded = true;

					var addressLogging = address.ToString()?.Replace("\n", "_").Replace("\r", "_");
					var nameLogging = geocoder.GetType().Name.Replace("\n", "_").Replace("\r", "_");
					var cacheStringLogging = cacheString.Replace("\n", "_").Replace("\r", "_");
					logger.DebugFormat("Retrieved point ({0}) from {1} for address {2}", addressLogging, nameLogging, cacheStringLogging);

					return true;
				}
				else
				{
					logger.Debug("geocoding yielded zero results");
					entityWithGeocode.GeocodingRetryCounter = 4;
					return false;
				}
			}
			catch (GoogleGeocodingException ex)
			{
				switch (ex.Status)
				{
					case GoogleStatus.OverQueryLimit:
						logger.Warn("Cannot geocode on google since you have exceeded your Google API query limit.", ex);
						geocoderCache.SetQuotaExceeded();
						return false;
					case GoogleStatus.RequestDenied:
						logger.Warn("Cannot geocode on google because your request denied. Please check your Google API key.", ex);
						return false;
					case GoogleStatus.ZeroResults:
						entityWithGeocode.GeocodingRetryCounter = 4;
						logger.Debug("geocoding yielded zero results", ex);
						return false;
					default:
						entityWithGeocode.GeocodingRetryCounter += 1;
						logger.Error("There was a problem requesting the Google Maps geocoding API.", ex);
						return false;
				}
			}
			catch (Exception ex)
			{
				entityWithGeocode.GeocodingRetryCounter += 1;
				logger.Error("There was a problem during a call to the geocoding API.", ex);
				return false;
			}
		}
		public virtual bool TryReverseGeocode(double latitude, double longitude, out string formattedAddress)
		{
			if (QuotaExceeded)
			{
				formattedAddress = null;
				return false;
			}

			try
			{
				var cacheString = $"{latitude}, {longitude}";
				var cachedResult = geocoderFormattedAddressCache.GetFormattedAddresses(cacheString);
				var formattedAddresses = cachedResult ?? geocoder.ReverseGeocodeAsync(latitude, longitude).Result.Select(x => x.FormattedAddress).ToArray();

				if (!formattedAddresses.Any())
				{
					formattedAddress = null;
					logger.Debug("reverse geocoding yielded zero results");
					return false;
				}

				if (cachedResult == null)
				{
					geocoderFormattedAddressCache.CacheFormattedAddresses(cacheString, formattedAddresses);
				}

				formattedAddress = formattedAddresses.First();
				return true;
			}
			catch (GoogleGeocodingException ex)
			{
				switch (ex.Status)
				{
					case GoogleStatus.OverQueryLimit:
						formattedAddress = null;
						logger.Warn("Cannot reverse geocode on google since you have exceeded your Google API query limit.", ex);
						geocoderCache.SetQuotaExceeded();
						return false;
					case GoogleStatus.RequestDenied:
						formattedAddress = null;
						logger.Warn("Cannot reverse geocode on google because your request denied. Please check your Google API key.", ex);
						return false;
					case GoogleStatus.ZeroResults:
						formattedAddress = null;
						logger.Debug("reverse geocoding yielded zero results", ex);
						return false;
					default:
						formattedAddress = null;
						logger.Error("There was a problem requesting the Google Maps geocoding API (reverse geocoding).", ex);
						return false;
				}
			}
			catch (Exception ex)
			{
				formattedAddress = null;
				logger.Error("There was a problem during a call to the geocoding API (reverse geocoding).", ex);
				return false;
			}
		}
	}

	public interface IGeocodingService : IDependency
	{
		IGeocoder geocoder { get; set; }
		bool QuotaExceeded { get; }
		bool GeocoderIsGoogle { get; }
		bool TryGeocode(IEntityWithGeocode entityWithGeocode);
		bool TryReverseGeocode(double latitude, double longitude, out string formattedAddress);
	}
}
