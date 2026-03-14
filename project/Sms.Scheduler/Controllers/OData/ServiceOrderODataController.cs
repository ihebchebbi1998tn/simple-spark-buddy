namespace Sms.Scheduler.Controllers.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Crm.Library.Api;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Service;
	using Crm.Service.Enums;
	using Crm.Service.Model;
	using Crm.Service.Rest.Model;

	using Microsoft.AspNetCore.OData.Query;
	using Microsoft.AspNetCore.Mvc;

	[ControllerName("CrmService_ServiceOrderHead")]
	public class ServiceOrderODataController : ODataControllerEx, IEntityApiController
	{
		private readonly IAppSettingsProvider appSettingsProvider;
		public ServiceOrderODataController(IAppSettingsProvider appSettingsProvider)
		{
			this.appSettingsProvider = appSettingsProvider;
		}
		public Type EntityType => typeof(ServiceOrderHead);
		[HttpGet]
		public virtual IActionResult GetGroupableProperties(ODataQueryOptions<ServiceOrderHeadRest> options)
		{
			var properties = new List<PropertyInfo>(typeof(ServiceOrderHead).GetProperties());
			var result = new List<string>();
			var maintenanceOrderGenerationMode = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.MaintenanceOrderGenerationMode);

			foreach (var prop in properties)
			{
				if (!prop.HasAttribute<GroupProperty>())
				{
					continue;
				}
				var value = prop.Name;
				if (value == "CustomerContact")
				{
					value = "Company";
				}
				if (value == "CountryKey")
				{
					value = "Country";
				}
				if (value == "AffectedInstallation")
				{
					value = "InstallationNo";
				}
				if (value == "ServiceContract")
				{
					value = "ServiceContractNo";
				}
				if (value == "ServiceObject")
				{
					value = "ServiceObjectNo";
				}
				result.Add("ServiceOrder." + value);
			}
			result.Add("ServiceOrder.CustomerNo");
			result.Add("ServiceOrder.StatusGroup");
			result.Add("ServiceOrder.MaterialAvailability");
			result.Add("ServiceOrder.ResponsibleUser");
			result.Add("ServiceOrder.ZipCodeArea");
			result.Add("ServiceOrder.DisplayPreferredUser");
			if (maintenanceOrderGenerationMode != MaintenanceOrderGenerationMode.JobPerInstallation)
			{
				result.Add("Installation.InstallationStatus");
				result.Add("Installation.InstallationType");
				result.Add("Installation.InstallationDescription");
			}
			result.Add("ServiceObject.ServiceObjectCategory");
			result.Add("ServiceObject.ServiceObjectName");

			if (options.Filter != null)
			{
				var settings = new ODataQuerySettings { EnableConstantParameterization = false, HandleNullPropagation = HandleNullPropagationOption.False };
				var queryable = result.Select(x => new ServiceOrderHeadRest { Name = x }).AsQueryable();
				var filteredNames = ((IEnumerable<ServiceOrderHeadRest>)options.Filter.ApplyTo(queryable, settings)).Select(x => x.Name).ToList();
				result = result.Where(x => filteredNames.Contains(x)).Select(x => x).ToList();
			}
			
			return Ok(result);
		}
	}
}
