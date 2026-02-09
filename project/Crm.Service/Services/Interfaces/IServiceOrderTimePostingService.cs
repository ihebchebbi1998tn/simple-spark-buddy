namespace Crm.Service.Services.Interfaces
{
	using Crm.Library.AutoFac;
	using Crm.Service.Model;

	public interface IServiceOrderTimePostingService : IDependency
	{
		void SetOrderTimesId(ServiceOrderTimePosting serviceOrderTimePosting);
	}
}
