namespace Crm.Service.EventHandler
{
	using System;

	using Crm.Library.Helper;
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;
	using Crm.Service.Services.Interfaces;

	public class MaintenancePlanModificationEventHandler : IEventHandler<EntityCreatedEvent<MaintenancePlan>>, IEventHandler<EntityModifiedEvent<MaintenancePlan>>
	{
		private readonly IMaintenancePlanService maintenancePlanService;
		private readonly IServiceOrderService serviceOrderService;
		private readonly DateTime toDate;

		public MaintenancePlanModificationEventHandler(IMaintenancePlanService maintenancePlanService, 
			IServiceOrderService serviceOrderService,
			IAppSettingsProvider appSettingsProvider)
		{
			toDate = DateTime.Today.AddDays(appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.CreateMaintenanceOrderTimeSpanDays));

			this.maintenancePlanService = maintenancePlanService;
			this.serviceOrderService = serviceOrderService;
		}

		public virtual void Handle(EntityCreatedEvent<MaintenancePlan> e)
		{
			GenerateUpcomingOrders(e.Entity);
		}
		public virtual void Handle(EntityModifiedEvent<MaintenancePlan> e)
		{
			GenerateUpcomingOrders(e.Entity);
		}
		protected virtual void GenerateUpcomingOrders(MaintenancePlan maintenancePlan)
		{
			if (maintenancePlan.GenerateMaintenanceOrders == false)
			{
				return;
			}
			var orders = maintenancePlanService.EvaluateMaintenancePlanAndGenerateOrders(maintenancePlan, toDate);
			foreach (var order in orders)
			{
				serviceOrderService.Save(order);
			}
		}
	}
}
