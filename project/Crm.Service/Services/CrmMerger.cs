namespace Crm.Service.Services
{
	using System;
	using System.Linq;

	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model;
	using Crm.Service.Model;
	using Crm.Service.Model.Relationships;

	public class CrmMerger : IMerger<Contact>, IMerger<Company>, IMerger<Person>
	{
		private readonly IRepositoryWithTypedId<Installation, Guid> installationRepository;
		private readonly IRepositoryWithTypedId<ServiceCase, Guid> serviceCaseRepository;
		private readonly IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderHeadRepository;
		private readonly IRepositoryWithTypedId<InstallationCompanyRelationship, Guid> installationCompanyRelationshipRepository;

		public CrmMerger(IRepositoryWithTypedId<Installation, Guid> installationRepository, IRepositoryWithTypedId<ServiceCase, Guid> serviceCaseRepository, IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderHeadRepository, IRepositoryWithTypedId<InstallationCompanyRelationship, Guid> installationCompanyRelationshipRepository)
		{
			this.installationRepository = installationRepository;
			this.serviceCaseRepository = serviceCaseRepository;
			this.serviceOrderHeadRepository = serviceOrderHeadRepository;
			this.installationCompanyRelationshipRepository = installationCompanyRelationshipRepository;
		}

		public virtual void Merge(Contact loser, Contact winner)
		{
			MergeInstallationReferences(loser, winner);
		}
		public virtual void Merge(Company loser, Company winner)
		{
			MergeInstallationReferences(loser, winner);
			MergeServiceCaseReferences(loser, winner);
			MergeServiceOrderHeadReferences(loser, winner);
		}
		public virtual void Merge(Person loser, Person winner)
		{
			MergeInstallationReferences(loser, winner);
			MergeServiceCaseReferences(loser, winner);
		}

		protected virtual void MergeInstallationReferences(Company loser, Company winner)
		{
			var loserInstallations = installationRepository.GetAll().Where(x => x.LocationContactId == loser.Id);
			foreach (var loserInstallation in loserInstallations)
			{
				loserInstallation.LocationContactId = winner.Id;
				installationRepository.SaveOrUpdate(loserInstallation);
			}
		}
		protected virtual void MergeServiceCaseReferences(Company loser, Company winner)
		{
			var loserServiceCases = serviceCaseRepository.GetAll().Where(x => x.AffectedCompanyKey == loser.Id);
			foreach (var loserServiceCase in loserServiceCases)
			{
				loserServiceCase.AffectedCompany = winner;
				serviceCaseRepository.SaveOrUpdate(loserServiceCase);
			}
		}
		protected virtual void MergeServiceOrderHeadReferences(Company loser, Company winner)
		{
			var loserServiceOrders = serviceOrderHeadRepository.GetAll().Where(x => x.CustomerContactId == loser.Id || x.InitiatorId == loser.Id || x.PayerId == loser.Id || x.InvoiceRecipientId == loser.Id);
			foreach (var loserServiceOrder in loserServiceOrders)
			{
				if (loserServiceOrder.CustomerContactId == loser.Id)
				{
					loserServiceOrder.CustomerContactId = winner.Id;
				}
				if (loserServiceOrder.InitiatorId == loser.Id)
				{
					loserServiceOrder.InitiatorId = winner.Id;
				}
				if (loserServiceOrder.PayerId == loser.Id)
				{
					loserServiceOrder.PayerId = winner.Id;
				}
				if (loserServiceOrder.InvoiceRecipientId == loser.Id)
				{
					loserServiceOrder.InvoiceRecipientId = winner.Id;
				}
				serviceOrderHeadRepository.SaveOrUpdate(loserServiceOrder);
			}
		}
		protected virtual void MergeInstallationReferences(Contact loser, Contact winner)
		{
			var installationCompanyRelationshipsWithReferencesToLoser = installationCompanyRelationshipRepository.GetAll().Where(x => x.ChildId == loser.Id);
			foreach (var installationCompanyRelationshipWithReferencesToLoser in installationCompanyRelationshipsWithReferencesToLoser)
			{
				if (installationCompanyRelationshipRepository.GetAll().Any(x => x.ParentId == installationCompanyRelationshipWithReferencesToLoser.ParentId && x.ChildId == winner.Id))
				{
					installationCompanyRelationshipRepository.Delete(installationCompanyRelationshipWithReferencesToLoser);
				}
				else
				{
					installationCompanyRelationshipWithReferencesToLoser.ChildId = winner.Id;
					installationCompanyRelationshipRepository.SaveOrUpdate(installationCompanyRelationshipWithReferencesToLoser);
				}
			}
		}
		protected virtual void MergeInstallationReferences(Person loser, Person winner)
		{
			var loserInstallations = installationRepository.GetAll().Where(x => x.LocationPersonId == loser.Id);
			foreach (var loserInstallation in loserInstallations)
			{
				loserInstallation.LocationPersonId = winner.Id;
				installationRepository.SaveOrUpdate(loserInstallation);
			}
		}
		protected virtual void MergeServiceCaseReferences(Person loser, Person winner)
		{
			var loserServiceCases = serviceCaseRepository.GetAll().Where(x => x.ContactPersonId == loser.Id);
			foreach (var loserServiceCase in loserServiceCases)
			{
				loserServiceCase.ContactPerson = winner;
				serviceCaseRepository.SaveOrUpdate(loserServiceCase);
			}
		}
	}
}
