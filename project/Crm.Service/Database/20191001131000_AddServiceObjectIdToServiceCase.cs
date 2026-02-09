namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191001131000)]
	public class AddServiceObjectIdToServiceCase : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceNotifications", "ServiceObjectId"))
			{
				Database.AddColumn("SMS.ServiceNotifications", new Column("ServiceObjectId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_ServiceNotifications_ServiceObject", "SMS.ServiceNotifications", "ServiceObjectId", "CRM.Contact", "ContactId");
			}
		}
	}
}