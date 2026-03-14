namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20211109090600)]
	public class SetSourcePermissionSchemaId : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[dbo].[PermissionSchema]"))
			{
				Database.ExecuteNonQuery(@"
				UPDATE [dbo].[PermissionSchema]
				SET SourcePermissionSchemaId = (SELECT TOP 1 [UId] FROM [dbo].[PermissionSchema] WHERE [Name] = 'TemplatePermissionSchema')
				WHERE [Name] = 'DefaultPermissionSchema' AND SourcePermissionSchemaId IS NULL");
			}
		}
	}
}
