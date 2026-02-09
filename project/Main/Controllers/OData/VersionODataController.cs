namespace Main.Controllers.OData
{
	using System.Net;
	using Crm.Library.Api.Controller;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Routing.Controllers;

	public class VersionODataController : ODataController, IODataOperationImportController
	{
		[HttpGet]
		[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
		public virtual IActionResult GetVersion() => Ok(typeof(VersionODataController).Assembly.GetName().Version?.ToString());
	}
}
