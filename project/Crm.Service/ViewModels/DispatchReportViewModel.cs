namespace Crm.Service.ViewModels
{
	using Crm.Library.ViewModels;
	using Crm.Service.Model;

	public class DispatchReportViewModel : HtmlTemplateViewModel
	{
		public DispatchReportViewModel(ServiceOrderDispatch dispatch)
		{
			Id = dispatch?.Id;
			ViewModel = "Crm.Service.ViewModels.DispatchReportPreviewModalViewModel";
		}
	}
}
