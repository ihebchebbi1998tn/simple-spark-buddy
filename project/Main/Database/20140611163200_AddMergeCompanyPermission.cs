namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140611163200)]
	public class AddMergeCompanyPermission : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'MergeCompany') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('MergeCompany', NULL) END");
		}
	}
}