namespace Main.ActionFilters
{
	using System.Globalization;
	using System.Threading;
	using System.Threading.Tasks;

	using Crm.Library.ActionFilterRegistry;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.ViewFeatures;

	using StackExchange.Profiling;

	public class LocalizationActionFilter : ICrmActionResultExecutor
	{
		// Members
		private readonly IClientSideGlobalizationService clientSideGlobalizationService;

		// Methods
		public virtual Task ExecuteAsync(ActionContext context, ViewDataDictionary viewData)
		{
			using (MiniProfiler.Current.Step("LocalizationActionFilter.ExecuteAsync"))
			{
				Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(clientSideGlobalizationService.GetCurrentLanguageCultureNameOrDefault());
				Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(clientSideGlobalizationService.GetCurrentCultureNameOrDefault());
			}

			return Task.CompletedTask;
		}

		// Constructor
		public LocalizationActionFilter(IClientSideGlobalizationService clientSideGlobalizationService)
		{
			this.clientSideGlobalizationService = clientSideGlobalizationService;
		}
	}
}
