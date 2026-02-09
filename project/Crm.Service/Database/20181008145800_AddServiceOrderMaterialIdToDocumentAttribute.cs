namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20181008145800)]
	public class AddServiceOrderMaterialIdToDocumentAttribute : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("CRM.DocumentAttributes", "ServiceOrderMaterialId"))
			{
				Database.AddColumn("CRM.DocumentAttributes", new Column("ServiceOrderMaterialId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_DocumentAttribute_ServiceOrderMaterial", "CRM.DocumentAttributes", "ServiceOrderMaterialId", "SMS.ServiceOrderMaterial", "Id");
			}
		}
	}
}
