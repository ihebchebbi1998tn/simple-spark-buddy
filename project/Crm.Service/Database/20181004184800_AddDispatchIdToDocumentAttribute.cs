namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20181004184800)]
	public class AddDispatchIdToDocumentAttribute : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("CRM.DocumentAttributes", "DispatchId"))
			{
				Database.AddColumn("CRM.DocumentAttributes", new Column("DispatchId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_DocumentAttribute_ServiceOrderDispatch", "CRM.DocumentAttributes", "DispatchId", "SMS.ServiceOrderDispatch", "DispatchId");
			}
		}
	}
}
