namespace Crm.Service.Team.Controllers
{
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	using UrlHelperExtensions = Crm.Library.Helper.UrlHelperExtensions;

	[Authorize]
	public class ServiceTeamController : Controller
	{
		[AllowAnonymous]
		[RenderAction("MaterialHeadResource")]
		public virtual ActionResult MaterialHeadResource()
		{
			return Content(UrlHelperExtensions.JsResource(Url,
				(string)"Crm.Service.Team",
				(string)"serviceTeamMaterialTs"));
		}
	}
}
