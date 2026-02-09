namespace Crm.Service.BusinessRules.InstallationPositionRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;
	public class DescriptionRequired : RequiredRule<InstallationPos>
	{
		public DescriptionRequired()
		{
			Init(i => i.Description);
		}

		protected override bool IsIgnoredFor(InstallationPos pos)
		{
			return !pos.IsGroupItem;
		}
	}
}