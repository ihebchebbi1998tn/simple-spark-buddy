namespace Crm.Service.EventHandler
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;

	public class ServiceOrderTimeDurationUpdater : IEventHandler<EntityCreatedEvent<ServiceOrderTimePosting>>, IEventHandler<EntityModifiedEvent<ServiceOrderTimePosting>>, IEventHandler<EntityDeletedEvent<ServiceOrderTimePosting>>
	{
		private readonly IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository;
		private readonly IRepositoryWithTypedId<ServiceOrderTimePosting, Guid> serviceOrderTimePostingRepository;

		public ServiceOrderTimeDurationUpdater(IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository, IRepositoryWithTypedId<ServiceOrderTimePosting, Guid> serviceOrderTimePostingRepository)
		{
			this.serviceOrderTimeRepository = serviceOrderTimeRepository;
			this.serviceOrderTimePostingRepository = serviceOrderTimePostingRepository;
		}

		protected virtual void UpdateServiceOrderTime(Guid id)
		{
			var serviceOrderTime = serviceOrderTimeRepository.Get(id);
			if (serviceOrderTime != null)
			{
				var timePostings = serviceOrderTimePostingRepository.GetAll().Where(x => x.OrderTimesId == id).ToList();
				serviceOrderTime.ActualDuration = (float)timePostings.Where(x => x.ServiceOrderTimePostingType == "Used").Sum(x => x.DurationInMinutes.GetValueOrDefault()) / 60;
			}
		}

		public virtual void Handle(EntityCreatedEvent<ServiceOrderTimePosting> e)
		{
			if (e.Entity.OrderTimesId.HasValue)
			{
				UpdateServiceOrderTime(e.Entity.OrderTimesId.Value);
			}
		}
		public virtual void Handle(EntityModifiedEvent<ServiceOrderTimePosting> e)
		{
			if (e.Entity.OrderTimesId.HasValue)
			{
				UpdateServiceOrderTime(e.Entity.OrderTimesId.Value);
			}
		}
		public virtual void Handle(EntityDeletedEvent<ServiceOrderTimePosting> e)
		{
			if (e.Entity.OrderTimesId.HasValue)
			{
				UpdateServiceOrderTime(e.Entity.OrderTimesId.Value);
			}
		}
	}
}
