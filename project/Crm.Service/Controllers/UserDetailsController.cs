namespace Crm.Service.Controllers
{
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class UserDetailsController : Controller
	{
		[RenderAction("UserDetailsMaterialTabHeader", Priority = 40)]
		public virtual ActionResult SkillsTabHeader() => PartialView("UserDetails/SkillsTabHeader");
		[RenderAction("UserDetailsMaterialTab", Priority = 40)]
		public virtual ActionResult SkillsTab() => PartialView("UserDetails/SkillsTab");

		public virtual ActionResult AddSkill() => PartialView("UserDetails/AddSkill");
	}
}
