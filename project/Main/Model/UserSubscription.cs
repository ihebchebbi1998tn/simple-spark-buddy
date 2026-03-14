namespace Main.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class UserSubscription : EntityBase<Guid>, ISoftDelete
	{
		public virtual string Username { get; set; }
		public virtual string EntityType { get; set; }
		public virtual Guid EntityKey { get; set; }
		public virtual bool IsSubscribed { get; set; }
	}
}
