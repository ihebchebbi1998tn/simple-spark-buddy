using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sms.Scheduler.Controllers
{
	[Authorize]
	public class TemplateController : Controller
	{
		public virtual ActionResult MapComponent() => PartialView();
	}
}
