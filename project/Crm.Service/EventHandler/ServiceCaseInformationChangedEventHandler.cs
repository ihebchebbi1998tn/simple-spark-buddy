namespace Crm.Service.EventHandler
{
	using System.Dynamic;
	using System.Linq;

	using Crm.Library.Modularization.Events;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Events;
	using Crm.Service.Model;
	using Crm.Service.Services.Interfaces;
	using Crm.Services.Interfaces;

	using Newtonsoft.Json;

	public class ServiceCaseInformationChangedEventHandler : IEventHandler<EntityModifiedEvent<ServiceCase>>
	{
		private readonly IEventAggregator eventAggregator;
		private readonly IUsergroupService userGroupService;
		private readonly IServiceObjectService serviceObjectService;
		private readonly IInstallationService installationService;
		private readonly ICompanyService companyService;
		private readonly IPersonService personService;
		private readonly IUserService userService;
		
		public ServiceCaseInformationChangedEventHandler(IEventAggregator eventAggregator, IUsergroupService userGroupService, IServiceObjectService serviceObjectService, IInstallationService installationService, ICompanyService companyService, IPersonService personService, IUserService userService)
		{
			this.eventAggregator = eventAggregator;
			this.userGroupService = userGroupService;
			this.serviceObjectService = serviceObjectService;
			this.installationService = installationService;
			this.companyService = companyService;
			this.personService = personService;
			this.userService = userService;
		}
		
		public virtual void Handle(EntityModifiedEvent<ServiceCase> e)
		{
			dynamic modifiedFields = new ExpandoObject();
			
			if (e.Entity.ResponsibleUser != e.EntityBeforeChange.ResponsibleUser)
			{
				modifiedFields.ResponsibleUser = userService.GetDisplayName(e.Entity.ResponsibleUser);
			}

			if (e.Entity.UserGroupKey != e.EntityBeforeChange.UserGroupKey)
			{
				if (e.Entity.UserGroupKey.HasValue)
				{
					var userGroup = userGroupService.GetUsergroup(e.Entity.UserGroupKey.Value);
					modifiedFields.UserGroup = userGroup.Name;
				}
				else
				{
					modifiedFields.UserGroup = null;
				}
			}

			if (e.Entity.Priority != e.EntityBeforeChange.Priority)
			{
				modifiedFields.Priority = e.Entity.Priority != null ? e.Entity.Priority.ToString() : null;
			}

			if (e.Entity.Category != e.EntityBeforeChange.Category)
			{
				modifiedFields.Category = e.Entity.Category != null ? e.Entity.Category.ToString() : null;
			}
			
			if (e.Entity.ServiceObjectId != e.EntityBeforeChange.ServiceObjectId)
			{
				if (e.Entity.ServiceObjectId.HasValue)
				{
					var serviceObject = serviceObjectService.GetServiceObject(e.Entity.ServiceObjectId.Value);
					modifiedFields.ServiceObject = serviceObject.Name;
				}
				else
				{
					modifiedFields.ServiceObject = null;
				}
			}

			if (e.Entity.AffectedCompanyKey != e.EntityBeforeChange.AffectedCompanyKey)
			{
				if (e.Entity.AffectedCompanyKey.HasValue)
				{
					var company = companyService.GetCompany(e.Entity.AffectedCompanyKey.Value);
					modifiedFields.AffectedCompany = company.Name;
				}
				else
				{
					modifiedFields.AffectedCompany = null;
				}
			}

			if (e.Entity.ContactPersonId != e.EntityBeforeChange.ContactPersonId)
			{
				if (e.Entity.ContactPersonId.HasValue)
				{
					var person = personService.GetPerson(e.Entity.ContactPersonId.Value);
					modifiedFields.ContactPerson = person.Name;
				}
				else
				{
					modifiedFields.ContactPerson = null;
				}
			}
			
			if (e.Entity.AffectedInstallationKey != e.EntityBeforeChange.AffectedInstallationKey)
			{
				if (e.Entity.AffectedInstallationKey.HasValue)
				{
					var installation = installationService.GetInstallation(e.Entity.AffectedInstallationKey.Value);
					modifiedFields.AffectedInstallation = installation.Name;
				}
				else
				{
					modifiedFields.AffectedInstallation = null;
				}
			}
			
			if ((modifiedFields as ExpandoObject).Any())
			{
				string json = JsonConvert.SerializeObject(modifiedFields, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
	 			eventAggregator.Publish(new ServiceCaseInformationChangedEvent(e.Entity, json));
			}
		}
	}
}
