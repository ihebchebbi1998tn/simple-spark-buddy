namespace Main.ActionFilters
{
	using Crm.Library.ActionFilterRegistry;
	using Crm.Library.BaseModel.ViewModels;

	public interface ICrmModelActionFilter : ICrmActionFilter
	{
		void ExecuteFilter(ICrmModel model);
	}
}
