namespace Crm.Service.Database
{
	using System.Text;
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130816124300)]
	public class CheckPermissions : Migration
	{
		public override void Up()
		{
			var sb = new StringBuilder();

			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateServiceContract') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('CreateServiceContract','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'EditServiceContract') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('EditServiceContract','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'DeleteServiceContract') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('DeleteServiceContract','Crm.Service') END");

			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateInstallationPos') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('CreateInstallationPos','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateInstallation') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('CreateInstallation','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'EditInstallation') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('EditInstallation','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'DeleteInstallation') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('DeleteInstallation','Crm.Service') END");

			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateServiceOrder') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('CreateServiceOrder','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'EditServiceOrder') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('EditServiceOrder','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'DeleteServiceOrder') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('DeleteServiceOrder','Crm.Service') END");

			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateServiceCase') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('CreateServiceCase','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'EditServiceCase') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('EditServiceCase','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'DeleteServiceCase') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('DeleteServiceCase','Crm.Service') END");

			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateArticle') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('CreateArticle','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'EditArticle') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('EditArticle','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateMaterial') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('DeleteArticle','Crm.Service') END");

			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'CreateDispatch') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('CreateDispatch','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'EditDispatch') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('EditDispatch','Crm.Service') END");
			sb.AppendLine("IF NOT EXISTS(SELECT * FROM [Crm].[Permission] WHERE Name = 'DeleteDispatch') BEGIN INSERT INTO [Crm].[Permission] (Name,PluginName) VALUES ('DeleteDispatch','Crm.Service') END");

			Database.ExecuteNonQuery(sb.ToString());
		}

		public override void Down()
		{
			
		}
	}
}