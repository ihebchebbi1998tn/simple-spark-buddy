namespace Main.Rest.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Mime;

	using Crm.Library.Api.Model;
	using Crm.Library.Extensions;
	using Crm.Library.Model.Site;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.OData.Edm;
	using Microsoft.OpenApi;
	using Microsoft.OpenApi.Extensions;

	using Microsoft.OpenApi.Models;
	using Microsoft.OpenApi.OData;
	using Microsoft.OpenApi.OData.Edm;

	using Newtonsoft.Json;

	using Microsoft.OpenApi.Any;

	public class ModelController : RestController
	{
		private readonly IModelProvider modelProvider;
		private readonly IUserService userService;
		private readonly ODataConventionModelBuilder oDataConventionModelBuilder;
		private readonly Site site;

		public ModelController(IModelProvider modelProvider, IUserService userService, RestTypeProvider restTypeProvider, ODataConventionModelBuilder oDataConventionModelBuilder, Site site)
			: base(restTypeProvider)
		{
			this.modelProvider = modelProvider;
			this.userService = userService;
			this.oDataConventionModelBuilder = oDataConventionModelBuilder;
			this.site = site;
		}

		public virtual ActionResult GetRules()
		{
			var rules = modelProvider.GetRulesForUser(userService.CurrentUser);

			return new ContentResult
			{
				Content = JsonConvert.SerializeObject(rules),
				ContentType = "text/json"
			};
		}

		[HttpGet("/api/swagger/swagger.custom.css")]
		[AllowAnonymous]
		public virtual IActionResult GetSwaggerCss()
		{
			return Content("div.model-example li.tabitem:first-child { display: none; }", "text/css");
		}
		[HttpGet("/api/swagger/swagger.json")]
		[AllowAnonymous]
		public virtual IActionResult GetSwaggerModel()
		{
			var siteHost = site.GetExtension<DomainExtension>().HostUri;
			var settings = new OpenApiConvertSettings
			{
				ServiceRoot = new Uri(siteHost.ToString().RemoveSuffix("/") + "/api"),
				TopExample = 5,
				PathProvider = new CustomODataPathProvider(),
				// ShowSchemaExamples = false
			};
			var model = oDataConventionModelBuilder.GetEdmModel().ConvertToOpenApi(settings);
			foreach (var example in model.Components.Examples)
			{
				if (example.Value.Value is OpenApiObject type == false)
				{
					continue;
				}
				foreach (var (propertyName, propertyValue) in type)
				{
					//single type reference
					if (propertyValue is OpenApiObject { Count: 1 } propObj && propObj.First().Key == "@odata.type")
					{
						type[propertyName] = new OpenApiString((propObj.First().Value as OpenApiString)?.Value ?? propertyName);
					}
					//array type reference
					if (propertyValue is OpenApiArray { Count: 1 } propArray && propArray.First() is OpenApiObject { Count: 1 } propArrayObj && propArrayObj.First().Key == "@odata.type")
					{
						propArray[0] = new OpenApiString((propArrayObj.First().Value as OpenApiString)?.Value ?? propertyName);
					}
				}
			}
			foreach (var path in model.Paths)
			{
				if (path.Value.Operations.TryGetValue(OperationType.Patch, out var patch))
				{
					path.Value.Operations.Add(OperationType.Put, patch);
					path.Value.Operations.Remove(OperationType.Patch);
				}
				foreach (var operation in path.Value.Operations)
				{
					operation.Value.Responses.Remove("default");
					foreach (var parameter in operation.Value.Parameters.Where(x => x.Name == "$search" || x.Reference?.Id == "search").ToList())
					{
						operation.Value.Parameters.Remove(parameter);
					}
					foreach (var tag in operation.Value.Tags)
					{
						var name = tag.Name.Split('.');
						if (name.Length > 1 && name[0] == name[1])
						{
							tag.Name = tag.Name[(name[0].Length + 1)..];
						}
					}
				}
			}
			model.Info = new()
			{
				Version = GetType().Assembly.GetName().Version.ToString(2),
				Title = "L-mobile API Explorer",
			};
			return Content(model.SerializeAsJson(OpenApiSpecVersion.OpenApi3_0), MediaTypeNames.Application.Json);
		}
		public class CustomODataPathProvider : ODataPathProvider
		{
			public override IEnumerable<ODataPath> GetPaths(IEdmModel model, OpenApiConvertSettings settings)
			{
				foreach (var path in base.GetPaths(model, settings))
				{
					if (path.Kind == ODataPathKind.Ref || path.Kind == ODataPathKind.NavigationProperty)
					{
						continue;
					}
					yield return path;
				}
			}
		}
	}
}
