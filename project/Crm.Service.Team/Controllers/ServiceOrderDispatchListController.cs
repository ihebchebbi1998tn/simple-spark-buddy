namespace Crm.Service.Team.Controllers
{
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class ServiceOrderDispatchListController : Controller
	{
		[RenderAction("ServiceOrderDispatchItemTemplateAttributes")]
		public virtual ActionResult ServiceOrderDispatchItemTemplateAttributes()
		{
			return PartialView();
		}
	}
}
