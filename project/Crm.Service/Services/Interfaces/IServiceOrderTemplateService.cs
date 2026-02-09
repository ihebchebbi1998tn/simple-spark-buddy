namespace Crm.Service.Services.Interfaces
{
	using Crm.Library.AutoFac;
	using Crm.Service.Model;
	using Crm.Service.Model.Relationships;

	public interface IServiceOrderTemplateService : IDependency
	{
		int Priority { get; }
		void CreateTemplateData(ServiceOrderHead serviceOrder, ServiceOrderHead serviceOrderTemplate, Installation installation, ServiceContractInstallationRelationship relationship = null);
	}
}