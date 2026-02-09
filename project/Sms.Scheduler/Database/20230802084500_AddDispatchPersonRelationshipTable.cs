namespace Sms.Scheduler.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	using Sms.Scheduler.Model;

	[Migration(20230802084500)]
	public class AddDispatchPersonRelationshipTable : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			if (!Database.TableExists("[SMS].[DispatchPersonAssignment]"))
			{
				Database.AddTable("[SMS].[DispatchPersonAssignment]",
					new Column("DispatchPersonAssignmentId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("DispatchKey", DbType.Guid, ColumnProperty.NotNull),
					new Column("ResourceKey", DbType.String, ColumnProperty.NotNull),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
				
				Database.ExecuteNonQuery($"ALTER TABLE [SMS].[DispatchPersonAssignment] ALTER COLUMN [ResourceKey] NVARCHAR(256) NOT NULL");
				Database.ExecuteNonQuery("ALTER TABLE SMS.DispatchPersonAssignment ADD FOREIGN KEY (ResourceKey) REFERENCES [CRM].[User](Username)");
			}
			helper.AddOrUpdateEntityAuthDataColumn<DispatchPersonAssignment>("SMS", "DispatchPersonAssignment");
		}
	}
}
