namespace Crm.Service.BackgroundServices
{
	using System;
	using System.Linq;

	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Helper;
	using Crm.Service.Model;
	using Crm.Service.Model.Extensions;
	using Crm.Service.Model.Lookup;
	using Crm.Service.SearchCriteria;
	using Crm.Service.Services.Interfaces;

	using log4net;

	using Microsoft.Extensions.Hosting;

	using NHibernate.Linq;

	using Quartz;

	[DisallowConcurrentExecution]
	public class MaintenanceOrderAgent : ManualSessionHandlingJobBase
	{
		private readonly IMaintenancePlanService maintenancePlanService;
		private readonly IServiceOrderService serviceOrderService;
		private readonly ILog logger;
		private readonly IRepositoryWithTypedId<MaintenancePlan, Guid> maintenancePlanRepository;
		private readonly IAppSettingsProvider appSettingsProvider;

		// Methods
		protected override void Run(IJobExecutionContext context)
		{
			var timespan = TimeSpan.FromDays(appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.CreateMaintenanceOrderTimeSpanDays));
			var targetDate = DateTime.Today + timespan;

			logger.InfoFormat("MaintenanceOrderAgent started: Processing maintenance plans with NextDate up to {0} (TimeSpan: {1} days)",
				targetDate.ToString("yyyy-MM-dd"), timespan.TotalDays);

			var criteria = new MaintenancePlanSearchCriteria
			{
				ToNextDate = targetDate
			};

			var maintenancePlans = maintenancePlanRepository
															.GetAll()
															.Where(x => x.GenerateMaintenanceOrders)
															.Where(x => x.ServiceContract != null && x.ServiceContract.StatusKey == ServiceContractStatus.ActiveKey)
															.Filter(criteria)
															.Fetch(x => x.ServiceContract);

			var totalPlans = maintenancePlans.Count();
			logger.InfoFormat("Found {0} maintenance plans eligible for processing", totalPlans);

			var counter = 0;
			var processedPlans = 0;
			var ordersGenerated = 0;
			var plansWithoutOrders = 0;

			var batch = maintenancePlans.Skip(counter).Take(50);
			while (batch.Any())
			{
				if (receivedShutdownSignal)
				{
					logger.Warn("MaintenanceOrderAgent: Shutdown signal received, stopping processing");
					break;
				}
				foreach (var maintenancePlan in batch)
				{
					if (receivedShutdownSignal)
					{
						logger.Warn("MaintenanceOrderAgent: Shutdown signal received, stopping processing");
						break;
					}
					try
					{
						BeginTransaction();

						logger.DebugFormat("Processing MaintenancePlan {0} (Contract: {1}, NextDate: {2})",
							maintenancePlan.Id,
							maintenancePlan.ServiceContract?.ContractNo ?? "null",
							maintenancePlan.NextDate?.ToString("yyyy-MM-dd") ?? "null");

						var orders = maintenancePlanService.EvaluateMaintenancePlanAndGenerateOrders(maintenancePlan, targetDate);
						if (orders.Any())
						{
							foreach (var order in orders)
							{
								serviceOrderService.Save(order);
							}
							ordersGenerated += orders.Count;
							logger.InfoFormat("Successfully processed MaintenancePlan {0}: Generated {1} maintenance orders",
								maintenancePlan.Id, orders.Count);
						}
						else
						{
							plansWithoutOrders++;
							logger.WarnFormat("MaintenancePlan {0} (Contract: {1}) did not generate any maintenance orders. Check previous warning logs for detailed reasons.",
								maintenancePlan.Id, maintenancePlan.ServiceContract?.ContractNo ?? "null");
						}

						processedPlans++;
						EndTransaction();
					}
					catch (Exception ex)
					{
						logger.Error(String.Format("An error occurred generating MaintenanceOrders for MaintenancePlan {0} (Contract: {1}): {2}",
							maintenancePlan.Id,
							maintenancePlan.ServiceContract?.ContractNo ?? "null",
							ex.Message), ex);
						RollbackTransaction();
					}
				}

				counter += 50;
				batch = maintenancePlans.Skip(counter).Take(50);

				logger.InfoFormat("MaintenanceOrderAgent progress: Processed {0}/{1} maintenance plans, Generated {2} orders, {3} plans without orders",
					processedPlans, totalPlans, ordersGenerated, plansWithoutOrders);
			}

			logger.InfoFormat("MaintenanceOrderAgent completed: Processed {0}/{1} maintenance plans, Generated {2} orders, {3} plans did not generate orders",
				processedPlans, totalPlans, ordersGenerated, plansWithoutOrders);
		}

		public MaintenanceOrderAgent(IMaintenancePlanService maintenancePlanService, IRepositoryWithTypedId<MaintenancePlan, Guid> maintenancePlanRepository, IServiceOrderService serviceOrderService, ISessionProvider sessionProvider, ILog logger, IAppSettingsProvider appSettingsProvider, IHostApplicationLifetime hostApplicationLifetime)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.maintenancePlanService = maintenancePlanService;
			this.maintenancePlanRepository = maintenancePlanRepository;
			this.serviceOrderService = serviceOrderService;
			this.logger = logger;
			this.appSettingsProvider = appSettingsProvider;
		}
	}
}
