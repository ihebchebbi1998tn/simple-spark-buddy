namespace Crm.Service.Model
{
	using System;

	public class ServiceContractInstallationSummary
	{
		public Guid InstallationId { get; set; }
		public string InstallationNo { get; set; }
		public string Description { get; set; }
		public Guid ServiceContractId { get; set; }
	}
}
