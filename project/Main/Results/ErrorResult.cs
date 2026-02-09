namespace Main.Results
{
	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;

	public class ErrorResult : ViewResult
	{

		public ErrorResult(ErrorViewModel errorViewModel)
		{
			ViewName = "Error/Error";
			ViewData.Model = errorViewModel;
		}
	}
}
