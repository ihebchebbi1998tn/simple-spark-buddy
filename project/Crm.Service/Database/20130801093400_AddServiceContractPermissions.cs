namespace Crm.Service.Database
{
	using System.Text;

    using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130801093400)]
    public class AddServiceContractPermissions : Migration
    {
        public override void Up()
        {
            var sb = new StringBuilder();

            sb.Append("IF EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateServiceContract') RETURN INSERT INTO [Crm].[Permission] (Name, PluginName) VALUES ('CreateServiceContract', 'Crm.Service') ");
            sb.Append("IF EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'EditServiceContract') RETURN INSERT INTO [Crm].[Permission] (Name, PluginName) VALUES ('EditServiceContract', 'Crm.Service') ");
            sb.Append("IF EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'DeleteServiceContract') RETURN INSERT INTO [Crm].[Permission] (Name, PluginName) VALUES ('DeleteServiceContract', 'Crm.Service')");

            Database.ExecuteNonQuery(sb.ToString());
        }

        public override void Down()
        {
            var sb = new StringBuilder();

            sb.Append("DELETE [Crm].[Permission] WHERE Name = 'CreateServiceContract' ");
            sb.Append("DELETE [Crm].[Permission] WHERE Name = 'EditServiceContract' ");
            sb.Append("DELETE [Crm].[Permission] WHERE Name = 'DeleteServiceContract'");

            Database.ExecuteNonQuery(sb.ToString());
        }
    }
}