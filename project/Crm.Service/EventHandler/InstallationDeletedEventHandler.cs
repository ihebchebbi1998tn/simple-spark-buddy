namespace Crm.Service.EventHandler
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;
	using Crm.Service.Model.Relationships;

	public class InstallationDeletedEventHandler : IEventHandler<EntityDeletedEvent<Installation>>
	{
		private readonly IRepositoryWithTypedId<ServiceContractInstallationRelationship, Guid> serviceContractRelationshipRepository;
		public InstallationDeletedEventHandler(IRepositoryWithTypedId<ServiceContractInstallationRelationship, Guid> serviceContractRelationshipRepository)
		{
			this.serviceContractRelationshipRepository = serviceContractRelationshipRepository;
		}

		public virtual void Handle(EntityDeletedEvent<Installation> e)
		{
			var relationships = serviceContractRelationshipRepository.GetAll().Where(rel => rel.ChildId == e.Entity.Id).ToList();
			foreach (var rel in relationships)
			{
				serviceContractRelationshipRepository.Delete(rel);
			}
		}
	}
}
