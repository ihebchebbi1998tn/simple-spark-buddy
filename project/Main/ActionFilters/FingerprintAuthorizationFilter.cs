namespace Main.ActionFilters
{
	using System;
	using System.Linq;

	using Crm.Library.ActionFilterRegistry;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Extensions.IIdentity;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc.Filters;

	using NHibernate;
	using NHibernate.Linq;

	public class FingerprintAuthorizationFilter : ICrmAuthorizationFilter
	{
		public virtual void OnAuthorization(AuthorizationFilterContext filterContext)
		{
			if (filterContext.HttpContext.User.Identity?.IsAuthenticated == false)
			{
				return;
			}

			var authenticationService = filterContext.HttpContext.GetService<IAuthenticationService>();
			var fingerprintClaim = filterContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Fingerprint");
			if (fingerprintClaim == null)
			{
				return;
			}

			var fingerprint = fingerprintClaim.Value;
			var username = filterContext.HttpContext.User.Identity.GetUserName();
			var deviceRepository = filterContext.HttpContext.GetService<IRepositoryWithTypedId<Device, Guid>>();
			var isValid = deviceRepository.GetAll().WithOptions(
				o =>
				{
					o.SetCacheable(true);
					o.SetCacheMode(CacheMode.Normal);
					o.SetReadOnly(true);
				}).Any(x => x.Username == username && x.Fingerprint == fingerprint);
			if (!isValid)
			{
				authenticationService.SignOut();
				filterContext.HttpContext.Response.Redirect("/Main/Account/Login", false);
			}
		}
	}
}
