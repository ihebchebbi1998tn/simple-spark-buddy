namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20121130145903)]
    public class AddColumnsKilometerAndBreakToSmsServiceOrderTimePosting : Migration
    {
        public override void Up()
        {
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimePostings]", new Column("BreakInMinutes", DbType.Int32, ColumnProperty.Null));
            Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimePostings]", new Column("Kilometers", DbType.Int32, ColumnProperty.Null));
		}

        public override void Down()
        {
            Database.RemoveColumnIfExisting("[SMS].[ServiceOrderTimePostings]", "BreakInMinutes");
            Database.RemoveColumnIfExisting("[SMS].[ServiceOrderTimePostings]", "Kilometers");
        }
    }
}