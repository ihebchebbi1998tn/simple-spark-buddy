namespace Main.Services
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Text;

	using Crm.Library.AutoFac;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Helper;
	using Crm.Library.Services.Interfaces;

	using log4net;

	using Main.Model.Lookups;

	public class LookupManager : ILookupManager
	{
		public class LogCache : HashSet<string>, ISingletonDependency { }
		private readonly ILog logger;
		private readonly LogCache logCache;

		private readonly ILookupTypeCache lookupTypeCache;
		private readonly IClientSideGlobalizationService clientSideGlobalizationService;
		private readonly ILookupService lookupService;
		private readonly Stopwatch stopwatch = new Stopwatch();

		public LookupManager(ILookupTypeCache lookupTypeCache, IClientSideGlobalizationService clientSideGlobalizationService, ILookupService lookupService, ILog logger, LogCache logCache)
		{
			this.clientSideGlobalizationService = clientSideGlobalizationService;
			this.lookupService = lookupService;
			this.lookupTypeCache = lookupTypeCache;
			this.logger = logger;
			this.logCache = logCache;
		}

		public virtual ILookupManager Instance
		{
			get { return this; }
		}
		public virtual IList<TLookup> ListAllLanguages<TLookup>()
			where TLookup : ILookup
		{
			return Interpolate(lookupService.GetLookup(typeof(TLookup)).Cast<TLookup>()).ToList();
		}

		public virtual IList<TLookup> List<TLookup>()
			where TLookup : ILookup
		{
			return List<TLookup>(GetCurrentLanguage());
		}

		public virtual IList<TLookup> List<TLookup>(bool withEmptyHead)
			where TLookup : ILookup, new()
		{
			var list = List<TLookup>();

			if (withEmptyHead)
			{
				list.Insert(0, new TLookup());
			}

			return list;
		}

		public virtual IList<TLookup> List<TLookup>(string language)
				where TLookup : ILookup
		{
			return List<TLookup>(null, language);
		}

		public virtual IList<TLookup> List<TLookup>(string language, bool withEmptyHead)
			where TLookup : ILookup, new()
		{
			var list = List<TLookup>(language);

			if (withEmptyHead)
			{
				list.Insert(0, new TLookup());
			}

			return list;
		}

		public virtual IList<TLookup> List<TLookup>(Expression<Func<TLookup, bool>> selector)
			where TLookup : ILookup
		{
			return List(selector, GetCurrentLanguage());
		}

		public virtual IList<TLookup> List<TLookup>(Expression<Func<TLookup, bool>> selector, string language)
			where TLookup : ILookup
		{
			var result = Interpolate(lookupService.GetLookup(selector)).ToList().AsEnumerable<TLookup>();

			if (language != null)
			{
				var culture = CultureInfo.GetCultureInfo(language);
				result = result.Where(l => l.Language == culture.TwoLetterISOLanguageName);
			}
			
			return result.OrderBy(x => x.SortOrder).ThenBy(x => x.Value).ThenBy(x => x.Language).ToList();
		}

		public virtual IEnumerable<ILookup> List(Type type)
		{
			return List(type, null);
		}

		public virtual IEnumerable<ILookup> List(Type type, string language)
		{
			var result = Interpolate(lookupService.GetLookup(type));

			if (language != null)
			{
				var culture = CultureInfo.GetCultureInfo(language);
				result = result.Where(l => l.Language == culture.TwoLetterISOLanguageName);
			}

			return result.OrderBy(x => x.SortOrder).ThenBy(x => x.Value).ThenBy(x => x.Language);
		}
		protected virtual IEnumerable<TLookup> Interpolate<TLookup>(IEnumerable<TLookup> lookups)
			where TLookup : ILookup
		{
			if (typeof(TLookup).HasAttribute<IgnoreMissingLookupsAttribute>())
			{
				return lookups;
			}
			var languageKeys = lookupService.GetLookupQuery<Language>()
				.Where(x => x.IsSystemLanguage)
				.Select(x => x.Key)
				.Distinct()
				.ToArray();

			var keyLanguageGroups = lookups.GroupBy(x => x.Key, e => new { e.Language, e.Value });
			var missingKeyLanguageEntries = keyLanguageGroups.Where(x => languageKeys.Any(lang => !x.Any(y => y.Language.Contains(lang))) || x.Any(z => z.Value.IsNull()));
			var result = lookups;
			foreach (var missingKeyLanguage in missingKeyLanguageEntries)
			{
				var missingLookups = languageKeys
					.Except(missingKeyLanguage.Select(x => x.Language))
					.Select(
						x =>
						{
							var existingLookup = lookups.First(first => Equals(first.Key, missingKeyLanguage.Key));
							var missingLookup = (TLookup)existingLookup.Clone();
							missingLookup.Id = missingLookup.Id.GetType().GetDefault();
							missingLookup.Language = x;
							missingLookup.Value = missingLookup.ToMissingTranslationString();
							return missingLookup;
						})
					.ToArray();
				
				foreach (var missingLookup in missingLookups)
				{
						LogMissingLookup(missingLookup);
				}

				result = result.Concat(missingLookups);
			}

			return result;
		}

		protected virtual void LogMissingLookup(ILookup missingLookup)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"Missing lookup translation:")
				.AppendLine($"Lookup type: {missingLookup.GetType().Name}")
				.AppendLine($"Language: {missingLookup.Language}")
				.Append($"Key: {missingLookup.Key}");
			var message = sb.ToString();
			if (logCache.Contains(message))
			{
				return;
			}
			logger.Warn(message);
			logCache.Add(message);
		}

	public virtual IEnumerable<TLookup> ListByKey<TLookup>(object key) where TLookup : ILookup
		{
			return ListByKey(typeof(TLookup), key).Cast<TLookup>().ToList();
		}

		public virtual IEnumerable<ILookup> ListByKey(Type type, object key)
		{
			var result = List(type)
				.Where(l => Equals(l.Key, key));
			return result;
		}

		public virtual IEnumerable<TLookup> ListByKeys<TLookup>(IEnumerable keys) where TLookup : ILookup
		{
			return ListByKeys(typeof(TLookup), keys).Cast<TLookup>();
		}

		public virtual IEnumerable<ILookup> ListByKeys(Type type, IEnumerable keys)
		{
			foreach (object key in keys)
			{
				foreach (ILookup lookup in ListByKey(type, key))
				{
					yield return lookup;
				}
			}
		}

		public virtual Dictionary<string, string> DictByKey<TLookup>(object key, IEnumerable<string> languages = null)
			where TLookup : ILookup
		{
			var result = lookupService.GetLookup<TLookup>(l => l.Key == key).ToList();

			if (languages == null)
			{
				return result.ToDictionary(r => r.Language, r => r.Value);
			}

			var dict = new Dictionary<string, string>();
			foreach (string language in languages)
			{
				var lookup = result.FirstOrDefault(r => r.Language == language);
				dict.Add(language, lookup != null ? lookup.Value : String.Empty);
			}

			return dict;
		}

		public virtual TLookup Get<TLookup>(Func<TLookup, bool> selector)
			where TLookup : ILookup
		{
			return Get(selector, GetCurrentLanguage());
		}

		public virtual TLookup Get<TLookup>(Func<TLookup, bool> selector, string twoLetterIsoLanguageName)
			where TLookup : ILookup
		{
			return List<TLookup>().First(l => selector(l) && l.Language == twoLetterIsoLanguageName);
		}

		public virtual TLookup Get<TLookup>(object key) where TLookup : ILookup
		{
			return Get<TLookup>(key, GetCurrentLanguage());
		}

		public virtual TLookup Get<TLookup>(object key, string language) where TLookup : ILookup
		{
			try
			{
				var cultureInfo = CultureInfo.GetCultureInfo(language);
				var result = lookupService.GetLookup<TLookup>(l => l.Key == key && l.Language == cultureInfo.TwoLetterISOLanguageName).FirstOrDefault();
				
				int intKey;
				if (result == null && key != null && key is string && typeof(Lookup<int>).IsAssignableFrom(typeof(TLookup)) && int.TryParse((string)key, out intKey))
				{
					result = Get<TLookup>(intKey, language);
				}

				return result;
			}
			catch (KeyNotFoundException ex)
			{
				var message = "Key {0} not found in Lookup dictionary.".WithArgs(typeof(TLookup).Name);
				throw new ApplicationException(message, ex);
			}
		}

		public virtual ILookup Get(object key, Type lookupType)
		{
			return Get(key, GetCurrentLanguage(), lookupType); // Global.Culture.TwoLetterISOLanguageName, lookupType);
		}

		public virtual ILookup Get(object key, string language, Type lookupType)
		{
			return List(lookupType).FirstOrDefault(l => Equals(l.Key, key) && Equals(l.Language, language));
		}

		public virtual TLookup GetDefault<TLookup>() where TLookup : ILookup
		{
			return (TLookup)GetDefault(typeof(TLookup));
		}

		// try to find default value by first looking for "Default" property, then for favorite, then for lowest sort order
		public virtual object GetDefault(Type lookupType)
		{
			var defaultLookupInfo = lookupType.GetField("Default", BindingFlags.Static | BindingFlags.Public);
			if (defaultLookupInfo != null)
			{
				return defaultLookupInfo.GetValue(null);
			}

			var lookupList = List(lookupType);

			var favoriteLookup = lookupList.FirstOrDefault(l => l.Favorite);
			if (favoriteLookup != null)
			{
				return favoriteLookup;
			}

			var firstLookup = lookupList.FirstOrDefault();

			return firstLookup ?? lookupType.GetDefault();
		}

		//public IEnumerable<ILookup> ListAll()
		//{
		//  foreach (var lookupType in lookups.Keys)
		//    yield return Get(lookupType, );

		//}

		public virtual TLookupKey GetFavoriteKey<TLookup, TLookupKey>()
			where TLookup : Lookup<TLookupKey>
		{
			return (TLookupKey)GetFavoriteKey(typeof(TLookup));
		}

		public virtual object GetFavoriteKey<TLookup>()
			where TLookup : ILookup
		{
			return GetFavoriteKey(typeof(TLookup));
		}

		public virtual object GetFavoriteKey(Type lookupType)
		{
			var favoriteKey = List(lookupType, Global.CurrentUICulture.TwoLetterISOLanguageName)
				.Where(l => l.Favorite)
				.Select(l => l.Key)
				.FirstOrDefault();

			return favoriteKey;
		}

		public virtual IEnumerable<Type> RegisteredTypes(bool onlyEditable = false)
		{
			var lookupTypes = lookupTypeCache.LookupTypes;

			if (onlyEditable)
			{
				lookupTypes = lookupTypes.Where(type => type.NotHasAttribute<NotEditable>()).ToArray();
			}

			foreach (Type lookupType in lookupTypes)
			{
				yield return lookupType;
			}
		}

		protected virtual string GetCurrentLanguage()
		{
			return clientSideGlobalizationService.GetCurrentLanguageCultureNameOrDefault();
		}
	}
}
