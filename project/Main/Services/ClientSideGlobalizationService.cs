namespace Main.Services
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Threading;

	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Extensions;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Services.Interfaces;

	using Main.Services.Interfaces;

	using Microsoft.AspNetCore.Http;

	public class ClientSideGlobalizationService : IClientSideGlobalizationService
	{
		private const string PathToGlobalizeCultures = "~/static-dist/cldr-data/main";
		private const string PathToResources = "~/Plugins/Main/Resources";

		private readonly IEnvironment environment;
		private readonly ISiteService siteService;
		private readonly IUserService userService;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly IPluginProvider pluginProvider;

		private string customCulture;
		private string customLanguage;

		public ClientSideGlobalizationService(IEnvironment environment, ISiteService siteService, IUserService userService, IHttpContextAccessor httpContextAccessor, IPluginProvider pluginProvider)
		{
			this.environment = environment;
			this.siteService = siteService;
			this.userService = userService;
			this.httpContextAccessor = httpContextAccessor;
			this.pluginProvider = pluginProvider;
		}

		public virtual bool DoesCultureExist(string cultureName)
		{
			if (cultureName == null)
			{
				return false;
			}

			var pathWithLanguageAndCulture = environment.MapPath($"{PathToGlobalizeCultures}/{cultureName}");
			// To prevent CultureNotFoundException on Pre .Net 4.5 operating systems
			// we will check if the cultureInfo class knows about our specific culture
			// see also: https://msdn.microsoft.com/en-us/library/ky2chs3h(v=vs.110).aspx
			var cultureInfoExists = CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture => string.Equals(culture.Name, cultureName, StringComparison.CurrentCultureIgnoreCase));
			return pathWithLanguageAndCulture.Exists && cultureInfoExists;
		}
		public virtual bool DoesLanguageExist(string languageName)
		{
			if (languageName == null)
			{
				return false;
			}

			var resourceFilename = $"Resources.{languageName}.resx";
			var languageResourcePath = environment.MapPath(PathToResources);

			return languageResourcePath.Exists && languageResourcePath.GetFiles($"Resources.{languageName}.resx").Any()
				|| ResourceExistsInCustomerPlugins();

			bool ResourceExistsInCustomerPlugins()
			{
				var customerPlugins = pluginProvider.ActivePluginDescriptors.Where(p => p.IsCustomerPlugin);
				return customerPlugins.Any(p =>
				{
					var resourcesPath = Path.Combine(environment.PluginPath.FullName, p.PluginName, "Resources", resourceFilename);
					return File.Exists(resourcesPath);
				});
			}
		}
		public virtual string GetCurrentCultureNameOrDefault()
		{
			if (customCulture != null)
			{
				return customCulture;
			}

			if (userService.CurrentUser != null && DoesCultureExist(userService.CurrentUser.DefaultLocale))
			{
				return userService.CurrentUser.DefaultLocale;
			}

			var acceptLanguage = httpContextAccessor.HttpContext?.Request.GetTypedHeaders().AcceptLanguage.FirstOrDefault()?.Value.Value;
			var acceptLanguageCultureInfo = acceptLanguage != null ? new CultureInfo(acceptLanguage) : null;

			if (acceptLanguageCultureInfo != null && DoesCultureExist(acceptLanguageCultureInfo.Name))
			{
				return acceptLanguageCultureInfo.Name;
			}

			if (acceptLanguageCultureInfo != null && DoesCultureExist(acceptLanguageCultureInfo.TwoLetterISOLanguageName))
			{
				return acceptLanguageCultureInfo.TwoLetterISOLanguageName;
			}

			var site = siteService.CurrentSite;
			if (site != null && DoesCultureExist(site.GetExtension<DomainExtension>().DefaultLocale))
			{
				return site.GetExtension<DomainExtension>().DefaultLocale;
			}

			var language = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
			if (DoesCultureExist(language))
			{
				return language;
			}

			return "en";
		}
		public virtual string GetCurrentCultureOrDefaultResourcePath()
		{
			var cultureName = GetCurrentCultureNameOrDefault();
			var resourcePath = $"{PathToGlobalizeCultures}/{cultureName}";
			return resourcePath;
		}
		public virtual string GetCurrentLanguageCultureNameOrDefault()
		{
			if (customLanguage != null)
			{
				return customLanguage;
			}

			if (userService.CurrentUser != null && DoesLanguageExist(userService.CurrentUser.DefaultLanguageKey))
			{
				return userService.CurrentUser.DefaultLanguageKey;
			}

			var site = siteService.CurrentSite;
			if (site != null && DoesLanguageExist(site.GetExtension<DomainExtension>().DefaultLanguageKey))
			{
				return site.GetExtension<DomainExtension>().DefaultLanguageKey;
			}

			var language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
			if (DoesLanguageExist(language))
			{
				return language;
			}

			return "en";
		}
		public virtual IEnumerable<string> GetCultureNames()
		{
			return environment.MapPath(PathToGlobalizeCultures).GetDirectories().Select(x => x.Name).Where(x => !x.Equals("root"));
		}
		public virtual void SetCurrentCultureName(string cultureName)
		{
			if (DoesCultureExist(cultureName))
			{
				customCulture = cultureName;
			}
			else
			{
				throw new FileNotFoundException($"Cannot set culture to {cultureName} - path does not exist");
			}
		}
		public virtual void SetCurrentLanguageName(string languageName)
		{
			if (DoesLanguageExist(languageName))
			{
				customLanguage = languageName;
			}
			else
			{
				throw new FileNotFoundException($"Cannot set language to {languageName} - path does not exist");
			}
		}
	}
}
