namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Service.Model;

	[RestTypeFor(DomainType = typeof(ServiceOrderErrorType))]
	public class ServiceOrderErrorTypeRest : RestEntityWithExtensionValues
	{
		public virtual string StatisticsKeyFaultImageKey { get; set; }
		public virtual string InternalRemark { get; set; }
		public virtual string Description { get; set; }
		public virtual bool IsSuspected { get; set; }
		public virtual bool IsConfirmed { get; set; }
		public virtual bool IsMainErrorType { get; set; }
		public virtual bool IsExported { get; set; }
		public virtual Guid? OrderId { get; set; }
		public virtual Guid? DispatchId { get; set; }
		public virtual Guid? ParentServiceOrderErrorTypeId { get; set; }
		public virtual Guid? ServiceOrderTimeId { get; set; }
		public virtual Guid? ServiceCaseId { get; set; }



		[NavigationProperty(nameof(DispatchId), nameof(ServiceOrderDispatchRest.ServiceOrderErrorTypes))]
		public ServiceOrderDispatchRest ServiceOrderDispatch { get; set; }

		[NavigationProperty(nameof(ParentServiceOrderErrorTypeId), nameof(ChildServiceOrderErrorTypes))]
		public ServiceOrderErrorTypeRest ParentServiceOrderErrorType { get; set; }

		[NavigationProperty(nameof(ServiceOrderTimeId), nameof(ServiceOrderTimeRest.ServiceOrderErrorTypes))]
		public ServiceOrderTimeRest ServiceOrderTime { get; set; }

		[NavigationProperty(nameof(OrderId), nameof(ServiceOrderHeadRest.ServiceOrderErrorTypes))]
		public ServiceOrderHeadRest ServiceOrder { get; set; }

		[NavigationProperty(nameof(ServiceCaseId), nameof(ServiceCaseRest.ServiceOrderErrorTypes))]
		public ServiceCaseRest ServiceCase { get; set; }

		[NavigationProperty(nameof(ParentServiceOrderErrorTypeId), nameof(ParentServiceOrderErrorType))]
		public ServiceOrderErrorTypeRest[] ChildServiceOrderErrorTypes { get; set; }

		[NavigationProperty(nameof(ServiceOrderErrorCauseRest.ServiceOrderErrorTypeId), nameof(ServiceOrderErrorCauseRest.ServiceOrderErrorType))]
		public ServiceOrderErrorCauseRest[] ServiceOrderErrorCauses { get; set; }


	}
}
