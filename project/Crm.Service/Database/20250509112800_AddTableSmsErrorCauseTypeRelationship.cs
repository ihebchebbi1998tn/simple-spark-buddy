namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20250509112800)]
	public class AddTableSmsErrorCauseTypeRelationship : Migration
	{
		public override void Up()
		{
			const string TableName = "[Sms].[ErrorCauseTypeRelationship]";
			if (!Database.TableExists(TableName))
			{
				Database.AddTable(TableName,
					new Column("ErrorCauseTypeRelationshipId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("StatisticsKeyCauseKey", DbType.Guid, ColumnProperty.NotNull),
					new Column("ErrorTypeKey", DbType.Guid, ColumnProperty.NotNull)
				);

				Database.AddForeignKey("FK_ErrorCauseTypeRelationship_ErrorType",
					TableName,
					"ErrorTypeKey",
					"[LU].[StatisticsKeyFaultImage]",
					"StatisticsKeyFaultImageId");
			}
		}
	}
}
