namespace Sms.Scheduler.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;

	using Sms.Scheduler.Model;

	[RestTypeFor(DomainType = typeof(ProfileResource))]
	public class ProfileResourceRest : IRestEntity, IAuthorisedRestEntity
	{
		public int Id { get; set; }
		public string ResourceKey { get; set; }
		public int ProfileKey { get; set; }
		public int? SortOrder { get; set; }
		[RestrictedField, NotReceived] public Guid? DomainId { get; set; }
	}
}
