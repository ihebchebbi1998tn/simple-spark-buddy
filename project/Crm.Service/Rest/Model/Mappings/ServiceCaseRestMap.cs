namespace Crm.Service.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Model;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	public class ServiceCaseRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ServiceCase, ServiceCaseRest>()
				.IncludeBase<Contact, ContactRest>()
				.ForMember(x => x.CompletionUserUser, m => m.MapFrom(x => x.CompletionUserObject))
				.ForMember(x => x.ResponsibleUserUser, m => m.MapFrom(x => x.ResponsibleUserObject))
				.ForAllMembers(m => m.MapAtRuntime());

			mapper.CreateMap<ServiceCaseRest, ServiceCase>()
				.IncludeBase<ContactRest, Contact>();
		}
	}
}
