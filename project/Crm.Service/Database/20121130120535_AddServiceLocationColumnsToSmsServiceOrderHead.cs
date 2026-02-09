namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20121130120535)]
    public class AddServiceLocationColumnsToSmsServiceOrderHead : Migration
    {
        public override void Up()
        {
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("ServiceLocationPhone", DbType.String, 100, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("ServiceLocationMobile", DbType.String, 100, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("ServiceLocationFax", DbType.String, 100, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("ServiceLocationEmail", DbType.String, 100, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderHead]", new Column("ServiceLocationResponsiblePerson", DbType.String, 100, ColumnProperty.Null));
		}

        public override void Down()
        {
            Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "ServiceLocationPhone");
            Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "ServiceLocationMobile");
            Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "ServiceLocationFax");
            Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "ServiceLocationEmail");
            Database.RemoveColumnIfExisting("[SMS].[ServiceOrderHead]", "ServiceLocationResponsiblePerson");
		
        }
    }
}