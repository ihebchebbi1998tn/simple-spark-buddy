namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20211217114000)]
	public class AddCountryKeyToDomain : Migration
	{
		public override void Up()
		{
            if (!Database.ColumnExists("[dbo].[Domain]", "DefaultCountryKey"))
            {
				Database.AddColumn("[dbo].[Domain]", "DefaultCountryKey", System.Data.DbType.String, ColumnProperty.Null);
			}
		}
	}
}
