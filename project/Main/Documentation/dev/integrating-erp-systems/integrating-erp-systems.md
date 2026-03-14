# Integrating ERP systems (Our Legacy Integration story)
Almost all Crm or Service installations will have to share data with the customers ERP or other legacy systems. The integration of data will allow for process automation between the systems. Existing data can be reused and does not have to be entered multiple times to different databases. 

The first step to integration is almost always read only integration. This means data is transferred from the ERP system to the Crm/Service system to fuel the Sales or service operations. In a second step data could be transferred from user input or process output of the Crm / Service system to further processing in the legacy (ERP or other) system. When it comes to write integration the integrity and quality of the to be transferred data will need to satisfy the legacy systems demand.

This document will explain the aspects of legacy integration. It will cover our preferred integration strategy and give insight to some viable alternative routes if that becomes necessary during the project.

## Architectural considerations
Most legacy systems use a database of one or the other kind. Often to be found systems include Oracle, MS SQL Server, Progress, DB/2 or other kinds of legacy or vendor specific storage solutions. The key aspect here is that the data is stored in a central, well structured and machine readable place. For an integration it comes down to the 3 main questions:

- How can one connect to the storage system
- What is the structure of the data (is there documentation or a person that could help with the integration)
- How do the servers interact on a network level basis (different subnets, firewall restrictions, port forwarding, etc.)

Once these questions are answered one can start with the basic integration of the legacy system. But keep in mind that for a production ready integration you need to take into account aspects like: transparency, performance, logging and stability. We will show some aspects of creating a successful integration in the later chapters of this document.

## Integrating database systems
As you probably know the main storage for the Crm / Service system is a SQL Server database that will be hosted on a SQL Server instance. Your task is to grab the relevant data structures from the legacy system and transfer them to the Crm / server structures. The easiest way to achieve this kind of integration is mapping the remote storage of the legacy system to a normalized form inside of the SQL Server instance of the Crm / Service database. Fortunately SQL Server is able to connect to a bunch of database systems using the so called "[Linked server](https://msdn.microsoft.com/en-us/library/ms188279.aspx)" technology.

A linked server acts like a proxy to the remote database system. You can issue queries right from your SQL Server Management studio. But keep in mind, that when using linked servers you should use the generic version of T-SQL that is implemented throughout the different databases. You can always opt to implement queries in the native dialact of the linked server using tools like [OPENQUERY](https://msdn.microsoft.com/en-us/library/ms188427.aspx).

Connecting to another database instance often requires specification of remote credentials for authentication. The credentials only need read only permissions, that should be no problem to obtain from the administrator of the project.

### MS SQL Server (MS Dynamics NAV, MS Dynamics AX)
Connecting to another instance of SQL Server is fairly straightforward. You can find instructions to create a new linked server [here](https://msdn.microsoft.com/en-us/library/ff772782.aspx#SSMSProcedure).

### Oracle (Infor Erp Com, Infor Erp Blending, etc.)
Connecting to an Oracle database requires a little more of preparation. SQL Server does not natively contain drivers to connect to the remote instance. But there are ways to add a linked server using a combination of [Oracle software](http://www.bogazci.com/2011/12/oracle-instant-client-installation/). Please keep in mind that you need the right version of tools for your host database instance. Most SQL Server installations are 64bit nowadays. So please take care to use the right packages.

### Progress (ProAlpha)
The last known good way to connect to a Progress system and transfer data includes creation of an System DNS ODBC connection using the Progress ODBC driver. Adding a linked server to the System DNS then allows you to access the remote database.

### Other database systems / CSV file integration
A ODBC driver exists for most of todays database systems as this is the de-facto standard for database integration. If your system was not present in the former list please assure that the vendor of the ERP database system supports standard ODBC access to the database. Create a System-DNS entry and access the database from the Linked Server for ODBC function in MS SQL Server.

Other forms of integration allow to read data from CSV or other structured files (XML, etc.). But using this kind of files the risk of malformatted files increases.

### Programmatic import
For some ERP systems (like SAP) no direct database access will be granted. That means the solutions described in this document won't apply to your project. Under all circumstances you will want to try to evaluate database access nonetheless due to the improved performance and maintainability. If no other options exist you will probably find yourself writing a set of C# files that execute the import by reading from Webservices, Remote APIs or other locations.

## The integration folder

When developing a legacy system integration it is recommended to stick to a set of best practices. This enables other developers to help out and understand the structure quickly. In the later chapters references are made to some templates that can be used to create new or extended integrations.

Common Integration structure pattern

	src
		Crm.Web
			Plugins
				Customer.XXX
					Integration
						Import
							I_External_X.sql
							I_External_Y.sql
						Linked Server
						Views
							V_External_X.sql
							V_External_Y.sql
					IMPORT_Daily_Prod.bat
					IMPORT_Daily_Test.bat
					IMPORT_Periodic_Prod.bat
					IMPORT_Periodic_Test.bat

The integration itself is not part of the actively maintained standard product at the creation of the document (April 2015)

## Normalizing legacy data to known formats - the X_External story
Once the connection to the remote database system is established one can start to create SELECT statements against the remote database structure. But this causes new challenges. Most of the time the database will have weird field and table names. These should not leak from the remote database structure to the Crm / Service system for 2 reasons:

- The mapping between remote and local storage structures should be done at a central and maintainable location
- All scripts that interact with the remote structure need to know the specific implementation leaving less room for reusage of once existing scripts

To solve both building an interface to the legacy system involves creation of normalized structures referred to as V_External (Views) or T_Extenal (Physical tables). Depending on the abilities of the remote database system choose one or the other. This document will focus on using external views for normalization.

It is preferable to create an individual view for each entity that will be integrated with the legacy system. The views will always have a similar structure:

	CREATE VIEW V_External_Company
	AS
	SELECT 
		a.FieldWithCompanyNo AS CompanyNo
		,a.FieldWithCompanyName AS Name
		,BINARY_CHECKSUM(
			a.FieldWithCompanyNo
			,a.FieldWithCompanyName
		) AS LegacyVersion
	FROM LinkedServer.Schema.RemoteTable a


or for Oracle using OPENQUERY

	CREATE VIEW V_External_Company
	AS
	SELECT a.*
		,BINARY_CHECKSUM(
			a.CompanyNo
			,a.Name
		) AS LegacyVersion
	FROM OPENQUERY(LINKEDSERVERNAME, 
		'SELECT
			FieldWithCompanyNo AS CompanyNo
			,FieldWithCompanyName AS Name
		FROM RemoteTable') AS a

Writing views like this focusses on the main aspects of mapping, the remote data selection process and converting the Remote structure to a local normalized structure wherever necessary. When type conversion between remote and local structure becomes necessary this should go into the views as well to make sure it is maintained at a central location.

For the creation of the views one should focus to keep the view files outside of the database so these can be easily transferred between the development and customer environment.

## Importing from your normalized tables - Merge
When it comes to importing from the normalized structures one SQL Server tool is the key to keeping things nice and clean. Meet the [SQL Server MERGE statement](https://msdn.microsoft.com/en-us/library/bb510625.aspx). The SQL Server merge statement basically allows to use a set of source records and merge them to your given target destination based on user defined conditions. This allows us to read data from the normalized structure and conditionally insert or update records in the target Crm / Service structure.

Merge statements consists of some common elements:

- Merge source
- Merge target
- Link condition e.g. What is the primary link between source and target
- What to do if target does not contain a record aka WHEN NOT MATCHED
- What to do if record exists in source and target aka WHEN MATCHED
- What to if record stops to exist in source aka WHEN NOT MATCHED IN SOURCE

Plus there is a nice addon called OUTPUT clause. The output clause gives you the results of the MERGE operation in a separate table. This table can be inspected and reused for other operations. Using the keyword *$action* you will be able to distinguish INSERT, UPDATE and DELETE operations.

## The two step import
In the Crm / Service system many tables exists that contain of records in at least 2 tables. The inheritance of classes forces the common elements to be stored in a root table that contains some kind of Discriminator ([read about it here](http://ayende.com/blog/3941/nhibernate-mapping-inheritance)). Most of the time the specific attributes for a class are then stored in a second table using the primary key of the root table as foreign key.

For integration of legacy systems this creates a new challenge: You have to merge to the root table, grab the inserted primary keys and store them temporarily to merge a second time to the extended table using the primary key from the root as a foreign key.

But fear not, there are common scripts that will help you understand and achieve the former.

### Prepare input storage
First we prepare an intermediate storage for our 2 step import process. This sounds odd at first, but you will find out about the use of this table later:

	IF OBJECT_ID('tempdb..#ContactImport') IS NOT NULL DROP TABLE #ContactImport
	CREATE TABLE #ContactImport (Change NVARCHAR(100), 
										ContactId INT, 
										LegacyId NVARCHAR(100))

Now we prepare our temporary import storage by reading from the source structure into a temporary storage table

	IF OBJECT_ID('tempdb..#Import_Company') IS NOT NULL DROP TABLE #Import_Company

	SELECT 
		v.CompanyNo
		,v.Name
		,v.LegacyVersion
	INTO #Import_Company
	FROM V_External_Company AS v

Please note the INTO #Import_RemoteTable part of the select statement. This way you don't need to declare the full table structure, using this and your view declaration the implicitly created table structure for #Import_RemoteTable will match the desired outcome.

### Create indices in temporary table -> Performance boost
Creating an index in your temporary import table will speed up the merge operation dramatically.

	CREATE NONCLUSTERED INDEX IX_#Company_CompanyNo ON #Import_Company ([CompanyNo] ASC)

### Merge operation - Step 1
In the first step we will take all data from the temporary import storage that does not exist in CRM.Contact table and insert records accordingly. Please note the conditions in the WHEN MATCHED part. The checks on LegacyVersion prevent unnecessary updates to the table (reducing table locks and performance problems).

	MERGE [CRM].[Contact] AS [target]
	USING #Import_Company AS [source]
	ON [target].[LegacyId] = source.[CompanyNo]
	AND [target].[ContactType] = 'Company'
	-- We already know a record with this LegacyId
	WHEN MATCHED 
		AND ([target].[LegacyVersion] IS NULL OR [target].[LegacyVersion] <> [source].[LegacyVersion]) 
		THEN
		UPDATE SET 
			[target].[LegacyId] = source.[CompanyNo]
			,[target].[LegacyVersion] = source.[LegacyVersion]
			,[target].[ModifyDate] = getutcdate()
			,[target].[ModifyUser] = 'Import'
			,[target].[Name] = source.[Name]
					
	-- If not found we try to insert with the appropriate data
	WHEN NOT MATCHED 
		THEN
		INSERT
		(
			[ContactType]
			,[LegacyId] 
			,[LegacyVersion] 
			,[IsExported] 
			,[Name]
			,[IsActive]
			,[Visibility]
			,[CreateDate] 
			,[ModifyDate]
			,[CreateUser] 
			,[ModifyUser]
		)
		VALUES 
		(
			'Company'
			,source.[CompanyNo]
			,source.[LegacyVersion]
			,1
			,source.[Name]
			,source.[IsActive]
			,4
			,getutcdate() 
			,getutcdate()
			,'Import' 
			,'Import'
		)
	-- Record was previously imported and is still active in target
	WHEN NOT MATCHED BY SOURCE 
		AND [target].[IsActive] = 1 AND [target].[ContactType] = 'Company' AND [target].[LegacyId] IS NOT NULL 
		THEN
		UPDATE SET 
			[IsActive] = 0
			,[ModifyDate] = GETUTCDATE()
			,[LegacyVersion] = NULL
	-- All records to the temp table including their action
	OUTPUT $action
			,inserted.ContactId
			,[source].CompanyNo AS LegacyId
	INTO #ContactImport;

With this statement the first part of the tables get published. But once you have a record in CRM.Contact you want to make sure to add to the CRM.Company table as well to make things complete. Please note the OUTPUT clause in the statement which makes sure all merge results get transferred to your intermediate storage table. This table will contain only the most necessary informations to complete the

### Merge operation - Step 2
For the 2 step merge process we will take the results of the first merge step into the root table, join them with the original source table and import to the extended entity storage table.

	MERGE [CRM].[Company] AS [target]
	USING (SELECT ci.ContactId, c.*
			FROM #Import_Company c
			JOIN #ContactImport ci ON c.CompanyNo = ci.LegacyId) AS [source]
	ON [target].[ContactKey] = [source].[ContactId]
	-- For all new records we insert
	WHEN NOT MATCHED THEN
		INSERT 
		(
			[ContactKey]
			,[ShortText]
			,[SearchText]
			,[CompanyTypeKey]
			,[IsOwnCompany]
		)				
		VALUES 
		(
			source.[ContactId] 
			,source.[Name]
			,source.[Name]
			,'Customer'
			,0
		)
	-- For all found temp records we update accordingly
	WHEN MATCHED 
		THEN
		UPDATE SET 
			[ShortText] = source.[Name]
			,[SearchText] = source.[Name];

	Here you see considerably less complexity. By joining the results of the first merge step and the original source data we only carry out plain INSERT or UPDATE operations. The JOIN also reduces the amount of data processed (resulting in faster merges).

## Importing data for distributed usage, adding offline clients to the mix
When it comes to adding offline clients to a system, one of the most important things to consider is the legacy data integration. Offline clients will sync their local storage depending upon the modification date found in the source tables of the remote Crm / Service database. This is due to the fact that the clients only want to sync new or changed records to keep network traffic at a low rate.

Therefore it is extremely important to make sure you update your ModifyDate columns during update operations using UTC time stamps. On the other hand you will only want to update the records if this becomes necessary. Calculating a record hash over the imported legacy records helps you make that decision.

---
IMPORTANT: When offline clients sync against your database, make sure to use LegacyVersion and ModifyDate (UTC) in all relevant tables to prevent unnecessary sync of data.

---

## How to import in a transparent fashion, logging and surveillance
When it comes to the maintenance of Import structures we found it very helpful if the Import scripts would create Log files or log messages in the CRM.Log table. For this purpose one can sprinkle some debugging statements into the merge process to help understand what's going on.

### Original row count
To know about the amount of records being used in the temporary import storage in the first place.

	DECLARE @logmessage NVARCHAR(4000);
	DECLARE @count bigint;

	SELECT
		...
	INTO #Import_Company
	FROM V_External_Company

	SELECT @count = COUNT(*) FROM #Import_Company
	SELECT @logmessage = CONVERT(nvarchar, @count) + ' Records transferred to input table'
	PRINT @logmessage

### Intermediate records
To find out about the amount of records transferred into the intermediate storage. Please take a close look at the Update count as it will most times relate to the amount of records being updated on the mobile client after a sync.

	MERGE
		...
	OUTPUT $action
		...
	INTO #ContactImport;

	SELECT @count = COUNT(*) FROM #ContactImport
	SELECT @logmessage = CONVERT(nvarchar, @count) + ' records processed to intermediate table'
	PRINT @logmessage

	SELECT @count = COUNT(*) FROM #ContactImport WHERE Change = 'INSERT'
	SELECT @logmessage = CONVERT(nvarchar, @count) + ' records inserted'
	PRINT @logmessage


	SELECT @count = COUNT(*) FROM #ContactImport WHERE Change = 'UPDATE'
	SELECT @logmessage = CONVERT(nvarchar, @count) + ' records updated'
	PRINT @logmessage

### Try Catch
When your import script has an error at one place or the other you probably want to log the exception to the CRM.Log structures. You can wrap all parts of the import process in a Try ... Catch block allowing for further inspection of the error.

	BEGIN TRY
		... Prepare intermediate table
		... Prepare input table
		... Merge 1st step
		... Merge 2nd step
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		SELECT
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();
			
		PRINT @ErrorMessage
	END CATCH

## Triggering import scripts
As we keep the import scripts as separate sql files in the integration folder it is easy to trigger the import using the command line tool *osql* which is provided as part of the SQL Server tools installation. You will want to create a bunch of batch files expressing what system and interval you're going to import to e.g. IMPORT_*DAILY*_Prod.bat

The file will contain the list of to be executed scripts together with some paths for logging:

	set db-server=SQL Server
	set db-name=LmobileProd
	set log-folder=C:\Integration\Log
	set integration-folder=C:\Integration

	osql -E -S %db-server% -d %db-name% -i "%integration-folder%\Import\I_External_CompanyType.sql" -o "%log-folder%\I_External_CompanyType.log"
	...

This batch file will then be scheduled using [windows scheduled tasks](http://windows.microsoft.com/en-us/windows/schedule-task).

---
Windows scheduled tasks by default doesn't let you run tasks more often than every 5 minutes (the smallest value in the drop down). To use a 1 Minute interval just type *1 minute" to the input field and you're good to go.

---

## Common Pitfalls
As with all complex systems adding database integration to a project can cause a lot of potential for errors. In some circumstances you will want to add additional functions to your import scripts or more complex logic to decide if a record needs to be updated or not.

- Try to question the import of the source data. Try to keep integrated row count as small as possible.
- When updating your data with external sources make sure to have a conflict resolve strategy (data loss can occur)
- Remember to store LegacyVersion to prevent unnecessary updates
- Always remember that Create and ModifyDate shall be stored using UTC instead of local time
- Try to keep import scripts as brief as possible, prevent additional code for better readability / maintenance

- BINARY_CHECKSUMs are okay but have their limitations: Example if a Decimal number changes by a factor of 10, e.g. 100,00 to 10,00 you will get the same checksum. -> Convert them to string before calculating the checksum
- Use a database connector for the legacy database that is compatible with your host architecture (e.g. 32 or 64bit)

- Try to store data from remote location in temporary host tables to prevent unnecessary cost intensive reads
- Use local temporary tables (#table) instead of table variables (@table) wo optimize memory usage with big tables 
- Don't join remote and local tables unless it becomes really necessary, joining hits performance a lot