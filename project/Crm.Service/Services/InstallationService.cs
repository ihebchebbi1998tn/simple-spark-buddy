namespace Crm.Service.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Relationships;
	using Crm.Service.Services.Interfaces;

	public class InstallationService : IInstallationService
	{
		// Members
		private readonly IRepositoryWithTypedId<Installation, Guid> installationRepository;
		private readonly IRepositoryWithTypedId<InstallationPos, Guid> installationPosRepository;
		private readonly IRepositoryWithTypedId<InstallationAddressRelationship, Guid> installationAddressRelationshipRepository;

		// Constructor
		public InstallationService(
			IRepositoryWithTypedId<Installation, Guid> installationRepository,
			IRepositoryWithTypedId<InstallationPos, Guid> installationPosRepository,
			IRepositoryWithTypedId<InstallationAddressRelationship, Guid> installationAddressRelationshipRepository)
		{
			this.installationRepository = installationRepository;
			this.installationPosRepository = installationPosRepository;
			this.installationAddressRelationshipRepository = installationAddressRelationshipRepository;
		}

		// Methods

		public virtual InstallationPos GetInstallationPosition(Guid id)
		{
			return installationPosRepository.Get(id);
		}

		public virtual IEnumerable<string> GetUsedInstallationHeadStatuses()
		{
			return installationRepository.GetAll().Select(c => c.StatusKey).Distinct().ToList();
		}

		public virtual IEnumerable<string> GetUsedInstallationTypes()
		{
			return installationRepository.GetAll().Select(c => c.InstallationTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedManufacturers()
		{
			return installationRepository.GetAll().Select(c => c.ManufacturerKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedInstallationAddressRelationshipTypes()
		{
			return installationAddressRelationshipRepository.GetAll().Select(c => c.RelationshipTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedQuantityUnits()
		{
			return installationPosRepository.GetAll().Select(c => c.QuantityUnitKey).Distinct();
		}
		
		public virtual Installation GetInstallation(Guid installationId)
		{
			return installationRepository.Get(installationId);
		}
	}
}
