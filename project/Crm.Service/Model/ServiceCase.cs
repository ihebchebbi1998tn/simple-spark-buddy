namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Model;
	using Crm.Model;
	using Crm.Model.Interfaces;
	using Crm.Service.Model.Lookup;

	using Main.Model;
	using Main.Model.Lookups;

	public class ServiceCase : Contact, IStatusChoosable<ServiceCaseStatus>, IEntityWithTags
	{
		public override string Name => ServiceCaseNo;
		public virtual DateTime? CompletionDate { get; set; }
		public virtual ServiceOrderHead CompletionServiceOrder { get; set; }
		public virtual Guid? CompletionServiceOrderId { get; set; }
		public virtual string CompletionUser { get; set; }
		public virtual User CompletionUserObject { get; set; }
		public virtual ServiceOrderHead OriginatingServiceOrder { get; set; }
		public virtual Guid? OriginatingServiceOrderId { get; set; }
		public virtual ServiceOrderTime OriginatingServiceOrderTime { get; set; }
		public virtual Guid? OriginatingServiceOrderTimeId { get; set; }
		public virtual string ServiceCaseNo { get; set; }
		public virtual Guid? ServiceCaseTemplateId { get; set; }
		public virtual ServiceObject ServiceObject { get; set; }
		public virtual Guid? ServiceObjectId { get; set; }
		public virtual ServiceOrderTime ServiceOrderTime { get; set; }
		public virtual Guid? ServiceOrderTimeId { get; set; }
		public virtual Guid? AffectedCompanyKey { get; set; }
		public virtual Company AffectedCompany { get; set; }

		public virtual Guid? ContactPersonId { get; set; }
		public virtual Person ContactPerson { get; set; }

		public virtual Guid? AffectedInstallationKey { get; set; }
		public virtual Installation AffectedInstallation { get; set; }

		public virtual string ErrorMessage { get; set; }

		public virtual DateTime? Reported { get; set; }
		public virtual DateTime? Planned { get; set; }
		public virtual DateTime? Executed { get; set; }
		public virtual DateTime? PickedUpDate { get; set; }

		public virtual int StatusKey { get; set; }
		public virtual string PriorityKey { get; set; }
		public virtual string CategoryKey { get; set; }
		public virtual Guid? UserGroupKey { get; set; }
		public virtual string ErrorCodeKey { get; set; }
		public virtual Guid? StationKey { get; set; }
		public virtual Station Station { get; set; }

		public virtual ErrorCode ErrorCode { get; set; }
		public virtual DateTime ServiceCaseCreateDate { get; set; }
		public virtual string ServiceCaseCreateUser{ get; set; }
		public virtual ServiceCaseStatus Status
		{
			get { return LookupManager.Get<ServiceCaseStatus>(StatusKey); }
			set { StatusKey = value.Key; }
		}
		public virtual ServicePriority Priority
		{
			get { return PriorityKey != null ? LookupManager.Get<ServicePriority>(PriorityKey) : null; }
			set { PriorityKey = value != null ? value.Key : null; }
		}
		public virtual ServiceCaseCategory Category
		{
			get { return CategoryKey != null ? LookupManager.Get<ServiceCaseCategory>(CategoryKey) : null; }
			set { CategoryKey = value != null ? value.Key : null; }
		}
		public virtual Usergroup UserGroup { get; set; }
		public virtual ICollection<string> RequiredSkillKeys { get; set; }

		public virtual List<Skill> RequiredSkills
		{
			get { return RequiredSkillKeys == null ? null : RequiredSkillKeys.Select(key => LookupManager.Get<Skill>(key)).ToList(); }
		}
		public virtual ICollection<string> RequiredAssetKeys { get; set; }

		public virtual List<Asset> RequiredAssets
		{
			get { return RequiredAssetKeys == null ? null : RequiredAssetKeys.Select(key => LookupManager.Get<Asset>(key)).ToList(); }
		}

		public virtual string StatisticsKeyProductTypeKey { get; set; }
		public virtual string StatisticsKeyMainAssemblyKey { get; set; }
		public virtual string StatisticsKeySubAssemblyKey { get; set; }
		public virtual string StatisticsKeyAssemblyGroupKey { get; set; }
		public virtual string StatisticsKeyFaultImageKey { get; set; }
		public virtual string StatisticsKeyRemedyKey { get; set; }
		public virtual string StatisticsKeyCauseKey { get; set; }
		public virtual string StatisticsKeyWeightingKey { get; set; }
		public virtual string StatisticsKeyCauserKey { get; set; }
		public virtual ICollection<ServiceOrderErrorType> ServiceOrderErrorTypes { get; set; }

		public ServiceCase()
		{
			ServiceOrderErrorTypes = new List<ServiceOrderErrorType>();
		}
	}
}
