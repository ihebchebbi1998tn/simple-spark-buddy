namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140602110400)]
	public class AddImportColumnsToCrmTenant : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Tenant]", "LegacyId"))
			{
				Database.AddColumn("[CRM].[Tenant]", new Column("LegacyId", DbType.String, 50, ColumnProperty.Null));
			}
			if (!Database.ColumnExists("[CRM].[Tenant]", "LegacyVersion"))
			{
				Database.AddColumn("[CRM].[Tenant]", new Column("LegacyVersion", DbType.Int64, 50, ColumnProperty.Null));
			}
		}
	}
}