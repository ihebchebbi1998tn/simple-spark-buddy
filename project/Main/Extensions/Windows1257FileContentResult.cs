namespace Main.Extensions
{
	using System.Threading.Tasks;

	using Crm.Library.Extensions;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	public class Windows1257FileContentResult : FileContentResult
	{
		public Windows1257FileContentResult(string fileContents, string contentType, string filename)
			: base(fileContents.GetWindows1257Bytes(), contentType)
		{
			FileDownloadName = filename;
		}
		public Windows1257FileContentResult(byte[] fileContents, string contentType, string filename)
			: base(fileContents, contentType)
		{
			FileDownloadName = filename;
		}
		public override async Task ExecuteResultAsync(ActionContext context)
		{
			FileDownloadName = FileDownloadName.RemoveIllegalCharacters();
			context.HttpContext.Response.Clear();
			context.HttpContext.Response.ContentType = ContentType;
			context.HttpContext.Response.Headers["Content-Disposition"] = string.Format("attachment; filename=\"{0}\"", FileDownloadName);

			var outputBytes = FileContents;

			await context.HttpContext.Response.Body.WriteAsync(outputBytes);
		}
	}
}
