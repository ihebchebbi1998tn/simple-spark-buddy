namespace Crm.Service.BusinessRules.InstallationPositionRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	public class ArticleRequired : RequiredRule<InstallationPos>
	{
		public ArticleRequired()
		{
			Init(x => x.ArticleId, "Material");
		}
		public override bool IsSatisfiedBy(InstallationPos entity) => string.IsNullOrWhiteSpace(entity.ItemNo) == false;
	}
}
