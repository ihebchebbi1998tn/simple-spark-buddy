namespace Crm.Service.Database
{
	using System.Data;

	using Library.Data.MigratorDotNet.Framework;

	[Migration(20160617184400)]
    public class AlterServiceOrderDispatchComponentType : Migration
    {
		public override void Up()
		{
			if (Database.ColumnExists("[SMS].[ServiceOrderDispatch]", "Component"))
			{
				Database.ChangeColumn("[SMS].[ServiceOrderDispatch]", new Column("Component", DbType.String, 20, ColumnProperty.Null));
			}
		}
	}
}