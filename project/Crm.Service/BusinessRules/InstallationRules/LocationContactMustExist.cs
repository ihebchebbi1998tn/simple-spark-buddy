namespace Crm.Service.BusinessRules.InstallationRules
{
	using Crm.Library.Extensions;
	using Crm.Library.Validation;
	using Crm.Service.Model;
	using Crm.Services.Interfaces;

	public class LocationContactMustExist : Rule<Installation>
	{
		private readonly IContactService contactService;

		// Methods
		protected override bool IsIgnoredFor(Installation installation)
		{
			return installation.LocationContactId.EqualsDefault();
		}

		public override bool IsSatisfiedBy(Installation installation)
		{
			return !installation.LocationContactId.HasValue || contactService.DoesContactExist(installation.LocationContactId.Value);
		}

		protected override RuleViolation CreateRuleViolation(Installation installation)
		{
			return RuleViolation(installation, i => i.LocationContactId, "LocationContact");
		}

		// Constructor
		public LocationContactMustExist(IContactService contactService)
			: base(RuleClass.MustExist)
		{
			this.contactService = contactService;
		}
	}
}
