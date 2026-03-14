namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Rest.Model;

	using Main.Rest.Model;
	[RestTypeFor(DomainType = typeof(Service.Model.ServiceObject))]

	public class ServiceObjectRest : ContactRest
	{
		public string ObjectNo { get; set; }
		public string CategoryKey { get; set; }
		public Guid StandardAddressId { get; set; }

		[NavigationProperty(nameof(StandardAddressId))]
		public AddressRest StandardAddress { get; set; }

		[NavigationProperty(nameof(AddressRest.CompanyId))]
		public AddressRest[] Addresses { get; set; }

		[NavigationProperty(nameof(InstallationRest.FolderId), nameof(InstallationRest.ServiceObject))]
		public InstallationRest[] Installations { get; set; }

		[NavigationProperty(nameof(ResponsibleUser))]
		public UserRest ResponsibleUserUser { get; set; }

		[NavigationProperty(nameof(TagRest.ContactKey))]
		public TagRest[] Tags { get; set; }
	}
}
