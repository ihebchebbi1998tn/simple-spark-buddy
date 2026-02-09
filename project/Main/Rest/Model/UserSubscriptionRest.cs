namespace Main.Rest.Model
{
	using System;

	using Crm.Library.Rest;

	using Main.Model;

	[RestTypeFor(DomainType = typeof(UserSubscription))]
	public class UserSubscriptionRest : RestEntity
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string EntityType { get; set; }
		public Guid EntityKey { get; set; }
		public bool IsSubscribed { get; set; }
	}
}
