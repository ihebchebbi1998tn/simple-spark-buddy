namespace Sms.Scheduler.Model
{
	using Crm.Library.BaseModel;
	using Crm.Library.Model;

	public class UserExtension : EntityExtension<User>
	{
		public virtual int? ActiveProfileId { get; set; }
	}
}
