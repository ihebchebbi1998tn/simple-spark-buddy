namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderErrorType : EntityBase<Guid>, ISoftDelete
	{

		public virtual string StatisticsKeyFaultImageKey { get; set; }
		public virtual StatisticsKeyFaultImage StatisticsKeyFaultImage
		{
			get { return StatisticsKeyFaultImageKey != null ? LookupManager.Get<StatisticsKeyFaultImage>(StatisticsKeyFaultImageKey) : null; }
		}

		public virtual string InternalRemark { get; set; }
		public virtual string Description { get; set; }
		public virtual bool IsSuspected { get; set; }
		public virtual bool IsConfirmed { get; set; }
		public virtual bool IsMainErrorType { get; set; }
		public virtual bool IsExported { get; set; }

		public virtual Guid? OrderId { get; set; }
		public virtual ServiceOrderHead ServiceOrderHead { get; set; }

		public virtual Guid? DispatchId { get; set; }
		public virtual ServiceOrderDispatch ServiceOrderDispatch { get; set; }

		public virtual Guid? ParentServiceOrderErrorTypeId { get; set; }
		public virtual ServiceOrderErrorType ParentServiceOrderErrorType { get; set; }

		public virtual Guid? ServiceOrderTimeId { get; set; }
		public virtual ServiceOrderTime ServiceOrderTime { get; set; }

		public virtual Guid? ServiceCaseId { get; set; }
		public virtual ServiceCase ServiceCase { get; set; }

		public virtual ICollection<ServiceOrderErrorType> ChildServiceOrderErrorTypes { get; set; }
		public virtual ICollection<ServiceOrderErrorCause> ServiceOrderErrorCauses { get; set; }


		public ServiceOrderErrorType()
		{
			ChildServiceOrderErrorTypes = new List<ServiceOrderErrorType>();
			ServiceOrderErrorCauses = new List<ServiceOrderErrorCause>();
		}

	}
}
