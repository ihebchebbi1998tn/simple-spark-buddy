namespace Crm.Service.EventHandler
{
	public class ReplenishmentOrderSender //: IEventHandler<EntityModifiedEvent<ReplenishmentOrder>>
		{
		//private readonly IReplenishmentOrderService replenishmentOrderService;

		//public ReplenishmentOrderSender(IReplenishmentOrderService replenishmentOrderService)
		//{
		//	this.replenishmentOrderService = replenishmentOrderService;
		//}

		//public virtual void Handle(EntityModifiedEvent<ReplenishmentOrder> e)
		//{
		//	if (!e.EntityBeforeChange.IsClosed && e.Entity.IsClosed)
		//	{
		//		e.Entity.CloseDate = DateTime.Now;
		//		replenishmentOrderService.SendOrderAsPdf(e.Entity);
		//	}
		//}
	}
}
