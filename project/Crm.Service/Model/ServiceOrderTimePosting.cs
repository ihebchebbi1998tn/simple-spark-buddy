namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Library.Model;
	using Crm.PerDiem.Model;

	public class ServiceOrderTimePosting : TimeEntry
	{
		public virtual Guid? ArticleId { get; set; }
		public virtual Article Article { get; set; }
		public virtual Guid OrderId { get; set; }
		public virtual ServiceOrderTimePosting ParentServiceOrderTimePosting { get; set; }
		public virtual Guid? ParentServiceOrderTimePostingId { get; set; }
		public virtual long? ParentServiceOrderTimePostingVersion { get; set; }
		public virtual ServiceOrderHead ServiceOrderHead { get; set; }
		public virtual Guid? OrderTimesId { get; set; }
		public virtual ServiceOrderTime ServiceOrderTime { get; set; }
		public virtual string OrderTimesIdAsString { get; set; }
		public virtual Decimal? Price { get; set; }
		public virtual float? DiscountPercent { get; set; }
		public virtual string InternalRemark { get; set; }
		public virtual string ServiceOrderTimePostingType { get; set; }
		public virtual Guid? UserId { get; set; }
		public virtual string UserUsername { get; set; }
		public virtual string UserDisplayName { get { return User != null ? User.DisplayName : null; } }
		public virtual User User { get; set; }

		public virtual string ItemNo{ get; set; }
		public virtual string ItemDescription
		{
			get {
				var localizedDescription = ItemNo != null ? LookupManager.Get<ArticleDescription>(ItemNo) : null;
				return localizedDescription != null && !String.IsNullOrWhiteSpace(localizedDescription.Value)
					? localizedDescription.Value
					: Article?.Description;
			}
		}

		public virtual Guid? DispatchId { get; set; }
		public virtual ServiceOrderDispatch ServiceOrderDispatch { get; set; }
		public virtual bool SignedByCustomer { get; set; }

		public virtual int? BreakInMinutes { get; set; }
		public virtual long Version { get; set; }
		public virtual ICollection<ServiceOrderTimePosting> ChildServiceOrderTimePostings { get; set; }
	}
}
