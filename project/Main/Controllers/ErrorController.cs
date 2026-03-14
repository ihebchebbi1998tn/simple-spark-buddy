namespace Main.Controllers
{
	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;

	public class ErrorController : Controller
	{
		[Route("/Error/500")]
		public virtual ActionResult Index()
		{
			return View("Error/Error", ErrorViewModel.InternalServerError);
		}

		[Route("/Error/404")]
		public new virtual ActionResult NotFound()
		{
			return View("Error/Error", ErrorViewModel.NotFound);
		}

		[Route("/Error/403")]
		public virtual ActionResult Forbidden()
		{
			return View("Error/Error", ErrorViewModel.Forbidden);
		}

		public virtual ActionResult Template()
		{
			return PartialView();
		}
	}
}
