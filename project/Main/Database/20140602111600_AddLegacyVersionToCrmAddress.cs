namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140602111600)]
	public class AddLegacyVersionToCrmAddress : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Address]", "LegacyVersion"))
			{
				Database.AddColumn("[CRM].[Address]", new Column("LegacyVersion", DbType.Int64, 50, ColumnProperty.Null));
			}
		}
	}
}