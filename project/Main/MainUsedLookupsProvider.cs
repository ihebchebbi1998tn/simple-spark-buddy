namespace Main
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Dynamic.Core;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Globalization;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Services.Interfaces;

	using Main.Model;
	using Main.Model.Lookups;

	public class MainUsedLookupsProvider : IUsedLookupsProvider
	{
		private readonly IUserService userService;
		private readonly IRepositoryWithTypedId<UserSkill, Guid> userSkillRepository;
		private readonly IRepositoryWithTypedId<UserAsset, Guid> userAssetRepository;
		public MainUsedLookupsProvider(IUserService userService, IRepositoryWithTypedId<UserSkill, Guid> userSkillRepository, IRepositoryWithTypedId<UserAsset, Guid> userAssetRepository)
		{
			this.userService = userService;
			this.userSkillRepository = userSkillRepository;
			this.userAssetRepository = userAssetRepository;
		}
		public virtual IEnumerable<object> GetUsedLookupKeys(Type lookupType)
		{
			if (lookupType == typeof(Skill))
			{
				return userSkillRepository.GetAll().Select(x => x.SkillKey).Distinct();
			}
			if (lookupType == typeof(Asset))
			{
				return userAssetRepository.GetAll().Select(x => x.AssetKey).Distinct();
			}

			if (lookupType == typeof(UserStatus))
			{
				return userService.GetUsers().Select(x => x.StatusKey).Distinct();
			}

			if (lookupType == typeof(Language))
			{
				return userService.GetUsers().Select(x => x.DefaultLanguageKey).Distinct();
			}

			return new List<object>();
		}
	}
}
