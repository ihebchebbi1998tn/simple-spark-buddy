namespace Main.Extensions
{
	using Crm.Library.Extensions;

	using Microsoft.AspNetCore.Http;

	public static class HttpRequestExtensions
	{
		public static string ExtractBackUrlForCurrentUrl(this HttpRequest request)
		{
			var returnUrl = request.GetFormValue("ReturnToRemember");
			if (returnUrl != null)
			{
				return returnUrl;
			}
			returnUrl = request.GetTypedHeaders().Referer?.ToString();
			if (returnUrl != null && !returnUrl.Contains("/Account/Login") && (request.Path.Value == null || !returnUrl.EndsWith(request.Path.Value)))
			{
				return returnUrl;
			}

			return null;
		}
	}
}
