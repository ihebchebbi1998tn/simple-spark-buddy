namespace Crm.Service.BackgroundServices
{
	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Modularization.Events;
	using Crm.Service.Events;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using log4net;

	using Microsoft.Extensions.Hosting;

	using Quartz;

	[DisallowConcurrentExecution]
	public class ServiceContractValidationCheckerAgent : ManualSessionHandlingJobBase
	{
		private readonly IRepository<ServiceContract> serviceContractRepository;
		private readonly IEventAggregator eventAggregator;

		protected override void Run(IJobExecutionContext context)
		{
			CheckValidation();
		}

		protected virtual void CheckValidation()
		{
			var serviceContracts = serviceContractRepository.GetAll();
			foreach (var serviceContract in serviceContracts)
			{
				if (serviceContract.StatusKey == ServiceContractStatus.PendingKey && serviceContract.CanSetToActive)
				{
					serviceContract.StatusKey = ServiceContractStatus.ActiveKey;
					serviceContractRepository.SaveOrUpdate(serviceContract);
					eventAggregator.Publish(new ServiceContractStatusChangedEvent(serviceContract));
				}
				if (serviceContract.HasExpired && serviceContract.StatusKey != ServiceContractStatus.InactiveKey && serviceContract.StatusKey != ServiceContractStatus.ExpiredKey)
				{
					serviceContract.StatusKey = ServiceContractStatus.ExpiredKey;
					serviceContractRepository.SaveOrUpdate(serviceContract);
					eventAggregator.Publish(new ServiceContractStatusChangedEvent(serviceContract));
				}
			}
		}

		public ServiceContractValidationCheckerAgent(ISessionProvider sessionProvider, IRepository<ServiceContract> serviceContractRepository, IEventAggregator eventAggregator, ILog logger, IHostApplicationLifetime hostApplicationLifetime)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.serviceContractRepository = serviceContractRepository;
			this.eventAggregator = eventAggregator;
		}
	}
}
