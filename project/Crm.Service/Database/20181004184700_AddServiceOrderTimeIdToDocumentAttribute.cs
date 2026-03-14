namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20181004184700)]
	public class AddServiceOrderTimeIdToDocumentAttribute : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("CRM.DocumentAttributes", "ServiceOrderTimeId"))
			{
				Database.AddColumn("CRM.DocumentAttributes", new Column("ServiceOrderTimeId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_DocumentAttribute_ServiceOrderTime", "CRM.DocumentAttributes", "ServiceOrderTimeId", "SMS.ServiceOrderTimes", "id");
			}
		}
	}
}
