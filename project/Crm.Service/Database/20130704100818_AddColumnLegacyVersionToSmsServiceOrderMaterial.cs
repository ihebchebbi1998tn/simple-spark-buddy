namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130704100818)]
	public class AddColumnLegacyVersionToSmsServiceOrderMaterial : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[SMS].[ServiceOrderMaterial]", "LegacyVersion"))
			{
				Database.AddColumn("[SMS].[ServiceOrderMaterial]", "LegacyVersion", DbType.Int64, ColumnProperty.Null);
			}
		}
		public override void Down()
		{
		}
	}
}