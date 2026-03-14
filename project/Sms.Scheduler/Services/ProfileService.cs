namespace Sms.Scheduler.Services
{
	using System;
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;

	using Newtonsoft.Json;

	using NHibernate.Linq;

	using NJsonSchema;

	using Sms.Scheduler.Model;

	public class ProfileService : IDependency
	{
		private readonly IRepositoryWithTypedId<Profile, int> profileRepository;
		private readonly IRepositoryWithTypedId<ProfileResource, int> profileResourceRepository;
		public ProfileService(IRepositoryWithTypedId<Profile, int> profileRepository, IRepositoryWithTypedId<ProfileResource, int> profileResourceRepository)
		{
			this.profileRepository = profileRepository;
			this.profileResourceRepository = profileResourceRepository;
		}
		protected virtual IQueryable<Profile> Query => profileRepository.Session.Query<Profile>()
			.Fetch(x => x.User)
			.OrderBy(x => x.ModifyDate);
		public virtual Profile GetDefaultProfile(string username)
		{
			var profiles = Query.Where(x => x.DefaultProfile && x.Username == username).ToList();
			if (profiles.Count > 1)
			{
				ResetDefaultProfile(username, profiles.Last().Id);
			}
			return profiles.LastOrDefault();
		}
		public virtual Profile CreateDefaultProfile(User user)
		{
			var result = new Profile
			{
				DefaultProfile = true,
				Name = "New Profile",
				Username = user.Id,
				ClientConfig = GetDefaultClientConfigJson()
			};
			return result;
		}

		public virtual Profile GetProfile(int id)
		{
			var profile = Query.SingleOrDefault(x => x.Id == id);
			return profile;
		}
		public virtual void ResetDefaultProfile(string username, int profileId) => profileRepository.Session.Query<Profile>().Where(x => x.Username == username && x.Id != profileId).Update(x => new { DefaultProfile = false });
		public virtual void SaveProfile(Profile profile)
		{
			profileRepository.Session.Clear();
			profileRepository.SaveOrUpdate(profile);
		}

		private static readonly Lazy<string> defaultClientConfigJson = new(() =>
		{
			var schema = JsonSchema.FromType<ClientConfig>();
			return schema.ToSampleJson().ToString();
		});
		private static readonly Lazy<ClientConfig> defaultClientConfig = new(() => JsonConvert.DeserializeObject<ClientConfig>(defaultClientConfigJson.Value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None }));

		public virtual ClientConfig GetDefaultClientConfig() => defaultClientConfig.Value;
		public virtual string GetDefaultClientConfigJson() => defaultClientConfigJson.Value;

		public virtual void RemoveRelatedResources(Profile profile) => profileResourceRepository.Session.Query<ProfileResource>().Where(x => x.ProfileKey == profile.Id).Delete();
	}

}
