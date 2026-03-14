namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Article.Rest.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.PerDiem.Rest.Model;
	using Crm.Service.Model;

	using Main.Rest.Model;

	[RestTypeFor(DomainType = typeof(ServiceOrderTimePosting))]
	public class ServiceOrderTimePostingRest : TimeEntryRest
	{
		public Guid? ParentServiceOrderTimePostingId { get; set; }
		public long? ParentServiceOrderTimePostingVersion { get; set; }
		public Guid? ServiceOrderTimeId { get; set; }
		public Guid? ArticleId { get; set; }
		public Guid? DispatchId { get; set; }
		public Guid OrderId { get; set; }
		public string ItemNo { get; set; }
		public string InternalRemark { get; set; }
		public string ServiceOrderTimePostingType { get; set; }
		public string Username { get; set; }
		public string TravelTypeKey { get; set; }
		public string DriverTypeKey { get; set; }
		public string DistanceUnitKey { get; set; }
		public int? Distance { get; set; }
		public string LocationFrom { get; set; }
		public string LocationTo { get; set; }
		public string LicensePlate { get; set; }
		[RestrictedField]
		public TimeSpan? Break { get; set; }
		[NotReceived]
		public long Version { get; set; }

		[NavigationProperty(nameof(ArticleId))]
		public ArticleRest Article { get; set; }

		[NavigationProperty(nameof(ParentServiceOrderTimePostingId), InverseProperty = nameof(ChildServiceOrderTimePostings))]
		public ServiceOrderTimePostingRest ParentServiceOrderTimePosting { get; set; }

		[NavigationProperty(nameof(Username))]
		public UserRest User { get; set; }

		[NavigationProperty(nameof(DispatchId), nameof(ServiceOrderDispatchRest.ServiceOrderTimePostings))]
		public ServiceOrderDispatchRest ServiceOrderDispatch { get; set; }

		[NavigationProperty(nameof(ServiceOrderTimeId), nameof(ServiceOrderTimeRest.Postings))]
		public ServiceOrderTimeRest ServiceOrderTime { get; set; }

		[NavigationProperty(nameof(OrderId), nameof(ServiceOrderHeadRest.ServiceOrderTimePostings))]
		public ServiceOrderHeadRest ServiceOrder { get; set; }

		[NavigationProperty(nameof(ParentServiceOrderTimePostingId), nameof(ParentServiceOrderTimePosting))]
		public ServiceOrderTimePostingRest[] ChildServiceOrderTimePostings { get; set; }
	}
}
