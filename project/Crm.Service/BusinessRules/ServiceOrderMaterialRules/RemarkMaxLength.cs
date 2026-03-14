using Crm.Service.Model;

namespace Crm.Service.BusinessRules.ServiceOrderMaterialRules
{
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;

	[Rule]
	public class InternalRemarkMaxLength : MaxLengthRule<ServiceOrderMaterial>
	{
		// Constructor
		public InternalRemarkMaxLength()
		{
			Init(d => d.InternalRemark, 4000);
		}
	}
	
	[Rule]
	public class ExternalRemarkMaxLength : MaxLengthRule<ServiceOrderMaterial>
	{
		// Constructor
		public ExternalRemarkMaxLength()
		{
			Init(d => d.ExternalRemark, 4000);
		}
	}
}
