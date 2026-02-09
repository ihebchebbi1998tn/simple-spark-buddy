namespace Crm.Service.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Model;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	public class InstallationRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Installation, InstallationRest>()
				.IncludeBase<Contact, ContactRest>()
				.ForMember(d => d.Address, m => m.MapFrom(s => s.LocationAddress))
				.ForMember(d => d.Company, m =>
				{
					m.MapFrom(s => s.LocationCompany);
					m.MapAtRuntime();
				})
				.ForMember(d => d.Person, m =>
				{
					m.MapFrom(s => s.LocationContactPerson);
					m.MapAtRuntime();
				})
				.ForMember(d => d.PreferredUserUser, m => m.MapFrom(s => s.PreferredUserObj))
				.ForMember(d => d.ResponsibleUserUser, m => m.MapFrom(s => s.ResponsibleUserObject));

			mapper.CreateMap<InstallationRest, Installation>()
				.IncludeBase<ContactRest, Contact>();
		}
	}
}
