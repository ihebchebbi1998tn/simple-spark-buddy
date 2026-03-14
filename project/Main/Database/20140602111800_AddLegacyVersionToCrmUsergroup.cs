namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140602111800)]
	public class AddLegacyVersionToCrmUsergroup : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Usergroup]", "LegacyVersion"))
			{
				Database.AddColumn("[CRM].[Usergroup]", new Column("LegacyVersion", DbType.Int64, 50, ColumnProperty.Null));
			}
		}
	}
}