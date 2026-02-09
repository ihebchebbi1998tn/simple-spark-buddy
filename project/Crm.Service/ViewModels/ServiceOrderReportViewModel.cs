namespace Crm.Service.ViewModels
{
	using Crm.Library.ViewModels;
	using Crm.Service.Model;

	public class ServiceOrderReportViewModel : HtmlTemplateViewModel
	{
		public ServiceOrderReportViewModel(ServiceOrderHead serviceOrder)
		{
			Id = serviceOrder?.Id;
			ViewModel = "Crm.Service.ViewModels.ServiceOrderReportViewModel";
		}
	}
}
