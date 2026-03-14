namespace Main.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class MenuEntry : EntityBase<Guid>, INoAuthorisedObject
	{
		public virtual string Category { get; set; }
		public virtual string Title { get; set; }
		public virtual int Priority { get; set; }
		public virtual string IconClass { get; set; }
	}
}
