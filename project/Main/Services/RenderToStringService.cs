namespace Main.Services
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	using Crm.Library.ActionFilterRegistry;
	using Crm.Library.AutoFac;
	using Crm.Library.Modularization;
	using Crm.Library.Services.Interfaces;

	using JetBrains.Annotations;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Abstractions;
	using Microsoft.AspNetCore.Mvc.ModelBinding;
	using Microsoft.AspNetCore.Mvc.Rendering;
	using Microsoft.AspNetCore.Mvc.Routing;
	using Microsoft.AspNetCore.Mvc.ViewFeatures;
	using Microsoft.AspNetCore.Routing;

	public class UrlHelperWrapper : IUrlHelper
	{
		private readonly IUrlHelper urlHelper;
		private readonly LinkGenerator linkGenerator;

		public UrlHelperWrapper(IUrlHelper urlHelper, LinkGenerator linkGenerator)
		{
			this.urlHelper = urlHelper;
			this.linkGenerator = linkGenerator;
		}

		public string Action(UrlActionContext actionContext)
		{
			return linkGenerator.GetPathByAction(urlHelper.ActionContext.HttpContext, actionContext.Action, actionContext.Controller, actionContext.Values);
		}
		public string Content(string contentPath)
		{
			return urlHelper.Content(contentPath);
		}
		public bool IsLocalUrl(string url)
		{
			return urlHelper.IsLocalUrl(url);
		}
		public string RouteUrl(UrlRouteContext routeContext)
		{
			return urlHelper.RouteUrl(routeContext);
		}
		public string Link(string routeName, object values)
		{
			return urlHelper.Link(routeName, values);
		}
		public ActionContext ActionContext => urlHelper.ActionContext;
	}

	public class RenderToStringService : IRenderViewToStringService
	{
		private readonly IEnumerable<ICrmActionResultExecutor> crmActionResultExecutors;
		private readonly Func<CrmViewPage> viewPageFactory;
		private readonly ICrmViewEngine viewEngine;
		private readonly ITempDataProvider tempDataProvider;
		private readonly LinkGenerator linkGenerator;
		private readonly IUrlHelperFactory urlHelperFactory;
		private readonly IHttpContextProvider httpContextProvider;

		public virtual string RenderPartialToString(ControllerContext controllerContext, string viewName, object model)
		{
			return RenderViewToString(controllerContext.RouteData.Values["Plugin"]?.ToString(), controllerContext.RouteData.Values["Controller"]?.ToString(), viewName, model);
		}
		public virtual string RenderViewToString(string plugin, string controller, string viewName, object model)
		{
			var httpContext = httpContextProvider.GetHttpContext();
			var routeData = new RouteData();
			routeData.Values.Add("Controller", controller);
			routeData.Values.Add("Plugin", plugin);
			var actionDescriptor = new ActionDescriptor();
			var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);
			var urlHelper = urlHelperFactory.GetUrlHelper(actionContext);
			httpContext.Items[typeof(IUrlHelper)] = new UrlHelperWrapper(urlHelper, linkGenerator);

			var viewEngineResult = viewEngine.FindView(actionContext, viewName, false);
			if (viewEngineResult.View == null)
			{
				throw new InvalidOperationException(String.Format("View: {0} was not found. Searched locations: {1}", viewName, string.Join(Environment.NewLine, viewEngineResult.SearchedLocations)));
			}

			var viewPage = viewPageFactory();

			var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
			var tempData = new TempDataDictionary(httpContext, tempDataProvider);
			viewData.Model = model;

			var stringWriter = new StringWriter();

			viewPage.ViewContext = new ViewContext(actionContext, viewEngineResult.View, viewData, tempData, stringWriter, new HtmlHelperOptions());

			foreach (var actionResultExecutor in crmActionResultExecutors)
			{
				actionResultExecutor.ExecuteAsync(actionContext, viewData);
			}

			viewPage.ViewContext.View.RenderAsync(viewPage.ViewContext).Wait();
			stringWriter.Flush();

			var html = stringWriter.ToString();
			return html;
		}
		public virtual string RenderViewToString(ControllerContext controllerContext, string viewName, object model)
		{
			return RenderViewToString(controllerContext.RouteData.Values["Plugin"]?.ToString(), controllerContext.RouteData.Values["Controller"]?.ToString(), viewName, model);
		}
		public virtual string RenderViewToString(ControllerContext controllerContext, string viewName, object model, TempDataDictionary tempData)
		{
			return RenderViewToString(controllerContext.RouteData.Values["Plugin"]?.ToString(), controllerContext.RouteData.Values["Controller"]?.ToString(), viewName, model);
		}
		public virtual string RenderViewToString(ControllerContext controllerContext, string viewName, string masterName, object model, TempDataDictionary tempData)
		{
			return RenderViewToString(controllerContext.RouteData.Values["Plugin"]?.ToString(), controllerContext.RouteData.Values["Controller"]?.ToString(), viewName, model);
		}
		public RenderToStringService(ICrmViewEngine viewEngine, Func<CrmViewPage> viewPageFactory, ITempDataProvider tempDataProvider, LinkGenerator linkGenerator, IEnumerable<ICrmActionResultExecutor> crmActionResultExecutors, IUrlHelperFactory urlHelperFactory, IHttpContextProvider httpContextProvider)
		{
			this.viewEngine = viewEngine;
			this.viewPageFactory = viewPageFactory;
			this.tempDataProvider = tempDataProvider;
			this.linkGenerator = linkGenerator;
			this.crmActionResultExecutors = crmActionResultExecutors;
			this.urlHelperFactory = urlHelperFactory;
			this.httpContextProvider = httpContextProvider;
		}
	}

	public interface IRenderViewToStringService : ITransientDependency
	{
		string RenderPartialToString(ControllerContext controllerContext, [AspMvcView] string viewName, object model);
		string RenderViewToString(string plugin, string controller, string viewName, object model);
		string RenderViewToString(ControllerContext controllerContext, string viewName, object model);
		string RenderViewToString(ControllerContext controllerContext, string viewName, object model, TempDataDictionary tempData);
		string RenderViewToString(ControllerContext controllerContext, string viewName, string masterName, object model, TempDataDictionary tempData);
	}
}
