namespace Main.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Library.Model;

	public class UserMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Usergroup, string>().ConvertUsing(x => x != null ? x.Name : null);
		}
	}
}
