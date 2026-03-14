namespace Crm.Service.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Model;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	public class ServiceObjectRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ServiceObject, Model.ServiceObjectRest>()
				.IncludeBase<Contact, ContactRest>()
				.ForMember(x => x.ResponsibleUserUser, m => m.MapFrom(s => s.ResponsibleUserObject))
				.ForAllMembers(x => x.MapAtRuntime());
				;

				mapper.CreateMap<Model.ServiceObjectRest, ServiceObject>()
					.IncludeBase<ContactRest, Contact>();
		}
	}
}
