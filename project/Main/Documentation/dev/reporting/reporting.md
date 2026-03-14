# Customizing the ServiceOrderDispatch report
## Overriding & extending the ViewModel

The dispatch report uses the same view for the report preview in the mobile service client and when generating a PDF on the server, but uses two different viewmodels. This means that when changing the logic, or adding additional data, these changes have to be done in both of the viewmodels.

### JavaScript ViewModel

The JavaScript viewmodel, which is used to render the dispatch report preview, can be changed by overriding the prototype of the viewmodel. The dispatch report preview uses the same viewmodel as the other tabs in the dispatch details, so the viewmodel located at window.Crm.Service.ViewModels.DispatchDetailsViewModel needs to be extended.

## How to define the margins and header / footer sizes

For headers and footer margins, the DispatchReportViewModel contains 4 properties:

- FooterContentSize (default: 2.5 cm, appsetting: *Report/FooterHeight*)
- FooterContentSpacing (default: 0.5 cm, appsetting: *Report/FooterSpacing*)
- HeaderContentSize (default: 3.5 cm, appsetting: *Report/HeaderHeight*)
- HeaderContentSpacing (default: 0.5 cm, appsetting: *Report/HeaderSpacing*)

To change the default values this can be done by overriding the properties in a custom viewmodel, or by changing the default values in the appsettings.config of the Main plugin.

The left and right page margins of the PDF will default to 2.5 cm and 2 cm and can be further modified using CSS margins.

## Add & format paging indicators

The dispatch report headers and footers are added to the PDF using iTextSharp PDF stamping. For each page the parameters [page] and [toPage] will be replaced before rendering to display the current page and the total number of pages:

```html
Page <span>[page]</span> of <span>[toPage]</span>
```

## Include additional CSS & JS assets

To include additional CSS or JS assets, the PluginRenderAction *TemplateHeadResource* can be used:

```c#
[AllowAnonymous]
[RenderAction("TemplateHeadResource", Priority = 50)]
public virtual ActionResult TemplateReportCss()
{
	return Content(WebExtensions.CssLink("templateReportCss"));
}
[AllowAnonymous]
[RenderAction("TemplateHeadResource", Priority = 50)]
public virtual ActionResult TemplateReportJs()
{
	return Content(WebExtensions.JsLink("templateReportJs"));
}
```

## Stamping and appendices

To stamp files (e.g. the company stationery) the *Stamp* method of the IPdfService can be used:

```c#
public class CustomServiceOrderService : ServiceOrderService, IServiceOrderService, IReplaceRegistration<ServiceOrderService>
{
	public override byte[] CreateDispatchReportAsPdf(ServiceOrderDispatch dispatch)
	{
		var report = base.CreateDispatchReportAsPdf(dispatch);
		var stationeryFile = appDataFolder.RetrieveBytes(stationeryFileName);
		var result = pdfService.Stamp(report, stationeryFile);
		return result;
	}
	//...
}
```

To add appendices (e.g. the terms and conditions) the *Merge* method of the IPdfService can be used:

```c#
public class CustomServiceOrderService : ServiceOrderService, IServiceOrderService, IReplaceRegistration<ServiceOrderService>
{
	public override byte[] CreateDispatchReportAsPdf(ServiceOrderDispatch dispatch)
	{
		var report = base.CreateDispatchReportAsPdf(dispatch);
		var termsAndConditionsFile = appDataFolder.RetrieveBytes(termsAndConditionsFileName);
		var result = pdfService.MergeFiles(report, termsAndConditionsFile);
		return result;
	}
	//...
}
```

## Changing the view

The dispatch report consists of three views which can be overridden individually to match the customer needs:

- *src\Crm.Web\Plugins\Crm.Service\Views\Shared\DispatchReport.ascx* - the main part of the report. This view is also used as the report preview on the mobile service client
- *src\Crm.Web\Plugins\Crm.Service\Views\Dispatch\ReportHeader.aspx* - the header of the report which will be repeated on every page
- *src\Crm.Web\Plugins\Crm.Service\Views\Dispatch\ReportFooter.aspx* - the footer of the report which will also be repeated on every page