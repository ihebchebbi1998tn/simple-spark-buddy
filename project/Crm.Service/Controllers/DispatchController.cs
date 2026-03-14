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

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Net.Http.Headers;

	public class DispatchController : CrmController
	{
		private readonly IServiceOrderService serviceOrderService;
		private readonly IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository;
		private readonly IAppSettingsProvider appSettingsProvider;

		[RenderAction("DispatchInstallationTemplate")]
		public virtual ActionResult InstallationTemplate() => PartialView();

		[RenderAction("AdHocServiceOrderCreateFormDispatchingDataBottom", Priority = 90)]
		public virtual ActionResult CreateTemplateStatus()
		{
			return PartialView();
		}
		[RequiredPermission(PermissionName.Read, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult DetailsTemplate()
		{
			return PartialView();
		}

		[RenderAction("LookupBasicInformation")]
		public virtual ActionResult LookupBasicInformation() => PartialView();

		[RenderAction("LookupEditBasicInformation")]
		public virtual ActionResult LookupEditBasicInformation() => PartialView();

		[RenderAction("DispatchDetailsMaterialTabHeader", Priority = 100)]
		public virtual ActionResult MaterialDetailsTabHeader() => PartialView();

		[RenderAction("DispatchDetailsMaterialTab", Priority = 100)]
		public virtual ActionResult MaterialDetailsTab()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTabHeader", Priority = 50)]
		[RequiredPermission(CrmPlugin.PermissionName.DocumentsTab, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult MaterialDocumentsTabHeader()
		{
			return PartialView("ContactDetails/MaterialDocumentsTabHeader");
		}

		[RenderAction("DispatchDetailsMaterialTab", Priority = 50)]
		[RequiredPermission(CrmPlugin.PermissionName.DocumentsTab, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult MaterialDocumentsTab()
		{
			return PartialView("ContactDetails/MaterialDocumentsTab");
		}

		[RenderAction("DocumentsTabPrimaryAction")]
		public virtual ActionResult MaterialDocumentsTabPrimaryAction()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTabHeader", Priority = 80)]
		public virtual ActionResult MaterialInstallationsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTab", Priority = 80)]
		public virtual ActionResult MaterialInstallationsTab()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTabHeader", Priority = 90)]
		public virtual ActionResult MaterialJobsTabHeader()
		{
			var maintenanceOrderGenerationMode = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.MaintenanceOrderGenerationMode);
			if (maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.JobPerInstallation)
			{
				return PartialView();
			}

			return new EmptyResult();
		}

		[RenderAction("DispatchDetailsMaterialTab", Priority = 90)]
		public virtual ActionResult MaterialJobsTab()
		{
			var maintenanceOrderGenerationMode = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.MaintenanceOrderGenerationMode);
			if (maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.JobPerInstallation)
			{
				return PartialView();
			}

			return new EmptyResult();
		}

		[RenderAction("DispatchDetailsMaterialTabHeader", Priority = 60)]
		public virtual ActionResult MaterialMaterialsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTab", Priority = 60)]
		public virtual ActionResult MaterialMaterialsTab()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTabHeader", Priority = 40)]
		[RequiredPermission(PermissionName.Index, Group = CrmPlugin.PermissionGroup.Note)]
		public virtual ActionResult MaterialNotesTabHeader()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTab", Priority = 40)]
		[RequiredPermission(PermissionName.Index, Group = CrmPlugin.PermissionGroup.Note)]
		public virtual ActionResult MaterialNotesTab()
		{
			return PartialView();
		}

		[RenderAction("DispatchMaterialsTabPrimaryAction")]
		public virtual ActionResult MaterialPrimaryActionAddServiceOrderMaterial()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTabHeader", Priority = 10)]
		[RequiredPermission(ServicePlugin.PermissionName.RelatedOrdersTab, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult MaterialRelatedOrdersTabHeader()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTab", Priority = 40)]
		[RequiredPermission(ServicePlugin.PermissionName.RelatedOrdersTab, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult MaterialRelatedOrdersTab()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTab", Priority = 55)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceCasesTab, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult MaterialServiceCasesTab()
		{
			return PartialView("../ServiceOrder/MaterialServiceCasesTab");
		}

		[RenderAction("DispatchDetailsMaterialTabHeader", Priority = 55)]
		[RequiredPermission(ServicePlugin.PermissionName.ServiceCasesTab, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult MaterialServiceCasesTabHeader()
		{
			return PartialView("../ServiceOrder/MaterialServiceCasesTabHeader");
		}

		[RenderAction("DispatchDetailsMaterialTabHeader", Priority = 70)]
		public virtual ActionResult MaterialTimePostingsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTab", Priority = 70)]
		public virtual ActionResult MaterialTimePostingsTab()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTabHeader", Priority = 75)]
		[RequiredPermission(ServicePlugin.PermissionName.ExpenseTab, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult MaterialExpensePostingsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTab", Priority = 75)]
		public virtual ActionResult MaterialExpensePostingsTab()
		{
			return PartialView();
		}


		[RenderAction("DispatchDetailsMaterialTabHeader", Priority = 79)]
		[RequiredPermission(ServicePlugin.PermissionName.ErrorTab, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult MaterialErrorTabHeader()
		{
			return PartialView();
		}

		[RenderAction("DispatchDetailsMaterialTab", Priority = 79)]
		public virtual ActionResult MaterialErrorTab()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Create, Group = MainPlugin.PermissionGroup.DocumentArchive)]
		public virtual ActionResult DocumentAttributeEditTemplate()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult SignatureEdit() => PartialView();

		[RequiredPermission(PermissionName.Read, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult ReportPreview() => PartialView();

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult ReportRecipientsTemplate() => PartialView();

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult ChangeStatusTemplate() => PartialView();

		[RequiredPermission(ServicePlugin.PermissionName.Cancel, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult Cancel() => PartialView();

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult RejectTemplate() => View("RejectTemplate");

		[RequiredPermission(PermissionName.Create, Group = ServicePlugin.PermissionGroup.Adhoc)]
		public virtual ActionResult AdHocTemplate() => PartialView();

		[RequiredPermission(PermissionName.Read, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult Appointment() => PartialView();

		public virtual ActionResult GetReportPdf(Guid dispatchId)
		{
			var dispatch = dispatchRepository.Get(dispatchId);
			var dispatchReport = serviceOrderService.CreateDispatchReportAsPdf(dispatchId);
			var filename = $"{dispatch.OrderHead.OrderNo} - {dispatch.Date.ToLocalTime().ToIsoDateString()} - {dispatch.DispatchedUsername}";

			return Pdf(dispatchReport, filename.RemoveIllegalCharacters());
		}

		public virtual ActionResult GetReport(Guid dispatchId, string orderNo, string format = "PDF")
		{
			var dispatch = dispatchRepository.Get(dispatchId);
			if (format.Equals("PDF", StringComparison.InvariantCultureIgnoreCase))
			{
				var name = serviceOrderService.GetReportName(dispatch);
				Response.GetTypedHeaders().ContentDisposition = new ContentDispositionHeaderValue("inline") { FileName = name };
				return Pdf(serviceOrderService.CreateDispatchReportAsPdf(dispatch));
			}

			if (format.Equals("HTML", StringComparison.InvariantCultureIgnoreCase))
			{
				var model = new DispatchReportViewModel(dispatch);
				return View("Report", model);
			}

			throw new ArgumentException(string.Format("Unknown format: {0}", format));
		}

		[RequiredPermission(PermissionName.Edit, Group = ServicePlugin.PermissionGroup.Dispatch)]
		public virtual ActionResult Schedule()
		{
			return PartialView();
		}

		[RenderAction("ServiceOrderDispatchScheduleExtension", Priority = 1)]
		public virtual ActionResult ScheduleForm()
		{
			return PartialView();
		}

		[RequiredPermission(ServicePlugin.PermissionName.AppointmentConfirmation, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("DispatchDetailsTopMenu", Priority = 90)]
		public virtual ActionResult TopMenuAppointmentConfirmation() => PartialView();

		[RequiredPermission(ServicePlugin.PermissionName.AppointmentRequest, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("DispatchDetailsTopMenu", Priority = 100)]
		public virtual ActionResult TopMenuAppointmentRequest() => PartialView();

		[RenderAction("DispatchDetailsTopMenu", Priority = 85)]
		public virtual ActionResult TemplateActionDivider()
		{
			return PartialView("ListDivider");
		}

		[RequiredPermission(ServicePlugin.PermissionName.Complete, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("DispatchDetailsTopMenu", Priority = 700)]
		public virtual ActionResult TopMenuCompleteDispatch() => PartialView();

		[RequiredPermission(ServicePlugin.PermissionName.ConfirmScheduled, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("DispatchDetailsTopMenu", Priority = 70)]
		public virtual ActionResult TopMenuConfirmAppointment() => PartialView();

		[RequiredPermission(ServicePlugin.PermissionName.RejectReleased, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("DispatchDetailsTopMenu", Priority = 90)]
		public virtual ActionResult TopMenuRejectDispatch() => PartialView();

		[RequiredPermission(ServicePlugin.PermissionName.ReportPreview, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("DispatchDetailsTopMenu", Priority = 900)]
		public virtual ActionResult TopMenuReportPreview() => PartialView();

		[RequiredPermission(ServicePlugin.PermissionName.ReportRecipients, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("DispatchDetailsTopMenu", Priority = 1000)]
		public virtual ActionResult TopMenuReportRecipients() => PartialView();

		[RequiredPermission(ServicePlugin.PermissionName.Reschedule, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("DispatchDetailsTopMenu", Priority = 60)]
		public virtual ActionResult TopMenuReschedule() => PartialView();

		[RequiredPermission(ServicePlugin.PermissionName.Signature, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("DispatchDetailsTopMenu", Priority = 800)]
		public virtual ActionResult TopMenuSignature() => PartialView();

		[RequiredPermission(ServicePlugin.PermissionName.Cancel, Group = ServicePlugin.PermissionGroup.Dispatch)]
		[RenderAction("DispatchDetailsTopMenu", Priority = 800)]
		public virtual ActionResult TopMenuCancel() => PartialView();

		[RenderAction("DispatchReportDetails", Priority = 800)]
		public virtual ActionResult DetailsTabReportDetails() => View("ServiceOrderMaterialDetails/DetailsTabReportDetails");

		[RenderAction("DispatchGeneralInformation", Priority = 800)]
		public virtual ActionResult DetailsTabGeneralInformation() => View("ServiceOrderMaterialDetails/DetailsTabGeneralInformation");

		public DispatchController(IPdfService pdfService, IRenderViewToStringService renderViewToStringService, IServiceOrderService serviceOrderService, IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository, IAppSettingsProvider appSettingsProvider)
			: base(pdfService, renderViewToStringService)
		{
			this.serviceOrderService = serviceOrderService;
			this.dispatchRepository = dispatchRepository;
			this.appSettingsProvider = appSettingsProvider;
		}
	}
}
