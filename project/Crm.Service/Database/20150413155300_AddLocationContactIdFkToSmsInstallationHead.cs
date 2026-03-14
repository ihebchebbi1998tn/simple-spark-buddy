namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	
	[Migration(20150413155300)]
	public class AddLocationContactIdFkToSmsInstallationHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_InstallationHead_LocationContact'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE ih SET ih.[LocationContactId] = NULL FROM [SMS].[InstallationHead] ih LEFT OUTER JOIN [CRM].[Contact] c on ih.[LocationContactId] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_InstallationHead_LocationContact", "[SMS].[InstallationHead]", "LocationContactId", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}