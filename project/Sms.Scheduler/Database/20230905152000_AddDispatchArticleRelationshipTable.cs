namespace Sms.Scheduler.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	using Sms.Scheduler.Model;

	[Migration(20230905152000)]
	public class AddDispatchArticleRelationshipTable : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			if (!Database.TableExists("[SMS].[DispatchArticleAssignment]"))
			{
				Database.AddTable("[SMS].[DispatchArticleAssignment]",
					new Column("DispatchArticleAssignmentId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("DispatchKey", DbType.Guid, ColumnProperty.NotNull),
					new Column("ResourceKey", DbType.Guid, ColumnProperty.NotNull),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
				
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[DispatchArticleAssignment] ADD FOREIGN KEY (ResourceKey) REFERENCES [CRM].[Article](ArticleId)");
			}
			helper.AddOrUpdateEntityAuthDataColumn<DispatchArticleAssignment>("SMS", "DispatchArticleAssignment");
		}
	}
}
