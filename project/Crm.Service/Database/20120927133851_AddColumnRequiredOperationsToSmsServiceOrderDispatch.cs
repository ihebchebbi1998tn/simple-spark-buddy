namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120927133851)]
    public class AddColumnRequiredOperationsToSmsServiceOrderDispatch : Migration
    {
        public override void Up()
        {
			if (!Database.ColumnExists("[SMS].[ServiceOrderDispatch]", "RequiredOperations"))
			{
				Database.AddColumn("[SMS].[ServiceOrderDispatch]", "RequiredOperations", DbType.String, 4000, ColumnProperty.Null);
			}
        }
        public override void Down()
		{
			if (Database.ColumnExists("[SMS].[ServiceOrderDispatch]", "RequiredOperations"))
			{
				Database.RemoveColumn("[SMS].[ServiceOrderDispatch]", "RequiredOperations");
			}
        }
    }
}