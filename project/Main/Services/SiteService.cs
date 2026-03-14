namespace Main.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Globalization;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Unicore;
	using Crm.Library.Validation.Interfaces;

	using LMobile.Unicore;

	using Main.Model.Lookups;
	using Main.Services.Interfaces;

	public class SiteService : ISiteService
	{
		private static object theLock = new object();
		// Members
		private readonly ISiteHolder siteHolder;
		private readonly IEnvironment environment;
		private readonly IEnumerable<IBusinessRuleValidator> validators;
		private readonly IDomainManager domainManager;
		private readonly IMapper mapper;

		// Methods
		public virtual Site CurrentSite
		{
			get
			{
				if (siteHolder.Site != null)
				{
					return siteHolder.Site;
				}
				lock (theLock)
				{
					if (siteHolder.Site == null)
					{
						var site = GetDefaultSite();
						if (site == null)
						{
							var domain = domainManager.Create("Default");
							site = mapper.Map(domain, new Site());
							site.GetExtension<DomainExtension>().DefaultLanguageKey = Language.English;
							site.GetExtension<DomainExtension>().Host = "http://localhost/";
							if (validators.All(x => x.IsValid(site)))
							{
								SaveSite(site);
							}
						}
						siteHolder.UpdateSite(site);
					}
					return siteHolder.Site;
				}
			}
		}

		public virtual Site GetDefaultSite()
		{
			return GetSite(UnicoreDefaults.CommonDomainId);
		}
		public virtual Site GetSite(Guid id)
		{
			var domain = domainManager.Find(id);
			var site = mapper.Map(domain, new Site());
			return site;
		}

		public virtual void SaveSite(Site site, List<string> activePlugins = null)
		{
			var currentlyActivePluginNames = environment.GetActivePluginNames().ToList();
			activePlugins = activePlugins ?? currentlyActivePluginNames;
			var oldActivePlugins = currentlyActivePluginNames;

			var activatedPluginNames = activePlugins.Except(oldActivePlugins, StringComparer.InvariantCultureIgnoreCase).ToArray();
			var deactivatedPluginNames = oldActivePlugins.Except(activePlugins, StringComparer.InvariantCultureIgnoreCase).ToArray();

			var domain = domainManager.Find(site.UId);
			mapper.Map(site, domain);
			domainManager.Update(domain);
			mapper.Map(domain, site);
			siteHolder.UpdateSite(site);

			if (activatedPluginNames.Any() || deactivatedPluginNames.Any())
			{
				// No need to call RestartSite. Changing the Plugins.dat will carry out a restart through FileSystemWatchers
				environment.SaveActivePluginNames(activePlugins);
			}
		}

		// Constructor
		public SiteService(IEnvironment environment,
			ISiteHolder siteHolder,
			IEnumerable<IBusinessRuleValidator> validators,
			IDomainManager domainManager,
			IMapper mapper)
		{
			this.environment = environment;
			this.siteHolder = siteHolder;
			this.validators = validators;
			this.domainManager = domainManager;
			this.mapper = mapper;
		}
	}
}
