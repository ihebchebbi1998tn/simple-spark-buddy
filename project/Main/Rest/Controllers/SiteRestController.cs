namespace Main.Rest.Controllers
{
	using System;
	using System.Linq;

	using Crm.Library.Model.Site;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using Main.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc;

	public class SiteRestController : RestController<Site>
	{
		private readonly ISiteService siteService;
		private readonly IRuleValidationService ruleValidationService;

		// Methods
		public virtual ActionResult Get(Guid id)
		{
			var site = siteService.CurrentSite;
			return Rest(site);
		}
		
		public virtual ActionResult Create(Site site)
		{
			var ruleViolations = ruleValidationService.GetRuleViolations(site);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			siteService.SaveSite(site);
			return Rest(site.UId.ToString());
		}
		
		public virtual ActionResult Update(Site site)
		{
			var ruleViolations = ruleValidationService.GetRuleViolations(site);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			siteService.SaveSite(site);
			return new EmptyResult();
		}

		public virtual ActionResult List()
		{
			var sites = new [] { siteService.CurrentSite };
			return Rest(sites, "Sites");
		}

		// Constructor
		public SiteRestController(ISiteService siteService, IRuleValidationService ruleValidationService, RestTypeProvider restTypeProvider)
			: base(restTypeProvider)
		{
			this.siteService = siteService;
			this.ruleValidationService = ruleValidationService;
		}
	}
}
