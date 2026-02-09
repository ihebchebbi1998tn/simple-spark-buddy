namespace Crm.Service.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Service.Model;

	public class ServiceOrderMaterialRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ServiceOrderMaterialRest, ServiceOrderMaterial>()
				.ForMember(x => x.PosNo, m => m.Condition(x => x.PosNo != null))
				.ForMember(x => x.Version, m => m.Ignore());
			mapper.CreateMap<ServiceOrderMaterial, ServiceOrderMaterialRest>()
				.ForMember(x => x.ServiceOrderTime, m =>
				{
					m.MapFrom(s => s.ServiceOrderTime);
					m.MapAtRuntime();
				});
		}
	}
}
