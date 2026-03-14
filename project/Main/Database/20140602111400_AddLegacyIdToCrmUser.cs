namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140602111400)]
	public class AddLegacyIdToCrmUser : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[User]", "LegacyId"))
			{
				Database.AddColumn("[CRM].[User]", new Column("LegacyId", DbType.String, 50, ColumnProperty.Null));
			}
		}
	}
}