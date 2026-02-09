namespace Main.Rest.Model
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	using Main.Model;

	[RestTypeFor(DomainType = typeof(UserAsset))]
	public class UserAssetRest : RestEntity
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		[NavigationProperty(nameof(Username))] public UserRest User { get; set; }
		public string AssetKey { get; set; }
		public DateTime? ValidFrom { get; set; }
		public DateTime? ValidTo { get; set; }
		public int? DaysToNotifyBeforeExpiration { get; set; }
	}
}
