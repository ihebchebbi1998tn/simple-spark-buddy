using Crm.Library.Data.MigratorDotNet.Framework;
using System;

namespace Crm.Service.Database
{
	[Migration(20160928115300)]
	public class CreateTableLuExpenseReportStatus : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[ExpenseReportStatus]"))
			{
				Database.ExecuteNonQuery(@"CREATE TABLE [LU].[ExpenseReportStatus](
					[ExpenseReportStatusId][int] IDENTITY(1, 1) NOT NULL,
					[Name][nvarchar](50) NOT NULL,
					[Language][nvarchar](2) NOT NULL,
					[Value][nvarchar](20) NOT NULL,
					[Favorite][bit] NOT NULL,
					[SortOrder][int] NOT NULL,
					[SettableStatuses] [nvarchar](500) NULL,
					[TenantKey][int] NULL,
					[ShowInMobileClient] [bit] NOT NULL DEFAULT((1)),
					[CreateDate][datetime] NOT NULL DEFAULT(getutcdate()),
					[ModifyDate][datetime] NOT NULL DEFAULT(getutcdate()),
					[CreateUser][nvarchar](256) NOT NULL DEFAULT('Setup'),
					[ModifyUser][nvarchar](256) NOT NULL DEFAULT('Setup'),
					[IsActive][bit] NOT NULL DEFAULT((1)),
				 CONSTRAINT[PK_ExpenseReportStatus] PRIMARY KEY CLUSTERED
				(
					[ExpenseReportStatusId] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]");

				if (Database.TableExists("[LU].[ExpenseReportStatus]") && Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM [LU].[ExpenseReportStatus]")) == 0)
				{
					Database.ExecuteNonQuery(@"INSERT INTO [LU].[ExpenseReportStatus]
                        ([Name]
                        ,[Language]
                        ,[Value]
                        ,[Favorite]
                        ,[SortOrder]
												,[SettableStatuses]
                        ,[TenantKey]
                        ,[ShowInMobileClient])
					VALUES
						('Open', 'en', 'Open', 0, 0, 'Closed', null, 0),
						('Offen', 'de', 'Open', 0, 0, 'Closed', null, 0),
						('Closed', 'en', 'Closed', 0, 2, 'Open', null, 1),
						('Geschlossen', 'de', 'Closed', 0, 2, 'Open', null, 1),
						('Request close', 'en', 'RequestClose', 0, 1, 'Closed', null, 1),
						('Schließen anfordern', 'de', 'RequestClose', 0, 1, 'Closed', null, 1)");
				}
			}
		}
	}
}