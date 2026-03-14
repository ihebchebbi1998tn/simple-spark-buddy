using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20210916133600)]
	public class MoveErrorCodeFromDispatchToOrder : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[ServiceOrderDispatch]") && Database.TableExists("[SMS].[ServiceOrderHead]"))
			{

				Database.ExecuteNonQuery(@"
					INSERT INTO SMS.ErrorCode ([Value], [Name], [Language])
					SELECT DISTINCT SMS.ServiceOrderHead.ErrorCode, SMS.ServiceOrderHead.ErrorCode, LU.[Language].[Value] 
					FROM SMS.ServiceOrderHead JOIN LU.[Language] ON 1=1 
					WHERE SMS.ServiceOrderHead.ErrorCode IS NOT NULL AND NOT EXISTS (
						SELECT TOP 1 NULL 
						FROM SMS.ErrorCode 
						WHERE SMS.ErrorCode.[Value] = SMS.ServiceOrderHead.ErrorCode
					) AND LU.[Language].IsSystemLanguage = 1");

				Database.ExecuteNonQuery(@"
					UPDATE soh
					SET soh.ErrorCode = sod.ErrorCode
					FROM SMS.ServiceOrderHead soh 
					LEFT JOIN SMS.ServiceOrderDispatch sod 
					ON soh.ContactKey = sod.OrderId
					WHERE soh.ErrorCode IS NULL AND sod.ErrorCode IS NOT NULL");
			}

			if (Database.TableExists("[SMS].[ServiceOrderDispatch]"))
			{
				Database.ExecuteNonQuery(@"
					ALTER TABLE [SMS].[ServiceOrderDispatch]
					DROP COLUMN ErrorCode;
				");
			}

			if (Database.TableExists("[SMS].[ServiceOrderHead]"))
			{
				Database.ExecuteNonQuery(@"
					ALTER TABLE [SMS].[ServiceOrderHead]
					ALTER COLUMN ErrorCode NVARCHAR(20) NULL;
				");
			}
		}
	}
}
