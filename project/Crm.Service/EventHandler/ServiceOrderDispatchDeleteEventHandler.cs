namespace Crm.Service.EventHandler
{
	using log4net;

	using Library.Data.Domain.DataInterfaces;
	using Library.Modularization.Events;
	using Model;
	using Model.Extensions;
	using Model.Helpers;

	public class ServiceOrderDispatchDeletedEventHandler : IEventHandler<EntityDeletedEvent<ServiceOrderDispatch>>
	{
		private readonly IRepository<ServiceOrderHead> serviceOrderRepository;
		private readonly IServiceOrderStatusEvaluator serviceOrderStatusEvaluator;
		private readonly ILog logger;

		public ServiceOrderDispatchDeletedEventHandler(IRepository<ServiceOrderHead> serviceOrderRepository, IServiceOrderStatusEvaluator serviceOrderStatusEvaluator, ILog logger)
		{
			this.serviceOrderRepository = serviceOrderRepository;
			this.serviceOrderStatusEvaluator = serviceOrderStatusEvaluator;
			this.logger = logger;
		}

		public virtual void Handle(EntityDeletedEvent<ServiceOrderDispatch> e)
		{
			logger.DebugFormat("Service Order no: {0} | Dispatch Id: {1} has been deleted | Old Dispatch Status: {2}",
				e.Entity.OrderHead.OrderNo, e.Entity.Id, e.Entity.Id, e.Entity.StatusKey);
			var oldStatusKey = e.Entity.OrderHead.StatusKey;

			serviceOrderStatusEvaluator.DynamicUpdateOrderStatus(e.Entity.OrderHead);
			logger.DebugFormat("Service Order no: {0} | Old Status: {1} | New Status: {2}",
				e.Entity.OrderHead.OrderNo, oldStatusKey, e.Entity.OrderHead);
		}
	}
}
