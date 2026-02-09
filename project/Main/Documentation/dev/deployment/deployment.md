# Deployment
Remember to build the correct configuration (Debug for Test, Release for Production)

----
__Important__ Validate your *.config files for correct connection strings after you grab them from the Build server artifacts.

----

## Initial deployment
Initial deployment always will be executed for the **Test** enviroment.

- Create a working branch cb-x.x-customer from current stable
- Add new config transformations for customer environment in config folder
  - jobs.debug.xml + jobs.release.xml
  - web.debug.config + web.release.config
  - plugins.dat
  - any jobs.<configuration>.xml + appSettings.<configuration>.xml for active plugins
- Push customer branch
- Run customer branch build on [ci.l-mobile.com](http://ci.l-mobile.com) and download precompiled artifacts to target Webserver
- Download Staging_Template database backup from [current stable artifacts](http://ci.l-mobile.com)
- Copy contents of previously downloaded artifacts to LmobileTest folder

## Moving Test to Production
- Nominate a responsible for the deployment if multiple developer work in a project
	- Make sure anybody knows about the deployment and is available if any problems
- Close open developments in the Test system, commit and push your changes, 
- Merge open cfb-* branches if any
- Make a Backup of current LmobileProd database (if already available) and restore it to the L-mobileTest database
  - If your database is linked to any Legacy system (ERP, DMS, etc.) and any of the connection properties are stored inside your database (e.g. Stored Procedures, Views, Synonyms, etc.) you should change the settings after restoring the database and starting your application
  - Remember to change settings stored in Crm.Site table
  - Please take care if ERP System was not restored as well that order numbers could be out of sync
  - If any offline databases exist, make sure to reinstall the app to prevent data failure
- Execute migrations in the LmobileTest system and make sure they succeed
- Start LmobileTest application and compare the current status against Production (if available)
  - If data was migrated: Check UI record counts in Prod vs. Test (e.g. Dashboard, Contacts, etc.)
  - Execute smoke tests (e.g. Login, Login Mobile, Create order, dispatch order, export to Legacy System) internally or with your project manager for the most critical and most used functions
    - Consider creating a TOP 5 used function list for the customer to check on every deployment
  - Now it is time for some performance checks, does the system perform with production data, try if you can improve the system by finding bottlenecks
- Showcase new features to your customer / project manager
- **IMPORTANT** Get Approval document from customer for latest changes at least via E-Mail stating minimum Date and Name of User + written statement **Features (name features) have been tested thoroughly and are ready for Production** or similar. If there are any open issues append them to the written approval with a delivery date for fixing
- If any problems occurs during the tests, consider moving back to development phase and repeat QA
- Negotiate production deployment date
  - Ask the customer to inform all users that the Production update will be due on this date
  - Advise your customer to train the users before making a deployment
  - **Important** If system contains Offline components, please make sure users sync pending changes before the Update, so you can expect a clean state during upgrade. If the update fails the chance is you loose no pending data. 

----
**Expert Advice** Don't deploy the system on your last working day before a holiday if you don't have written approval and are aware of the consequences. 

The best day for deployments is a working day in the morning e.g. Tuesday morning at around 8 AM. That way you are in the office if something goes wrong. Deploying on friday afternoon is considered bad practice, cause you and your customers are going to the weekend but your customer is likely going to check the system on saturday afternoon. Every bug will be a bad start on monday mornings.

**Don't deploy on Friday afternoons!**

If you manage to deploy to a small group first (e.g. Project manager, customer key users, etc.), always do so, you will get feedback before things go out of control.

----

- After you have written approval and permission for deploy of your customer:
  - Create a clean release build from ci.l-mobile.com for all components. 
  - Force the application offline during the planned time period by stopping the application pool
  - Make a back up of the SQL Server production database
  - Make a back up of the inetpub\wwwroot\LmobileProd folder
  - Run migrations in Production system and make sure they succeed
  - Update the inetpub folder for Production system using your clean release build artifacts
  - Start the application pool and restart the system
  - Execute smoke tests in production (e.g. Login, schedule a dispatch, create an order) for the most critical and most used functions
  - If a serious problem occurs you may consider downgrading and fixing the problems in the Test environment instead.
  - If smoke Tests succeed: Write a Release note and publish it to the project participants
  - **Important** Merge your cb-x.x-customer branch to your cb-stable-x.x-customer to have a root for hotfixing. If if doesn't exist, create a new named branch cb-stable-x.x-customer
- Grab a coffee and look at the beauty of your new features in production

## Hotfixes in Production
Sometimes (hopefully not so often) customers will find bugs after you moved into production. To solve these situations easily it is important to create a cb-stable-x.x-customer after every successful deployment to the production system. 

----
**Important** This branch is meant to be used for hot fixes only. Don't ever try to start building new features in here.

----

- Update your working copy to cb-stable-x.x-customer head revision
- Find and fix the bug using a copy of the production database
- Commit and deploy
- Validate your fix in Production and inform your customer
- Merge hotfix from cb-stable-x.x-customer to your cb-x.x-customer branch to make sure it is removed for the next iteration as well

## Upgrading from release x.x to x.y
- Close all open developments in cb-x.x-customer and move them to production as described above
- Create a new branch cb-x.y-customer from current release x.y stable
- Merge latest cb-stable-x.x-customer to cb-x.y-customer resolving conflicts where necessary
- Deploy to the target Webserver Test enviroment using the above described steps
- After creating the new branches close previous cb-x.x-customer and cb-stable-x.x-customer branches to prevent confusion