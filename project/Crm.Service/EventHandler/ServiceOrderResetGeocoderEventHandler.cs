namespace Crm.Service.EventHandler
{
	using System;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;

	public class ServiceOrderResetGeocoderEventHandler : IEventHandler<EntityModifiedEvent<ServiceOrderHead>>
	{
		private readonly IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository;
		public ServiceOrderResetGeocoderEventHandler(IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository)
		{
			this.serviceOrderRepository = serviceOrderRepository;
		}

		public virtual void Handle(EntityModifiedEvent<ServiceOrderHead> e)
		{
			var serviceOrderBefore = e.EntityBeforeChange;
			var serviceOrder = e.Entity;

			if (serviceOrder.Longitude.HasValue && serviceOrder.Latitude.HasValue
				&& (serviceOrderBefore.City != serviceOrder.City
				|| serviceOrderBefore.CountryKey != serviceOrder.CountryKey
				|| serviceOrderBefore.Street != serviceOrder.Street
				|| serviceOrderBefore.ZipCode != serviceOrder.ZipCode))
			{
				serviceOrder.GeocodingRetryCounter = 0;
				serviceOrder.Longitude = null;
				serviceOrder.Latitude = null;

				serviceOrderRepository.SaveOrUpdate(serviceOrder);
			}
		}
	}
}
