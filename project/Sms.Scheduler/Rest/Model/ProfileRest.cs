namespace Sms.Scheduler.Rest.Model
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;
	using Crm.Rest.Model;

	using Main.Rest.Model;

	using Sms.Scheduler.Model;

	[RestTypeFor(DomainType = typeof(Profile))]
	public class ProfileRest : IRestEntity, IAuthorisedRestEntity
	{
		public int Id { get; set; }
		public virtual Guid InternalId { get; set; }
		public virtual string Username { get; set; }
		public virtual string Name { get; set; }
		public virtual bool DefaultProfile { get; set; }
		public virtual int TemplateKey { get; set; }
		[RestrictedField] public virtual ClientConfig ClientConfig { get; set; }
		[ExplicitExpand, NotReceived] public virtual UserRest User { get; set; }
		[RestrictedField, NotReceived] public virtual Guid? DomainId { get; set; }
		[RestrictedField] public string[] ResourceKeys { get; set; }
	}
}
