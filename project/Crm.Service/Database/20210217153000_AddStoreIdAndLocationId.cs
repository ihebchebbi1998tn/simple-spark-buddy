namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
    using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
    using System.Data;

    [Migration(20210217153000)]
	public class AddStoreIdAndLocationId : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("LocationId", DbType.Guid, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("StoreId", DbType.Guid, ColumnProperty.Null));
		}
	}
}