namespace Crm.Service.Controllers
{
	using System;
	using System.Globalization;
	using System.Linq;

	using Crm.Article.Model.Enums;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Services.Interfaces;
	using Crm.Service.ViewModels;

	using Main;
	using Main.Controllers;
	using Main.Services;

	using Microsoft.AspNetCore.Mvc;

	public class ServiceOrderController : CrmController
	{
		private readonly IServiceOrderService serviceOrderService;
		private readonly IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository;
		private readonly IRepositoryWithTypedId<ServiceOrderMaterial, Guid> serviceOrderMaterialRepository;
		private readonly IClientSideGlobalizationService clientSideGlobalizationService;

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult CloseTemplate() => View();

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult CreateInvoice() => PartialView();

		[RenderAction("ServiceOrderCreateInvoice", Priority = 100)]
		public virtual ActionResult CreateInvoiceMaterialsAndTimes() => PartialView();

		[RenderAction("ServiceOrderCreateInvoice", Priority = 50)]
		public virtual ActionResult CreateInvoiceStatisticsKeys() => PartialView();

		[RenderAction("ServiceOrderCreateInvoice", Priority = 40)]
		public virtual ActionResult CreateInvoiceErrors() => PartialView();

		[RequiredPermission(PermissionName.Read, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult DetailsTemplate() => View();

		[RenderAction("MaterialServiceOrderHeaderExtensions", Priority = 50)]
		public virtual ActionResult DropboxBlock() => PartialView("ContactDetailsDropboxBlock", typeof(ServiceOrderHead));

		[RenderAction("LookupBasicInformation", Priority = 30)]
		public virtual ActionResult LookupBasicInformation() => PartialView();

		[RenderAction("LookupEditBasicInformation", Priority = 30)]
		public virtual ActionResult LookupEditBasicInformation() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTabHeader", Priority = 100)]
		public virtual ActionResult MaterialDetailsTabHeader() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTab", Priority = 100)]
		public virtual ActionResult MaterialDetailsTab() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTab", Priority = 95)]
		[RequiredPermission(ServicePlugin.PermissionName.DispatchesTab, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult MaterialDispatchesTab() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTabHeader", Priority = 95)]
		[RequiredPermission(ServicePlugin.PermissionName.DispatchesTab, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult MaterialDispatchesTabHeader() => PartialView();

		[RenderAction("ServiceOrderDispatchesTabPrimaryAction")]
		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult MaterialDispatchesTabPrimaryAction() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTabHeader", Priority = 90)]
		public virtual ActionResult MaterialJobsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("ServiceOrderDetailsMaterialTab", Priority = 90)]
		public virtual ActionResult MaterialJobsTab()
		{
			return PartialView("../Dispatch/MaterialJobsTab");
		}

		[RenderAction("ServiceOrderDetailsMaterialTabHeader", Priority = 80)]
		public virtual ActionResult MaterialInstallationsTabHeader() => PartialView("../Dispatch/MaterialInstallationsTabHeader");

		[RenderAction("ServiceOrderDetailsMaterialTab", Priority = 80)]
		public virtual ActionResult MaterialInstallationsTab() => PartialView("../Dispatch/MaterialInstallationsTab");

		[RenderAction("ServiceOrderDetailsMaterialTabHeader", Priority = 69)]
		[RenderAction("ServiceOrderTemplateDetailsTabHeader", Priority = 69)]
		public virtual ActionResult MaterialMaterialsTabHeader() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTab", Priority = 69)]
		[RenderAction("ServiceOrderTemplateDetailsTab", Priority = 69)]
		public virtual ActionResult MaterialMaterialsTab() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTabHeader", Priority = 50)]
		[RenderAction("ServiceOrderTemplateDetailsTabHeader", Priority = 50)]
		public virtual ActionResult MaterialDocumentsTabHeader() => PartialView("ContactDetails/MaterialDocumentsTabHeader");

		[RenderAction("ServiceOrderDetailsMaterialTab", Priority = 50)]
		[RenderAction("ServiceOrderTemplateDetailsTab", Priority = 50)]
		public virtual ActionResult MaterialDocumentsTab() => PartialView("ContactDetails/MaterialDocumentsTab");

		[RenderAction("ServiceOrderDetailsMaterialTabHeader", Priority = 55)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceCasesTab, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult MaterialServiceCasesTabHeader() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTab", Priority = 55)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceCasesTab, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult MaterialServiceCasesTab() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTabHeader", Priority = 40)]
		[RenderAction("ServiceOrderTemplateDetailsTabHeader", Priority = 40)]
		[RequiredPermission(PermissionName.Index, Group = CrmPlugin.PermissionGroup.Note)]
		public virtual ActionResult MaterialNotesTabHeader() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTab", Priority = 40)]
		[RenderAction("ServiceOrderTemplateDetailsTab", Priority = 40)]
		[RequiredPermission(PermissionName.Index, Group = CrmPlugin.PermissionGroup.Note)]
		public virtual ActionResult MaterialNotesTab() => PartialView();

		[RenderAction("ServiceOrderMaterialsTabPrimaryAction")]
		public virtual ActionResult MaterialPrimaryActionAddServiceOrderMaterial() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTabHeader", Priority = 70)]
		[RenderAction("ServiceOrderTemplateDetailsTabHeader", Priority = 70)]
		public virtual ActionResult MaterialTimePostingsTabHeader() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTab", Priority = 70)]
		[RenderAction("ServiceOrderTemplateDetailsTab", Priority = 70)]
		public virtual ActionResult MaterialTimePostingsTab() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTabHeader", Priority = 65)]
		public virtual ActionResult MaterialExpensePostingsTabHeader() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTab", Priority = 65)]
		public virtual ActionResult MaterialExpensePostingsTab() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTabHeader", Priority = 68)]
		public virtual ActionResult MaterialErrorTabHeader() => PartialView();

		[RenderAction("ServiceOrderDetailsMaterialTab", Priority = 68)]
		public virtual ActionResult MaterialErrorTab() => PartialView();

		[RequiredPermission(ServicePlugin.PermissionName.NoInvoice, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult NoInvoiceTemplate() => PartialView();

		[RenderAction("ServiceOrderDetailsTopMenu", Priority = 80)]
		[RequiredPermission(MainPlugin.PermissionName.DownloadAsPdf, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult TopMenuDownloadReport() => PartialView();

		[RequiredPermission(ServicePlugin.PermissionName.NoInvoice, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		[RenderAction("ServiceOrderDetailsTopMenu", Priority = 70)]
		public virtual ActionResult TopMenuNoInvoice() => PartialView();

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("ServiceOrderDetailsTopMenu", Priority = 60)]
		public virtual ActionResult TopMenuSchedule() => PartialView();

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		[RenderAction("ServiceOrderDetailsTopMenu", Priority = 50)]
		public virtual ActionResult TopMenuClose() => PartialView();

		[RenderAction("ServiceOrderDetailsTopMenu")]
		public virtual ActionResult EditVisibilityTopMenu()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.ServiceOrder)]
		public virtual ActionResult CreateTemplate()
		{
			return PartialView();
		}

		[RenderAction("ServiceOrderCreateForm", Priority = 500)]
		public virtual ActionResult CreateTemplateGeneral()
		{
			return PartialView();
		}

		[RenderAction("ServiceOrderCreateForm", Priority = 400)]
		public virtual ActionResult CreateTemplateDispatchLocation()
		{
			return PartialView();
		}

		[RenderAction("ServiceOrderCreateForm", Priority = 300)]
		public virtual ActionResult CreateTemplateSchedulingInformation()
		{
			return PartialView();
		}

		[RenderAction("ServiceOrderCreateForm", Priority = 200)]
		public virtual ActionResult CreateTemplateVisibility()
		{
			return PartialView();
		}

		[RenderAction("DocumentAttributeItemExtensions")]
		public virtual ActionResult DispatchLinkDocumentAttribute() => PartialView();

		[RenderAction("MaterialNoteItemBodyExtensions")]
		public virtual ActionResult DispatchLinkNote() => PartialView();

		[HttpPost]
		public virtual ActionResult GetOrderMaterialTotalPrice(Guid id)
		{
			var materialList = serviceOrderMaterialRepository.GetAll().Where(x => x.OrderId == id && x.ServiceOrderMaterialType == "Invoice").ToList();

			var sum = materialList.Sum(material => material.Quantity * (material.Price.ValueOrDefault() - (material.DiscountType == DiscountType.Absolute ? material.Discount : 0)) * (1 - (material.DiscountType == DiscountType.Percentage ? material.Discount / 100 : 0)));
			var cultureInfo = CultureInfo.GetCultureInfo(clientSideGlobalizationService.GetCurrentCultureNameOrDefault());
			return Json(new
			{
				MaterialCount = materialList.Count(),
				TotalPrice = sum.ToString("n" + cultureInfo.NumberFormat.CurrencyDecimalDigits, cultureInfo.NumberFormat)
			});
		}

		public virtual ActionResult DownloadAsPdf(Guid orderId)
		{
			var bytes = serviceOrderService.CreateServiceOrderReportAsPdf(orderId);
			return Pdf(bytes);
		}

		public virtual ActionResult GetReport(Guid id, string format = "PDF")
		{
			var serviceOrder = serviceOrderRepository.Get(id);
			if (format.Equals("PDF", StringComparison.InvariantCultureIgnoreCase))
			{
				return Pdf(serviceOrderService.CreateServiceOrderReportAsPdf(serviceOrder));
			}

			if (format.Equals("HTML", StringComparison.InvariantCultureIgnoreCase))
			{
				var model = new ServiceOrderReportViewModel(serviceOrder);
				return View("Report", model);
			}

			throw new ArgumentException(string.Format("Unknown format: {0}", format));
		}

		public ServiceOrderController(
			IServiceOrderService serviceOrderService,
			IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository,
			IRepositoryWithTypedId<ServiceOrderMaterial, Guid> serviceOrderMaterialRepository,
			IPdfService pdfService,
			IRenderViewToStringService renderViewToStringService,
			IAppSettingsProvider appSettingsProvider,
			IClientSideGlobalizationService clientSideGlobalizationService)
			: base(pdfService, renderViewToStringService)
		{
			this.serviceOrderService = serviceOrderService;
			this.serviceOrderRepository = serviceOrderRepository;
			this.serviceOrderMaterialRepository = serviceOrderMaterialRepository;
			this.clientSideGlobalizationService = clientSideGlobalizationService;
		}
	}
}
