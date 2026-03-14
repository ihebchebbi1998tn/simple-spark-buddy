using Microsoft.AspNetCore.Mvc;

namespace Crm.Service.Controllers
{
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class TechnicianController : Controller
	{
		[RenderAction("AccountInfoTab", Priority = 40)]
		[RenderAction("UserDetailsTabExtensions", Priority = 40)]
		public virtual ActionResult UserDetailsTabExtension() => PartialView();
	}
}
