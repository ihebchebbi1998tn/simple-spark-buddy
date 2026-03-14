namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Service.Model;

	[Migration(20190919162100)]
	public class AddServiceCaseTemplate : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("SMS.ServiceCaseTemplate"))
			{
				Database.AddTable("SMS.ServiceCaseTemplate", 
					new Column("ServiceCaseTemplateId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("CategoryKey", DbType.String, 20, ColumnProperty.Null),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("PriorityKey", DbType.String, 20, ColumnProperty.Null),
					new Column("ResponsibleUser", DbType.String, 256, ColumnProperty.Null),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
				var helper = new UnicoreMigrationHelper(Database);
				helper.AddEntityTypeAndAuthDataColumnIfNeeded<ServiceCaseTemplate>("SMS", "ServiceCaseTemplate");
			}
		}
	}
}
