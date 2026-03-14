namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	using System.Text;

	[Migration(20160415113200)]
	public class AddPermissionDeleteServiceContractToSmsServiceContract : Migration
	{
		public override void Up()
		{
			var query = new StringBuilder();
			query.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission]")
				.AppendLine("WHERE PGroup = 'ServiceContract' AND Name = 'SetStatus')")
				.AppendLine("BEGIN")
				.AppendLine("INSERT INTO [Crm].[Permission] ([Name],[PluginName],[CreateDate],[ModifyDate],[Status],[Type],[PGroup],[CreateUser],[ModifyUser],[IsActive])")
				.AppendLine("VALUES ('SetStatus','Crm.Service',GETUTCDATE(),GETUTCDATE(),'1','0','ServiceContract','Setup','Setup','1')")
				.AppendLine("END");

			Database.ExecuteNonQuery(query.ToString());
		}
	}
}
