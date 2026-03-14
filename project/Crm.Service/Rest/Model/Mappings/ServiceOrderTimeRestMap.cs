namespace Crm.Service.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Service.Model;

	public class ServiceOrderTimeRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ServiceOrderTime, ServiceOrderTimeRest>()
				.ForMember(x => x.EstimatedDuration, m => m.MapFrom(s => s.EstimatedDuration))
				.ForMember(x => x.ActualDuration, m => m.MapFrom(s => s.ActualDuration))
				.ForMember(x => x.InvoiceDuration, m => m.MapFrom(s => s.InvoiceDuration))
				.ForAllMembers(m => m.MapAtRuntime());
			mapper.CreateMap<ServiceOrderTimeRest, ServiceOrderTime>()
				.ForMember(x => x.ActualDuration, m => m.MapFrom(x => x.ActualDuration.HasValue ? (float?)x.ActualDuration.Value.TotalHours : null))
				.ForMember(x => x.EstimatedDuration, m => m.MapFrom(x => x.EstimatedDuration.HasValue ? (float?)x.EstimatedDuration.Value.TotalHours : null))
				.ForMember(x => x.InvoiceDuration, m => m.MapFrom(x => x.InvoiceDuration.HasValue ? (float?)x.InvoiceDuration.Value.TotalHours : null))
				.ForMember(x => x.PosNo, m => m.Condition(x => x.PosNo != null));
		}
	}
}
