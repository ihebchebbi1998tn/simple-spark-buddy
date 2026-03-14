namespace Crm.Service.Model.Extensions
{
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Attributes;
	using Crm.Model.Notes;

	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	using Newtonsoft.Json;

	public class NoteExtensions : EntityExtension<Note>
	{
		[UI(Hidden = true)]
		public Guid? DispatchId { get; set; }
		[UI(UIignore = true), JsonIgnore, NotMapped]
		public virtual ServiceOrderDispatch ServiceOrderDispatch { get; set; }
	}
}
