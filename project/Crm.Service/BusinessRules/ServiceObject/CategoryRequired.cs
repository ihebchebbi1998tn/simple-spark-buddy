namespace Crm.Service.BusinessRules.ServiceObject
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class CategoryRequired : RequiredRule<ServiceObject>
	{
		// Constructor
		public CategoryRequired()
		{
			Init(x => x.CategoryKey, propertyNameReplacementKey: "Category");
		}
	}
}