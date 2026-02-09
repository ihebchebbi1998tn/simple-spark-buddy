using System;
using System.Data;
using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20210819162000)]
	public class FixErrorCodeKeyTypes : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[ServiceOrderDispatch]"))
			{
				Database.ExecuteNonQuery(@"
					ALTER TABLE [SMS].[ServiceOrderDispatch]
					ALTER COLUMN ErrorCode NVARCHAR(20) NULL;
				");
			}

			if (Database.TableExists("[SMS].[ServiceNotifications]"))
			{
				Database.ExecuteNonQuery(@"
					ALTER TABLE [SMS].[ServiceNotifications]
					ALTER COLUMN ErrorCode NVARCHAR(20) NULL;
				");
			}

			if (Database.TableExists("[SMS].[ErrorCode]"))
			{
				Database.ExecuteNonQuery(@"
					ALTER TABLE [SMS].[ErrorCode]
					ALTER COLUMN [Value] NVARCHAR(20) NULL;
				");
			}
		}
	}
}
