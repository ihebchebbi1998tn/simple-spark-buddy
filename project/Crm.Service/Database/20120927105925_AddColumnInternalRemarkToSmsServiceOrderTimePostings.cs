namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120927105925)]
    public class AddColumnInternalRemarkToSmsServiceOrderTimePostings : Migration
    {
        public override void Up()
        {
			if (!Database.ColumnExists("[SMS].[ServiceOrderTimePostings]", "InternalRemark"))
			{
				Database.AddColumn("[SMS].[ServiceOrderTimePostings]", "InternalRemark", DbType.String, 4000, ColumnProperty.Null);
			}
        }
        public override void Down()
		{
			if (Database.ColumnExists("[SMS].[ServiceOrderTimePostings]", "InternalRemark"))
			{
				Database.RemoveColumn("[SMS].[ServiceOrderTimePostings]", "InternalRemark");
			}
        }
    }
}