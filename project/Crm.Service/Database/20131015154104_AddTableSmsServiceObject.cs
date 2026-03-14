namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131015154104)]
	public class AddTableSmsServiceObject : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("SMS.ServiceObject"))
			{
				Database.AddTable("SMS.ServiceObject",
					new Column("ContactKey", DbType.Int32, ColumnProperty.PrimaryKey));
				Database.AddForeignKey("FK_ServiceObject_Contact", "SMS.ServiceObject", "ContactKey", "CRM.Contact", "ContactId");
			}
		}

		public override void Down()
		{
		}
	}
}