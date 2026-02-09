namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20150413155303)]
	public class AddPreferredUserFkToSmsInstallationHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_InstallationHead_PreferredUser'") == 0)
			{
				Database.ChangeColumn("[SMS].[InstallationHead]", new Column("PreferredUser", DbType.String, 256, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE ih SET ih.[PreferredUser] = NULL FROM [SMS].[InstallationHead] ih LEFT OUTER JOIN [CRM].[User] u on ih.[PreferredUser] = u.[Username] WHERE u.[Username] IS NULL");
				Database.AddForeignKey("FK_InstallationHead_PreferredUser", "[SMS].[InstallationHead]", "PreferredUser", "[CRM].[User]", "Username", ForeignKeyConstraint.NoAction);
			}
		}
	}
}