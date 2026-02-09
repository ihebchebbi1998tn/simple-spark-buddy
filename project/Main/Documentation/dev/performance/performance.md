# Optimize performance for web applications

All recommendations presented here are based on experience for a complex application based on Asp.Net and the associated data structure (e.g. Crm or SMS, possibly with offline components).

## Database

- SQL Server Express limitations through full SQL Server. SQL Server Express can only allocate 1GB memory per instance ([Microsoft comparasion](https://docs.microsoft.com/en-us/sql/sql-server/editions-and-components-of-sql-server-version-15?view=sql-server-ver15))
- Memory, memory, storage and a bit of CPU ie at least 8GB, 4 cores or better.
  > "If your server is slow and you’ve got less than 64GB of memory, learn how to explain business costs as I explain in the video. It’s smarter to buy $500 worth of memory rather than spend days struggling with a problem and making risky changes to your server. Sure, the business is going to argue against you – that’s their job, and it’s your job to clearly explain the pros and cons of each side" via [Brentozar](http://www.brentozar.com/archive/2013/09/how-to-prove-your-sql-server-needs-more-memory-video/)

- Physically separate the database server and web server but connect them with high performance (e.g. Gigabit Ethernet)  
- Look for suspicious queries, that are, profiling the database and look for any suspicious pattern. For example with the help of [SQL Express Profiler](http://expressprofiler.codeplex.com)

- The right indices are especially important for complex queries, for this there are a few sources:
    - http://www.brentozar.com/sql/index-all-about-sql-server-indexes/

~~~
-- Create demo table
CREATE TABLE Sales(
	 ID INT IDENTITY(1,1)
	,ProductCode VARCHAR(20)
	,Price FLOAT(53)
	,DateTransaction DATETIME);

-- Create test data proc
CREATE PROCEDURE InsertIntoSales
AS 
SET NOCOUNT ON
BEGIN
DECLARE @PC VARCHAR(20)='A12CB'
DECLARE @Price INT = 50
DECLARE @COUNT INT = 0
      WHILE @COUNT<200000
      BEGIN
      SET @PC=@PC+CAST(@COUNT AS VARCHAR(20))
      SET @Price=@Price+@COUNT
      INSERT INTO Sales VALUES (@PC,@Price,GETDATE())
      SET @PC='A12CB'
      SET @Price=50
      SET @COUNT=@COUNT+1
      END
END

-- execute it
EXEC InsertIntoSales

SET STATISTICS IO ON

-- Read without index, watch logical reads
SELECT * FROM Sales WHERE ID=189923
-- Create clustered Index on ID Column
CREATE CLUSTERED INDEX CL_ID ON SALES(ID);
-- Now Re-Read with index, watch logical reads drop to 3
SELECT * FROM Sales WHERE ID=189923

--- Non-Clustered Index ---

-- Read without index, watch logical reads
SELECT * FROM Sales WHERE ProductCode like 'A12CB908%' ORDER BY Price
-- Create nonclustered Index on ProductCode Column
CREATE NONCLUSTERED INDEX NONCI_PC ON SALES(ProductCode);
-- Now Re-Read with index, watch logical reads drop
SELECT * FROM Sales WHERE ProductCode like 'A12CB908%' ORDER BY Price

DROP INDEX Sales.CL_ID;
DROP INDEX Sales.NONCI_PC;
~~~
via [codeproject](http://www.codeproject.com/Articles/190263/Indexes-in-MS-SQL-Server)
	
- If deadlocks occur: First try to reduce the transaction time by optimizing the queries 
  - Less complex queries
  - Smaller amounts of data (e.g. through paging with e.g. TOP 25)
  - Improved indexes ie less time in the transaction
  - Check indices for insert and update as well
-  [Snapshot Isolation](http://www.brentozar.com/archive/2013/01/implementing-snapshot-or-read-committed-snapshot-isolation-in-sql-server-a-guide/) can be used to drastically reduce deadlocks.
	- Transaction-related snapshots are managed in the TempDB
	- SELECTS access a consistent database status before the last transaction

~~~

ALTER DATABASE MyDatabase
SET ALLOW_SNAPSHOT_ISOLATION ON

ALTER DATABASE MyDatabase
SET READ_COMMITTED_SNAPSHOT ON

~~~
	
- Maintenance of the database
  - Reduce volume periodically, for example by archiving
  - In the Crm, for example, delete the Crm.Log table on a rolling basis every 7 days
~~~
USE <DATABASE>

DECLARE @db AS NVARCHAR(100)
SELECT @db = DB_NAME()

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = object_id('CRM.Log') AND NAME ='IX_Log_Date')
CREATE NONCLUSTERED INDEX IX_Log_Date ON [CRM].[Log] ([Date])

DELETE FROM CRM.[Log] WHERE [date] < DATEADD(day, -7, GETDATE())

DBCC SHRINKDATABASE(@db)

~~~

- A good option to maintain your database according to [brentozar.com](http://www.brentozar.com/archive/2013/09/index-maintenance-sql-server-rebuild-reorganize/) is to periodically call the [IndexOptimize](https://ola.hallengren.com/sql-server-index-and-statistics-maintenance.html) script of [Ola Hallengren](https://ola.hallengren.com). See examples on the bottom of the page for more detailled explanations.

~~~~
EXECUTE dbo.IndexOptimize @Databases = 'USER_DATABASES',
@FragmentationLow = NULL,
@FragmentationMedium = 'INDEX_REORGANIZE,INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationHigh = 'INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE',
@FragmentationLevel1 = 5,
@FragmentationLevel2 = 30,
@UpdateStatistics = 'ALL',
@OnlyModifiedStatistics = 'Y'

~~~~

## Database Maintenance

A new background agent called DatabaseCleanupAgent is implemented to our system. It runs once a night by default and deleted deprecated data from tables. The deprecation measured in days can be adjusted in the appSettings configuration of the main plugin.

	- Maintenance/PostingDeprecationDays (int)
	- Maintenance/MessageDeprecationDays (int)
	- Maintenance/LogDeprecationDays (int)
	- Maintenance/ErrorLogDeprecationDays (int)
	- Maintenance/ReplicatedClientDeprecationDays (int)

Additionally the defragmentation/reorganization of indices is performed by the background agent. For this, you only have to add the prodcedures of OlaHallengreen's script to your databases. The scripts are added in "\tools\OlaHallengren" of your source folder.
The FragmentationLevels for the OlaHallengreen script is adjustable in the appSettings configuration.
	
	- Maintenance/FragmentationLevel1 (int) percentage
	- Maintenance/FragmentationLevel2 (int) percentage

	- Fragmentation < Level 1: Do Nothing
	- Fragmentation > Level && < Level 2 : Reorganize index, if not possible rebuild the index online.
	- Fragmentation > Level 2: Rebuild the index online, if not possible rebuild the index offline 

For detailed information about the configuration keys, take a look into configuration section of our dev documentation.

## IIS

- Memory, memory, memory and a bit of CPU (at least 8 GB, 4 cores better more)
- 64-bit worker processes to allocate enough memory. 
- 32-bit worker processes have private memory limits of approx. 2 GB ([ depending on the architecture ](http://blogs.msdn.com/b/tom/archive/2008/04/10/chat-question-memory-limits-for-32-bit-and-64-bit-processes.aspx)), followed by OutOfMemoryExceptions (can be remedied through timely recycling).
  
![32Bit Deactivate process](64bit_wp.png "32Bit Deactivate process")

- Pre-compile views
  - ASCX and ASPX files are compiled the first time they are called by the worker process, ie access to the hard disk + memory and CPU to compile the views, runtime errors may only be noticed at this moment due to faulty views. With precompiled views, a binary version is already stored on the server. Advantage: The first access to the page is 2-3 seconds faster, depending on the memory configuration.

![Precompiled views](precompiled_views.png "Precompiled views")

- Shut down the log level in production, info or warning and only reactivate it when necessary

----

- Set recycling (memory and time)
  - With 32 bit or depending on the memory configuration to avoid OutOfMemory exceptions. Nightly recycling is good.
  - Issue Recycle application pool after inactivity (otherwise the application starts within the day after 20 minutes of inactivity)

 
![Deactivate idle timeout](idle_timeout.png "Deactivate idle timeout")
  
- Use HTTPPing to preheat the page
  - After the restart in the night a first access to load the required resources into the memory = first access in the morning :)
- Compress and cache resources (dynamic content) = is deactivated by default in IIS and prevents, for example, remaining calls to an API from being compressed (gzip)

![Compress dynamic content](dynamic_content_compression_iis.png "Compress dynamic content")

---
The IIS standard installation does not include the module for compressing dynamic content.

---

## Application


- Set Quartz.Net [CRON Trigger](http://www.quartz-scheduler.org/documentation/quartz-1.x/tutorials/crontrigger) correctly
  - Check Jobs.xmls in all plugins
  - Do not trigger unnecessarily often
  - Remove unnecessary triggers e.g. Dropbox or SmtpDropbox
- Use paging (e.g. for address coding) to process only limited amounts of data
- Activate profiling (? Profile = true) in the url to check the loading times of the page

---
**Anti-Pattern**
 A trigger should export available objects to the connected ERP system every 5 minutes. For this purpose, a separate background agent with trigger was defined in the customer plug-in. Disadvantage: the system is unusable every 5 minutes. The CRON expression for this job was:
  
every second in the 5th minute
~~~
* 0/5 * ? * MON-FRI
~~~

not as originally planned: every 5 minutes
~~~
0 0/5 * ? * MON-FRI
~~~

---

## programming

- Careless use of resources in the NHibernate
  - Reloading lazy objects at runtime is better where possible, eg loading collections or parent relationships with the help of eager fetch strategies
  - Frequent and thoughtless database queries
- In general, every database query is valuable, ie. as few DB round trips as possible

**Example of eager fetch strategy**

~~~~
public override IQueryable<ServiceOrderHead> Eager(IQueryable<ServiceOrderHead> entities)
{
	entities.Fetch(x => x.CustomerContact);
	entities.Fetch(x => x.Payer);
	entities.Fetch(x => x.Initiator);
	entities.Fetch(x => x.ServiceObject);
	entities.Fetch(x => x.InvoiceRecipient);
	...
	
~~~~