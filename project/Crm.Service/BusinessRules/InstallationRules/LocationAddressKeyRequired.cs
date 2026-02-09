namespace Crm.Service.BusinessRules.InstallationRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class LocationAddressKeyRequired : RequiredRule<Installation>
	{
		protected override bool IsIgnoredFor(Installation installation)
		{
			return !installation.LocationContactId.HasValue;
		}

		public LocationAddressKeyRequired()
		{
			Init(i => i.LocationAddressKey);
		}
	}
}