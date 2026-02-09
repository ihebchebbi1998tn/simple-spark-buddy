namespace Crm.Service.Controllers
{
	using System;

	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Services.Interfaces;

	using Main.Controllers;
	using Main.Services;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	public class ReplenishmentOrderController : CrmController
	{
		private readonly IReplenishmentOrderService replenishmentOrderService;
		private readonly IResourceManager resourceManager;

		public ReplenishmentOrderController(IPdfService pdfService, IRenderViewToStringService renderViewToStringService, IResourceManager resourceManager, IReplenishmentOrderService replenishmentOrderService)
			: base(pdfService, renderViewToStringService)
		{
			this.resourceManager = resourceManager;
			this.replenishmentOrderService = replenishmentOrderService;
		}
		public virtual ActionResult CreatePdf(Guid id)
		{
			var bytes = replenishmentOrderService.CreateReportAsPdf(id);
			var filename = "{0}_{1}".WithArgs(resourceManager.GetTranslation("ReplenishmentOrder"), DateTime.Now.ToString("yyyy-MM-dd"));
			return Pdf(bytes, filename);
		}

		[RequiredPermission(PermissionName.Index, Group = ServicePlugin.PermissionGroup.ReplenishmentOrder)]
		public virtual ActionResult ReportPreview()
		{
			return PartialView();
		}

		[AllowAnonymous]
		[RenderAction("ReplenishmentOrderReportResource", Priority = 100)]
		public virtual ActionResult ReportResources()
		{
			return PartialView();
		}
	}
}
