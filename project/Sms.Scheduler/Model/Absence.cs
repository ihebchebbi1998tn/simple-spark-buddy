namespace Sms.Scheduler.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.PerDiem.Model.Lookups;

	public class Absence : EntityBase<Guid>, ISoftDelete
	{
		public virtual string Description { get; set; }
		public virtual string TimeEntryTypeKey { get; set; }
		public virtual string ResponsibleUser { get; set; }
		public virtual DateTime From { get; set; } // From and To are intended to save the start and end time.
		public virtual DateTime To { get; set; }

		public virtual TimeEntryType TimeEntryType => TimeEntryTypeKey != null ? LookupManager.Get<TimeEntryType>(TimeEntryTypeKey) : null;
	}
}
