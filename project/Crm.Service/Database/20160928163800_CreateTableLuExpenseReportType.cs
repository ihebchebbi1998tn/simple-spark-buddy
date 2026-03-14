using Crm.Library.Data.MigratorDotNet.Framework;
using System;

namespace Crm.Service.Database
{
	[Migration(20160928163800)]
	public class CreateTableLuExpenseReportType : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[ExpenseReportType]"))
			{
				Database.ExecuteNonQuery(@"CREATE TABLE [LU].[ExpenseReportType](
					[ExpenseReportTypeId][int] IDENTITY(1, 1) NOT NULL,
					[Name][nvarchar](50) NOT NULL,
					[Language][nvarchar](2) NOT NULL,
					[Value][nvarchar](20) NOT NULL,
					[Favorite][bit] NOT NULL,
					[SortOrder][int] NOT NULL,
					[TenantKey][int] NULL,
					[CreateDate][datetime] NOT NULL DEFAULT(getutcdate()),
					[ModifyDate][datetime] NOT NULL DEFAULT(getutcdate()),
					[CreateUser][nvarchar](256) NOT NULL DEFAULT('Setup'),
					[ModifyUser][nvarchar](256) NOT NULL DEFAULT('Setup'),
					[IsActive][bit] NOT NULL DEFAULT((1)),
				 CONSTRAINT[PK_ExpenseReportType] PRIMARY KEY CLUSTERED
				(
					[ExpenseReportTypeId] ASC
				)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
				) ON[PRIMARY]");
			}

			if (Database.TableExists("[LU].[ExpenseReportType]") && Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM [LU].[ExpenseReportType]")) == 0)
			{
				Database.ExecuteNonQuery(@"INSERT INTO [LU].[ExpenseReportType]
           ([Name]
           ,[Language]
           ,[Value]
           ,[Favorite]
           ,[SortOrder]
           ,[TenantKey])
					VALUES
						('Weekly', 'en', 'Weekly', 0, 0, null),
						('Custom', 'en', 'Custom', 0, 1, null),
						('Wöchentlich', 'de', 'Weekly', 0, 0, null),
						('Benutzerdefiniert', 'de', 'Custom', 0, 1, null)");
			}
		}
	}
}