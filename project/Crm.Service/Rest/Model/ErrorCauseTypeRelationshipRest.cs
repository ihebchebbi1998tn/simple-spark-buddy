namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Service.Model;
	using Crm.Service.Rest.Model.Lookups;

	[RestTypeFor(DomainType = typeof(ErrorCauseTypeRelationship))]
	public class ErrorCauseTypeRelationshipRest : RestEntityWithExtensionValues
	{
		public virtual Guid StatisticsKeyCauseKey { get; set; }
		public virtual Guid ErrorTypeKey { get; set; }
		[NotReceived, ExplicitExpand] public virtual StatisticsKeyCauseRest StatisticsKeyCause { get; set; }
	}
}
