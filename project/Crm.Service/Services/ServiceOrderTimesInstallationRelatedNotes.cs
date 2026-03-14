namespace Crm.Service.Services
{
	using System;
	using System.Linq;

	using Crm.Controllers;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model;
	using Crm.Service.Model;

	public class ServiceOrderTimesInstallationRelatedNotes : IRelatedContact<Installation>
	{
		private readonly IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository;
		public ServiceOrderTimesInstallationRelatedNotes(IRepositoryWithTypedId<ServiceOrderTime, Guid> serviceOrderTimeRepository)
		{
			this.serviceOrderTimeRepository = serviceOrderTimeRepository;
		}
		public virtual IQueryable<Contact> RelatedContact(Contact contact)
		{
			var query = serviceOrderTimeRepository.GetAll()
								.Where(h => h.ServiceOrderHead.IsTemplate == false && h.InstallationId == contact.Id).Select(x => x.ServiceOrderHead);

			return query;
		}

	}
}
