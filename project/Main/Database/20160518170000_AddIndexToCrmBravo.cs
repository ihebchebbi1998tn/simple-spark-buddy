namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160518170000)]
	public class AddIndexToCrmBravo : Migration
	{
		public override void Up()
		{
			const string query = @"IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_Bravo_CompanyKey')" +
													 "CREATE NONCLUSTERED INDEX [IX_Bravo_CompanyKey] ON [CRM].[Bravo]([CompanyKey] ASC)";

			Database.ExecuteNonQuery(query);
		}
	}
}