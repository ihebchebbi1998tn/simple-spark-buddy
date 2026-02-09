namespace Crm.Service.Team.Controllers
{
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class ArticleDetailsController : Controller
	{
		[RenderAction("ArticleDetailsMaterialTab", Priority = 30)]
		public virtual ActionResult TeamsTab()
		{
			return PartialView();
		}

		[RenderAction("ArticleDetailsMaterialTabHeader", Priority = 30)]
		public virtual ActionResult TeamsTabHeader()
		{
			return PartialView();
		}
	}
}
