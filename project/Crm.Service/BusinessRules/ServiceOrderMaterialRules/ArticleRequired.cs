namespace Crm.Service.BusinessRules.ServiceOrderMaterialRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ArticleRequired : RequiredRule<ServiceOrderMaterial>
	{
		public ArticleRequired()
		{
			Init(e => e.ArticleId, "ItemNo");
		}
		public override bool IsSatisfiedBy(ServiceOrderMaterial entity) => string.IsNullOrWhiteSpace(entity.ItemNo) == false;
	}
}
