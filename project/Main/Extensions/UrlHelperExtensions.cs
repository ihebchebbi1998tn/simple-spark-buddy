namespace Main.Extensions
{
	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Modularization.Menu;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	public static class HttpContextExtensions
{
		public static string GetCurrentPluginName(this HttpContext httpContext)
		{
			var routeValues = httpContext.Request.RouteValues;
			var pluginName = routeValues.ContainsKey("plugin") ? (string)routeValues["plugin"] : "main";
			return pluginName;
		}
}

	public static class UrlHelperExtensions
	{
		public static string Home(this IUrlHelper urlHelper)
		{
			return urlHelper.RouteUrl("Home");
		}
		public static string MenuEntry(this IUrlHelper urlHelper, MenuEntry menuEntry, User user)
		{
			if (menuEntry.Url.IsNotNullOrEmpty())
			{
				return urlHelper.Content(menuEntry.Url);
			}

			if (menuEntry.RouteName.IsNotNullOrEmpty())
			{
				return urlHelper.RouteUrl(menuEntry.RouteName, new { plugin = menuEntry.Plugin });
			}

			return "#";
		}
	}
}
