namespace Sms.Scheduler.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Service.Model;

	public class ServiceOrderDispatchExtension : EntityExtension<ServiceOrderDispatch>
	{
		public virtual Guid? SchedulingGroupId { get; set; }
	}
}
