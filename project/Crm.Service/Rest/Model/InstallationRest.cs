namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Article.Rest.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Model;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	using Main.Rest.Model;

	using AddressRest = Crm.Rest.Model.AddressRest;

	[RestTypeFor(DomainType = typeof(Installation))]
	public class InstallationRest : ContactRest
	{
		public string InstallationNo { get; set; }
		public string LegacyInstallationId { get; set; }
		public string InstallationTypeKey { get; set; }
		public string Description { get; set; }
		public Guid? LocationAddressKey { get; set; }
		public Guid? LocationContactId { get; set; }
		public Guid? LocationPersonId { get; set; }
		public Guid? FolderId { get; set; }
		public string StatusKey { get; set; }
		public virtual string ManufacturerKey { get; set; }
		public virtual string StatisticsKeyProductTypeKey { get; set; }
		public string PreferredUser { get; set; }
		public DateTime? ManufactureDate { get; set; }
		public DateTime? KickOffDate { get; set; }
		public DateTime? WarrantyFrom { get; set; }
		public DateTime? WarrantyUntil { get; set; }
		public string TechnicianInformation { get; set; }
		public string ExactPlace { get; set; }
		public string Room { get; set; }
		public string ExternalReference { get; set; }
		public Guid? StationKey { get; set; }
		public string CustomItemNo { get; set; }
		public string CustomItemDesc { get; set; }
		public Guid? ArticleKey { get; set; }
		[NavigationProperty(nameof(StationKey))] public StationRest Station { get; set; }
		[NavigationProperty(nameof(LocationAddressKey))] public AddressRest Address { get; set; }
		[NavigationProperty(nameof(LocationContactId))] public CompanyRest Company { get; set; }
		[NavigationProperty(nameof(LocationPersonId))] public PersonRest Person { get; set; }
		[NavigationProperty(nameof(PreferredUser))] public UserRest PreferredUserUser { get; set; }
		[NavigationProperty(nameof(ResponsibleUser))] public UserRest ResponsibleUserUser { get; set; }

		[NavigationProperty(nameof(ServiceContractInstallationRelationshipRest.ChildId), nameof(ServiceContractInstallationRelationshipRest.Child))]
		public ServiceContractInstallationRelationshipRest[] ServiceContractInstallationRelationships { get; set; }

		[NavigationProperty(nameof(FolderId), nameof(ServiceObjectRest.Installations))]
		public ServiceObjectRest ServiceObject { get; set; }

		[NavigationProperty(nameof(TagRest.ContactKey))] public TagRest[] Tags { get; set; }

		[NavigationProperty(nameof(ArticleKey))] public ArticleRest Article { get; set; }
	}
}
