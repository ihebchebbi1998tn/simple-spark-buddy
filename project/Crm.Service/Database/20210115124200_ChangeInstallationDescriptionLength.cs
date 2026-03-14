namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210115124200)]
	public class ChangeInstallationDescriptionLength : Migration
	{
		public override void Up()
		{
			var contactNameLength = (int)Database.ExecuteScalar(
				@"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = 'CRM' AND
								TABLE_NAME = 'Contact' AND
								COLUMN_NAME = 'Name'");

			var installationDescriptionLength = (int)Database.ExecuteScalar(
				@"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = 'SMS' AND
								TABLE_NAME = 'InstallationHead' AND
								COLUMN_NAME = 'Description'");

			if (contactNameLength == 450 && installationDescriptionLength < 450)
			{
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[InstallationHead] ALTER COLUMN [Description] NVARCHAR(450) NOT NULL");
			}
		}
	}
}