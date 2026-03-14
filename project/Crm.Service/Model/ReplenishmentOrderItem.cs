namespace Crm.Service.Model
{
	using Crm.Article.Model.Lookups;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Extensions;
	using System;
	using System.Collections.Generic;

	using Crm.Article.Model;

	public class ReplenishmentOrderItem : EntityBase<Guid>, ISoftDelete
	{
		public virtual Guid ReplenishmentOrderId { get; set; }
		public virtual ReplenishmentOrder ReplenishmentOrder { get; set; }

		public virtual Guid? ArticleId { get; set; }
		public virtual Article Article { get; set; }
		public virtual string MaterialNo { get; set; }
		public virtual string Description { get; set; }
		public virtual decimal Quantity { get; set; }
		public virtual string QuantityUnitKey { get; set; }
		public virtual Guid? QuantityUnitEntryKey { get; set; }
		public virtual QuantityUnitEntry QuantityUnitEntry { get; set; }
		public virtual string Remark { get; set; }
		public virtual ICollection<ServiceOrderMaterial> ServiceOrderMaterials { get; set; } = new List<ServiceOrderMaterial>();

		public virtual QuantityUnit QuantityUnit
		{
			get { return QuantityUnitKey != null ? LookupManager.Get<QuantityUnit>(QuantityUnitKey) : null; }
		}
		public virtual string QuantityUnitString => QuantityUnit?.Value ?? Article?.QuantityUnit?.Value ?? QuantityUnitKey;

		public virtual string MaterialNoWithDescription
		{
			get { return "{0} - {1}".WithArgs(MaterialNo, Description); }
		}
	}
}
