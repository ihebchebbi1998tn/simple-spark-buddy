namespace Crm.Service.Controllers.OData
{
	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Mapping;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.PerDiem.Controllers.OData;
	using Crm.Service.Model;
	using Crm.Service.Rest.Model;

	using Microsoft.AspNetCore.OData.Query;
	using Microsoft.AspNetCore.Mvc;

	[ControllerName("CrmService_ServiceOrderExpensePosting")]
	public class ServiceOrderExpensePostingODataController : DistinctDateODataController<ServiceOrderExpensePosting, ServiceOrderExpensePostingRest>
	{
		public ServiceOrderExpensePostingODataController(IRepository<ServiceOrderExpensePosting> repository, IODataMapper mapper)
			: base(repository, mapper)
		{
		}
		[HttpGet]
		public virtual IActionResult GetDistinctServiceOrderExpensePostingDates(ODataQueryOptions<ServiceOrderExpensePostingRest> options) => base.GetDistinctDates(options, x => x.Date);
	}
}
