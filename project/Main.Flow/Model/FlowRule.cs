using Crm.Library.BaseModel;
using System;

namespace Main.Flow.Model 
{
	using Crm.Library.BaseModel.Interfaces;

	public enum Actions
	{
		Created,
		Modified,
		Deleted
	}

	public class FlowRule : EntityBase<Guid>, ISoftDelete
	{
		public virtual string EntityType { get; set; }
		public virtual Actions Action { get; set; }
		public virtual string Description { get; set; }
		public virtual string Endpoint { get; set; }
		public virtual string Username { get; set; }
		public virtual string Password { get; set; }
	}
}
