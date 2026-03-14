namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210308090400)]
	public class AddServiceOrderDispatchReportRecipient : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[ServiceOrderDispatchReportRecipient]"))
			{
				Database.AddTable(
					"[SMS].[ServiceOrderDispatchReportRecipient]",
					new Column("ServiceOrderDispatchReportRecipientId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("DispatchId", DbType.Guid, ColumnProperty.NotNull),
					new Column("Email", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, ColumnProperty.Null),
					new Column("Locale", DbType.String, ColumnProperty.Null),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));

				Database.AddForeignKey("FK_ServiceOrderDispatchReportRecipient_ServiceOrderDispatch", "SMS.ServiceOrderDispatchReportRecipient", "DispatchId", "SMS.ServiceOrderDispatch", "DispatchId");

				/* https://docs.microsoft.com/en-us/sql/t-sql/functions/string-split-transact-sql?view=sql-server-ver15#compatibility-level-130
				IF (SELECT CAST(compatibility_level AS int) FROM sys.databases WHERE name = DB_NAME()) < 130
				BEGIN
					DECLARE @database_name nvarchar(128) = DB_NAME()
					DECLARE @sql nvarchar(max)
					SET @sql = 'ALTER DATABASE [' + @database_name + '] SET COMPATIBILITY_LEVEL = 130';
					exec sp_executesql @sql
				END
				*/
				Database.ExecuteNonQuery("INSERT INTO [SMS].[ServiceOrderDispatchReportRecipient] (DispatchId, Email) SELECT DispatchId, value FROM SMS.ServiceOrderDispatch CROSS APPLY STRING_SPLIT(ReportRecipients, ',')");
				Database.RemoveColumn("[SMS].[ServiceOrderDispatch]", "ReportRecipients");
			}
		}
	}
}