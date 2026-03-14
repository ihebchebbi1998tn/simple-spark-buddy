namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190920080800)]
	public class AddServiceCaseTemplateIdToServiceCase : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceNotifications", "ServiceCaseTemplateId"))
			{
				Database.AddColumn("SMS.ServiceNotifications", new Column("ServiceCaseTemplateId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_ServiceNotifications_ServiceCaseTemplate", "SMS.ServiceNotifications", "ServiceCaseTemplateId", "SMS.ServiceCaseTemplate", "ServiceCaseTemplateId");
			}
		}
	}
}