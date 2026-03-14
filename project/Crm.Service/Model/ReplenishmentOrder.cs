namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Model;

	public class ReplenishmentOrder : EntityBase<Guid>, ISoftDelete
	{
		public virtual string ResponsibleUser { get; set; }
		public virtual User ResponsibleUserObject { get; set; }
		public virtual bool IsClosed { get; set; }
		public virtual DateTime? CloseDate { get; set; }
		public virtual bool IsSent { get; set; }
		public virtual string SendingError { get; set; }
		public virtual ICollection<ReplenishmentOrderItem> Items { get; set; }
		public virtual string ClosedBy { get; set; }
		public virtual bool IsExported { get; set; }
		public ReplenishmentOrder()
		{
			Items = new List<ReplenishmentOrderItem>();
		}
	}
}
