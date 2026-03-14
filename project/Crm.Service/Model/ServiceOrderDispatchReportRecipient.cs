namespace Crm.Service.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class ServiceOrderDispatchReportRecipient : EntityBase<Guid>, INoAuthorisedObject, ISoftDelete
	{
		public virtual ServiceOrderDispatch Dispatch { get; set; }
		public virtual Guid DispatchId { get; set; }
		public virtual string Email { get; set; }
		public virtual bool Internal { get; set; }
		public virtual string Language { get; set; }
		public virtual string Locale { get; set; }
	}
}