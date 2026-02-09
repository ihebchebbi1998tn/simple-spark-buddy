namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120927105924)]
    public class AddColumnExternalRemarkToSmsServiceOrderMaterial : Migration
    {
        public override void Up()
        {
			if (Database.ColumnExists("[SMS].[ServiceOrderMaterial]", "Comment"))
			{
				Database.RenameColumn("[SMS].[ServiceOrderMaterial]", "Comment", "InternalRemark");
			}
			if (!Database.ColumnExists("[SMS].[ServiceOrderMaterial]", "ExternalRemark"))
			{
				Database.AddColumn("[SMS].[ServiceOrderMaterial]", "ExternalRemark", DbType.String, 4000, ColumnProperty.Null);
			}
        }
        public override void Down()
		{
			if (Database.ColumnExists("[SMS].[ServiceOrderMaterial]", "InternalRemark"))
			{
				Database.RenameColumn("[SMS].[ServiceOrderMaterial]", "InternalRemark", "Comment");
			}
			if (Database.ColumnExists("[SMS].[ServiceOrderMaterial]", "ExternalRemark"))
			{
				Database.RemoveColumn("[SMS].[ServiceOrderMaterial]", "ExternalRemark");
			}
        }
    }
}