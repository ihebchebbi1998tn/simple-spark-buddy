namespace Crm.Service.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	public class ServiceOrderHeadRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ServiceOrderHead, ServiceOrderHeadRest>()
				.IncludeBase<Contact, ContactRest>()
				.ForMember(d => d.Installation, m => m.MapFrom(s => s.AffectedInstallation))
				.ForMember(d => d.Company, m => m.MapFrom(s => s.CustomerContact))
				.ForMember(d => d.ServiceCaseNo, m => m.MapFrom(s => s.ServiceCase.ServiceCaseNo))
				.ForMember(d => d.ResponsibleUserUser, m => m.MapFrom(s => s.ResponsibleUserObject))
				.ForMember(d => d.PreferredTechnicianUser, m => m.MapFrom(s => s.PreferredTechnician))
				.ForMember(d => d.PreferredTechnician, m => m.MapFrom(s => s.PreferredTechnicianUsername))
				.ForMember(d => d.PreferredTechnicianUsergroupObject, m => m.MapFrom(s => s.PreferredTechnicianUsergroup))
				.ForMember(d => d.Name, m => m.Ignore())
				.ForAllMembers(x => x.MapAtRuntime());
			mapper.CreateMap<ServiceOrderHeadRest, ServiceOrderHead>()
				.IncludeBase<ContactRest, Contact>()
				.ForMember(x => x.PreferredTechnicianUsergroup, m => m.MapFrom(s => s.PreferredTechnicianUsergroupObject))
				.ForMember(d => d.PreferredTechnicianUsername,
					m =>
					{
						m.MapFrom(s => s.PreferredTechnician);
						m.MapAtRuntime();
					})
				.ForMember(d => d.PreferredTechnician, m => m.Ignore())
				.ForMember(d => d.Name, m => m.Ignore());
		}
	}
}
