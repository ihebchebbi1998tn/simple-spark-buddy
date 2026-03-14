namespace Crm.Service.BusinessRules.InstallationRules
{
	using System;

	using Crm.Library.Validation;
	using Crm.Service.Model;
	using Crm.Services.Interfaces;

	public class LocationPersonMustBePartOfLocationContactStaff : Rule<Installation>
	{
		private readonly IPersonService personService;

		// Methods
		protected override bool IsIgnoredFor(Installation installation)
		{
			return installation.LocationContactId.GetValueOrDefault() == Guid.Empty || installation.LocationPersonId.GetValueOrDefault() == Guid.Empty;
		}

		public override bool IsSatisfiedBy(Installation installation)
		{
			return personService.GetPerson(installation.LocationPersonId.Value).ParentId == installation.LocationContactId;
		}

		protected override RuleViolation CreateRuleViolation(Installation installation)
		{
			return RuleViolation(installation, i => i.LocationContactPerson, "LocationContactPerson");
		}

		// Constructor
		public LocationPersonMustBePartOfLocationContactStaff(IPersonService personService)
			: base(RuleClass.Undefined)
		{
			this.personService = personService;
		}
	}
}
