namespace Crm.Service.Team.Controllers
{
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Authorization;
	[Authorize]
	public class UserDetailsController : Controller
	{
		[RenderAction("UserDetailsServiceView", Priority = 30)]
		public virtual ActionResult UserDetailsServiceTeamView() => PartialView();

		[RenderAction("UserDetailsServiceEdit", Priority = 30)]
		public virtual ActionResult UserDetailsServiceTeamEdit() => PartialView();
	}
}
