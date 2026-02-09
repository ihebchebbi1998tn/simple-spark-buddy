# Recipes for developing in Customer Plugins
The following document will give you an insight into often used functions in the customization of the Crm / Service application. Please use it as a reference to get guidelines for developing new or changing existing features. If you find the information to be outdated or inadequate please contact either [Oliver Joest](oliver.joest@l-mobile.com) or [Oliver Titze](oliver.titze@l-mobile.com).

## Working with Customer Plugins
As the system grows and expands we keep all code for a single customer at a central location, the **Customer Plugin**. A running system is designed to stack the plugins the right way, so that a customer plugin may expand or override the underlying base system.

You can think of the system as a layered cake with the following structure

Customer Plugin Layer

----
Generic Plugin Layer

----
Base layer 

Whereas the customer Plugin is the highest available layer today. To create a customer Plugin some conventions are demanded:

- Name your Customer plugin in the format Customer.[CustomerName]
- The Project should be added to the solution folder *Customer plugins*
- A solution should **never** contain more than **one** individual customer plugin

Example

    src
	    Crm.Web
		    Plugins
			    Crm.Article
			    Crm.Campaigns
			    ...
			    Customer.CustomerName
			    ...

- Whenever you choose to add functions you are requested to do so via the Customer Plugin layer
- Whenever you are requested to extend existing base layer functions you are requested to do so via the Customer Plugin

### The CustomerNamePlugin.cs file
The customer project will contain a file called [CustomerName]Plugin.cs. This file will include the following elements

	namespace Customer.ACME
	{
		using Crm.Library.Modularization;

		[Plugin(Requires="Crm.Article,Crm.Service")]
		public class CustomerACMEPlugin : CustomerPlugin
		{
			...
		}
	}

In brief the following file contains several important informations. 

### PluginAttribute
The class Attribute *PluginAttribute* allows you to specify other required Plugins that the customer plugin requires. The order specified does not matter for the dependency selection.

----
Although it is not necessary to type all direct and indirect dependencies of the plugin, it is considerered best practice to include all dependencies for a better overview. E.g. Crm.Service already requires Crm.Article and could be omitted from the customer plugin

----

### CustomerPlugin inheritance
The class itself should inherit the class *CustomerPlugin* and not the more generic *Plugin* class as this would leave the system unable to sort the Customer plugin to the highest level of layering.

## Background services
Systems often need to carry out time consuming tasks that may be run asynchronously. Sometimes systems should just execute tasks in certain intervals (e.g. every minute) to process other data. Either way you will want to create some infrastructure to carry out recurring tasks. These are called Background services and come in 2 flavors.

### Background Services - Automatic Session Handling
When your customer project demands for recurring processes to be executed you can include new Background Services in the corresponding folder:

	Customer.ACME
		BackgroundServices
			CustomBackgroundService.cs
		jobs.xml

The class will contain at least the following elements

	namespace Customer.ACME.BackgroundServices
	{
	
		using Crm.Library.BackgroundServices;
		using Quartz;

		[DisallowConcurrentExecution]
		public class CustomBackgroundService : JobBase
		{
			protected override void Run(IJobExecutionContext context)
			{
				// Periodic executed Code goes here
			}
		}
	}

Jobs that are created this way will automatically handle session transactions before and after the Run method. Please be aware that you should not execute long running tasks
in this context, because holding the transaction open for a longer time will lead to deadlocks on the tables. If you need to manage the session and transaction times your self you may choose

### Background Services - Manual Session Handling
With manual session handling you have the chance to open and close transactions while you process your data. This is useful if you have plenty of reccords that need processing in batches.
By using the manual session handling you're able to control the locking time of incorporated tables. If there are long running calls against external systems you should choose the Manual Session Handling.

	namespace Customer.Grimme.BackgroundServices
	{
		using System;
		using System.Linq;

		using Crm.Library.BackgroundServices;
		using Crm.Service.Model;

		using Customer.Grimme.Services.Interfaces;

		using Quartz;

		[DisallowConcurrentExecution]
		public class OrderExportAgent : ManualSessionHandlingJobBase
		{
			protected override void Run(IJobExecutionContext context)
			{
				var collection = exportService.GetUnexportedEntities().ToList();

				foreach (var entity in collection)
				{
					try
					{
						BeginTransaction();
						exportService.Export(entity);
						EndTransaction();
					}
					catch (Exception e)
					{
						Logger.Error(String.Format("Error exporting entity {0}", entity.Id), e);
						RollbackTransaction();
					}
				}
			}
		}
		
As you can see above an individual session will be created for each order exported.

----
Bottom line: Choose the manual session handling if you need full control, choose the job base if the automatic opening and closing of sessions is sufficient

----

### Background Services - jobs.xml
The jobs.xml file in the root of the customer plugin will specify a trigger for the new background processing task. This will look like

	<job-scheduling-data xmlns="http://quartznet.sourceforge.net/JobSchedulingData"
			 xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
			 version="2.0">

		<processing-directives>
			<overwrite-existing-data>true</overwrite-existing-data>
		</processing-directives>

		<schedule>
			<job>
				<name>CustomBackgroundService</name>
				<group>Customer.Acme>group>
				<description>Executed periodically.</description>
				<job-type>
				Customer.ACME.BackgroundServices.CustomBackgroundService, 
				Customer.ACME
				</job-type>
				<durable>true</durable>
				<recover>false</recover>
			</job>
			<trigger>
				<cron>
					<name>CustomBackgroundServiceTrigger</name>
					<group>Customer.ACME</group>
					<description>Runs every five minutes.</description>
					<job-name>CustomBackgroundService</job-name>
					<job-group>Customer.ACME</job-group>
					<cron-expression>0 0/5 * 1/1 * ?</cron-expression>
				</cron>
			</trigger>
		</schedule>

----
Please consider adding a jobs.template.xml and corresponding jobs.debug.xml and jobs.release.xml files if you wish to transform the contents of the jobs file based on the configuration you're going to build

----

## Business Rules
Business rules are used to handle form processing in the system. Whenever a form element is rendered using the HtmlHelper extensions the business rules will be attached to the 
corresponding input field. When the user submits a form to save an entity all active business rules are evaluated and validation messages will be rendered to the failing input element.

### CrmModelBinder for new created classes

### Overriding rules from Customer plugin

### Ignoring rules from Customer plugin

### Creating new rules from Customer plugin

*Rule Types*

- Crm.Library.Validation.BaseRules

### HtmlHelperExtensions for Business rules

#### WrappedXXX for server side forms

#### TemplateInput for Knockout bound forms

## Routing

### Create new Routes for your customer plugin
If you need any new controllers try to stick to the default route scheme Plugin/Controller/Action/Id. Nonetheless you have to register 2 common routes for your customer plugin:

	namespace Customer.ACME
	{
		using System;

		using Crm.Library.Modularization.Registrars;
		using Crm.Library.Routing.Constraints;

		public class Routes : IRouteRegistrar
		{
			public virtual RoutePriority Priority
			{
				get { return RoutePriority.Low; }
			}
			public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
			{
				endpoints.MapControllerRoute(
					null,
					"Customer.ACME/{controller}/{action}/{id?}",
					new { action = "Index", plugin = "Customer.ACME" },
					new { plugin = "Customer.ACME" }
					);
			}
		}
	}
	
----
Please don't register new custom routes because that will impact the performance of the routing module. 

----

### Redirecting existing routes to customer plugin

	public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
	{
		...
		endpoints.MapControllerRoute(
			null,
			"Crm.Project/ProjectList/GetAll",
			new 
			{ 
				plugin = "Customer.ACME", 
				controller = "ProjectList", 
				action = "GetAll" 
			}
		);
		...
	}

Optionally you can declare different controller and action names if necessary, just specify them in the override definition for Controller and Action parameters e.g.

	public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
	{
		...
		endpoints.MapControllerRoute(
			null,
			"Crm.Project/ProjectList/GetAll",
			new 
			{ 
				plugin = "Customer.ACME", 
				controller = "CustomProjectList", 
				action = "CustomGetAll" 
			}
		);
		...
	}

## Menu extensions~~~~
You're able to extend and modify the existing main menus of the application. By registering new menu entries to the left, right or mobile menu you're able to extend the application with custom implementations. To change the default menu structure you have to create a custom MenuRegistrar class. To extend certain parts of the menu you may choose to inherit the following interfaces:

- IMenuRegistrar<MaterialMainMenu> for the menu in the material client
- IMenuRegistrar<MaterialUserProfileMenu> for the collapsible menu in the material client
~~~~
```c#
namespace Customer.ACME
{
	using Crm;
	using Crm.Library.Modularization.Menu;
	
	public class ACMEMenuRegistrar : IMenuRegistrar<MaterialMainMenu>, IMenuRegistrar<MaterialUserProfileMenu>
	{
		public virtual void Initialize(MenuProvider<MaterialMainMenu> menuProvider)
		{
			// operations on menu for material client
		}
		public virtual void Initialize(MenuProvider<MaterialUserProfileMenu> menuProvider)
		{
			// operations on collapsible menu for material client
		}
	}
}
```

### Unregistering existing menu entries
When you do want to completely remove already existing menu entries from other plugins you need to know the category and title they were registered with. To find out about the plugin you may inspect the incorporated link of the Menu entry e.g. `<a href="/Crm.Order/OrderList" class="">Auftrag</a>`. With this information you can look up the registered menu entries from Crm.Order plugin by looking at the corresponding OrderMenuRegistrar class. With this information you may:

```c#
...
public virtual void Initialize(MenuProvider<MainMenu> menuProvider)
{
	...
	menuProvider.Unregister(category: "Main", title:"Order");
	...
}
...
```

to remove the main entry completely.

### Registering new menu entries
To add new entries you can call the register method on your menuProvider instance.

```c#
...
public virtual void Initialize(MenuProvider<MainMenu> menuProvider)
{
	...
	menuProvider.Register(category: "Main", 
							title: "ACME");
	menuProvider.Register(category: "ACME", 
							title: "CustomProcess", 
							url: "~/Customer.ACME/CustomProcess", 
							priority: 100);
	...
}
...
```

in the upper example some details are noteworthy. The registration is a 2 step process. In the first step a new category *ACME* is registered onto the *Main* level of the left menu.
In the next step a CustomProcess is registered in the ACME category which leads to the specified Url. If you would need to register multiple entries below the *ACME* category you could customize the order by specifying different priority values.

```c#
...
menuProvider.Register(category: "ACME", 
						title: "CustomProcess1", 
						url: "~/Customer.ACME/CustomProcess1", 
						priority: 100);
menuProvider.Register(category: "ACME", 
						title: "CustomProcess2", 
						url: "~/Customer.ACME/CustomProcess2", 
						priority: 90);
...
```

----
When rendering menu entries the category and title attributes will be processed by a localize call. So please make sure to add the corresponding Resource entries for localization. 

----

### Menu entries, adding permissions
After you register your new menu entries you can request individual Permissions to access the elements.

```c#
...
menuProvider.AddPermission(category: "ACME", 
							title: "CustomProcess1", 
							permissionGroup: "ACME", 
							permissionName: "CustomProcess1");
...
```

----
Please remember to add the corresponding permissions from code or via Database migration afterwards.

----

### Menu entries, adding icons
When registering new menu entries for the material client, the optional named parameter *iconClass*  can be used to specify a matching icon. The icon font included in the client is the [Material Design Iconic Font](http://zavoloklom.github.io/material-design-iconic-font/icons.html).

```c#
...
menuProvider.AddPermission(category: "ACME", 
							title: "CustomProcess1", 
							iconClass: "zmdi zmdi-home"
							permissionGroup: "ACME", 
							permissionName: "CustomProcess1");
...
```

## Generating IDs on the client-side
How to generate IDs on the client-side without ID collisions.

### Client Side Additions

#### 1 - Register the ID provider:

    window.Crm.Offline.IdProvider.registerHighLowProvider(
        'CrmService_ServiceOrderDispatch',
        'ServiceOrderDispatch',
        'Crm.Service');

#### 2- Initialize the ID providers

    window.Crm.Offline.Bootstrapper.initializeIdProviders.then(function() { //you are ready to generate IDs at this point

#### 3- Calculate and attach the new ID when saving your new entities

    var myNewId = window.Crm.Offline.IdProviders.CrmService_ServiceOrderDispatch.getId();

### Server Side Changes

 * Change class to be EntityBase<long> instead of int
 * Migrate table (e.g. \src\Crm.Web\Plugins\Sms.TimeManagement\Database\20140206125023_ChangeSmsTimeManagementEventToHighLowIds.cs)

		[Migration(20140206125023)]
		public class ChangeSmsTimeManagementEventToHighLowIds : Migration
		{
			private const int Low = 32;

			public override void Up()
			{
				Database.RemovePrimaryKey("SMS", "TimeManagementEvent");
				Database.RenameColumn("[SMS].[TimeManagementEvent]", "TimeManagementEventId", "Id_Identity");
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeManagementEvent] ADD TimeManagementEventId bigint NULL");
				Database.ExecuteNonQuery("UPDATE [SMS].[TimeManagementEvent] SET TimeManagementEventId = Id_Identity");
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeManagementEvent]
					ALTER COLUMN TimeManagementEventId bigint NOT NULL");
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeManagementEvent] DROP COLUMN Id_Identity");
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeManagementEvent] ADD CONSTRAINT PK_TimeManagementEvent
					PRIMARY KEY(TimeManagementEventId)");
				Database.ExecuteNonQuery("BEGIN IF ((SELECT COUNT(*) FROM dbo.hibernate_unique_key
					WHERE tablename = '[SMS].[TimeManagementEvent]') = 0) INSERT INTO hibernate_unique_key
					(next_hi, tablename) values ((select (COALESCE(max(TimeManagementEventId), 0) / " + Low + ") + 1
					from [SMS].[TimeManagementEvent] where TimeManagementEventId is not null), '[SMS].[TimeManagementEvent]') END");
			}
			public override void Down()
			{
				throw new NotImplementedException();
			}
		}
 * Change NHibernate key mapping to Generators.Assigned
 * Create HighLowDefinition for class

		public class TimeManagementEventHighLowDefinition : HighLowDefinition
		{
			protected override string Tablename
			{
				get { return "[SMS].[TimeManagementEvent]"; }
			}
		}
 * Implement IHighLowEntity<>

		public class TimeManagementEvent : EntityBase<long>, IHighLowEntity<TimeManagementEventHighLowDefinition>

 * Change SyncService / Reference to Repositories like:
     * DefaultSyncService<TimeManagementEvent> -> DefaultSyncService<TimeManagementEvent, long>
     * IRepository<TimeManagementEvent> -> IRepositoryWithTypedId<TimeManagementEvent, long>

## Creating and querying a hybrid model
This page explains creating an offline (client-side) model and using this model to query both online and offline sources, in a step-by-step fashion.

 [[warning:This page is a work in progress]] 

### Step 1 - Tell the application to generate a client side model for your entity

	[ProvidesClientSideModelDefinition(typeof(Model.Order), typeof(Model.OrderItem))]
	public class OrderPlugin : Plugin
	{
		public new static string Name = "Crm.Order";
	}

In this example, the application will (try to) generate a client side definition of the entities "Order" and "OrderItem". It is important to add this attribute to the Plugin definition as the generator (`GetDefinitions` method of `Crm.Offline.Controllers.OfflineController`) will only check those to gather type information. When making queries from JayData, the library will use these models to make request on both online and offline databases/services. It is also required that the Entities defined here have a Rest model.

If you want to define your model manually, here's an example to get you started (No need to do this when you use the attributes in your plugin definition):

	//TODO: to reduce the cargo-cult effect, explain what each step does.
	var definition = {
		ItemNo: { type: "string" },
		Description: { type: "string" },
		ArticleType: { type: "string" },
		Price: { type: "number" },
		IsActive: { type: "bool" }
	}
	var modelPath = "Crm.Article.Article"; //by convention
	var modelDbPath = "CrmArticle_Article"; //by convention
	$data.define(modelPath, definition);
	namespace('Crm.Offline').SyncObjects.push(function () { return window.ko.observableArray([]).config({
		storage: modelDbPath,
		model: "Article",
		pluginName: "Crm.Article"
	});
	window.Helper.Offline.registerTable("CrmArticle_Article", "Article", "Crm.Article", definition);
	Offline.EntityStore = $data.EntityContext.extend("Offline.EntityStore", { "CrmArticle_Article" : definition });

### Step 2 - Create a sync service

Derive from the `DefaultSyncService<TEntity, TId>`. Application will use this service to decide what to sync and how to save. Here's an example:

	public class OrderSyncService : DefaultSyncService<Order, long>
	{
		public override Order Save(Order entity)
		{
			/* save the entity here */
			return entity;
		}
		public override IQueryable<Order> GetAll(DateTime? lastSync)
		{
			/* entities is available to query the database */
			return entities.Since(lastSync);
		}
		public override IQueryable<Order> Filter(IQueryable<Order> entities, User user, DateTime? lastSync)
		{
			return entities.Where(o => o.ResponsibleUser == user.Id).Since(lastSync);
		}
	}

The code is mostly self-explanatory. You can look at the `DefaultSyncService` implementation to have a better idea of what you can override to alter the sync process.
Now, when you visit */Crm.Offline/Offline/Sync*, you'll have your entities synced to the client.

The name of the database will be {plugin name without dots}_{entity name}. For example, Orders will be stored in the `CrmOrder_OrderItem` table and Articles will be in `CrmArticle_Article`.

### Step 3 - Altering your entity to support client-side ID generation

It's <s>easy</s> *not hard* to allow the creation of new entities on the client side and having the client deal with the new IDs it generates. This step is explained in a separate page: [[link:/project/advanced-topics/targeting-both-offline-and-online-modes/generating-ids-on-the-client-side/||Generating IDs on the client-side]]

### Step 4 - Using the generated model to query offline and online sources

JayData is used in the Crm to query the data. Here is an example query which will work both online and offline (after a sync):

	window.currentDatabase.Crm_Company
		.find(companyId)
		.then(function (company) {
			// company is the entity with the given id
		});

