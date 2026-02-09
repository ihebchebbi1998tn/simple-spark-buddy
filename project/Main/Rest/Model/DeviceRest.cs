namespace Main.Rest.Model
{
	using System.ComponentModel.DataAnnotations.Schema;

	using Crm.Library.Model;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Device))]
	public class DeviceRest : RestEntityWithExtensionValues
	{
		public string Fingerprint { get; set; }
		public string Token { get; set; }
		public string Username { get; set; }
		public string DeviceInfo { get; set; }
		[NotMapped] public override bool IsActive { get; set; }
	}
}
