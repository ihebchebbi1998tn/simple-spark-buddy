namespace Sms.Scheduler.Controllers.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Api;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.Api.Extensions;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Service.Model;
	using Crm.Service.Rest.Model;

	using Main.Replication.Model;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Formatter;
	using Microsoft.AspNetCore.OData.Query;

	using Sms.Scheduler.Model;

	[ControllerName("CrmService_ServiceOrderDispatch")]
	public class DispatchODataController : ODataControllerEx, IEntityApiController
	{
		private readonly IRepositoryWithTypedId<DispatchPersonAssignment, Guid> dispatchPersonAssignmentRepository;
		private readonly IRepositoryWithTypedId<ReplicatedEntityGuid, Guid> replicatedEntityGuidRepository;
		public Type EntityType => typeof(ServiceOrderDispatch);
		public DispatchODataController(IRepositoryWithTypedId<DispatchPersonAssignment, Guid> dispatchPersonAssignmentRepository, IRepositoryWithTypedId<ReplicatedEntityGuid, Guid> replicatedEntityGuidRepository)
		{
			this.dispatchPersonAssignmentRepository = dispatchPersonAssignmentRepository;
			this.replicatedEntityGuidRepository = replicatedEntityGuidRepository;
		}

		[HttpPost]
		public virtual IActionResult GetDispatchesInRange(ODataActionParameters parameters)
		{
			var technicians = parameters.GetValue<IEnumerable<string>>("technicians").ToArray();
			var startDate = parameters.GetValue<DateTimeOffset>("startDate").UtcDateTime;
			var endDate = parameters.GetValue<DateTimeOffset>("endDate").UtcDateTime;

			var query = dispatchPersonAssignmentRepository
				.GetAll()
				.Where(a => technicians.Contains(a.ResourceKey) && a.Dispatch.Date < endDate && startDate < a.Dispatch.EndDate)
				.Select(a => a.Dispatch.Id)
				.Distinct()
				.ToList();

			return Ok(query);
		}

		[HttpGet]
		public virtual IActionResult IsReplicated(ODataQueryOptions<ServiceOrderDispatchRest> options, Guid dispatchId) => Ok(replicatedEntityGuidRepository.GetAll().Any(e => e.EntityId == dispatchId));
	}
}
