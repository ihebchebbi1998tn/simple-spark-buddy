namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191127095700)]
	public class AddServiceOrderTemplateIdToServiceOrder : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderHead", "ServiceOrderTemplateId"))
			{
				Database.AddColumn("SMS.ServiceOrderHead", new Column("ServiceOrderTemplateId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_ServiceOrderHead_ServiceOrderTemplate", "SMS.ServiceOrderHead", "ServiceOrderTemplateId", "CRM.Contact", "ContactId");
			}
		}
	}
}