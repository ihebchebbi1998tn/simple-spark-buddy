namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class InstallationPos : EntityBase<Guid>, ISoftDelete
	{
		public virtual Article Article { get; set; }
		public virtual Guid? ArticleId { get; set; }
		public virtual Guid? RelatedInstallationId { get; set; }
		public virtual Installation RelatedInstallation { get; set; }
		public virtual Installation Installation { get; set; }
		public virtual Guid InstallationId { get; set; }
		public virtual string PosNo { get; set; }
		public virtual string ItemNo { get; set; }
		public virtual string Description { get; set; }
		public virtual string Supplier { get; set; }
		public virtual decimal Quantity { get; set; }
		public virtual string QuantityUnitKey { get; set; }
		public virtual QuantityUnit QuantityUnit
		{
			get { return QuantityUnitKey != null ? LookupManager.Get<QuantityUnit>(QuantityUnitKey) : null; }
		}
		public virtual Guid? QuantityUnitEntryKey { get; set; }
		public virtual QuantityUnitEntry QuantityUnitEntry { get; set; }
		public virtual bool IsInstalled { get; set; }
		public virtual DateTime? InstallDate { get; set; }
		public virtual string Comment { get; set; }
		public virtual DateTime? WarrantyStartSupplier { get; set; }
		public virtual DateTime? WarrantyEndSupplier { get; set; }
		public virtual DateTime? WarrantyStartCustomer { get; set; }
		public virtual DateTime? WarrantyEndCustomer { get; set; }
		public virtual string RdsPpClassification { get; set; }
		public virtual string LegacyInstallationId { get; set; }
		public virtual bool IsGroupItem { get; set; }
		public virtual int GroupLevel { get; set; }
		public virtual InstallationPos Parent { get; set; }
		public virtual Guid? ReferenceId { get; set; }
		public virtual DateTime? RemoveDate { get; set; }

		public virtual ICollection<InstallationPos> Children { get; set; }
		public virtual int ChildrenCount { get; set; }
		public virtual Guid ThumbnailFileResourceId { get; set; }
		public virtual string BatchNo { get; set; }
		public virtual string SerialNo { get; set; }
		public virtual bool IsSerial { get; set; }
	}
}
