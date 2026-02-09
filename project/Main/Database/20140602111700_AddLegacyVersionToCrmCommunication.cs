namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140602111700)]
	public class AddLegacyVersionToCrmCommunication : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Communication]", "LegacyVersion"))
			{
				Database.AddColumn("[CRM].[Communication]", new Column("LegacyVersion", DbType.Int64, 50, ColumnProperty.Null));
			}
		}
	}
}