namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240209162000)]
	public class AddValidFromToUserSkillAndUserAsset : Migration {
		public override void Up()
		{
			if (Database.TableExists("[CRM].[UserSkill]"))
			{
				Database.AddColumn("[CRM].[UserSkill]", new Column("ValidFrom", DbType.DateTime, ColumnProperty.Null));

			}
			if (Database.TableExists("[CRM].[UserAsset]"))
			{
				Database.AddColumn("[CRM].[UserAsset]", new Column("ValidFrom", DbType.DateTime, ColumnProperty.Null));
			}
		}
	}
}
