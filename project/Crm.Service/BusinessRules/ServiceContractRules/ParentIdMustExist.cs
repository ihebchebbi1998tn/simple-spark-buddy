namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using Crm.Library.Extensions;
	using Crm.Library.Validation;
	using Crm.Service.Model;
	using Crm.Services.Interfaces;

	public class ParentIdMustExist : Rule<ServiceContract>
	{
		private readonly ICompanyService companyService;

		// Methods
		protected override bool IsIgnoredFor(ServiceContract contract)
		{
			return contract.ParentId.GetValueOrDefault().EqualsDefault();
		}

		public override bool IsSatisfiedBy(ServiceContract contract)
		{
			return companyService.DoesCompanyExist(contract.ParentId.Value);
		}

		protected override RuleViolation CreateRuleViolation(ServiceContract contract)
		{
			return RuleViolation(contract, c => c.ParentId, "BusinessPartner");
		}

		// Constructor
		public ParentIdMustExist(ICompanyService companyService)
			: base(RuleClass.MustExist)
		{
			this.companyService = companyService;
		}
	}
}
