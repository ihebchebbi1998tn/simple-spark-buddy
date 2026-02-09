namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190924091600)]
	public class AddOriginatingServiceOrderIdToServiceCase : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceNotifications", "OriginatingServiceOrderId"))
			{
				Database.AddColumn("SMS.ServiceNotifications", new Column("OriginatingServiceOrderId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_ServiceNotifications_OriginatingServiceOrder", "SMS.ServiceNotifications", "OriginatingServiceOrderId", "CRM.Contact", "ContactId");
			}
		}
	}
}