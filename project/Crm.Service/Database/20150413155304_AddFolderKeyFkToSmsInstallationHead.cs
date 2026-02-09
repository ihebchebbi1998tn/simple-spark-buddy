namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413155304)]
	public class AddFolderKeyFkToSmsInstallationHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_InstallationHead_Folder'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE ih SET ih.[FolderKey] = NULL FROM [SMS].[InstallationHead] ih LEFT OUTER JOIN [CRM].[Contact] c on ih.[FolderKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_InstallationHead_Folder", "[SMS].[InstallationHead]", "FolderKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}