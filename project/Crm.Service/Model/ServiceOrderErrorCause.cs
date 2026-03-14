namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderErrorCause : EntityBase<Guid>, ISoftDelete
	{

		public virtual string StatisticsKeyCauseKey { get; set; }
		public virtual StatisticsKeyCause StatisticsKeyCause
		{
			get { return StatisticsKeyCauseKey != null ? LookupManager.Get<StatisticsKeyCause>(StatisticsKeyCauseKey) : null; }
		}

		public virtual string InternalRemark { get; set; }
		public virtual string Description { get; set; }
		public virtual bool IsSuspected { get; set; }
		public virtual bool IsConfirmed { get; set; }
		public virtual bool IsExported { get; set; }

		public virtual Guid? DispatchId { get; set; }
		public virtual ServiceOrderDispatch ServiceOrderDispatch { get; set; }
		public virtual Guid? ParentServiceOrderErrorCauseId { get; set; }
		public virtual ServiceOrderErrorCause ParentServiceOrderErrorCause { get; set; }
		public virtual Guid? ServiceOrderErrorTypeId { get; set; }
		public virtual ServiceOrderErrorType ServiceOrderErrorType { get; set; }

		public virtual ICollection<ServiceOrderErrorCause> ChildServiceOrderErrorCauses { get; set; } = new List<ServiceOrderErrorCause>();
	}
}
