namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Article.Model;
	using Crm.Article.Model.Enums;
	using Crm.Article.Model.Lookups;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderTime : ServiceOrderPos, IExportable
	{
		public virtual Guid? ArticleId { get; set; }
		public virtual Article Article { get; set; }
		public virtual DateTime? CompleteDate { get; set; }
		public virtual string TimeZone { get; set; }
		public virtual string CompleteUser { get; set; }
		public virtual Guid OrderId { get; set; }
		public virtual string PosNo { get; set; }
		public virtual string ItemNo { get; set; }
		public virtual string Description { get; set; }
		public virtual string Comment { get; set; }
		public virtual float? EstimatedDuration { get; set; }
		public virtual float? ActualDuration { get; set; }
		public virtual float? InvoiceDuration { get; set; }
		public virtual decimal? Price { get; set; }
		public virtual decimal? TotalValue { get; set; }
		public virtual decimal Discount { get; set; }
		public virtual DiscountType DiscountType { get; set; }
		public virtual string CausingItemNo { get; set; }
		public virtual string CausingItemSerialNo { get; set; }
		public virtual string CausingItemPreviousSerialNo { get; set; }
		public virtual string Diagnosis { get; set; }
		public virtual string StatusKey { get; set; }
		public virtual ServiceOrderTimeStatus Status { get { return StatusKey != null ? LookupManager.Get<ServiceOrderTimeStatus>(StatusKey) : null; } }

		public virtual string LocationKey { get; set; }
		public virtual ServiceOrderTimeLocation Location { get { return LocationKey != null ? LookupManager.Get<ServiceOrderTimeLocation>(LocationKey) : null; } }

		public virtual string PriorityKey { get; set; }
		public virtual ServiceOrderTimePriority Priority { get { return PriorityKey != null ? LookupManager.Get<ServiceOrderTimePriority>(PriorityKey) : null; } }

		public virtual string CategoryKey { get; set; }
		public virtual ServiceOrderTimeCategory Category { get { return CategoryKey != null ? LookupManager.Get<ServiceOrderTimeCategory>(CategoryKey) : null; } }

		public virtual string NoCausingItemSerialNoReasonKey { get; set; }
		public virtual NoCausingItemSerialNoReason NoCausingItemSerialNoReason { get { return NoCausingItemSerialNoReasonKey != null ? LookupManager.Get<NoCausingItemSerialNoReason>(NoCausingItemSerialNoReasonKey) : null; } }

		public virtual string NoCausingItemPreviousSerialNoReasonKey { get; set; }
		public virtual NoCausingItemPreviousSerialNoReason NoCausingItemPreviousSerialNoReason { get { return NoCausingItemPreviousSerialNoReasonKey != null ? LookupManager.Get<NoCausingItemPreviousSerialNoReason>(NoCausingItemPreviousSerialNoReasonKey) : null; } }

		public virtual bool IsCostLumpSum { get; set; }
		public virtual bool IsMaterialLumpSum { get; set; }
		public virtual bool IsTimeLumpSum { get; set; }
		public virtual string InvoicingTypeKey { get; set; }

		public virtual DateTime? TransferDate { get; set; }
		public virtual bool CreatedLocal { get; set; }
		public virtual bool HasTool { get; set; }
		public virtual Guid? InstallationPosId { get; set; }
		public virtual Guid? InstallationId { get; set; }
		public virtual Installation Installation { get; set; }

		public virtual ServiceOrderHead ServiceOrderHead { get; set; }
		public virtual ICollection<ServiceOrderTimeDispatch> ServiceOrderTimeDispatches { get; set; }
		public virtual ICollection<ServiceOrderTimePosting> Postings { get; set; }
		public virtual ICollection<ServiceCase> ServiceCases { get; set; }
		public virtual ICollection<ServiceOrderMaterial> ServiceOrderMaterials { get; set; }
		public virtual ICollection<ServiceOrderErrorType> ServiceOrderErrorTypes { get; set; }

		public virtual string ItemDescription
		{
			get {
				var localizedDescription = ItemNo != null ? LookupManager.Get<ArticleDescription>(ItemNo) : null;
				return localizedDescription != null && !String.IsNullOrWhiteSpace(localizedDescription.Value)
					? localizedDescription.Value
					: Article?.Description;
			}
		}
		public virtual bool IsExported { get; set; }

		public virtual string RichDescription
		{
			get
			{
				var description = new List<string>();

				if (Installation != null)
				{
					description.Add(Installation.FullDescription);
				}
				if (!string.IsNullOrEmpty(ItemNo))
				{
					description.Add(ItemNo);
				}
				if (!string.IsNullOrEmpty(Description))
				{
					description.Add(Description);
				}
				
				return description.ToString(" / ");
			}
		}

		public ServiceOrderTime()
		{
			StatusKey = ServiceOrderTimeStatus.CreatedKey;
			ServiceOrderTimeDispatches = new List<ServiceOrderTimeDispatch>();
			Postings = new List<ServiceOrderTimePosting>();
			ServiceCases = new List<ServiceCase>();
			ServiceOrderMaterials = new List<ServiceOrderMaterial>();
			ServiceOrderErrorTypes = new List<ServiceOrderErrorType>();
		}
	}
}
