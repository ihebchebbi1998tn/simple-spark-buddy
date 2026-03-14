namespace Main.ViewModels
{
	using Microsoft.AspNetCore.Routing;

	public class AuthorizationErrorViewModel : ErrorViewModel
	{
		public RouteValueDictionary NewLogOnData { get; set; }
	}
}
