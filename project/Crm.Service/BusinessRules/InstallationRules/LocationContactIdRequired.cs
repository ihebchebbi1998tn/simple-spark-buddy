namespace Crm.Service.BusinessRules.InstallationRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class LocationContactIdRequired : RequiredRule<Installation>
	{
		public LocationContactIdRequired()
		{
			Init(i => i.LocationContactId);
		}
	}
}