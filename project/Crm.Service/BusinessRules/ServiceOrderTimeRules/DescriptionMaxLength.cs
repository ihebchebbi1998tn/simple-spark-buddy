namespace Crm.Service.BusinessRules.ServiceOrderTimeRules
{
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	[Rule]
	public class DescriptionMaxLength : MaxLengthRule<ServiceOrderTime>
	{
		public DescriptionMaxLength()
		{
			Init(d => d.Description, 500);
		}
	}
}
