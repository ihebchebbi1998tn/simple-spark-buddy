using Crm.Library.Helper;
using Crm.Library.Modularization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Main.Flow.Controllers
{
	[Authorize]
	public class FlowController : Controller
	{
		public FlowController()
		{
		}

		[AllowAnonymous]
		[RenderAction("MaterialHeadResource")]
		public virtual ActionResult CustomerJsMaterialHeadResource() => Content(Url.JsResource("Main.Flow", "flowTs"));
	}
}
