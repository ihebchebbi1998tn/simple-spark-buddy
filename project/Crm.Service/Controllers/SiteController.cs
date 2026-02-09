namespace Crm.Service.Controllers
{
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Mvc;

	public class SiteController : Controller
	{
		[RenderAction("SiteDetailsTab")]
		public virtual ActionResult SiteDetailsTab()
		{
			return PartialView();
		}
		[RenderAction("SiteEditReport")]
		public virtual ActionResult SiteEditReport()
		{
			return PartialView();
		}
	}
}
