namespace Crm.Service.Database
{
  using Crm.Library.Data.MigratorDotNet.Framework;

  using System.Text;

	[Migration(20160415164800)]
	public class AddPermissionSetStatusServiceContractToSmsServiceContract : Migration
	{
		public override void Up()
		{
      var query = new StringBuilder();
      query.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission]")
        .AppendLine("WHERE PGroup = 'ServiceContract' AND Name = 'Delete')")
        .AppendLine("BEGIN")
        .AppendLine("INSERT INTO [Crm].[Permission] ([Name],[PluginName],[CreateDate],[ModifyDate],[Status],[Type],[PGroup],[CreateUser],[ModifyUser],[IsActive])")
        .AppendLine("VALUES ('Delete','Crm.Service',GETUTCDATE(),GETUTCDATE(),'1','0','ServiceContract','Setup','Setup','1')")
        .AppendLine("END");

      Database.ExecuteNonQuery(query.ToString());
		}
	}
}
