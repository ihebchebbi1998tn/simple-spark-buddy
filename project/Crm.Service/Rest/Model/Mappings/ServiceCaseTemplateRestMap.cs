namespace Crm.Service.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Service.Model;

	public class ServiceCaseTemplateRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ServiceCaseTemplate, ServiceCaseTemplateRest>()
				.ForMember(x => x.ResponsibleUserUser, m => m.MapFrom(x => x.ResponsibleUserObject))
				.ForAllMembers(m => m.MapAtRuntime());
		}
	}
}
