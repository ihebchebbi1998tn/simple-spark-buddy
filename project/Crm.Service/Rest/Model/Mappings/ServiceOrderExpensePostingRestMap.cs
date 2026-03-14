namespace Crm.Service.Rest.Model.Mappings
{
	using System;

	using AutoMapper;

	using Crm.Article.Services.Interfaces;
	using Crm.Library.AutoMapper;
	using Crm.Library.Services.Interfaces;
	using Crm.PerDiem.Model;
	using Crm.PerDiem.Rest.Model;
	using Crm.Service.Model;

	public class ServiceOrderExpensePostingRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ServiceOrderExpensePosting, ServiceOrderExpensePostingRest>()
				.IncludeBase<Expense, ExpenseRest>()
				.ForMember(x => x.ServiceOrderTime, m =>
				{
					m.MapFrom(s => s.ServiceOrderTime);
					m.MapAtRuntime();
				})
				.ForMember(x => x.ServiceOrder, m =>
				{
					m.MapFrom(s => s.ServiceOrderHead);
					m.MapAtRuntime();
				});

			mapper.CreateMap<ServiceOrderExpensePostingRest, ServiceOrderExpensePosting>()
				.IncludeBase<ExpenseRest, Expense>();
		}
	}
}
