namespace Crm.Service.BusinessRules.ServiceContractRules
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class ContractNoMustBeUnique : Rule<ServiceContract>
	{
		private readonly IRepositoryWithTypedId<ServiceContract, Guid> serviceContractRepository;

		protected override bool IsIgnoredFor(ServiceContract serviceContract)
		{
			return serviceContract.ContractNo.IsNullOrEmpty();
		}

		public override bool IsSatisfiedBy(ServiceContract serviceContract)
		{
			return !serviceContractRepository.GetAll().Any(x => x.ContractNo == serviceContract.ContractNo && x.Id != serviceContract.Id);
		}

		protected override RuleViolation CreateRuleViolation(ServiceContract serviceContract)
		{
			return RuleViolation(serviceContract, s => s.ContractNo);
		}

		public ContractNoMustBeUnique(IRepositoryWithTypedId<ServiceContract, Guid> serviceContractRepository)
			: base(RuleClass.Unique)
		{
			this.serviceContractRepository = serviceContractRepository;
		}
	}
}
