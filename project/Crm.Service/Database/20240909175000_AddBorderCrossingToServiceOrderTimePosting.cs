namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240909175000)]
	public class AddBorderCrossingToServiceOrderTimePosting : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimePostings]", new Column("BorderCrossingTime", DbType.DateTime, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimePostings]", new Column("BorderCrossingTimeZone", DbType.String, 50, ColumnProperty.Null));
		}
	}
}
