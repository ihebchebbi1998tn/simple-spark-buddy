namespace Sms.Scheduler.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Library.Extensions;

	using Newtonsoft.Json;

	using Sms.Scheduler.Model;

	public class ProfileRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Sms.Scheduler.Model.Profile, ProfileRest>()
				.ForMember(x => x.ClientConfig, m => m.MapFrom(x => JsonConvert.DeserializeObject<ClientConfig>(x.ClientConfig, new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.None})))
				;
			mapper.CreateMap<ProfileRest, Sms.Scheduler.Model.Profile>()
				.ForMember(x => x.ClientConfig, m => m.MapFrom(x => JsonConvert.SerializeObject(x.ClientConfig, new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.None})))
				;
		}
	}
}
