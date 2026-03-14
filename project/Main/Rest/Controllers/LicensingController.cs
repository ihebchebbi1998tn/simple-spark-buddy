namespace Main.Rest.Controllers
{
	using Crm.Library.Licensing;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;

	using Microsoft.AspNetCore.Mvc;

	public class LicensingController : RestController
	{
		private readonly IEnvironment environment;
		private readonly ILicensingService licensingService;

		public LicensingController(RestTypeProvider restTypeProvider, IEnvironment environment, ILicensingService licensingService)
			: base(restTypeProvider)
		{
			this.environment = environment;
			this.licensingService = licensingService;
		}

		public virtual ActionResult Get()
		{
			var domainId = environment.GetDomainId().ToString();
			return Rest(new
			{
				DomainId = domainId,
				Expires = licensingService.GetExpireDate(domainId),
				UserLicenses = licensingService.GetUserLicenseCount(domainId)
			});
		}
	}
}
