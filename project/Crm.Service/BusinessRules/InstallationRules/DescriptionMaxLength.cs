namespace Crm.Service.BusinessRules.InstallationRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class DescriptionMaxLength : MaxLengthRule<Installation>
	{
		public DescriptionMaxLength()
		{
			Init(i => i.Description, 450);
		}
	}
}