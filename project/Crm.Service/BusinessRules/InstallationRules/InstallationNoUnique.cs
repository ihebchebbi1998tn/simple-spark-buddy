namespace Crm.Service.BusinessRules.InstallationRules
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Validation;
	using Crm.Service.Model;

	public class InstallationNoUnique : Rule<Installation>
	{
		private readonly IRepositoryWithTypedId<Installation, Guid> installationRepository;

		// Methods
		protected override bool IsIgnoredFor(Installation installation)
		{
			return installation.InstallationNo.IsNullOrEmpty();
		}

		public override bool IsSatisfiedBy(Installation installation)
		{
			return !installationRepository.GetAll().Any(x => x.InstallationNo == installation.InstallationNo && x.Id != installation.Id);
		}

		protected override RuleViolation CreateRuleViolation(Installation installation)
		{
			return RuleViolation(installation, i => i.InstallationNo);
		}

		// Constructor
		public InstallationNoUnique(IRepositoryWithTypedId<Installation, Guid> installationRepository)
			: base(RuleClass.Unique)
		{
			this.installationRepository = installationRepository;
		}
	}
}
