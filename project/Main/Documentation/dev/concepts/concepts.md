# Concepts of development
This section tries to focus on the bird view perspective of development. Some topics with a broad reach are discussed on a very high level. See further chapters for more details.

## E-Mail Dropbox
E-Mail Dropbox functionality comes in 2 different modes. POP3 and SMTP where each one has different requirements for setup, operation and maintenance.

The general idea behind the E-Mail Dropbox was that users are able to forward or pass message copies to the Crm/Service system with no more additional effort than Forwarding or Blind Carbon Copy a mail to the system.

The system should than inspect the message and find appropriate information to link the message to the corresponding entities. This chapter will introduce you to the technical foundations of the delivery. The inspection of messages is described elsewhere.

If the server is not able to relate the message to any object in the system the original sender receives a so called *bump message*. That message describes the problems the automatic inspection process found with the specific mail so the user can correct the issues before sending new mails.

- Make sure the Crm/Service server has well configured SMTP settings and is able to send mail, otherwise the bump-messages are dropped.

### SMTP Dropbox
The SMTP Dropbox is a small embedded SMTP server running inside the Crm/Service instance. It requires the application to be up and running as the SMTP Dropbox is started together with the application.

An SMTP Server is basically a small daemon listening on a specified port for incoming requests. SMTP uses a set of well [documented and simple commands](http://en.wikipedia.org/wiki/Simple_Mail_Transfer_Protocol#SMTP_transport_example) to retrieve the messages and respond to the calling process.

The most common scenario is a company using the Crm/Service system and using Microsoft Exchange as a mail server system. In that case sending mails to the Crm/Server system is as easy as the following steps:

**Requirements for the SMTP Dropbox**

- Make sure the server is able to send mail (system\.net smtp settings)
- Create a subdomain to run the Crm/Service mail domain for e.g. lmobile\.example\.com
- Add this subdomain to the web.config of the system so users will see their personal addresses ending with this hostname e.g. ab67b8d78@lmobile\.example\.com
- Create a new Send connector in outlook for the domain mail\.example\.com
- Specify a Smart Host and port for the Send connector to use
    - Use the IP Address or hostname of your L-mobile Crm/Service server
    - The port that the SMTP E-Mail Dropbox listener runs at can be specified in the jobs.xml file of the Crm/Service application. *Default port* is 25
- Make sure the Firewall on the server allows incoming connections on the specified ports
- Make sure the application is **always-on**. Because the SMTP Server is only started when the Application pool is
    - No Idle timeout
    - Automatic start of the application pool
    - HttpPing if necessary (after periodic recycles)
- After creating the Send Connector all mails sent via your Exchange system will be relayed to the Crm/Service system and processed accordingly
- If you want to use the SMTP E-Mail Dropbox make sure to operate both servers on different ports (e.g. 25 and 26, because each port can only be bound once)

---
**Please note** There are other Mail server solutions out there. It may give you the option to specify something similar to the Send Connector. But your mileage may vary depending on the system.

---

To make the SMTP Dropbox work globally (apart from mails sent using the corporate Exchange server) you would need to make sure all mail servers forward outgoing messages to your Crm/Service SMTP Server. This can be achieved by adding a global [MX DNS record](http://en.wikipedia.org/wiki/MX_record) for your subdomain. That way the server must be accessible from the Internet and even anonymous web servers are able to relay to the system.

### POP3 Dropbox
The POP3 Dropbox is an earlier implementation of the E-Mail Dropbox. It consists of the idea that you can specify a wildcard E-Mail recipient for each (Sub-) Domain. That way you told your Host that all incoming messages for *@lmobile\.example\.com would be stored in the central E-Mail mailbox for e.g. dropbox@l-mobile\.example\.com (think of this as the central postmaster).

Because you don't want that to happen on your primary domain, the specification of a subdomain is generally required. Otherwise all mails relayed to your primary domain (say info@example\.com) would be redirected to the central mailbox.

The POP3 E-Mail Dropbox Agent is a daemon process running inside the Crm/Service application. It will fire every once in a while (default every 3 seconds) and log in to the central POP3 mailbox checking for new incoming messages. The messages are downloaded from the server and processed afterwards. In any case the messages will be deleted from the mailbox after succesful retrieval.

**Requirements for POP3 Dropbox**

- Make sure the server is able to send mail (system\.net smtp settings)
- You need to specify the subdomain in the web.config of the application
- Prepare the wildcard email mailbox and handling with your domain hosted
- Note the credentials used to login that central mailbox
- Enter the user credentials to the job-data-map of the DropboxAgent in jobs.xml 
- Please make sure to use different subdomains and mailboxes for Test and Production to prevent accidental mail download
- Check the mail server and polling settings, sometimes polling a mailbox to often is considered harmful and the mailbox will be locked automatically (to prevent malicious attacks)
- Make sure the mailbox password does not expire. The Dropbox agent is a technical process and no one is going to enter a new password

---
**Perfomance** The POP3 dropbox is polling the mailserver consistently and creates additional network overhead.

---

### Recommendation
The SMTP Dropbox should be used wherever appropriate because it will fire only when an E-Mail arrives.  You should make sure only one of the 2 Agents is started.

## Database Versioning

**Preface**
Every developer uses a dedicated instance of the database
All changes are recorded in the version control

**Needed Functions**
Create a new database from scratch (Baseline)
Populate the baseline with the necessary Reference-Data
Populate the baseline with Test Data
Apply changes that were made to the database after creating the baseline

![Database Versioning in multiple developer environment](img/databaseversioning.gif "Database Versioning in multiple developer environment")

### Create

The database can be created using either

- the DeployBaseline.bat script
- downloading an empty database from the TeamCity build artifacts
- automatically as part of the database migrations running on application startup

### Test data

The test data is created inside a [OneTimeSetup](https://docs.nunit.org/articles/nunit/writing-tests/attributes/onetimesetup.html) method `CreateTestData`.
It will submit all objects defined under the TestData subfolder to the oData API. This requires the TestData to be submitted in an order which doesn't violate any foreign key constraints.

### Modify aka. Migrations

After setting the Baseline no changes should be made to the create scripts. Instead we need a mechanism to populate changes to existing databases without recreating them every time. I decided to go with an approach that is inspired by ruby migrations. Basically we create objects with a version information attached to change our database.

				
	[Migration(20090311134435)]
	public class Address_AddSomeColumn : Migration
	{
		public override void Up()
		{
			Database.AddColumn("Address", "SomeColumn", DbType.VarChar, 50)
		}
	}
				
			
The Up methods are executed by the framework when applying the migration. After executing the Migration a new version information is stored in the database. We have the power to create new tables, change existing columns and do some more stuff necessary to upgrade the database. The Version is basically a timestamp that makes sure we can upgrade the database until a specific version (date).

### Buildserver Integration

The build server uses the same method, getting the latest version of the source code and applying all migrations to bring the database to the current version.

### References

[Get Your Database Under Version Control](http://www.codinghorror.com/blog/archives/001050.html)
[Versioning Databases](http://odetocode.com/Blogs/scott/archive/2008/02/02/11721.aspx)
[Migrator.Net](http://code.google.com/p/migratordotnet/)
[Guide to Ruby on Rails Migrations](http://www.oracle.com/technology/pub/articles/kern-rails-migrations.html)

## Logging
We use Logging very extensively. On the server log4net is used, on the client js logging is available.

### log4net - SmtpAppender
- You can use SmtpAppender for debugging purposes to send logging messages to your email address.
- Open log4net **config file** in \src\Crm.Web\App_Data\config\log4net.config
- **Register** SmtpAppender in the root element.
- Enter your **email address** to which the messages should be sent.
- You can let all other configuration as they are - even the dummy email provider. It just works!
- See as well [Apache log4net](https://logging.apache.org/log4net/)

**Example**

	<log4net>
		<root>
			<level value="INFO" />
			<appender-ref ref="SmtpAppender"/>
		</root>

		<appender name="SmtpAppender" type="log4net.Appender.SmtpAppender">
			<to value="<recipient@example.com>" />
			<from value="<sender@example.com>" />
			<Username value="<sender-user>"/>
			<password value="<sender-pass>"/>
			<authentication value="Basic"/>
			<subject value="test logging message from log4net" />
			<smtpHost value="<smtp.example.com>" />
			<bufferSize value="512" />
			<lossy value="true" />
			<filter type="log4net.Filter.LevelRangeFilter">
		  <levelMin value="ERROR" />
		  <levelMax value="FATAL" />
		</filter> 
			<evaluator type="log4net.Core.LevelEvaluator">
				<threshold value="ERROR"/>
			</evaluator>
			<layout type="log4net.Layout.PatternLayout">
				<conversionPattern value="%newline%date [%thread] %-5level %logger [%property{NDC}] - %message%newline%newline%newline" />
			</layout>
		</appender>
	<log4net>	
	
----
**Variables** Please remember to fill in the < > marked fields in the above config.

----


## Testing - Towards reliable state
Tests provide guarantees about the working state of the features you develop.

### Selenium Tests
Selenium Tests are user interface tests that check if the user-facing part of the application functions without any errors. A user can programatically click buttons, navigate pages, submit forms and nearly anything else that a user of the application can achieve by using their keyboard and mouse. 

Additional Information is available in the [Selenium Documentation](http://seleniumhq.org/docs).

### Things to Know Before Writing Tests
There are some important points that you'll need to know before writing Selenium Tests, in order to make them work successfully. Here is the list of them:

- All animations are disabled when pages are called with a token

- Try to prevent login whenever possible, directly call the url you want to test and do it there instead of coming the long way home

- A token is automatically added from the main call to all subsequent ajax requests and forms  
  `GoToUrl( url , token )`  
  [[warning: There still isn't way to append tokens on redirects]]

- There are a bunch of WebDriverExtension methods to make sure you can wait for almost every situation

- Always add a bunch of extra time-out seconds as the server needs them

- Take care when checking the Visibility in Selenium WebElements (problematic with validation spans)

- Always create a page object to prevent Selectors flying all over the place

- Refactor your tests whenever they feel cluttered

- Think of using CSS Selectors instead of XPath as they tend to be a little faster

### Unit Test Data
Whenever you're writing Selenium Integration tests it is pleasant to have some Test-Data available which gets populated before running the Integration test suite. To achieve this a set of Data is created prior to running the unit tests. The test data is scripted in the `*.sql` files in the folder `src/Crm.Database/Create/TestData`. It should be published right after running the `DeployBaseline` and before running any migrations.
The test data should contain only relevant data, this means it will populate real or fictive Companies instead of dummy data like *"A new company"* etc.

One goal is to have a good set of test data available that is recreated reliably and doesn't change between releases. This way a demo system for sales purposes can be created from the command line.

## Translations
It is very important to add a translation to every non-user-generated string, as the CRM can be used with a growing list of languages.

### How to Add a Translation

[[thumbnail:translation.png]] Adding a translation is as easy as opening the corresponding resource file for the language you want to edit and adding a new line with a unique key that represents the string you want to add and the value, which will be used as the translation when the application is viewed in the language you are editing. Adding a comment, and specifying where that string is used (even though the key should clearly indicate that as well) also helps other people who may re-use the translation a lot, while preventing any misuse. 

### How to Use a Translation

A translated string is easily inserted to a page by calling a function as the following:

    <%= Html.Localize("TheTranslationKey", "Fallback text (Displayed if no translation present)") %>

### Things Not to Forget

-    Every translated string **MUST** be a sentence or a word with a meaning **on its own**. Translations of single words like "of", "a" and "so" aren't possible for many languages without a context.
-    Comments and self explanatory keys are encouraged and very important for re-usability.
-    Translation keys must be static. User generated content or anything that is read dynamically **MUST NOT** be translated.
-    Translations **MUST NOT** be parts of sentences. 
-    Translations **CANNOT** have embedded HTML.

## Targeting both Offline and Online modes
This page will include some information that may be handy when developing components which work both in online and offline modes for L-mobile Crm.
