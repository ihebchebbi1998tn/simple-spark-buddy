namespace Crm.Service.Database
{
  using System.Data;

  using Crm.Library.Data.MigratorDotNet.Framework;
  using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

  [Migration(20160223132311)]
  public class AddColumnDispatchIdToCrmNote : Migration
  {
    public override void Up()
    {
      Database.AddColumnIfNotExisting("CRM.Note", new Column("DispatchId", DbType.Int64, ColumnProperty.Null));
    }

    public override void Down()
    {
      Database.RemoveColumnIfExisting("CRM.Note", "DispatchId");
    }
  }
}