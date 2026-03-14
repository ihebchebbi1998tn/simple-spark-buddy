namespace Crm.Service.BusinessRules.ServiceOrderMaterialRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class DescriptionMaxLength : MaxLengthRule<ServiceOrderMaterial>
	{
		public DescriptionMaxLength()
		{
			Init(d => d.Description, 500);
		}
	}
}