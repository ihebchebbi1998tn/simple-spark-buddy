namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120827160312)]
    public class AddColumnGroupKeyToInstallationType : Migration
    {
        public override void Up()
        {
            if (!Database.ColumnExists("[SMS].[InstallationType]", "GroupKey"))
            {
                Database.AddColumn("[SMS].[InstallationType]", "GroupKey", DbType.String, 20, ColumnProperty.Null);
            }
        }
        public override void Down()
        {
            if (Database.ColumnExists("[SMS].[InstallationType]", "GroupKey"))
            {
                Database.RemoveColumn("[SMS].[InstallationType]", "GroupKey");
            }
        }
    }
}