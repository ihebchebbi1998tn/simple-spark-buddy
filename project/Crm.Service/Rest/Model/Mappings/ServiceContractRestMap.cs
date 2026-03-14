namespace Crm.Service.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Model;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	public class ServiceContractRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ServiceContract, ServiceContractRest>()
				.IncludeBase<Contact, ContactRest>()
				.ForMember(x => x.Installations, m => m.MapAtRuntime());

			mapper.CreateMap<ServiceContractRest, ServiceContract>()
				.IncludeBase<ContactRest, Contact>();
		}
	}
}
