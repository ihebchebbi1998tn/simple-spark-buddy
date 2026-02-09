namespace Main.BusinessRules.LicenseRules
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Licensing;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Validation;

	using User = Crm.Library.Model.User;

	public class UserLicenseCountRule : Rule<User>
	{
		private readonly ILicensingService licensingService;
		private readonly IEnvironment environment;
		private readonly IRepositoryWithTypedId<User, Guid> userRepository;

		public override bool IsSatisfiedBy(User user)
		{
			var userLicenseCount = licensingService.GetUserLicenseCount(environment.GetDomainId().ToString());
			var licensedUsernames = userRepository.GetAll()
				.Where(x => x.LicensedAt != null && !x.Discharged)
				.Select(x => x.Id)
				.ToList();

			if (!licensedUsernames.Contains(user.Id) && user.LicensedAt != null && !user.Discharged)
			{
				licensedUsernames.Add(user.Id);
			} 
			return licensedUsernames.Count() <= userLicenseCount;
		}

		protected override RuleViolation CreateRuleViolation(User user)
		{
			return RuleViolation(user, x => x.LicensedAt);
		}

		public UserLicenseCountRule(ILicensingService licensingService, IEnvironment environment, IRepositoryWithTypedId<User, Guid> userRepository)
			: base(RuleClass.MustExist)
		{
			this.licensingService = licensingService;
			this.environment = environment;
			this.userRepository = userRepository;
		}
	}
}
