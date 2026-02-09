namespace Crm.Service.Services
{
	using System.Linq;

	using Crm.Controllers;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model;
	using Crm.Service.Model;

	public class InstallationRelatedServiceOrderHead : IRelatedContact<Installation>
	{
		private readonly IRepository<ServiceOrderHead> serviceOrderRepository;
		public InstallationRelatedServiceOrderHead(IRepository<ServiceOrderHead> serviceOrderRepository)
		{
			this.serviceOrderRepository = serviceOrderRepository;
		}
		public virtual IQueryable<Contact> RelatedContact(Contact contact)
		{
			var installation = serviceOrderRepository
												.GetAll()
												.Where(x => x.IsTemplate == false && x.AffectedInstallation.Id == contact.Id).Select(y=>y);

			return installation;
		}
	}
}
