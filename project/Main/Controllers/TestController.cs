namespace Main.Controllers
{
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class TestController : Controller
	{
		public virtual ActionResult Index() => View();
		public virtual ActionResult Index1() => View();
		public virtual ActionResult Index2() => View();
		public virtual ActionResult Index3() => View();
		public virtual ActionResult Index4() => View();
		public virtual ActionResult Index5() => View();
		[RenderAction("TestRenderAction", Priority = 40)]
		public virtual ActionResult RenderAction1() => View();
		[RenderAction("TestRenderAction", Priority = 30)]
		public virtual ActionResult RenderAction2() => View();
		[RenderAction("TestRenderAction", Priority = 20)]
		public virtual ActionResult RenderAction3() => View();
		[RenderAction("TestRenderAction", Priority = 10)]
		public virtual ActionResult RenderAction4() => View();
		public virtual ActionResult Action1() => View();
		public virtual ActionResult Action2() => View();
		public virtual ActionResult Action3() => View();
		public virtual ActionResult Action4() => View();
	}
}
