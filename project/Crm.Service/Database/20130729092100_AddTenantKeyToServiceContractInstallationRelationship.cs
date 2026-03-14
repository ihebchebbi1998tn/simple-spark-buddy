namespace Crm.Service.Database
{
	using System.Text;

    using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130729092100)]
    public class AddTenantKeyToServiceContractInstallationRelationship : Migration
    {
        public override void Up()
        {
            var sb = new StringBuilder();
            sb.Append("IF NOT EXISTS(");
            sb.Append("SELECT TOP 1 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_NAME] = 'ServiceContractInstallationRelationship' AND [COLUMN_NAME] = 'TenantKey') ");
            sb.Append("BEGIN ");
            sb.Append("ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ");
            sb.Append("ADD [TenantKey] INT NULL ");
            sb.Append("END");

            Database.ExecuteNonQuery(sb.ToString());
        }

        public override void Down()
        {
        }
    }
}