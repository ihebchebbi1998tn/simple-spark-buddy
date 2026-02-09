namespace Main.Rest.Controllers
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using Main.Model.Lookups;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	public class ResourceControllerCache : ConcurrentDictionary<string, SerializableDictionary<string, string>>, ISingletonDependency
	{
	}

	public class ResourceController : RestController
	{
		private readonly IResourceManager resourceManager;
		private readonly ILookupManager lookupManager;
		private readonly IClientSideGlobalizationService clientSideGlobalizationService;
		private readonly ResourceControllerCache resourceControllerCache;

		public ResourceController(IResourceManager resourceManager, ILookupManager lookupManager, IClientSideGlobalizationService clientSideGlobalizationService, RestTypeProvider restTypeProvider, ResourceControllerCache resourceControllerCache)
			: base(restTypeProvider)
		{
			this.resourceManager = resourceManager;
			this.lookupManager = lookupManager;
			this.clientSideGlobalizationService = clientSideGlobalizationService;
			this.resourceControllerCache = resourceControllerCache;
		}

		public virtual ActionResult Get(string id, string language)
		{
			var translation = !String.IsNullOrWhiteSpace(language) ? resourceManager.GetTranslation(id, language) : resourceManager.GetTranslation(id, clientSideGlobalizationService.GetCurrentLanguageCultureNameOrDefault());
			var result = new KeyValuePair<string, string>(id, translation);
			return Rest(result);
		}

		[AllowAnonymous]
		public virtual ActionResult GetGlobalizationData()
		{
			var result = new SerializableDictionary<string, string>();

			var languageCultureName = clientSideGlobalizationService.GetCurrentLanguageCultureNameOrDefault();
			result.Add("languageCultureName", languageCultureName);

			var cultureName = clientSideGlobalizationService.GetCurrentCultureNameOrDefault();
			result.Add("cultureName", cultureName);

			return Rest(result);
		}

		public virtual ActionResult List(string language)
		{
			var languages = !String.IsNullOrWhiteSpace(language) ? new[] { language } : lookupManager.List<Language>().Where(x => x.IsSystemLanguage).Select(x => x.Key).Distinct().ToArray();
			var result = new SerializableDictionary<string, SerializableDictionary<string, string>>();
			foreach (var languageCode in languages)
			{
				var languageContent = resourceControllerCache.GetOrAdd(
					languageCode,
					x =>
					{
						var languageContentDictionary = new SerializableDictionary<string, string>();
						foreach (var phrase in resourceManager.GetPhrases(languageCode: languageCode))
						{
							if (!languageContentDictionary.ContainsKey(phrase.Key))
							{
								languageContentDictionary.Add(phrase.Key, resourceManager.GetTranslation(phrase.Key, languageCode));
							}
						}

						return languageContentDictionary;
					});

				result.Add(languageCode, languageContent);
			}
			if (!String.IsNullOrWhiteSpace(language))
			{
				return Rest(result[language]);
			}
			return Rest(result);
		}
		[AllowAnonymous]
		public virtual ActionResult ListLocales()
		{
			return Rest(clientSideGlobalizationService.GetCultureNames());
		}
	}
}
