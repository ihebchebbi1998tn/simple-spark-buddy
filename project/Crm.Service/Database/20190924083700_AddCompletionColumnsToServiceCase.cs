namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20190924083700)]
	public class AddCompletionColumnsToServiceCase : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceNotifications", new Column("CompletionDate", DbType.DateTime, ColumnProperty.Null));
			if (!Database.ColumnExists("SMS.ServiceNotifications", "CompletionServiceOrderId"))
			{
				Database.AddColumn("SMS.ServiceNotifications", new Column("CompletionServiceOrderId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_ServiceNotifications_CompletionServiceOrder", "SMS.ServiceNotifications", "CompletionServiceOrderId", "CRM.Contact", "ContactId");
			}
			Database.AddColumnIfNotExisting("SMS.ServiceNotifications", new Column("CompletionUser", DbType.String, 256, ColumnProperty.Null));
		}
	}
}