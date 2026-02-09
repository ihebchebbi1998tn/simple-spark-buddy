namespace Crm.Service.Model;

using Crm.Article.Model;
using Crm.Library.BaseModel;

public class ArticleExtension : EntityExtension<Article>
{
	public virtual bool CanBeAddedAfterCustomerSignature { get; set; }
	public virtual bool IsDefaultForServiceOrderTimes { get; set; }
	public virtual bool IsHidden { get; set; }
	public virtual bool ShowDistanceInput { get; set; }
	public virtual bool IsServiceOrderExpense { get; set; }
	public virtual bool CanBeEditedAfterCustomerSignature { get; set; }

}
