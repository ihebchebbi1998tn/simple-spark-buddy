namespace Crm.Service.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class ServiceOrderTimeDispatch : EntityBase<Guid>, ISoftDelete, INoAuthorisedObject
	{
		public virtual ServiceOrderDispatch ServiceOrderDispatch { get; set; }
		public virtual Guid ServiceOrderDispatchId { get; set; }
		public virtual ServiceOrderTime ServiceOrderTime { get; set; }
		public virtual Guid ServiceOrderTimeId { get; set; }
	}
}
