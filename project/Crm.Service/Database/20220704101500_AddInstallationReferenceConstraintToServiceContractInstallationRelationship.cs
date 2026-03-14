namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220704101500)]
	public class AddInstallationReferenceConstraintToServiceContractInstallationRelationship : Migration
	{
		public override void Up()
		{
			const string ForeignKeyName = "FK_ServiceContractInstallationRelationship_Installation2";
			if ((int)Database.ExecuteScalar($"SELECT COUNT(name) FROM sys.foreign_keys WHERE [name] LIKE '{ForeignKeyName}'") == 0)
			{
				Database.ExecuteNonQuery(@$"ALTER TABLE [SMS].[ServiceContractInstallationRelationship]
					WITH CHECK ADD CONSTRAINT [{ForeignKeyName}]
					FOREIGN KEY([InstallationKey])
					REFERENCES [SMS].[InstallationHead] ([ContactKey])");
			}
		}
	}
}
