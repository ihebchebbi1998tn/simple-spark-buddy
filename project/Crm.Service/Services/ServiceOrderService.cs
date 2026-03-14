namespace Crm.Service.Services
{
	using System;
	using System.Globalization;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Helper;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Extensions;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Services.Interfaces;
	using Crm.Service.ViewModels;

	using Main.Services;
	using Main.Services.Interfaces;

	public class ServiceOrderService : IServiceOrderService
	{
		private readonly IRenderViewToStringService renderViewToStringService;
		private readonly IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository;
		private readonly IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository;
		private readonly INumberingService numberingService;
		private readonly IPdfService pdfService;
		private readonly IUserService userService;

		public virtual string GetNewOrderNo(ServiceOrderType serviceOrderType)
		{
			return numberingService.GetNextFormattedNumber(serviceOrderType.NumberingSequence);
		}

		public virtual byte[] CreateServiceOrderReportAsPdf(Guid orderId)
		{
			var order = serviceOrderRepository.Get(orderId);
			return CreateServiceOrderReportAsPdf(order);
		}
		public virtual byte[] CreateServiceOrderReportAsPdf(ServiceOrderHead order)
		{
			var model = new ServiceOrderReportViewModel(order);
			var reportHtml = renderViewToStringService.RenderViewToString("Crm.Service", "ServiceOrder", "Report", model);
			var reportPdf = pdfService.Html2Pdf(reportHtml);
			return reportPdf;
		}
		public virtual byte[] CreateDispatchReportAsPdf(Guid dispatchId)
		{
			var dispatch = dispatchRepository.Get(dispatchId);
			return CreateDispatchReportAsPdf(dispatch);
		}
		public virtual byte[] CreateDispatchReportAsPdf(ServiceOrderDispatch dispatch)
		{
			var model = new DispatchReportViewModel(dispatch);
			var reportHtml = renderViewToStringService.RenderViewToString("Crm.Service", "Dispatch", "Report", model);
			var reportPdf = pdfService.Html2Pdf(reportHtml);
			return reportPdf;
		}
		public virtual string GetReportName(ServiceOrderDispatch dispatch)
		{
			var userDisplayName = userService.GetDisplayName(dispatch.DispatchedUsername);
			var date = dispatch.Date.ToLocalTime().ToString();
			var orderNo = dispatch.OrderHead.OrderNo;
			return $"{orderNo} - {userDisplayName} - {date}";
		}
		public virtual void Save(ServiceOrderHead serviceOrderHead)
		{
			if (serviceOrderHead.OrderNo == null && serviceOrderHead.IsTransient())
			{
				serviceOrderHead.OrderNo = GetNewOrderNo(serviceOrderHead.Type);
			}
			serviceOrderRepository.SaveOrUpdate(serviceOrderHead);
		}

		// Constructor
		public ServiceOrderService(
			INumberingService numberingService,
			IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository,
			IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository,
			IRenderViewToStringService renderViewToStringService,
			IPdfService pdfService,
			IUserService userService)
		{
			this.numberingService = numberingService;
			this.serviceOrderRepository = serviceOrderRepository;
			this.dispatchRepository = dispatchRepository;
			this.renderViewToStringService = renderViewToStringService;
			this.pdfService = pdfService;
			this.userService = userService;
		}
	}
}
