using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Main.Flow.Controllers
{
	[Authorize]
	public class FlowRuleController : Controller
	{
		public FlowRuleController()
		{
		}
		public virtual ActionResult EditTemplate() => PartialView();
	}
}
