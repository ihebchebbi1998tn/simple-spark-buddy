namespace Main.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;

	[RestrictedType(TypeRestriction.NoDelete | TypeRestriction.NoPost)]
	[RestTypeFor(DomainType = typeof(Crm.Library.Model.Site.Site))]
	public class Site : IRestEntity, IRestEntityWithExtensionValues
	{
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
		[NotReceived]
		public Guid Id { get; set; }
		[NotReceived]
		[RestrictedField]
		public DateTime ModifiedAt { get; set; }
		[RestrictedField]
		public string Name { get; set; }
	}
}
