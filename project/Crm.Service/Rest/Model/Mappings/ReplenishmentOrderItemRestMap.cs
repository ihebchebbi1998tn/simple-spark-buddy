namespace Crm.Service.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Service.Model;

	public class ReplenishmentOrderItemRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ReplenishmentOrderItem, Model.ReplenishmentOrderItemRest>()
				.ForMember(x => x.ReplenishmentOrder, m =>
				{
					m.MapFrom(x => x.ReplenishmentOrder);
					m.MapAtRuntime();
				});
		}
	}
}
