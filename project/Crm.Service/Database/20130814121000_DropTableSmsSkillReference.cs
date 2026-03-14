namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130814121000)]
	public class DropTableSmsSkillReference : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[SkillReference]"))
			{
				Database.RemoveTable("[SMS].[SkillReference]");
			}
		}
	}
}