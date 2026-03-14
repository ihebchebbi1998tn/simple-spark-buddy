namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20121205164559)]
    public class AddColumnIsExportedToSmsServiceOrderTimePosting : Migration
    {
        public override void Up()
        {
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimePostings]", new Column("IsExported", DbType.Boolean, ColumnProperty.NotNull, false));
		}

        public override void Down()
        {
            Database.RemoveColumnIfExisting("[SMS].[ServiceOrderTimePostings]", "IsExported");
        }
    }
}