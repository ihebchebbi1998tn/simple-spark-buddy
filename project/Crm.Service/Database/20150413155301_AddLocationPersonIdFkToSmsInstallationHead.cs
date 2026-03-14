namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413155301)]
	public class AddLocationPersonIdFkToSmsInstallationHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_InstallationHead_LocationPerson'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE ih SET ih.[LocationPersonId] = NULL FROM [SMS].[InstallationHead] ih LEFT OUTER JOIN [CRM].[Contact] c on ih.[LocationPersonId] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_InstallationHead_LocationPerson", "[SMS].[InstallationHead]", "LocationPersonId", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}