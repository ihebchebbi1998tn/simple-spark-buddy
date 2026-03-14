namespace Crm.Service.Team.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.Model;
	using Crm.Service.Model;

	using Newtonsoft.Json;

	public class ServiceOrderDispatchExtension : EntityExtension<ServiceOrderDispatch>
	{
		[JsonIgnore]
		public virtual Usergroup Team { get; set; }
		public Guid? TeamId { get; set; }
	}
}
