namespace Crm.Service.BusinessRules.InstallationPositionRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Service.Model;
	public class BatchNoMaxLength : MaxLengthRule<InstallationPos>
	{
		public BatchNoMaxLength()
		{
			Init(p => p.BatchNo, 250);
		}
	}
}
