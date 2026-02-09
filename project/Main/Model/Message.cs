namespace Main.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.BaseClasses;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	using Main.Model.Enums;

	public class Message : EntityBase<Guid>, ISoftDelete
	{
		public virtual string From { get; set; }
		public virtual List<string> Recipients { get; set; }
		public virtual List<string> Bcc { get; set; }
		public virtual string Subject { get; set; }
		public virtual string Body { get; set; }
		public virtual bool IsBodyHtml { get; set; }
		public virtual MessageState State { get; set; }
		public virtual ICollection<Guid> AttachmentIds { get; set; }
		public virtual string ErrorMessage { get; set; }
		public virtual string LanguageKey { get; set; }

		public Message()
		{
			Recipients = new List<string>();
			Bcc = new List<string>();
			Body = String.Empty;
			State = MessageState.Pending;
			AttachmentIds = new LazyList<Guid>();
			LanguageKey = "de";
		}
	}
}
