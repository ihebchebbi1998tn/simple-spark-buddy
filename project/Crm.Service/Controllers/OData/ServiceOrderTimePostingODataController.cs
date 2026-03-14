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

	[ControllerName("CrmService_ServiceOrderTimePosting")]
	public class ServiceOrderTimePostingODataController : DistinctDateODataController<ServiceOrderTimePosting, ServiceOrderTimePostingRest>
	{
		public ServiceOrderTimePostingODataController(IRepository<ServiceOrderTimePosting> repository, IODataMapper mapper)
			: base(repository, mapper)
		{
		}
		[HttpGet]
		public virtual IActionResult GetDistinctServiceOrderTimePostingDates(ODataQueryOptions<ServiceOrderTimePostingRest> options) => base.GetDistinctDates(options, x => x.From.Value);
	}
}
