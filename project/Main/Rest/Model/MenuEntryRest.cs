namespace Main.Rest.Model
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	using Crm.Library.Rest;

	using Main.Model;

	[RestTypeFor(DomainType = typeof(MenuEntry))]
	public class MenuEntryRest : RestEntity
	{
		public Guid Id { get; set; }
		public virtual string Category { get; set; }
		public virtual string Title { get; set; }
		public virtual int Priority { get; set; }
		[NotMapped]
		public override bool IsActive { get; set; }
		public virtual string IconClass { get; set; }
	}
}
