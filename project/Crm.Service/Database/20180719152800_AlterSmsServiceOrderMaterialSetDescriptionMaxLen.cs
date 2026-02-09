namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180719152800)]
	public class AlterSmsServiceOrderMaterialSetDescriptionMaxLen : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("[SMS].[ServiceOrderMaterial]", new Column("Description", DbType.String, 150, ColumnProperty.NotNull));
		}
	}
}