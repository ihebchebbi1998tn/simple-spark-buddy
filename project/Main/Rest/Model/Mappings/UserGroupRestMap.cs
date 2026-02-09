namespace Main.Rest.Model.Mappings
{
	using System.Linq;

	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Library.Model;


	public class UserGroupRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Usergroup, UsergroupRest>()
				.ForMember(d => d.UsersIds, m => m.MapFrom(s => s.Members.Select(u => u.Username)));
		}
	}
}
