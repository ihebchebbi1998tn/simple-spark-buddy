namespace Crm.Service.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using Crm.Library.AutoFac;
	using Model;

	public interface IInstallationService : IDependency
	{
		InstallationPos GetInstallationPosition(Guid id);
		IEnumerable<string> GetUsedInstallationHeadStatuses();
		IEnumerable<string> GetUsedInstallationTypes();
		IEnumerable<string> GetUsedManufacturers();
		IEnumerable<string> GetUsedInstallationAddressRelationshipTypes();
		IEnumerable<string> GetUsedQuantityUnits();
		Installation GetInstallation(Guid id);
	}
}
