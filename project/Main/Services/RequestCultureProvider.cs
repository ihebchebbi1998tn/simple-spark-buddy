namespace Main.Services
{
	using System.Threading.Tasks;

	using Crm.Library.Extensions;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Localization;

	public class RequestCultureProvider : IRequestCultureProvider
	{
		public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
		{
			var clientSideGlobalizationService = httpContext.GetService<IClientSideGlobalizationService>();
			var result = new ProviderCultureResult(clientSideGlobalizationService.GetCurrentCultureNameOrDefault(), clientSideGlobalizationService.GetCurrentLanguageCultureNameOrDefault());
			return Task.FromResult(result);
		}
	}
}
