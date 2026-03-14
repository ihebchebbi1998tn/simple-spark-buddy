namespace Crm.Service.BusinessRules.InstallationPositionRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class QuantityRequired : RequiredRule<InstallationPos>
	{
		public QuantityRequired()
		{
			Init(x => x.Quantity);
		}

		public override bool IsSatisfiedBy(InstallationPos pos)
		{
			return pos.Quantity != 0;
		}
	}
}
