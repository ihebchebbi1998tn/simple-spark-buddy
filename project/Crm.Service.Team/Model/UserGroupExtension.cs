namespace Crm.Service.Team.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.Model;

	public class UserGroupExtension : EntityExtension<Usergroup>
	{
		public bool IsServiceTeam { get; set; }
		public string MainResourceId { get; set; }
		public DateTime? ValidFrom { get; set; }
		public DateTime? ValidTo { get; set; }
	}
}
