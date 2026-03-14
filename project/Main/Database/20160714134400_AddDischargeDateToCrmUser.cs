namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160714134400)]
	public class AddDischargeDateToCrmUser : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[User]", "DischargeDate"))
			{
				Database.AddColumn("[CRM].[User]", new Column("DischargeDate", DbType.DateTime, ColumnProperty.Null));
			}
		}
	}
}