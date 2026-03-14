namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180719134200)]
	public class AlterSmsServiceOrderTimesSetDescriptionMaxLen : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("[SMS].[ServiceOrderTimes]", new Column("Description", DbType.String, 150, ColumnProperty.Null));
		}
	}
}