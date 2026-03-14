# Documentation of application settings

## Something you need to know for this document
### How do I identify a configuration switch as price list position?
All configurations which are price list positions can be identified by following pattern:

CRM/sales:
`CRM/sales: SW<6 digit number>`

Service:
`Service: SW<6 digit number>`

# Documentation of plugin settings

## Crm.Article

Enables article feature.

**<u>Please note:</u>**
This plugin is required for other plugin(s). If it is required it will be added automatically be dependency resolution of the other plugin.

## Crm.Campaigns (CRM/sales: SW000344)
Enables campaign feature.

### FinishedCampaignsSelectableForDays (int)
Define if campaign is selectable for contacts after finsihed. Default value is 60.

### CampaignPriceStep (int)
Define quantity step when using up and down arrows in UI. Default value is 100.


## Crm.Configurator (CRM/sales: SW000347)

**<u>Please note:</u>**

When Crm.Configurator plugin will be enabled, it changes necessary lookup configuration in [LU].[OrderEntryType] automatically.

## Crm.Documentation

Enables documentation accessability.

## Crm.Documentation.CommandLine

Internal usage.

## Crm.DynamicForms

Enables dynamic form features. It is required e.g. for SMS.Checklists.

**<u>Please note:</u>**
This plugin is required for other plugin(s). If it is required it will be added automatically be dependency resolution of the other plugin.

## Crm.ErpExtension (CRM/sales: SW000531 + SW000532)

Enables ERP documents and turnover statistics. The single features can be activated separately by permissions.

**<u>Please note:</u>**

| Feature                                  | Permission Group / Permission | Price list position |
| ---------------------------------------- | ----------------------------- | ------------------- |
| ERP documents tab in company details view (sales client) | Company / ErpDocumentsTab     | SW000531            |
| ERP documents tab in project details view (sales client) | Project / ErpDocumentsTab     | SW000531            |
| Statistics tab in company details view (sales client) | Company / TurnoverTab         | SW000532            |

## Crm.InforExtension (CRM/sales: SW000267; service: SW000270)

Standard interface to ERP Infor COM.

**<u>Please note:</u>**

- A service to handle the data on Infor site needs to be installed.
- No reading interface from Infor COM to L-mobile is implemented.
- The following data can be written from L-mobile to Infor COM:
  - Companies
  - Persons
  - Communication data


## Crm.Offline (CRM/sales: SW000327)

Enables offline sales client and technician client.

## Crm.Order (CRM/sales: SW000345, SW000346)
Enables orders and offers.

**<u>Attention:</u>**

- SW000345 can be enabled/disabled by lookup [LU].[OrderEntryType]. Please change IsActive of SingleDelivery appropriately.

- SW000346 can be enabled/disabled by lookup [LU].[OrderEntryType]. Please change IsActive of MultiDelivery appropriately.

**<u>Please note:</u>**
Crm.Configurator changes its lookup in [LU].[OrderEntryType] automatically when activating the configurator plugin.

## Crm.Project (CRM/sales: SW000342)

Enables project module.

## Crm.ProjectOrders (CRM/sales: SW000342)
Enables order integration to projects. When this plugin is enabled the corresponding orders which are created from a project will be referenced in the project. A order tab in projects will be visible to access the corresponding orders. It is also possible to create orders in projects.

## Crm.Service (Service: SW000351)
Activates basic service features.

**<u>Attention:</u>**

- ServiceContract can be enabled/disabled by permission (SW000371)

  PGroup: WebAPI; Permission: ServiceContract

- ServiceObject can be enabled/disabled by permission (SW000372)

  PGroup: WebAPI; Permission: ServiceObject


## Crm.VisitReport (CRM/sales: SW000340; SW000341)

**<u>Attention:</u>**
<u>By activating this plugin VisitReport (SW000340) and Tour planing (SW000341) functionality will be added to the application. Up from V4.5 it will be possible to activate them separately by permission.</u>

## Main (CRM/sales: SW000325; Service: SW000351)
This is the base module and required for CRM/sales and service application.

## Main.SmtpDropbox (CRM/sales: SW000325; Service: SW000351)

Enables dropbox module with smtp.

## Sms.Checklists (service: SW000373)

Enables dynamic form integration for checklists.

## Sms.TimeManagement (service: SW000367)

Enables time management integration.


# Documentation of configuration settings

All configuration settings of our web-configs will be grouped by plugins. Additionally only settings that are / were used by our code will be documented. Quotations aren't qualified or may be wrong descriptions and have to be checked.


## Something you need to know for this chapter

### How to create "string arrays"

Create string arrays comma separated without space.


## Main

### Address/DisplayOnlyRegionKey (bool)

If this is set to true, only in the csv export the region key is displayed instead of the region name

### AllowCompanyTypeSelection (bool)

When set to false, the CompanyType dropdown is disabled. For a new company the company type is set to the CompanyType which is marked as favorite in the lookup table. Feature can be useful if contacts are imported from an ERP whereas the companies created in the L-mobile system should all belong to a specific company type, e.g. "Prospect".

### ApplePushNotification/CertificateFileName (string)

Sets the certificate filename to register the apple devices to the apple push notification service.

### ApplePushNotification/CertificatePassword (string)

Sets the password to register the apple devices to the apple push notification service.

### ApplePushNotification/ProductionEnvironment (bool)

Defines if the certificate is used as production certificate.

### CefToPdfPath (string)

Optional path to manually deployed CefToPdf to use with StaticDeployment. Can be used to share a CefToPdf deployment between multiple applications.

### Configuration/BravoActiveForCompanies (bool)

Decides if bravos are available for companies.

### Configuration/BravoActiveForPersons (bool)

Decides if bravos are available for persons.

### CompanyGroupFlags/AreSearchable (bool)

This option addes the company group flags completely. I.E. company group flags are editable in company and filterable using contact filter.

### CompanyNoIsGenerated(bool)

Decides if the CompanyNo is automatically generated by L-mobile

### CompanyNoIsCreateable(bool)

Decides if a user can create a custom CompanyNo when creating a company

### CompanyNoIsEditable(bool)

Decides if a user can edit an existing CompanyNo

### Cordova/AndroidAppLink (string)

Sets the link at login to l-mobile client app in google playstore.

### Cordova/AppleIosAppLink (string)

Sets the link at login to l-mobile client app in apple appstore.

### Cordova/Windows10AppLink (string)

 Will set the link at login to l-mobile client for window appstore.

### DropboxForwardPrefixes (string)

This defines the prefixes of the email subject to detect forwarded emails.

### DropboxDomain (string)

This key sets the domain of the user dropbox address as well as of the project dropboxaddress. E.g: key="crm.example.com" results as (6e732bc93ad@*crm.example.com*)

### MinFileSizeInBytes (int)
This key is used to limit the storage of attached file belonging to Dropbox Email Note

### MinPasswordStrength (int)
Minimum password strength score from 0 to 4 (inclusive) when a user changes the password, with 0 being least secure and 4 being most secure, calculated from the number of guesses estimated to be needed.

### DropboxLogMessages (bool)
Decides if the recieved dropbox messages should be logged to disk (always, not just in case of error).

### FileResource/AllowedContentTypes (string[])
Optional whitelist of allowed content types for file uploads. If empty all content types are allowed.

### FileResource/ContentTypesOpenedWithoutSandbox (string[])
Optional whitelist of allowed content types which will be opened directly instead of rendered inside a sandboxed iframe. If empty all content types are allowed. All image types can be allowed by using `image/*` like in html inputs

### Geocoder/GeocoderService (string)

Defines which geocoding service is used. 

<u>Options:</u>

- "mapquest"
- "bing"
- "yahoo"
- "google"

<u>Default value:</u>
The default is google, even if the GeocoderService string is empty.

### Geocoder/BingMapsApiKey (string)

Contains the api key for Bing maps.

### Geocoder/GoogleMapsApiKey (string)

Contains the api key for Google maps.

### Geocoder/MapQuestApiKey (string)

vContains the api key for Map Quest.

### Geocoder/YahooMapsApiKey (string)

Contains the api key for Yahoo maps.

### Geocoder/YahooMapsApiSecret (string)

Contains the api key secret for Yahoo maps.

### Lucene/LegacyNameIsDefault (bool)

Decides if contacts are filterable for their legacyId using quick search (e.g. filter in right upper corner of dashboard).

### MapTileLayerUrl (string)

URL for tiles for leaflet maps. 

### PasswordReset/ExpirationInMinutes (int)

Duration in minutes a password reset email is valid 

### PasswordReset/MaxEmailsPerHour (int)

Maximum amount of password reset emails a user can send each hour

### Person/BusinessTitleIsLookup (bool)

Decides if the businesstitle of persons is a lookup or a free text to set.

### Person/DepartmentIsLookup (bool)

Decides if the department of persons is a lookup or a free text to set.

### PersonNoIsGenerated(bool)

Decides if the PersonNo is automatically generated by L-mobile

### PersonNoIsCreateable(bool)

Decides if a user can create a custom PersonNo when creating a person

### PersonNoIsEditable(bool)

Decides if a user can edit an existing PersonNo

### RedisConfiguration (string)

Configuration command string for redis. If emtpy a local standalone redis server will be started.

### Report/HeaderHeight (double)

Sets the header height of reports in cm.

### Report/HeaderSpacing (double)

Sets the content spacing of report headers in cm.

### Report/FooterHeight (double)

Sets the footer height of reports in cm.

### Report/FooterSpacing (double)

Sets the content spacing of the report footers in cm.

### Site/HostEditable (bool)

When set to true, the site host can be edited in the site settings.

### Site/PluginsEditable (bool)

When set to true, the active plugins can be edited in the site settings.

### StripLeadingZerosFromLegacyId (bool)

When set to true, leading zeros from Legacy Ids are removed when being displayed.

### UseActiveDirectoryAuthenticationService (bool; CRM/sales: SW000336; Service: SW000366)

Enables active directory authentication for login.

### ActiveDirectoryEndpoint

Connection string to use for active directory authentication.

### Maintenance/PasswordResetTokenDeprecationDays (int)

Password reset tokens are deleted after n days by DatabaseCleanupAgent. (default 30)

### Maintenance/PostingDeprecationDays (int)

Postings that are 'Processed', 'Skipped' or 'Stale' are deleted after n days by DatabaseCleanupAgent. (default 90)

### Maintenance/MessageDeprecationDays (int)

Messages that are sent (state == 1) are deleted after n days by DatabaseCleanupAgent. (default 30)

### Maintenance/LogDeprecationDays (int)

Info- and warninglogs are deleted after n days by DatabaseCleanupAgent. (default 14)

### Maintenance/ErrorLogDeprecationDays (int)

Errorlogs are deleted after n days by DatabaseCleanupAgent. (default 30)

### Maintenance/ReplicatedClientDeprecationDays (int)

Replicated Clients are deleted by DatabaseCleanupAgent, if the LastSync is older than n days. (default 90)

### Maintenance/FragmentationLevel1 (int 0-100)

If an index got a fragmentation above n % it will be reorganized or rebuild online. (default 5)

### Maintenance/FragmentationLevel2 (int 0-100)

If the fragmentation of an index is above n % it will be rebuild online, if not possible it will be rebuild offline. (Table is not accessable during this time) (default 15)

### Maintenance/CommandTimeout (int)

Defines how long the cleanup operations can take before returning a timeout in seconds. (default 1800)

## Main

### Posting/MaxRetries (int)
Maximum amount of retries for each `Posting` to be processed.

### Posting/RetryAfter (int)
Amount of time in _minutes_ between two tries.

## Main.SmtpDropbox
### SmtpDropboxAgent/Port (int) 
Defines the port, SmtpDropboxAgent is listening on.

## Crm.Campaigns

### CampaignNoIsGenerated(bool)

Decides if the CampaignNo is automatically generated by L-mobile

### CampaignNoIsCreateable(bool)

Decides if a user can create a custom CampaignNo when creating a campaign

### CampaignNoIsEditable(bool)

Decides if a user can edit an existing CampaignNo 


## Crm.ErpExtension

### EnableAddressExport (bool)

If set true, addresses are exported to erp manually or via LegacyUpdateService.

### EnableCompanyExport (bool) 

If set true, companies are exported to erp manually or via LegacyUpdateService.
### EnableCommunicationExport (bool)

If set true, communications are exported to erp manually or via LegacyUpdateService.

### EnablePersonExport (bool) 

If set true, persons are exported to erp manually or via LegacyUpdateService.

### ErpSystemID (string) 

Is used to locate the erp system in ObjectLinks. The ErpSystemId is located in vpps.ini (according to infor).
### ErpSystemName (string) 
Additional to ErpSystemId, in SAP the ErpSystemName is used in ObjectLinks to open documents in the erp system.
### ObjectLinkIntegration (string) 
Provides type of ObjectLink, used to open documents in the erp system. 

<u>Options:</u>

- "InforObjectLink" for infor
- "D3Link" for d.velop dms
- "SapLink" for sap

### TurnoverCurrencyKey (string)

Sets the currency for turnover according to LU.Currency.

## Crm.InforExtension
### InforExport/InforErpComVersion (string) 
Used to pick a suitable adapter for Infor version. 

<u>Options:</u>

- "6.3"
- "7.1"

### InforExport/ShortFieldNames (bool) 
Has to be set true if oracle version is below 10. If so, fieldnames aren't allowed to exceed 16 characters.



## Crm.Order
### OffersEnabled (bool) (CRM/sales: SW000345, SW000346)

Defines if offers are available or not.
### OrderBarcodeEnabled (bool)

Adds the possibility to add items to order via barcode scanner.

### OrderBillingAddressEnabled (bool)

If true, a billing address can be set in orders. 

### OrderComissionEnabled (bool)

If true, a comission number can be added to orders.

### OrderDeliveryAddressEnabled (bool)

If true, a delivery address can be set in orders. 

### OrderItemDiscountEnabled (bool) 

Enables possibility to set discounts on order positions.
### OrderPrivateDescriptionEnabled (bool)

Adds a private description to orders.

### OrderSignatureEnabled (bool) 
If true, a signature input is added to orders.

### Order/OrderNoIsGenerated(bool)

Decides if the OrderNo is automatically generated by L-mobile

### Order/OrderNoIsCreateable(bool)

Decides if a user can create a custom OrderNo when creating an order

### Order/OrderNoIsEditable(bool)

Decides if a user can edit an existing OrderNo

### Offer/OfferNoIsGenerated(bool)

Decides if the OfferNo is automatically generated by L-mobile

### Offer/OfferNoIsCreateable(bool)

Decides if a user can create a custom OfferNo when creating an offer

### Offer/OfferNoIsEditable(bool)

Decides if a user can edit an existing OfferNo 

### PDFHeaderMargin (double) 
Sets the header margin of pdf in cm.
### PDFFooterMargin (double) 
Sets the footer margin of pdf in cm.
### PDFFooterTextPush (double) 
Sets the distance of the footer of PDF in cm.



## Crm.Project
### Configuration/BravoActiveForProjects (bool)

Activates bravos for projects, available in project details sidebar.

### ProjectsHaveAddresses (bool) 

If set to true, in project details the StandardAddress of the Project instead of the StandardAddress of the Parent is displayed.

## Crm.PerDiem

### Email/PerDiemReportApprovers (string)

Leaders who will receive email notification from `PerDiemReportApprovalNotifier` about Per Diem reports with _Request closed_ status waiting for close approval. List of emails separated by comma.

### Email/PerDiemReportRecipients (string)

Recipients who will receive closed Per Diem reports sent by `PerDiemReportSenderAgent`. List of emails separated by comma.

### Email/SendPerDiemReportToResponsibleUser (bool)

Decides whether the perdiem report will be sent to the responsible user.

### Expense/ClosedExpensesHistorySyncPeriod (int)

Selects expenses x days from past to sync them to service client.

### Expense/MaxDaysAgo (int)

Sets the maximum of selectable days in the past at creating / editing expenses.

### PerDiemReport/ShowClosedReportsSince (int)

Shows closed per diem reports which are at most x days old only.

### TimeEntry/MinutesInterval (int)

Alters the interval of minutes selection for timeentries. 

<u>Default value:</u>

5

### TimeEntry/AllowOverlap (bool)

Enables to book timeentries with overlap.

<u>Default value:</u>

false


### TimeEntry/ClosedTimeEntriesHistorySyncPeriod (int)

Selects timeentries x days from past to sync them to service client.

### TimeEntry/DefaultStart (string)

Sets a default start time for time entries.

<u>Format:</u>

hh:mm

### TimeEntry/MaxDaysAgo (int) 
Sets the maximum of selectable days in the past at creating / editing timeentries.

### TimeEntry/DefaultWorkingHoursPerDay (int)
Default value for user working hours per day. Used in all-day time entry creation. Default value is 8. `WorkingHoursPerDay` user config is using this value when create a new user.

### TimeEntry/ShowTimeZone (bool)
If set to true, it allows the user to select timezone for time recording related entities. 

## Crm.Project

### Potential/PotentialNoIsGenerated(bool)

Decides if the PotentialNo is automatically generated by L-mobile

### Potential/PotentialNoIsCreateable(bool)

Decides if a user can create a custom PotentialNo when creating a potential

### Potential/PotentialNoIsEditable(bool)

Decides if a user can edit an existing PotentialNo 

### ProjectNoIsGenerated(bool)

Decides if the ProjectNo is automatically generated by L-mobile

### ProjectNoIsCreateable(bool)

Decides if a user can create a custom ProjectNo when creating a project

### ProjectNoIsEditable(bool)

Decides if a user can edit an existing ProjectNo 

## Crm.Service
### AdHoc/AdHocNumberingSequenceName (string)

Required to retrieve next formatted adhoc order no. 

<u>Default value:</u>

"SMS.ServiceOrderHead.AdHoc"

### Dispatch/DispatchNoIsGenerated(bool)

Decides if the DispatchNo is automatically generated by L-mobile

### Dispatch/DispatchNoIsCreateable(bool)

Decides if a user can create a custom DispatchNo when creating a dispatch

### Dispatch/DispatchNoIsEditable(bool)

Decides if a user can edit an existing DispatchNo 

### Dispatch/SuppressEmptyMaterialsInReport(bool)

Decides if the materials/costs list should be hidden in the report when it is empty and no lumpsum entries are present

### Dispatch/SuppressEmptyExpensePostingsInReport(bool)

Decides if the expense postings list should be hidden in the report when it is empty and no lumpsum entries are present

### Dispatch/SuppressEmptyErrorTypesInReport(bool)

Decides if the error types and causes should be hidden in the report when it is empty

### Dispatch/SuppressEmptyTimePostingsInReport(bool)

Decides if the time postings list should be hidden in the report when it is empty and no lumpsum entries are present

### Dispatch/SuppressExpensePostingsInReport(bool)

Decides if the expense postings list should be hidden in the report

### Dispatch/SuppressErrorTypesInReport(bool)

Decides if the errors list should be hidden in the report

### Dispatch/SuppressEmptyJobsInReport(bool)

Decides if the job group header should be hidden in the report when both material and time posting lists are empty and no lumpsum entries are present

### Email/ClosedByRecipientForReplenishmentReport (bool) 

If true, adds the user, which closed the replenishmeht order to the recipient list of report.

### Email/DispatchReportRecipients (string array)

Defines the recipients of the dispatch report. It also defines the recipients of serviceorder report. As possible value e.g: "defualt@example.com,german@example.com"

### Email/ReplenishmentOrderRecipients (string array)

Add recipients to replenishmentorders by default. As possible value e.g: "defualt@example.com,german@example.com"

### Email/SendDispatchNotificationEmails (bool) 

If true, sends out emails to the dispatch user if dispatch is created with status released, or if edited from scheduled to released.

### Email/SendDispatchRejectNotificationEmails (bool) 
If true, sends out email to the dispatch responsible user if dispatch got rejected.

### Email/SendDispatchFollowUpOrderNotificationEmails (bool)
If true, sends out email to the dispatch responsible user (or if not set, the dispatcher) if dispatch is marked with _follow up service order_.

### Email/SendDispatchReportsOnCompletion (bool) 
If true, sends out dispatch report to all dispatch report recipients by DispatchReportSendarAgent.

### Email/SendDispatchReportToDispatcher (bool) 
If true, adds dispatch crateuser to internal recipient list.

### Email/SendDispatchReportToTechnician (bool) 
If true, adds dispatched user to internal recipient list.

### Email/SendServiceOrderReportsOnCompletion (bool)

If true, sends out serviceorder report by ServiceOrderReportSenderAgent.

### Email/SendServiceOrderReportToDispatchers (bool)

If true, sends out serviceorder report to all createusers of attached dispatches.

### Export/ExportDispatchReportsControlFileExtension (string)

Defines the file extension of contol file.

### Export/ExportDispatchReportsControlFileContent (string)

Defines the pattern for control file content. 

<u>Default value:</u> 

`"&quot;Controlfile&quot;"`

### Export/ExportDispatchReportsControlFilePattern (string)

Defines the filename pattern for the report control file.

<u>Default value:</u> 

`"{Date-yyyy}{Date-MM}{Date-dd}_{DispatchId}"`

### Export/ExportDispatchReportsFilePattern (string)

Defines filename pattern for reports saved to disk. 

<u>Default value:</u> 

`"{Date-yyyy}{Date-MM}{Date-dd}_{DispatchId}"`

### Export/ExportDispatchReportsOnCompletion (bool) 

If true, saves all dispatch reports to the configured path, where it's status is "Closed" or "ClosedNotComplete"

### Export/ExportDispatchReportsPath (string) 
Defines the path where dispatch reports are saved to disk. 

<u>Default value:</u> 

`"\\unc\path\{Date-yyyy}\"`

### Export/ExportServiceOrderReportsOnCompletion (bool) 
If true, saves all serviceorder reports to the configured path, where it's serviceorder status is "Closed".

### Export/ExportServiceOrderReportsPath (string) 
Defines the export path where serviceorder reports are saved to disk.

### Export/ExportServiceOrderReportsUncDomain (string)

 Domain for network access.

### Export/ExportServiceOrderReportsUncPassword (string)

 Password for network access.

### ExportServiceOrderReportsUncUser (string) 

 User for network access.

### ReplenishmentOrder/ClosedReplenishmentOrderHistorySyncPeriod (int)

Selects closed replenishment orders x days from past to sync them to service client.

### InstallationNoIsGenerated(bool)

Decides if the InstallationNo is automatically generated by L-mobile

### InstallationNoIsCreateable(bool)

Decides if a user can create a custom InstallationNo when creating an installation

### InstallationNoIsEditable(bool)

Decides if a user can edit an existing InstallationNo 

### Service/Dispatch/Requires/CustomerSignature (bool)

If true, dispatches can't be completed in service client if it's not signed by customer.

### Service/Dispatch/Show/EmptyTimesOrMaterialsWarning (string)

"NONE", "WARN" and "ERROR" are the acceptable values. Decides if a warning message will be displayed when someone tries to save a dispatch without any material and time posting. NONE stands for no warning at all, WARN means a warning message will be displayed, but the dispatch can be saved, ERROR will block the save completely if the warning message presents.

### ServiceCase/OnlyInstallationsOfReferencedCustomer (bool)

If true, only installations, referenced by customer are available in service cases. Selected service object will override this.

### ServiceCase/Signature/Enable/Originator (bool)

Enables signature for originator at choosing signing person in service client.

### ServiceCase/Signature/Enable/Technician (bool)

Enables signature for technician at choosing signing person in service client.

### ServiceCase/ServiceCaseNoIsGenerated(bool)

Decides if the ServiceCaseNo is automatically generated by L-mobile

### ServiceCase/ServiceCaseNoIsCreateable(bool)

Decides if a user can create a custom ServiceCaseNo when creating a service case

### ServiceCase/ServiceCaseNoIsEditable(bool)

Decides if a user can edit an existing ServiceCaseNo 

### ServiceContract/OnlyInstallationsOfReferencedCustomer (bool)

If true, only installations, referenced by customer are available in service contracts. Selected service object will override this.

### ServiceContract/MaintenanceOrderGenerationMode (string)

Available options: JobPerInstallation, OrderPerInstallation.

### ServiceContract/CreateMaintenanceOrderTimeSpanDays (int)

Default time span when evaluate and generate maintenance orders.

### ServiceContract/MaintenancePlan/AvailableTimeUnits (string array)

Selectable time units for maintenance plan creation.

### ServiceContract/ReactionTime/AvailableTimeUnits (string array)

Selectable time units for reaction times.

### ServiceContract/ServiceContractNoIsGenerated(bool)

Decides if the ServiceContractNo is automatically generated by L-mobile

### ServiceContract/ServiceContractNoIsCreateable(bool)

Decides if a user can create a custom ServiceContractNo when creating a service contract

### ServiceContract/ServiceContractNoIsEditable(bool)

Decides if a user can edit an existing ServiceContractNo 

### ServiceObject/ObjectNoIsGenerated(bool)

Decides if the ObjectNo is automatically generated by L-mobile

### ServiceObject/ObjectNoIsCreateable(bool)

Decides if a user can create a custom ObjectNo when creating a service object

### ServiceObject/ObjectNoIsEditable(bool)

Decides if a user can edit an existing ObjectNo 

### ServiceOrderMaterial/CreateReplenishmentOrderItemsFromServiceOrderMaterial (bool)

 If true, the technician is able to add / update serviceorder material item to replenishment order.

### ServiceOrderMaterial/ShowPricesInMobileClient (bool)

If true, serviceorder material prices are shown in technician client.

### ServiceOrder/DefaultDuration (string)

Sets a default duration to adhoc orders at creation.

<u>Format:</u>

hh:mm

### ServiceOrder/OnlyInstallationsOfReferencedCustomer (bool)

If true, only installations, referenced by customer are available in service orders. Selected service object will override this.

### ServiceOrder/GenerateAndAttachJobsToUnattachedTimePostings (bool)

If true (default), new service order times (jobs) will be created or existing ones matches to newly added time postings. If false, time postings will not be automatically attached to jobs.

### ServiceOrder/OrderNoIsGenerated(bool)

Decides if the OrderNo is automatically generated by L-mobile

### ServiceOrder/OrderNoIsCreateable(bool)

Decides if a user can create a custom OrderNo when creating a service order

### ServiceOrder/OrderNoIsEditable(bool)

Decides if a user can edit an existing OrderNo

### ServiceOrderDispatch/ReadGeolocationOnDispatchStart (bool)

If true, geolocation is saved in dispatch when technician has started working on order.

### ServiceOrderExpensePosting/UseArticleAsExpenseType

If true Articles are used as a ExpenseType. Usually this should be true when a ERP system is used and the service order expenses should be exported to the ERP System. When false, the value from the LU.ExpenseType table is used.

### ServiceOrderTimePosting/ClosedTimePostingsHistorySyncPeriod (int)

Selects timepostings x days from past to sync them to service client.

### ServiceOrderTimePosting/MaxDaysAgo (int) 

Sets the maximum of selectable days in the past at creating / editing timepostings.

### ServiceOrderTimePosting/ShowTechnicianSelection (bool)

Enables technician selection in service client on creating timepostings.

### ServiceOrderTimePosting/MinutesInterval (int)

Alters the interval of minutes selection for serviceorder timepostings.

<u>Default value:</u>

5

### ServiceOrderTimePosting/AllowOverlap (bool)

Enables to book timepostings with overlap.

<u>Default value:</u>

false

### UserExtension/OnlyUnusedLocationNosSelectable (bool) 
If true, in user administration, only unused locations are selectable to users.

### AttentionTasks/CheckAllInstallationsWarranties (bool)

Specify if the system have to check all installation warranties before creating the attention tasks ofr the service order or not. 

<u>Default value:</u>

false

## Crm.VisitReport
### DefaultVisitTimeSpanHours (double)
Default value for visits.
### Visit/AvailableTimeUnits (string array)
AvailableTimeUnits (comma separated). The list of selectable time units for Company visit requency. Default values: _Year_, _Quarter_, _Month_, _Week_

## Sms.Checklists
### Dispatch/CustomerSignatureValidateChecklists (bool)
If _true_ Checklists are validated when customer signs dispatch.

## Sms.Scheduler

### WorkingTime/FromDay (int)
  Sets the first day of week for the `Timeline`. The value is a zero based index of day of weeek, 0 = Sunday.
### WorkingTime/ToDay (int)
  Sets the last day of week for the `Timeline`. The value is a zero based index of day of weeek, 0 = Sunday.
### WorkingTime/FromHour (int)
  Fallback value for start of working time displayed on the `Timeline`. Valid values are [0;24]
### WorkingTime/ToHour (int)
  Fallback value for end of working time displayed on the `Timeline`. Valid values are [0;24]
### WorkingTime/MinutesInterval (int)
  Allows to specify the magnitude of a time step used in the timepickers for working times
### WorkingTime/IgnoreWorkingTimesInEndDateCalculation (bool)
  If true the events on the `Timeline` will ignore working times when calculating their end dates.
### ServiceOrderZipCodeAreaLength (int)
  Indicates the number of charecters to use as ZipCodeArea
### DispatchesAfterReleaseAreEditable (bool)
  If true the `Planning board` will allow editing `Dispatches` after they've been released
### DashboardCalendar/ShowAbsencesInCalendar (bool)
 If true (default) the `Absences` created in the `Planning board` will be displayed on the `Dashboard`'s calendar

## Sms.TimeManagement

### GeolocationGetCurrentPositionTimeout (int) 
 Defines time, after position locating request times out. Value should be in _millisecond_.


 ## Main
 ### MaxFileLengthInKb (int)

Set the maximum file size to upload. Attention: Request length limited by httpRuntime maxRequestLength=".."

### Task/AttentionTaskTypeKey (string)

Contain the key for the Attention task type 

<u>Default value:</u>

106