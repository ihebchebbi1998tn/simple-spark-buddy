namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Service.Model;

	[RestTypeFor(DomainType = typeof(ServiceOrderErrorCause))]
	public class ServiceOrderErrorCauseRest : RestEntityWithExtensionValues
	{
		public virtual string StatisticsKeyCauseKey { get; set; }
		public virtual string InternalRemark { get; set; }
		public virtual string Description { get; set; }
		public virtual bool IsSuspected { get; set; }
		public virtual bool IsConfirmed { get; set; }
		public virtual bool IsExported { get; set; }
		public virtual Guid? DispatchId { get; set; }
		public virtual Guid? ParentServiceOrderErrorCauseId { get; set; }
		public virtual Guid? ServiceOrderErrorTypeId { get; set; }

		[NavigationProperty(nameof(ParentServiceOrderErrorCauseId), nameof(ChildServiceOrderErrorCauses))]
		public ServiceOrderErrorCauseRest ParentServiceOrderErrorCause { get; set; }
		[NavigationProperty(nameof(DispatchId))]
		public ServiceOrderDispatchRest ServiceOrderDispatch { get; set; }

		[NavigationProperty(nameof(ServiceOrderErrorTypeId), nameof(ServiceOrderErrorTypeRest.ServiceOrderErrorCauses))]
		public ServiceOrderErrorTypeRest ServiceOrderErrorType { get; set; }

		[NavigationProperty(nameof(ParentServiceOrderErrorCauseId), InverseProperty = nameof(ParentServiceOrderErrorCause))]
		public ServiceOrderErrorCauseRest[] ChildServiceOrderErrorCauses { get; set; }
	}
}
