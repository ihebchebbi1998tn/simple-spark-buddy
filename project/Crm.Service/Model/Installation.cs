namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Article.Model;
	using Crm.Library.Model;
	using Crm.Model;
	using Crm.Model.Interfaces;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Model.Relationships;

	public class Installation : Contact, IEntityWithTags, IWithFolder
	{
		public virtual string InstallationNo { get; set; }
		public virtual string ExactPlace { get; set; }
		public virtual string Room { get; set; }
		public virtual string MaintenanceContractNo { get; set; }
		public virtual string LegacyInstallationId { get; set; }
		public virtual DateTime? KickOffDate { get; set; }
		public virtual DateTime? ManufactureDate { get; set; }
		public virtual string SoftwareVersion { get; set; }
		public virtual int Priority { get; set; }
		public virtual DateTime? WarrantyFrom { get; set; }
		public virtual DateTime? WarrantyUntil { get; set; }
		public virtual string TechnicianInformation { get; set; }
		public virtual string ExternalReference { get; set; }

		public virtual string PreferredUser { get; set; }
		public virtual User PreferredUserObj { get; set; }

		public virtual Guid? LocationContactId { get; set; }
		public virtual Company LocationCompany { get; set; }

		public virtual string Description { get; set; }

		public virtual Guid? LocationPersonId { get; set; }
		public virtual Person LocationContactPerson { get; set; }

		public virtual Guid? LocationAddressKey { get; set; }
		public virtual Address LocationAddress { get; set; }

		public virtual Guid? FolderId { get; set; }
		public virtual string FolderName => ServiceObject?.Name;
		public virtual Folder Folder { get => ServiceObject; set => ServiceObject = (ServiceObject)value; }
		public virtual ServiceObject ServiceObject { get; set; }

		public virtual string StatusKey { get; set; }
		public virtual InstallationHeadStatus Status
		{
			get { return StatusKey != null ? LookupManager.Get<InstallationHeadStatus>(StatusKey) : null; }
		}

		public virtual string InstallationTypeKey { get; set; }
		public virtual InstallationType InstallationType
		{
			get { return InstallationTypeKey != null ? LookupManager.Get<InstallationType>(InstallationTypeKey) : null; }
		}

		public virtual string ManufacturerKey { get; set; }
		public virtual Manufacturer Manufacturer
		{
			get { return ManufacturerKey != null ? LookupManager.Get<Manufacturer>(ManufacturerKey) : null; }
		}	
		
		public virtual string StatisticsKeyProductTypeKey { get; set; }
		public virtual StatisticsKeyProductType StatisticsKeyProductType
		{
			get { return StatisticsKeyProductTypeKey != null ? LookupManager.Get<StatisticsKeyProductType>(StatisticsKeyProductTypeKey) : null; }
		}

		public virtual Guid? StationKey { get; set; }
		public virtual Station Station { get; set; }
		
		public virtual string CustomItemNo { get; set; }
		public virtual string CustomItemDesc { get; set; }
		public virtual Guid? ArticleKey { get; set; }
		public virtual Article Article { get; set; }

		public virtual string BaseInstallationNo { get; set; }

		public virtual ICollection<InstallationCompanyRelationship> AdditionalContacts { get; set; }
		public virtual ICollection<InstallationAddressRelationship> AddressRelationships { get; set; }
		public virtual ICollection<ServiceContractInstallationRelationship> ServiceContractInstallationRelationships { get; set; }

		public virtual string FullDescription
		{
			get { return String.IsNullOrWhiteSpace(Description) ? InstallationNo : String.Format("{0} - {1}", InstallationNo, Description); }
		}
		public override string ToString()
		{
			return String.IsNullOrWhiteSpace(Description) ? InstallationNo : String.Format("{0} - {1}", InstallationNo, Description);
		}

		// Constructor
		public Installation()
		{
			AdditionalContacts = new List<InstallationCompanyRelationship>();
			AddressRelationships = new List<InstallationAddressRelationship>();
			ServiceContractInstallationRelationships = new List<ServiceContractInstallationRelationship>();
		}
	}
}
