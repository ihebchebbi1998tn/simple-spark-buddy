namespace Crm.Service.EventHandler
{
	using System;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Modularization.Events;
	using Crm.Service.Events;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	public class ServiceContractValidToChangedEventHandler : IEventHandler<EntityCreatedEvent<ServiceContract>>, IEventHandler<EntityModifiedEvent<ServiceContract>>
	{
		private readonly IRepository<ServiceContract> serviceContractRepository;
		private readonly IEventAggregator eventAggregator;

		public virtual void Handle(EntityCreatedEvent<ServiceContract> e)
		{
			e.Entity.StatusKey = GetNewStatusKey(e.Entity);
			serviceContractRepository.SaveOrUpdate(e.Entity);
		}

		public virtual void Handle(EntityModifiedEvent<ServiceContract> e)
		{
			var manuallyChangedStatus = e.EntityBeforeChange.StatusKey != e.Entity.StatusKey;
			var newStatusKey = manuallyChangedStatus ? e.Entity.StatusKey : GetNewStatusKey(e.Entity, e.EntityBeforeChange);
			var statusChanged = e.EntityBeforeChange.StatusKey != newStatusKey;

			if (!statusChanged)
				return;

			if (!manuallyChangedStatus)
			{
				e.Entity.StatusKey = newStatusKey;
			}

			serviceContractRepository.SaveOrUpdate(e.Entity);
			eventAggregator.Publish(new ServiceContractStatusChangedEvent(e.Entity));
		}

		protected virtual string GetNewStatusKey(ServiceContract serviceContract, ServiceContract serviceContractBefore = null)
		{
			var today = DateTime.UtcNow.Date;
			if (serviceContractBefore != null && serviceContractBefore.StatusKey == ServiceContractStatus.InactiveKey && serviceContract.StatusKey == ServiceContractStatus.InactiveKey)
			{
				return ServiceContractStatus.InactiveKey;
			}
			if (serviceContract.HasExpired)
			{
				return ServiceContractStatus.ExpiredKey;
			}
			if (today < serviceContract.ValidFrom)
			{
				return ServiceContractStatus.PendingKey;
			}
			if ((today >= serviceContract.ValidFrom && !serviceContract.ValidTo.HasValue) || (today >= serviceContract.ValidFrom && serviceContract.ValidTo.HasValue && today <= serviceContract.ValidTo))
			{
				return ServiceContractStatus.ActiveKey;
			}

			return ServiceContractStatus.ActiveKey;
		}

		public ServiceContractValidToChangedEventHandler(IRepository<ServiceContract> serviceContractRepository, IEventAggregator eventAggregator)
		{
			this.serviceContractRepository = serviceContractRepository;
			this.eventAggregator = eventAggregator;
		}
	}
}
