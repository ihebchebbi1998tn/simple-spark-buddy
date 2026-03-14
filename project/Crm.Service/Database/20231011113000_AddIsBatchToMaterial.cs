namespace Crm.Service.Database
{
    using Crm.Library.Data.MigratorDotNet.Framework;
    using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

    using System.Data;

    [Migration(20231011113000)]
    public class AddIsBatchToMaterial : Migration
    {
        public override void Up()
        {
            Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("IsBatch", DbType.Boolean, ColumnProperty.NotNull, false));
        }
    }
}