# Development Environment
For the development of CRM and service components, there exists a typical environment that would speed up the developers significantly.

## Essentials
The typical L-mobile development environment is as follows.

These components are required to build and run the L-mobile CRM / service locally

*    Microsoft Windows 7 or later (x64 is preferred and supported) (Should be installed by IT)
*    Microsoft Visual Studio 2015 (Should be installed by IT)
*    [Microsoft SQL Server 2014 R2 Express with Management Studio](http://www.microsoft.com/sqlserver/en/us/editions/express.aspx)
*    [TortoiseHG](http://tortoisehg.bitbucket.org/)

----
**Important** These components are very important for a compatible and productive environment

*    [Resharper](http://www.jetbrains.com/resharper/) (L-mobile has a license)
*    a good editor like [Notepad++](http://notepad-plus-plus.org/download/)

----

## Required Basic Knowledge
The essential knowledge needed to be able to extend and build upon the L-mobile CRM / service project.

In the CRM project, you will typically encounter C# and Javascript code. However, familiarity with these two languages by itself would not be enough, as there are frameworks and conventions in place.

The most important frameworks that the developers need to be familiar with are [jQuery](http://www.jquery.com) and [ASP.NET MVC](http://www.asp.net/mvc).

Meanwhile, being able to write proper [Selenium test](#selenium-tests) enable application developers to make sure the user interface keeps working while the application gets extended.

## Information Channels
Communication Channels for Data Exchange. The channels can be used to communicate with other developers or get the latest information about the project when you need help, detailed information or feedback on development, documentation or decision making.

### HG / Mercurial
Mercurial is used to manage versions of the source code, ease collaboration and enable reverting to older versions when necessary. TortoiseHG is used as the preferred client for connecting to the L-mobile HG repositories.

To access the L-mobile HG repositories, please enter the following url to your HG client:  
`https://svn.l-mobile.com/hg/Crm` 

For further information about the usage of the TortoiseHG client, please refer to its [manual](http://tortoisehg.bitbucket.org/docs.html).

You'll need an account for accessing the repositories. If you still don't have an account, please contact your lead developer.

### TeamCity server
This software runs on the build server which re-builds the project on every commit by any developer. TeamCity is used to track the status of these builds. When you go to [Teamcity](http://ci.l-mobile.com/), you will see the list of the projects that are being tracked by TeamCity. By clicking on the name of any of those projects, you can see detailed information about the recent builds like the list of the modified files from the last build, test pass/fail status, any warnings/errors encountered during the build and the build time.

### L-mobile projects
This software helps you keep track of the bugs assigned to you while allowing you to add some meta data, see the list of modified files and comment on them.

----
**Important** It's never a "bug", it's always an "issue"

----

You can directly go to [the Bug Tracker](https://pm.l-mobile.com) and use the filtering options available to see the issues assigned to you. However, you will need an account to be able to do that.

### Socialcast
Think of it as a Twitter or facebook, which only L-mobile employees can access and doesn't have any character limits, allows commenting, adding links, adding images and custom groups. Why are you waiting?

[L-mobile Bluebox Site](https://bluebox.l-mobile.com)

### Skype
If you haven't heard already, "Skype’s text, voice and video make it simple to share experiences with the people that matter to you, wherever they are". Other developers should matter to you. You get the idea.

You can ask your colleagues to find out about the Skype ids of your fellow developers.

To open an account and install Skype please go to the [Skype official page](http://www.skype.com/).

