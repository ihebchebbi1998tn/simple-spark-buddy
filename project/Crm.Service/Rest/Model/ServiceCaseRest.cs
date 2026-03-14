namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Model;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	using Main.Rest.Model;

	[RestTypeFor(DomainType = typeof(ServiceCase))]
	public class ServiceCaseRest : ContactRest
	{
		public DateTime? CompletionDate { get; set; }

		[NavigationProperty(nameof(CompletionServiceOrderId))]
		public ServiceOrderHeadRest CompletionServiceOrder { get; set; }
		public Guid? CompletionServiceOrderId { get; set; }
		public string CompletionUser { get; set; }

		[NavigationProperty(nameof(CompletionUser))]
		public UserRest CompletionUserUser { get; set; }

		[NavigationProperty(nameof(OriginatingServiceOrderId))]
		public ServiceOrderHeadRest OriginatingServiceOrder { get; set; }
		public Guid? OriginatingServiceOrderId { get; set; }

		[NavigationProperty(nameof(OriginatingServiceOrderTimeId))]
		public ServiceOrderTimeRest OriginatingServiceOrderTime { get; set; }
		public Guid? OriginatingServiceOrderTimeId { get; set; }

		[NavigationProperty(nameof(ServiceObjectId))]
		public ServiceObjectRest ServiceObject { get; set; }
		public Guid? ServiceObjectId { get; set; }
		public string ErrorMessage { get; set; }
		public string ServiceCaseNo { get; set; }
		public Guid? ServiceCaseTemplateId { get; set; }

		[NavigationProperty(nameof(ServiceOrderTimeId), nameof(ServiceOrderTimeRest.ServiceCases))]
		public ServiceOrderTimeRest ServiceOrderTime { get; set; }
		public Guid? ServiceOrderTimeId { get; set; }

		[NavigationProperty(nameof(TagRest.ContactKey))]
		public TagRest[] Tags { get; set; }
		public DateTime? Reported { get; set; }

		[NavigationProperty(nameof(ResponsibleUser))]
		public UserRest ResponsibleUserUser { get; set; }
		public DateTime? Planned { get; set; }
		public DateTime? Executed { get; set; }
		public DateTime? PickedUpDate { get; set; }

		[NavigationProperty(nameof(AffectedInstallationKey))]
		public InstallationRest AffectedInstallation { get; set; }
		public Guid? AffectedInstallationKey { get; set; }

		[NavigationProperty(nameof(AffectedCompanyKey))]
		public CompanyRest AffectedCompany { get; set; }
		public Guid? AffectedCompanyKey { get; set; }

		[NavigationProperty(nameof(ContactPersonId))]
		public PersonRest ContactPerson { get; set; }
		public Guid? ContactPersonId { get; set; }
		public int StatusKey { get; set; }
		public string PriorityKey { get; set; }
		public string CategoryKey { get; set; }
		public string ErrorCodeKey { get; set; }
		public Guid? StationKey { get; set; }

		[NavigationProperty(nameof(StationKey))]
		public StationRest Station { get; set; }
		[RestrictedField] public string[] RequiredSkillKeys { get; set; }
		[RestrictedField] public string[] RequiredAssetKeys { get; set; }
		public DateTime ServiceCaseCreateDate { get; set; }
		public string ServiceCaseCreateUser { get; set; }

		public string StatisticsKeyProductTypeKey { get; set; }
		public string StatisticsKeyMainAssemblyKey { get; set; }
		public string StatisticsKeySubAssemblyKey { get; set; }
		public string StatisticsKeyAssemblyGroupKey { get; set; }
		public string StatisticsKeyFaultImageKey { get; set; }
		public string StatisticsKeyRemedyKey { get; set; }
		public string StatisticsKeyCauseKey { get; set; }
		public string StatisticsKeyWeightingKey { get; set; }
		public string StatisticsKeyCauserKey { get; set; }
		public Guid? UserGroupKey { get; set; }
		[NavigationProperty(nameof(UserGroupKey))]
		public UsergroupRest UserGroup { get; set; }

		[NavigationProperty(nameof(ServiceOrderErrorTypeRest.ServiceCaseId), nameof(ServiceOrderErrorTypeRest.ServiceCase))]
		public ServiceOrderErrorTypeRest[] ServiceOrderErrorTypes { get; set; }
	}
}
