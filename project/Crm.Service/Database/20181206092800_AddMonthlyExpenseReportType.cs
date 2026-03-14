using Crm.Library.Data.MigratorDotNet.Framework;
using System;

namespace Crm.Service.Database
{
	[Migration(20181206092800)]
	public class AddMonthlyExpenseReportType : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[LU].[ExpenseReportType]") && Convert.ToInt32(Database.ExecuteScalar("SELECT COUNT(*) FROM [LU].[ExpenseReportType] WHERE [Value] = 'Monthly'")) == 0)
			{
				Database.ExecuteNonQuery(@"INSERT INTO [LU].[ExpenseReportType]
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
