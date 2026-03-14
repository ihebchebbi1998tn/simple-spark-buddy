using System;

namespace Crm.Service.Services
{
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Service.Model;
	using Crm.Service.Services.Interfaces;

	public class DispatchRestService : IDispatchRestService
	{
		private readonly IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository;
		public DispatchRestService(IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository)
		{
			this.dispatchRepository = dispatchRepository;
		}
		public virtual void CreateDispatch(ServiceOrderDispatch dispatch)
		{
			dispatchRepository.SaveOrUpdate(dispatch);
		}
		public virtual void UpdateDispatch(ServiceOrderDispatch dispatch)
		{
			dispatchRepository.SaveOrUpdate(dispatch);
		}
		public virtual void DeleteDispatch(ServiceOrderDispatch dispatch)
		{
			dispatchRepository.Delete(dispatch);
		}
	}
}
