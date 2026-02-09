namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220325172600)]
	public class AddEntityNameToNote : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Note]", "EntityName"))
			{
				Database.AddColumn("[CRM].[Note]", new Column("EntityName", DbType.String, 255, ColumnProperty.Null));
			}
		}
	}
}
