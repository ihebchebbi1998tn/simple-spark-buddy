namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Article.Model;
	using Crm.Article.Model.Enums;
	using Crm.Article.Model.Lookups;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Model;
	using Crm.Service.Model.Lookup;

	public class ServiceOrderMaterial : ServiceOrderPos, IExportable
	{
		public virtual Guid? ArticleId { get; set; }
		public virtual Article Article { get; set; }
		public virtual Guid OrderId { get; set; }
		public virtual ServiceOrderType OrderType { get; set; }
		public virtual string PosNo { get; set; }
		public virtual string ItemNo { get; set; }
		public virtual string Description { get; set; }
		public virtual string ArticleTypeKey { get; set; }
		public virtual string InternalRemark { get; set; }
		public virtual string ExternalRemark { get; set; }
		public virtual ServiceOrderMaterial ParentServiceOrderMaterial { get; set; }
		public virtual Guid? ParentServiceOrderMaterialId { get; set; }
		public virtual long? ParentServiceOrderMaterialVersion { get; set; }
		public virtual decimal Quantity { get; set; }
		public virtual string QuantityUnitKey { get; set; }
		public virtual QuantityUnit QuantityUnit { get { return QuantityUnitKey != null ? LookupManager.Get<QuantityUnit>(QuantityUnitKey) : null; } }
		public virtual Guid? QuantityUnitEntryKey { get; set; }
		public virtual QuantityUnitEntry QuantityUnitEntry { get; set; }
		public virtual decimal? Price { get; set; }
		public virtual string ServiceOrderMaterialType { get; set; }
		public virtual decimal? TotalValue { get; set;}
		public virtual decimal Discount { get; set; }
		public virtual DiscountType DiscountType { get; set; }
		public virtual string FromWarehouse { get; set; }
		public virtual string FromLocation { get; set; }
		public virtual string FromStorageArea { get; set; }
		public virtual string ToWarehouse { get; set; }
		public virtual string ToLocation { get; set; }
		public virtual int Status { get; set; }
		public virtual DateTime? TransferDate { get; set; }
		public virtual bool BuiltIn { get; set; }
		public virtual bool IsSerial { get; set; }
		public virtual bool CreatedLocal { get; set; }
		public virtual string CommissioningStatusKey { get; set; }
		public virtual CommissioningStatus CommissioningStatus
		{
			get { return CommissioningStatusKey != null ? LookupManager.Get<CommissioningStatus>(CommissioningStatusKey) : null; }
		}
		public virtual Guid? ServiceOrderTimeId { get; set; }
		public virtual ServiceOrderTime ServiceOrderTime { get; set; }
		public virtual ServiceOrderHead ServiceOrderHead { get; set; }
		public virtual Guid? ReplenishmentOrderItemId { get; set; }
		public virtual ReplenishmentOrderItem ReplenishmentOrderItem { get; set; }
		public virtual int SerialCount { get; set; }

		public virtual int DocsCount { get; set; }
		public virtual string ItemDescription
		{
			get {
				var localizedDescription = ItemNo != null ? LookupManager.Get<ArticleDescription>(ItemNo) : null;
				return localizedDescription != null && !String.IsNullOrWhiteSpace(localizedDescription.Value)
									? localizedDescription.Value
									: Article?.Description;
			}
		}
		public virtual bool IsExported {get; set; }
		public virtual Guid? DispatchId { get; set; }
		public virtual ServiceOrderDispatch ServiceOrderDispatch { get; set; }
		public virtual bool SignedByCustomer { get; set; }
		public virtual string SerialNo { get; set; }
		public virtual string PreviousSerialNo { get; set; }
		public virtual Guid? SerialId { get; set; }
		public virtual Serial Serial { get; set; }
		public virtual string BatchNo { get; set; }
		public virtual bool IsBatch { get; set; }
		public virtual string NoPreviousSerialNoReasonKey { get; set; }
		public virtual bool? IsInstalled { get; set; }
		public virtual long Version { get; set; }
		public virtual ICollection<ServiceOrderMaterial> ChildServiceOrderMaterials { get; set; }
		public virtual ICollection<DocumentAttribute> DocumentAttributes { get; set; }

	}
}
