using Crm.Library.Data.MigratorDotNet.Framework;
using System;

namespace Crm.Service.Database
{
	[Migration(20181206093000)]
	public class AddMonthlyTimeEntryReportType : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[LU].[TimeEntryReportType]") && Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM [LU].[TimeEntryReportType] WHERE [Value] = 'Monthly'")) == 0)
			{
				Database.ExecuteNonQuery(@"INSERT INTO [LU].[TimeEntryReportType]
           ([Name]
           ,[Language]
           ,[Value]
           ,[Favorite]
           ,[SortOrder])
					VALUES
						('Monthly', 'en', 'Monthly', 0, 0),
						('Monatlich', 'de', 'Monthly', 0, 0)");
			}
		}
	}
}
