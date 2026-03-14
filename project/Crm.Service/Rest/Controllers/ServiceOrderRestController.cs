namespace Crm.Service.Rest.Controllers
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Text;
	using System.Xml.Serialization;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation;
	using Crm.Service.Model;
	using Crm.Service.Model.Extensions;
	using Crm.Service.Rest.Model;
	using Crm.Service.SearchCriteria;
	using Crm.Service.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc;

	public class ServiceOrderRestController : RestController<ServiceOrderHead>
	{
		private readonly IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository;
		private readonly IDispatchRestService dispatchRestService;
		private readonly IRuleValidationService ruleValidationService;
		private readonly IMapper mapper;

		protected virtual ServiceOrderDispatch Map(ServiceOrderDispatchRest dispatchRest, ServiceOrderDispatch existingDispatch)
		{
			if (existingDispatch == null)
			{
				return mapper.Map<ServiceOrderDispatchRest, ServiceOrderDispatch>(dispatchRest);
			}

			return mapper.Map(dispatchRest, existingDispatch);
		}

		protected virtual ServiceOrderDispatchRest DeserializeXml()
		{
			var restType = typeof(ServiceOrderDispatchRest);

			using (StreamReader stream = new StreamReader(Request.Body))
			{
				string xmlAsString = stream.ReadToEndAsync().Result;
				using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(xmlAsString)))
				{
					return (ServiceOrderDispatchRest)new XmlSerializer(restType /*, xRoot*/).Deserialize(memoryStream);
				}
			}
		}

		protected virtual ActionResult ErrorResponse(params RuleViolation[] violations)
		{
			Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return Rest(violations, "RuleViolations");
		}
		public virtual ActionResult ListDispatches(string serviceOrderNo)
		{
			var dispatches = dispatchRepository.GetAll().Filter(new ServiceOrderDispatchSearchCriteria { OrderNo = serviceOrderNo });
			return Rest(dispatches, "Dispatches");
		}
		public virtual ActionResult CreateDispatch(string serviceOrderNo)
		{
			var dispatchRest = DeserializeXml();
			var dispatch = Map(dispatchRest, null);

			dispatch.IsActive = true;

			var ruleViolations = ruleValidationService.GetRuleViolations(dispatch);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			dispatchRestService.CreateDispatch(dispatch);

			return Rest(dispatch.Id);
		}
		public virtual ActionResult UpdateDispatch(string serviceOrderNo)
		{
			var dispatchRest = DeserializeXml();
			var existingDispatch = dispatchRepository.Get(dispatchRest.Id);
			if (existingDispatch == null)
			{
				return ErrorResponse(new RuleViolation("IdDoesNotExist"));
			}

			var dispatch = Map(dispatchRest, existingDispatch);

			if (dispatch.OrderHead.OrderNo != serviceOrderNo)
			{
				return Rest(new RuleViolation("ServiceOrderHasNoDispatchWithThisId"));
			}

			var ruleViolations = ruleValidationService.GetRuleViolations(dispatch);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			dispatchRestService.UpdateDispatch(dispatch);

			return new EmptyResult();
		}
		public virtual ActionResult DeleteDispatch(string serviceOrderNo, Guid dispatchId)
		{
			var dispatch = dispatchRepository.Get(dispatchId);

			if (dispatch == null)
			{
				return new EmptyResult();
			}

			if (dispatch.OrderHead.OrderNo != serviceOrderNo)
			{
				return Rest(new RuleViolation("ServiceOrderHasNoDispatchWithThisId"));
			}

			dispatchRestService.DeleteDispatch(dispatch);

			return new EmptyResult();
		}
		public ServiceOrderRestController(
			IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository,
			IDispatchRestService dispatchRestService,
			IRuleValidationService ruleValidationService,
			RestTypeProvider restTypeProvider,
			IMapper mapper)
			: base(restTypeProvider)
		{
			this.dispatchRepository = dispatchRepository;
			this.dispatchRestService = dispatchRestService;
			this.ruleValidationService = ruleValidationService;
			this.mapper = mapper;
		}
	}
}
