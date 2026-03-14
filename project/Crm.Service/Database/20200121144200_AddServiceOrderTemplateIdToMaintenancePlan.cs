namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200121144200)]
	public class AddServiceOrderTemplateIdToMaintenancePlan : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.MaintenancePlan", "ServiceOrderTemplateId"))
			{
				Database.AddColumn("SMS.MaintenancePlan", new Column("ServiceOrderTemplateId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_MaintenancePlan_ServiceOrderTemplate", "SMS.MaintenancePlan", "ServiceOrderTemplateId", "CRM.Contact", "ContactId");
			}
		}
	}
}