namespace Main.Rest.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;

	using Main.Model;
	using Main.Model.Enums;

	[RestTypeFor(DomainType = typeof(Message))]
	public class MessageRest : RestEntityWithExtensionValues
	{
		public ICollection<Guid> AttachmentIds { get; set; }
		public List<string> Bcc { get; set; }
		public string Body { get; set; }
		public string ErrorMessage { get; set; }
		public string From { get; set; }
		public bool IsBodyHtml { get; set; }
		public string[] Recipients { get; set; }
		public MessageState State { get; set; }
		public string Subject { get; set; }
	}
}
