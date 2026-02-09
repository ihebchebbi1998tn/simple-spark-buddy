namespace Crm.Service.EventHandler
{
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderReturnsToOpenStateEventHandler : IEventHandler<EntityModifiedEvent<ServiceOrderHead>>
	{
		public virtual void Handle(EntityModifiedEvent<ServiceOrderHead> e)
		{
			if ((e.EntityBeforeChange.IsReadyForInvoice() || e.EntityBeforeChange.IsInvoiced() || e.EntityBeforeChange.IsClosed()) 
			    && !(e.Entity.IsReadyForInvoice() || e.Entity.IsInvoiced() || e.Entity.IsClosed()) && !e.Entity.IsTemplate)
			{
				e.Entity.NoInvoiceReasonKey = null;
				e.Entity.InvoiceReasonKey = null;
			}
		}
	}
}
