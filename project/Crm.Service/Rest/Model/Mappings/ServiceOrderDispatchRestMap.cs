namespace Crm.Service.Rest.Model.Mappings
{
	using System;

	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;

	public class ServiceOrderDispatchRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ServiceOrderDispatch, ServiceOrderDispatchRest>()
				.ForMember(d => d.OrderNo, m => m.MapFrom(d => d.OrderHead.OrderNo))
				.ForMember(d => d.Username, m => m.MapFrom(d => d.DispatchedUsername))
				.ForMember(d => d.ServiceOrder, m =>
				{
					m.MapFrom(s => s.OrderHead);
					m.MapAtRuntime();
				})
				.ForMember(d => d.CurrentServiceOrderTime, m =>
				{
					m.MapFrom(s => s.CurrentServiceOrderTime);
					m.MapAtRuntime();
				})
				.ForMember(d => d.ServiceOrderTimePostings, m => m.MapFrom(s => s.TimePostings))
				.ForMember(d => d.ServiceOrderExpensePostings, m => m.MapFrom(s => s.ExpensePostings))
				.ForMember(d => d.ServiceOrderErrorTypes, m => m.MapFrom(s => s.ServiceOrderErrors));

			mapper.CreateMap<ServiceOrderDispatchRest, ServiceOrderDispatch>()
				.ForMember(d => d.DispatchedUsername, m => m.MapFrom(d => d.Username))
				//needed because of bad NH mapping and event handlers
				.ForMember(d => d.DispatchedUser, m => m.MapFrom((source, destination, member, context) => context.GetService<IUserService>().GetUser(source.Username)))
				.ForMember(d => d.OrderHead, m => m.MapFrom((source, destination, member, context) => context.GetService<IRepositoryWithTypedId<ServiceOrderHead, Guid>>().Get(source.OrderId)));
		}
	}
}
