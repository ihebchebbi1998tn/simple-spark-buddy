namespace Crm.Service.Team.Controllers
{
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class ServiceOrderDispatchController : Controller
	{
		[RenderAction("MaterialDispatchHeaderExtensions")]
		public virtual ActionResult MaterialDispatchHeaderExtensions()
		{
			return PartialView();
		}

		[RenderAction("ServiceOrderDispatchScheduleExtension", Priority = 10)]
		public virtual ActionResult ServiceOrderDispatchScheduleExtension()
		{
			return PartialView();
		}
	}
}
