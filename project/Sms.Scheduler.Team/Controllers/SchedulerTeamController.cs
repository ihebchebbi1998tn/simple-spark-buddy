namespace Sms.Scheduler.Team.Controllers
{
	using Crm.Library.Helper;
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class SchedulerTeamController : Controller
	{
		[AllowAnonymous]
		[RenderAction("MaterialHeadResource")]
		public virtual ActionResult MaterialHeadResource()
		{
			return Content(Url.JsResource("Sms.Scheduler.Team", "schedulerTeamTs"));
		}
		[RenderAction("SchedulerAddResourceFormBegin")]
		public virtual ActionResult SchedulerAddResourceFormBegin() => PartialView();
		[RenderAction("SchedulerAddResourceFormEnd")]
		public virtual ActionResult SchedulerAddResourceFormEnd() => PartialView();
		[RenderAction("SchedulerEditModalAssignResources")]
		public virtual ActionResult SchedulerEditModalAssignResources() => PartialView();
	}
}
