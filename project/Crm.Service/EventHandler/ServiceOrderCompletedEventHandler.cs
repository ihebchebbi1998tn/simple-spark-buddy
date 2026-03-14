namespace Crm.Service.EventHandler
{
	using System;

	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderCompletedEventHandler : IEventHandler<EntityModifiedEvent<ServiceOrderHead>>
	{
		public virtual void Handle(EntityModifiedEvent<ServiceOrderHead> e)
		{
			if (e.Entity.Status.IsCompleted() && !e.EntityBeforeChange.Status.IsCompleted() && !e.Entity.IsTemplate)
			{
				e.Entity.Completed = DateTime.UtcNow;
			}
		}
	}
}
