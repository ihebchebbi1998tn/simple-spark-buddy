namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220119101000)]
	public class AddDefaultCountryToDomain : Migration
	{
		public override void Up()
		{
            var sql = "UPDATE [dbo].[Domain] SET DefaultCountryKey = '100' WHERE DefaultCountryKey IS NULL";
			Database.ExecuteNonQuery(sql);
		}
	}
}
