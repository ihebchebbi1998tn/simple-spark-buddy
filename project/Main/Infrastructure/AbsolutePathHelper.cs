namespace Main.Infrastructure
{
	using System;

	using Crm.Library.AutoFac;
	using Crm.Library.Model.Site;
	using Crm.Library.Services.Interfaces;

	using JetBrains.Annotations;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Abstractions;
	using Microsoft.AspNetCore.Mvc.Routing;
	using Microsoft.AspNetCore.Routing;

	public class AbsolutePathHelper : IAbsolutePathHelper
	{
		private readonly Site site;
		private readonly IUrlHelperFactory urlHelperFactory;
		private readonly IHttpContextProvider httpContextProvider;
		private readonly LinkGenerator linkGenerator;

		public AbsolutePathHelper(Site site, IUrlHelperFactory urlHelperFactory, IHttpContextProvider httpContextProvider, LinkGenerator linkGenerator)
		{
			this.site = site;
			this.urlHelperFactory = urlHelperFactory;
			this.httpContextProvider = httpContextProvider;
			this.linkGenerator = linkGenerator;
		}

		public virtual string GetPath(string action, string controller, RouteValueDictionary routeValues, bool isMaterialUrl = false)
		{
			var builder = new UriBuilder(site.GetExtension<DomainExtension>().HostUri);
			if (isMaterialUrl)
				builder.Path = "/Main/Home/MaterialIndex#";
			var httpContext = httpContextProvider.GetHttpContext();
			var routeData = new RouteData();
			routeData.Values.Add("Controller", controller);
			routeData.Values.Add("Plugin", routeValues?["plugin"]);
			var actionDescriptor = new ActionDescriptor();
			var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);
			var urlHelper = urlHelperFactory.GetUrlHelper(actionContext);
			var urlActionContext = new UrlActionContext { Action = action, Controller = controller, Values = routeValues };
			return linkGenerator.GetUriByAction(urlHelper.ActionContext.HttpContext, urlActionContext.Action, urlActionContext.Controller, urlActionContext.Values, builder.Scheme, new HostString(builder.Host, builder.Port), builder.Path);
		}
	}

	public interface IAbsolutePathHelper : IDependency
	{
		string GetPath([AspMvcAction] string action, [AspMvcController] string controller, RouteValueDictionary routeValues, bool isMaterialUrl = false);
	}
}
