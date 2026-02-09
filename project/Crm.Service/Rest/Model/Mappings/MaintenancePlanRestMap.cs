namespace Crm.Service.Rest.Model.Mappings
{
	using System;

	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	public class MaintenancePlanRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<MaintenancePlan, MaintenancePlanRest>()
				.IncludeBase<Contact, ContactRest>()
				.ForMember(x => x.ServiceContract, m =>
				{
					m.MapFrom(s => s.ServiceContract);
					m.MapAtRuntime();
				})
				.ForMember(x => x.ServiceOrders, m => m.MapAtRuntime());
			
			mapper.CreateMap<MaintenancePlanRest, MaintenancePlan>()
				.IncludeBase<ContactRest, Contact>()
				.ForMember(d => d.ServiceContract, m => m.MapFrom((source, destination, member, context) => context.GetService<IRepositoryWithTypedId<ServiceContract, Guid>>().Get(source.ServiceContractId)))
				.ForMember(d => d.ServiceOrderTemplate, m => m.MapFrom((source, destination, member, context) => source.ServiceOrderTemplateId != null ? context.GetService<IRepositoryWithTypedId<ServiceOrderHead, Guid>>().Get((Guid)source.ServiceOrderTemplateId) : null));
		}
	}
}
