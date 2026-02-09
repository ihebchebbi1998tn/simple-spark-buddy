namespace Sms.Scheduler.Model
{
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using System;
	using Crm.Service.Model;

	public class DispatchPersonAssignment : EntityBase<Guid>, ISoftDelete, INoAuthorisedObject
	{
		public virtual Guid DispatchKey { get; set; }
		public virtual ServiceOrderDispatch Dispatch { get; set; }
		public virtual string ResourceKey { get; set; }
	}
}
