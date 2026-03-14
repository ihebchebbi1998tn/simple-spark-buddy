namespace Crm.Service.Controllers
{
	using System;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Enums;
	using Crm.Service.Model;
	using Crm.Service.Services.Interfaces;
	using Crm.Service.ViewModels;

	using Main;
	using Main.Controllers;
	using Main.Services;

	using Microsoft.AspNetCore.Mvc;

	public class ServiceOrderTypeController : CrmController
	{
		[RenderAction("LookupBasicInformation", Priority = 20)]
		public virtual ActionResult LookupBasicInformation() => PartialView();

		[RenderAction("LookupEditBasicInformation", Priority = 20)]
		public virtual ActionResult LookupEditBasicInformation() => PartialView();
		public ServiceOrderTypeController(IPdfService pdfService, IRenderViewToStringService renderViewToStringService)
			: base(pdfService, renderViewToStringService)
		{
		}
	}
}
