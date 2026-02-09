namespace Crm.Service.BusinessRules.ServiceOrderTimeRules
{
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;

	[Rule]
	public class CommentMaxLength : MaxLengthRule<ServiceOrderTime>
	{
		// Constructor
		public CommentMaxLength()
		{
			Init(d => d.Comment, 4000);
		}
	}
}
