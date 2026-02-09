namespace Main.ActionFilters
{
	using Crm.Library.ActionFilterRegistry;
	using Crm.Library.Extensions;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc.Filters;

	public class DischargedUserAuthorizationFilter : ICrmAuthorizationFilter
	{
		public virtual void OnAuthorization(AuthorizationFilterContext filterContext)
		{
			var userService = filterContext.HttpContext.GetService<IUserService>();
			var authenticationService = filterContext.HttpContext.GetService<IAuthenticationService>();
			if (userService.CurrentUser != null && (userService.CurrentUser.Discharged || userService.CurrentUser.LicensedAt == null))
			{
				authenticationService.SignOut();
				filterContext.HttpContext.Response.Redirect("~/Main/Account/Login", false);
			}
		}
	}
}
