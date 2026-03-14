namespace Main.Controllers.OData;

using System.Collections.Generic;
using System.Linq;
using System.Net;

using Crm.Library.Api.Controller;
using Crm.Library.Api.Extensions;
using Crm.Library.Model;
using Crm.Library.Model.Authorization;
using Crm.Library.Services.Interfaces;

using Main.Rest.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;

public class DocumentGeneratorODataController : ODataControllerEx, IODataOperationImportController
{
	private readonly IEnumerable<IDocumentGeneratorService> documentGeneratorServices;
	public DocumentGeneratorODataController(IEnumerable<IDocumentGeneratorService> documentGeneratorServices)
	{
		this.documentGeneratorServices = documentGeneratorServices;
	}

	[HttpPost]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	[RequiredPermission(nameof(DocumentGeneratorEntry), Group = PermissionGroup.WebApiWrite)]
	public virtual IActionResult RetryDocumentGeneration(ODataActionParameters parameters)
	{
		var entries = parameters.GetValue<IEnumerable<DocumentGeneratorEntry>>("Entries");

		foreach (var entry in entries)
		{
			var service = documentGeneratorServices.Single(x => x.GetType().FullName == entry.GeneratorService);
			service.Retry(entry.Id);
		}

		return Ok();
	}
}
