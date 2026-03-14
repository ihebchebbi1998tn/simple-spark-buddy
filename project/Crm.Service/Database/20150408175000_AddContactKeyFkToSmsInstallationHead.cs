namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	
	[Migration(20150408175000)]
	public class AddContactKeyFkToSmsInstallationHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_InstallationHead_Contact'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE ih SET ih.[ContactKey] = NULL FROM [SMS].[InstallationHead] ih LEFT OUTER JOIN [CRM].[Contact] c on ih.[ContactKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_InstallationHead_Contact", "[SMS].[InstallationHead]", "ContactKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}