namespace Main.Controllers
{
	using System.Net.Mime;

	using Crm.Library.Extensions;
	using Crm.Library.Services.Interfaces;

	using JetBrains.Annotations;

	using Main.Services;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.ViewFeatures;

	[Authorize]
	public class CrmController : Controller
	{
		private readonly IPdfService pdfService;
		protected readonly IRenderViewToStringService renderViewToStringService;

		public CrmController(IPdfService pdfService, IRenderViewToStringService renderViewToStringService)
		{
			this.pdfService = pdfService;
			this.renderViewToStringService = renderViewToStringService;
		}

		#region Pdf

		protected virtual FileResult Pdf(byte[] fileContents)
		{
			return File(fileContents, MediaTypeNames.Application.Pdf);
		}

		protected virtual FileResult Pdf(byte[] fileContents, string fileDownloadName)
		{
			return File(fileContents, MediaTypeNames.Application.Pdf, fileDownloadName.AppendIfMissing(".pdf"));
		}

		public virtual byte[] ViewAsPdf([AspMvcView] string viewName, object model, double headerMargin = 0, double footerMargin = 0, bool addPageNumbers = false)
		{
			var html = renderViewToStringService.RenderViewToString(ControllerContext, viewName, model);
			var bytes = pdfService.Html2Pdf(html, headerMargin, footerMargin);
			return bytes;
		}

		#endregion Pdf

		public virtual string RenderPartialToString([AspMvcView] string viewName, object model, bool useAbsoluteUris = false)
		{
			var result = renderViewToStringService.RenderPartialToString(ControllerContext, viewName, model);
			if (useAbsoluteUris)
			{
				result = pdfService.ReplaceRelativeWithAbsolutePaths(result);
			}
			return result;
		}
		public virtual string RenderViewToString(string viewName, object model)
		{
			return renderViewToStringService.RenderViewToString(ControllerContext, viewName, model);
		}
		public virtual string RenderViewToString(ControllerContext controllerContext, HttpContext httpContext, string viewName, object model, TempDataDictionary tempData)
		{
			return renderViewToStringService.RenderViewToString(ControllerContext, viewName, model, tempData);
		}
		public virtual string RenderViewToString(ControllerContext controllerContext, HttpContext httpContext, string viewName, string masterName, object model, TempDataDictionary tempData)
		{
			return renderViewToStringService.RenderViewToString(ControllerContext, masterName, viewName, model, tempData);
		}
	}
}
