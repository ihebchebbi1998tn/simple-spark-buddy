namespace Main.Flow.Rest.Model
{
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	using Main.Flow.Model;

	[RestTypeFor(DomainType = typeof(FlowRule))]
	public class FlowRuleRest : RestEntityWithExtensionValues
	{
		public virtual string EntityType { get; set; }
		[RestrictedField]
		public virtual string Action { get; set; }
		public virtual string Description { get; set; }
		public virtual string Endpoint { get; set; }
		public virtual string Username { get; set; }
		public virtual string Password { get; set; }
	}
}
