namespace Crm.Service.Team.Controllers
{
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class UserGroupController : Controller
	{
		[RenderAction("UserGroupCreate", Priority = 100)]
		public virtual ActionResult UserGroupCreate() => PartialView();

		[RenderAction("UserGroupDetailsGeneralEdit", Priority = 100)]
		public virtual ActionResult UserGroupDetailsGeneralEdit() => PartialView();

		[RenderAction("UserGroupDetailsGeneralView", Priority = 100)]
		public virtual ActionResult UserGroupDetailsGeneralView() => PartialView();
	}
}
