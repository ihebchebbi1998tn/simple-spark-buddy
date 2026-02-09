namespace Crm.Service.BusinessRules.ServiceOrderMaterialRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ServiceOrderMaterialTypeRequired : RequiredRule<ServiceOrderMaterial>
	{
		public ServiceOrderMaterialTypeRequired()
		{
			Init(x => x.ServiceOrderMaterialType);
		}
	}
}
