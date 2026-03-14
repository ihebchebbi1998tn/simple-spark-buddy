namespace Sms.Scheduler.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	using LMobile.Unicore;

	public class ProfileResource : EntityBase<int>
	{
		public virtual string ResourceKey { get; set; }
		public virtual int ProfileKey { get; set; }
		public virtual int? SortOrder { get; set; }
	}
}
